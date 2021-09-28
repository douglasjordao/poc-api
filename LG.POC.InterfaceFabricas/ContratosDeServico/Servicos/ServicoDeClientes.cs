using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using System.Collections.Generic;

namespace LG.POC.InterfaceFabricas.ContratosDeServico.Servicos
{
    public interface ServicoDeClientes
    {
        DtoCliente Insira(DtoClienteInsercao dto);
        IList<DtoCliente> ObtenhaTodos();
        DtoCliente ObtenhaPorCodigo(int codigo);
        void Atualize(DtoCliente dto);
        void Exclua(int codigo);
        void ExcluaVarios(int[] codigos);
    }
}
