using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using System.Collections.Generic;

namespace LG.POC.InterfaceFabricas.Fabricas.Negocio
{
    public static class FabricaDeDtoPedido
    {
        public static DtoPedido Crie()
        {
            return FabricaGenerica.Crie<DtoPedido>();
        }

        public static DtoPedido Crie(int codigo, int codigoCliente, string nomeCliente, string data, decimal valorPedido, bool enviarPorEmail)
        {
            return FabricaGenerica.Crie<DtoPedido>(codigo, codigoCliente, nomeCliente, data, valorPedido, enviarPorEmail);
        }

        public static DtoPedido Crie(int codigo, int codigoCliente, string nomeCliente, IList<DtoProduto> produtos, string data, decimal valorPedido, bool enviarPorEmail)
        {
            return FabricaGenerica.Crie<DtoPedido>(codigo, codigoCliente, nomeCliente, produtos, data, valorPedido, enviarPorEmail);
        }
    }
}
