using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;

namespace LG.POC.InterfaceFabricas.Fabricas.Servicos
{
    public static class FabricaDeServicoDeProduto
    {
        public static ServicoDeProdutos Crie()
        {
            return FabricaGenerica.Crie<ServicoDeProdutos>();
        }
    }
}
