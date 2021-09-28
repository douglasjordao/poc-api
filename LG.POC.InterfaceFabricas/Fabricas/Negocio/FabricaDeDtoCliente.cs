using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;

namespace LG.POC.InterfaceFabricas.Fabricas.Negocio
{
    public static class FabricaDeDtoCliente
    {
        public static DtoCliente Crie()
        {
            return FabricaGenerica.Crie<DtoCliente>();
        }

        public static DtoCliente Crie(int codigo, string nome, string email, string contato)
        {
            return FabricaGenerica.Crie<DtoCliente>(codigo, nome, email, contato);
        }
    }
}
