using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;
using System.Collections.Generic;
using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;
using LG.POC.InterfaceFabricas.Fabricas.Mapeadores;
using LG.POC.ServicoMapeadores.Validacoes;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Utilidades.Excecao;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;

namespace LG.POC.ServicoMapeadores.Servicos.ContratosImplementados
{
    public class ServicoDeProdutosImpl : ServicoDeProdutos
    {

        #region Propriedades

        public IMapeadorDeProdutos Mapeador { get; set; }
        #endregion

        #region Construtor

        public ServicoDeProdutosImpl()
        {
            Mapeador = FabricaDeMapeadorDeProdutos.Crie();
        }

        #endregion

        #region Manutenção
        public void Atualize(DtoProduto dto)
        {
            var produto = ObtenhaPorCodigo(dto.Codigo);

            if (produto == null) throw new KeyNotFoundException();

            var produtos = ObtenhaTodos();

            dto.ValideDados(produtos);

            Mapeador.Atualize(dto);
        }

        public void Exclua(int codigo)
        {
            var produto = ObtenhaPorCodigo(codigo);

            if (produto == null) throw new KeyNotFoundException();

            var servicoDePedidos = FabricaDeServicoDePedidos.Crie();

            var pedidos = servicoDePedidos.ObtenhaTodos();

            produto.ValidePendencias(pedidos);

            Mapeador.Exclua(codigo);
        }

        public void ExcluaVarios(int[] codigos)
        {
            if (codigos == null || codigos.Length == 0) throw new ExcecaoDadosInvalidos(MensagensExcecaoProduto.DADOS_NULOS);

            var produtos = new List<DtoProduto>();

            var servicoDePedidos = FabricaDeServicoDePedidos.Crie();

            var pedidos = servicoDePedidos.ObtenhaTodos();

            foreach (var codigo in codigos)
            {
                produtos.Add(ObtenhaPorCodigo(codigo));
            }

            foreach (var produto in produtos)
            {
                produto.ValidePendencias(pedidos);
            }

            Mapeador.ExcluaVarios(codigos);
        }

        public DtoProduto Insira(DtoProdutoCadastro dto)
        {
            var produtos = Mapeador.ObtenhaTodos();

            dto.ValideDados(produtos);

            return Mapeador.Insira(dto);
        }
        #endregion

        #region Consulta
        public DtoProduto ObtenhaPorCodigo(int codigo)
        {
            var produto = Mapeador.ObtenhaPorCodigo(codigo);

            if (produto == null) throw new KeyNotFoundException();

            return produto;
        }

        public IList<DtoProduto> ObtenhaTodos()
        {
            return Mapeador.ObtenhaTodos();
        }
        #endregion
    }
}
