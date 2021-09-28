using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;

namespace LG.POC.InterfaceFabricas.Fabricas.Negocio
{
    public static class FabricaDeDtoProduto
    {
        public static DtoProduto Crie()
        {
            return FabricaGenerica.Crie<DtoProduto>();
        }

        public static DtoProduto Crie(int codigo, string descricao, decimal valor, int quantidade)
        {
            return FabricaGenerica.Crie<DtoProduto>(codigo, descricao, valor, quantidade);
        }
    }
}
