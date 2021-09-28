using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using System.Collections.Generic;

namespace LG.POC.InterfaceFabricas.Interfaces.Mapeadores
{
    public interface IMapeadorDeProdutos
    {
        DtoProduto Insira(DtoProdutoCadastro dto);
        IList<DtoProduto> ObtenhaTodos();
        DtoProduto ObtenhaPorCodigo(int codigo);
        void Atualize(DtoProduto dto);
        void Exclua(int codigo);
        void ExcluaVarios(int[] codigos);
    }
}
