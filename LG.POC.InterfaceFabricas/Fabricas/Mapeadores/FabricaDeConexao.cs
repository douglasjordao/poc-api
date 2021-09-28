using System.Data.SqlClient;

namespace LG.POC.InterfaceFabricas.Fabricas.Mapeadores
{
    public static class FabricaDeConexao
    {
        public static SqlConnection ObtenhaConexao()
        {
            const string strConn = @"Server=pc-1642\SQLEXPRESS;Database=POC_DB; Uid=sa; Pwd=fpw;";

            var conexao = new SqlConnection(strConn);

            return conexao;
        }
    }
}
