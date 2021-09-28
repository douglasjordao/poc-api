using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;

namespace LG.POC.InterfaceFabricas.Fabricas.Mapeadores
{
    public static class FabricaDeMapeadorDePedidos
    {
        public static IMapeadorDePedidos Crie()
        {
            return FabricaGenerica.Crie<IMapeadorDePedidos>();
        }
    }
}
