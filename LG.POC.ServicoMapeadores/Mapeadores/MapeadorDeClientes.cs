using System.Collections.Generic;
using System.Data.SqlClient;
using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;
using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.Fabricas.Mapeadores;
using LG.POC.InterfaceFabricas.Fabricas.Negocio;
using LG.POC.InterfaceFabricas.Utilidades.Extensoes;

namespace LG.POC.ServicoMapeadores.Mapeadores
{
    public class MapeadorDeClientes : IMapeadorDeClientes
    {
        #region Manutenção
        public void Atualize(DtoCliente dto)
        {
            var sql = "UPDATE POC_CLIENTES ";
            sql += "SET NOME = @NOME, EMAIL = @EMAIL, CONTATO = @CONTATO ";
            sql += "WHERE CODIGO = @CODIGO;";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var transacao = conexao.BeginTransaction())
                using (var comando = conexao.CreateCommand())
                {

                    comando.Transaction = transacao;

                    comando.CommandText = sql;

                    comando.AdicioneParametros(dto);

                    comando.ExecuteNonQuery();

                    transacao.Commit();

                }
            }
        }

        public void Exclua(int codigo)
        {
            var sql = "DELETE FROM POC_CLIENTES WHERE CODIGO = @CODIGO;";

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                conexao.Open();

                using (var transacao = conexao.BeginTransaction())
                using (var comando = conexao.CreateCommand())
                {
                    comando.Transaction = transacao;

                    comando.CommandText = sql;

                    comando.Parameters.AddWithValue("@CODIGO", codigo);

                    comando.ExecuteNonQuery();

                    transacao.Commit();
                }
            }
        }

        public void ExcluaVarios(int[] codigos)
        {
            var sql = "DELETE FROM POC_CLIENTES ";
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

        public DtoCliente Insira(DtoClienteInsercao dto)
        {
            DtoCliente cliente = FabricaDeDtoCliente.Crie();

            cliente.Nome = dto.Nome;
            cliente.Email = dto.Email;
            cliente.Contato = dto.Contato;

            using (var conexao = FabricaDeConexao.ObtenhaConexao())
            {
                var sql = "INSERT INTO POC_CLIENTES ";
                sql += "(NOME, EMAIL, CONTATO) OUTPUT INSERTED.CODIGO ";
                sql += "VALUES (@NOME, @EMAIL, @CONTATO);";

                conexao.Open();

                using (var transacao = conexao.BeginTransaction())
                using (var comando = conexao.CreateCommand())
                {

                    comando.Transaction = transacao;

                    comando.CommandText = sql;

                    comando.AdicioneParametros(dto);

                    cliente.Codigo = (int)comando.ExecuteScalar();

                    transacao.Commit();
                }
            }
            return cliente;
        }
        #endregion

        #region Consulta
        public DtoCliente ObtenhaPorCodigo(int codigo)
        {
            DtoCliente dto = null;

            var sql = "SELECT * FROM POC_CLIENTES WHERE CODIGO = @CODIGO;";

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

        public IList<DtoCliente> ObtenhaTodos()
        {
            var sql = "SELECT * FROM POC_CLIENTES ORDER BY CODIGO;";

            DtoCliente dto = null;

            IList<DtoCliente> clientes = new List<DtoCliente>();

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

                            clientes.Add(dto);
                        }
                    }
                }
                return clientes;
            }
        }
        #endregion

        #region Métodos Auxiliares
        private static DtoCliente CarregueDto(SqlDataReader leitor)
        {
            DtoCliente dto;
            var codigo = leitor.GetInt32(leitor.GetOrdinal("CODIGO"));
            var nome = leitor.GetString(leitor.GetOrdinal("NOME"));
            var email = (leitor.GetValue(leitor.GetOrdinal("EMAIL")).GetType() == typeof(System.DBNull))
                ? null
                : leitor.GetString(leitor.GetOrdinal("EMAIL"));
            var contato = leitor.GetString(leitor.GetOrdinal("CONTATO"));

            dto = FabricaDeDtoCliente.Crie(codigo, nome, email, contato);

            return dto;
        }
        #endregion
    }
}
