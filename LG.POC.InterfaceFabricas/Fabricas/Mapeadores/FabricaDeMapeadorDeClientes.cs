using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;

namespace LG.POC.InterfaceFabricas.Fabricas.Mapeadores
{
    public static class FabricaDeMapeadorDeClientes
    {
        public static IMapeadorDeClientes Crie()
        {
            return FabricaGenerica.Crie<IMapeadorDeClientes>();
        }
    }
}
