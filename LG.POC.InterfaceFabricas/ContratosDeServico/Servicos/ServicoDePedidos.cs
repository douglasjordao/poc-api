using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using System.Collections.Generic;

namespace LG.POC.InterfaceFabricas.ContratosDeServico.Servicos
{
    public interface ServicoDePedidos
    {
        DtoPedido Insira(DtoPedidoInsercao dto);
        IList<DtoPedido> ObtenhaTodos();
        DtoPedido ObtenhaPorCodigo(int codigo);
        void Exclua(int codigo);
        void ExcluaVarios(int[] codigos);
    }
}
