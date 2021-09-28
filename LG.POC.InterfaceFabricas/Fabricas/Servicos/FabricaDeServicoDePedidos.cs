using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;

namespace LG.POC.InterfaceFabricas.Fabricas.Servicos
{
    public static class FabricaDeServicoDePedidos
    {
        public static ServicoDePedidos Crie()
        {
            return FabricaGenerica.Crie<ServicoDePedidos>();
        }
    }
}
