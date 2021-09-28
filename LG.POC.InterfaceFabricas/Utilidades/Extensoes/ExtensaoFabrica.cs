using System;

namespace LG.POC.InterfaceFabricas.Utilidades.Extensoes
{
    public static class ExtensaoFabrica
    {
        public static bool EhDto(this Type tipo)
        {
            return tipo.Name.Contains("Dto");
        }

        public static bool EhServico(this Type tipo)
        {
            return tipo.Name.Contains("Servico");
        }

        public static bool EhMapeador(this Type tipo)
        {
            return tipo.Name.Contains("Mapeador");
        }
    }
}
