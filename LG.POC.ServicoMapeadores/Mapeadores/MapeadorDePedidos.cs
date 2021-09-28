using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;
using LG.POC.InterfaceFabricas.Fabricas.Mapeadores;
using LG.POC.InterfaceFabricas.Fabricas.Negocio;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;

namespace LG.POC.ServicoMapeadores.Mapeadores
{
    public class MapeadorDePedidos : IMapeadorDePedidos
    {
        #region Manutenção

        public void Exclua(int codigo)
        {
            var sql = "DELETE FROM POC_PEDIDOS WHERE CODIGO = @CODIGO";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var transacao = conexao.BeginTransaction())
                {
                    using (var comando = conexao.CreateCommand())
                    {
                        comando.Transaction = transacao;

                        comando.CommandText = sql;

                        comando.Parameters.AddWithValue("@CODIGO", codigo);

                        comando.ExecuteNonQuery();
                    }
                    transacao.Commit();
                }
            }
        }

        public void ExcluaVarios(int[] codigos)
        {
            var sql = "DELETE FROM POC_PEDIDOS ";
            sql += "WHERE CODIGO IN (";

            for (int i = 0; i < codigos.Length; i++)
            {
                if (codigos.Length - i > 1)
                {
                    sql += "@CODIGO" + i + ", ";
                }
                else
                {
                    sql += "@CODIGO" + i;
                }

            }

            sql += ");";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();
                using (var comando = conexao.CreateCommand())
                using (var transacao = conexao.BeginTransaction())
                {
                    comando.Transaction = transacao;

                    comando.CommandText = sql;

                    for (int i = 0; i < codigos.Length; i++)
                    {
                        comando.Parameters.AddWithValue("@CODIGO" + i, codigos[i]);
                    }

                    comando.ExecuteNonQuery();

                    transacao.Commit();
                }
            }
        }

        public DtoPedido Insira(DtoPedidoInsercao dto)
        {
            var cliente = FabricaDeServicoDeClientes.Crie().ObtenhaPorCodigo(dto.CodigoCliente);

            var pedido = FabricaDeDtoPedido.Crie();

            pedido.DataPedido = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            pedido.CodigoCliente = dto.CodigoCliente;
            pedido.EnviarPorEmail = dto.EnviarPorEmail;
            pedido.NomeCliente = cliente.Nome;
            pedido.Produtos = new List<DtoProduto>();

            foreach (var item in dto.Produtos)
            {
                var produto = FabricaDeServicoDeProduto.Crie().ObtenhaPorCodigo(item.Codigo);

                produto.Quantidade = dto.Produtos[dto.Produtos.IndexOf(item)].Quantidade;

                if (produto != null) pedido.Produtos.Add(produto);

                pedido.ValorPedido += produto.Valor * produto.Quantidade;
            }

            var sql = "INSERT INTO POC_PEDIDOS ";
            sql += "(CODIGO_CLIENTE, NOME_CLIENTE, VALOR_PEDIDO, DATA_PEDIDO, ENVIAR_POR_EMAIL) OUTPUT INSERTED.CODIGO ";
            sql += "VALUES (@CODIGOCLIENTE, @NOMECLIENTE, @VALORPEDIDO, @DATAPEDIDO, @ENVIARPOREMAIL);";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var transacao = conexao.BeginTransaction())
                {
                    using (var comando = conexao.CreateCommand())
                    {
                        comando.Transaction = transacao;

                        comando.CommandText = sql;

                        comando.Parameters.AddWithValue("@CODIGOCLIENTE", cliente.Codigo);
                        comando.Parameters.AddWithValue("@NOMECLIENTE", cliente.Nome);
                        comando.Parameters.AddWithValue("@VALORPEDIDO", pedido.ValorPedido);
                        comando.Parameters.AddWithValue("@DATAPEDIDO", pedido.DataPedido);
                        comando.Parameters.AddWithValue("@ENVIARPOREMAIL", dto.EnviarPorEmail);

                        pedido.Codigo = (int)comando.ExecuteScalar();

                        sql = "INSERT INTO POC_PRODUTOS_PEDIDO ";
                        sql += "(CODIGO_PEDIDO, CODIGO_PRODUTO, QUANTIDADE) ";
                        sql += $"VALUES ({pedido.Codigo}, @CODIGOPRODUTO, @QUANTIDADE);";

                        comando.Parameters.Clear();

                        comando.CommandText = sql;
                        
                        foreach (var produto in dto.Produtos)
                        {
                            if (comando.Parameters.Count == 0)
                            {
                                comando.Parameters.AddWithValue("@CODIGOPRODUTO", produto.Codigo);
                                comando.Parameters.AddWithValue("@QUANTIDADE", produto.Quantidade);
                            }
                            else
                            {
                                comando.Parameters[comando.Parameters.IndexOf("@CODIGOPRODUTO")].Value = produto.Codigo;
                                comando.Parameters[comando.Parameters.IndexOf("@QUANTIDADE")].Value = produto.Quantidade;
                            }

                            comando.ExecuteNonQuery();
                        }
                    }
                    transacao.Commit();
                }
            }
            return pedido;
        }
        #endregion

        #region Consulta

        public DtoPedido ObtenhaPorCodigo(int codigo)
        {
            var sql = "SELECT PED.CODIGO AS 'CODIGO PEDIDO', ";
            sql += "CLI.CODIGO AS 'CODIGO CLIENTE', CLI.NOME AS 'CLIENTE', ";
            sql += "PRD.CODIGO AS 'CODIGO PRODUTO', PRD.DESCRICAO AS 'PRODUTO', PRD.VALOR AS 'VALOR UNITARIO', PDP.QUANTIDADE AS 'QUANTIDADE', ";
            sql += "PED.VALOR_PEDIDO AS 'TOTAL PEDIDO', PED.DATA_PEDIDO AS 'DATA', PED.ENVIAR_POR_EMAIL AS 'ENVIAR POR EMAIL' ";

            sql += "FROM POC_PEDIDOS PED ";

            sql += "LEFT JOIN POC_CLIENTES CLI ";
            sql += "ON CLI.CODIGO = PED.CODIGO_CLIENTE ";

            sql += "LEFT JOIN POC_PRODUTOS_PEDIDO PDP ";
            sql += "ON PDP.CODIGO_PEDIDO = PED.CODIGO ";

            sql += "LEFT JOIN POC_PRODUTOS PRD ";
            sql += "ON PDP.CODIGO_PRODUTO = PRD.CODIGO ";

            sql += "WHERE PED.CODIGO = @CODIGO;";

            DtoPedido dto = null;

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var comando = conexao.CreateCommand())
                {
                    comando.CommandText = sql;

                    comando.Parameters.AddWithValue("@CODIGO", codigo);

                    using (var leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            var produto = CarregueProduto(leitor);

                            if (dto == null)
                            {
                                dto = CarregueDto(leitor);

                                dto.Produtos = new List<DtoProduto>();
                            }

                            dto.Produtos.Add(produto);
                        }
                    }
                }
            }
            return dto;
        }



        public IList<DtoPedido> ObtenhaTodos()
        {
            var sql = "SELECT PED.CODIGO AS 'CODIGO PEDIDO', ";
            sql += "CLI.CODIGO AS 'CODIGO CLIENTE', CLI.NOME AS 'CLIENTE', ";
            sql += "PRD.CODIGO AS 'CODIGO PRODUTO', PRD.DESCRICAO AS 'PRODUTO', PRD.VALOR AS 'VALOR UNITARIO', PDP.QUANTIDADE AS 'QUANTIDADE', ";
            sql += "PED.VALOR_PEDIDO AS 'TOTAL PEDIDO', PED.DATA_PEDIDO AS 'DATA', PED.ENVIAR_POR_EMAIL AS 'ENVIAR POR EMAIL' ";

            sql += "FROM POC_PEDIDOS PED ";

            sql += "LEFT JOIN POC_CLIENTES CLI ";
            sql += "ON CLI.CODIGO = PED.CODIGO_CLIENTE ";

            sql += "LEFT JOIN POC_PRODUTOS_PEDIDO PDP ";
            sql += "ON PDP.CODIGO_PEDIDO = PED.CODIGO ";

            sql += "LEFT JOIN POC_PRODUTOS PRD ";
            sql += "ON PDP.CODIGO_PRODUTO = PRD.CODIGO ";

            DtoPedido dto = null;

            IList<DtoPedido> pedidos = new List<DtoPedido>();

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var comando = conexao.CreateCommand())
                {
                    comando.CommandText = sql;

                    using (var leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            if (dto == null || dto.Codigo != leitor.GetInt32(leitor.GetOrdinal("CODIGO PEDIDO")))
                            {
                                dto = CarregueDto(leitor);

                                dto.Produtos = new List<DtoProduto>();

                                pedidos.Add(dto);
                            }

                            var produto = CarregueProduto(leitor);

                            pedidos[pedidos.IndexOf(dto)].Produtos.Add(produto);
                        }
                    }
                }
            }
            return pedidos;
        }
        #endregion

        #region Métodos Auxiliares

        private static DtoPedido CarregueDto(SqlDataReader leitor)
        {
            return FabricaDeDtoPedido.Crie(
                leitor.GetInt32(leitor.GetOrdinal("CODIGO PEDIDO")),
                leitor.GetInt32(leitor.GetOrdinal("CODIGO CLIENTE")),
                leitor.GetString(leitor.GetOrdinal("CLIENTE")),
                leitor.GetString(leitor.GetOrdinal("DATA")),
                leitor.GetDecimal(leitor.GetOrdinal("TOTAL PEDIDO")),
                leitor.GetBoolean(leitor.GetOrdinal("ENVIAR POR EMAIL"))
            );
        }
        private static DtoProduto CarregueProduto(SqlDataReader leitor)
        {
            return FabricaDeDtoProduto.Crie(
                leitor.GetInt32(leitor.GetOrdinal("CODIGO PRODUTO")),
                leitor.GetString(leitor.GetOrdinal("PRODUTO")),
                leitor.GetDecimal(leitor.GetOrdinal("VALOR UNITARIO")),
                leitor.GetInt32(leitor.GetOrdinal("QUANTIDADE"))
            );
        }
        #endregion
    }
}
