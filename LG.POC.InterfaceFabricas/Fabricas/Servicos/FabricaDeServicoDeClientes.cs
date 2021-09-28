using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;

namespace LG.POC.InterfaceFabricas.Fabricas.Servicos
{
    public static class FabricaDeServicoDeClientes
    {
        public static ServicoDeClientes Crie()
        {
            return FabricaGenerica.Crie<ServicoDeClientes>();
        }
    }
}
