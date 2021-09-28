using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;

namespace LG.POC.InterfaceFabricas.Fabricas.Mapeadores
{
    public static class FabricaDeMapeadorDeProdutos
    {
        public static IMapeadorDeProdutos Crie()
        {
            return FabricaGenerica.Crie<IMapeadorDeProdutos>();
        }
    }
}
