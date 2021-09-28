using System.Collections.Generic;
using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;
using LG.POC.InterfaceFabricas.Fabricas.Mapeadores;
using System.Data.SqlClient;
using LG.POC.InterfaceFabricas.Fabricas.Negocio;
using LG.POC.InterfaceFabricas.Utilidades.Extensoes;

namespace LG.POC.ServicoMapeadores.Mapeadores
{
    class MapeadorDeProdutos : IMapeadorDeProdutos
    {
        #region Manutenção

        public void Atualize(DtoProduto dto)
        {
            var sql = "UPDATE POC_PRODUTOS ";
            sql += "SET DESCRICAO = @DESCRICAO, VALOR = @VALOR, QUANTIDADE = @QUANTIDADE ";
            sql += "WHERE CODIGO = @CODIGO;";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var comando = conexao.CreateCommand())
                {
                    using (var transacao = conexao.BeginTransaction())
                    {
                        comando.Transaction = transacao;

                        comando.CommandText = sql;

                        comando.AdicioneParametros(dto);

                        comando.ExecuteNonQuery();

                        transacao.Commit();
                    }
                }
            }
        }

        public void Exclua(int codigo)
        {
            var sql = "DELETE FROM POC_PRODUTOS ";
            sql += "WHERE CODIGO = @CODIGO;";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var comando = conexao.CreateCommand())
                {
                    using (var transacao = conexao.BeginTransaction())
                    {
                        comando.Transaction = transacao;

                        comando.CommandText = sql;

                        comando.Parameters.AddWithValue("@CODIGO", codigo);

                        comando.ExecuteNonQuery();

                        transacao.Commit();
                    }
                }
            }
        }

        public void ExcluaVarios(int[] codigos)
        {
            var sql = "DELETE FROM POC_PRODUTOS ";
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

        public DtoProduto Insira(DtoProdutoCadastro dto)
        {
            DtoProduto produto = null;

            var sql = "INSERT INTO POC_PRODUTOS ";
            sql += "(DESCRICAO, VALOR, QUANTIDADE) OUTPUT INSERTED.CODIGO ";
            sql += "VALUES (@DESCRICAO, @VALOR, @QUANTIDADE);";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var transacao = conexao.BeginTransaction())
                using (var comando = conexao.CreateCommand())
                {

                    comando.Transaction = transacao;

                    comando.CommandText = sql;

                    comando.AdicioneParametros(dto);

                    produto = FabricaDeDtoProduto.Crie((int)comando.ExecuteScalar(), dto.Descricao, dto.Valor, dto.Quantidade);

                    transacao.Commit();

                }
            }
            return produto;
        }
        #endregion

        #region Consulta

        public DtoProduto ObtenhaPorCodigo(int codigo)
        {
            var sql = "SELECT * FROM POC_PRODUTOS WHERE CODIGO = @CODIGO;";

            DtoProduto dto = null;

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
                            dto = CarregueDto(leitor);
                        }
                    }
                }
            }
            return dto;
        }

        public IList<DtoProduto> ObtenhaTodos()
        {
            var sql = "SELECT * FROM POC_PRODUTOS;";

            DtoProduto dto = null;

            IList<DtoProduto> produtos = new List<DtoProduto>();

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
                            dto = CarregueDto(leitor);

                            produtos.Add(dto);
                        }
                    }
                }
            }
            return produtos;
        }
        #endregion

        #region Métodos Auxiliares
        private static DtoProduto CarregueDto(SqlDataReader leitor)
        {
            DtoProduto dto;
            var codigo = leitor.GetInt32(leitor.GetOrdinal("CODIGO"));
            var descricao = leitor.GetString(leitor.GetOrdinal("DESCRICAO"));
            var valor = leitor.GetDecimal(leitor.GetOrdinal("VALOR"));
            var quantidade = leitor.GetInt32(leitor.GetOrdinal("QUANTIDADE"));

            dto = FabricaDeDtoProduto.Crie(codigo, descricao, valor, quantidade);

            return dto;
        }
        #endregion
    }
}
