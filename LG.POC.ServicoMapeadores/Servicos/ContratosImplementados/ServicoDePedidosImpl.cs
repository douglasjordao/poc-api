using System.Collections.Generic;
using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;
using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;
using LG.POC.InterfaceFabricas.Fabricas.Mapeadores;
using LG.POC.ServicoMapeadores.Validacoes;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Utilidades.Excecao;

namespace LG.POC.ServicoMapeadores.Servicos.ContratosImplementados
{
    public class ServicoDePedidosImpl : ServicoDePedidos
    {
        #region Propriedades

        public IMapeadorDePedidos Mapeador { get; set; }
        #endregion

        #region Construtor

        public ServicoDePedidosImpl()
        {
            Mapeador = FabricaDeMapeadorDePedidos.Crie();
        }
        #endregion

        #region Manutenção

        public void Exclua(int codigo)
        {
            var pedido = ObtenhaPorCodigo(codigo);

            if (pedido == null) throw new KeyNotFoundException();

            Mapeador.Exclua(codigo);

            AumenteQuantidadeDeProdutos(pedido);
        }

        public void ExcluaVarios(int[] codigos)
        {
            if (codigos == null || codigos.Length == 0) throw new ExcecaoDadosInvalidos(MensagensExcecaoPedido.DADOS_NULOS);

            var pedidos = new List<DtoPedido>();

            foreach (var codigo in codigos)
            {
                var pedido = Mapeador.ObtenhaPorCodigo(codigo);

                if (pedido != null)
                {
                    pedidos.Add(pedido);
                }
            }

            Mapeador.ExcluaVarios(codigos);

            foreach (var pedido in pedidos)
            {
                AumenteQuantidadeDeProdutos(pedido);
            }
        }

        public DtoPedido Insira(DtoPedidoInsercao dto)
        {
            dto.ValideDados();

            var pedido = Mapeador.Insira(dto);

            DiminuaQuantidadeDeProdutos(dto);

            return pedido;
        }
        #endregion

        #region Consulta

        public DtoPedido ObtenhaPorCodigo(int codigo)
        {
            var pedido = Mapeador.ObtenhaPorCodigo(codigo);

            if (pedido == null) throw new KeyNotFoundException();

            return pedido;
        }

        public IList<DtoPedido> ObtenhaTodos()
        {
            return Mapeador.ObtenhaTodos();
        }
        #endregion

        #region Métodos Auxiliares

        private static void AumenteQuantidadeDeProdutos(DtoPedido pedido)
        {
            var servico = FabricaDeServicoDeProduto.Crie();

            foreach (var produto in pedido.Produtos)
            {
                var produtoCadastrado = servico.ObtenhaPorCodigo(produto.Codigo);

                produtoCadastrado.Quantidade += produto.Quantidade;

                servico.Atualize(produtoCadastrado);
            }
        }

        private static void DiminuaQuantidadeDeProdutos(DtoPedidoInsercao dto)
        {
            var servico = FabricaDeServicoDeProduto.Crie();

            foreach (var produto in dto.Produtos)
            {
                var produtoCadastrado = servico.ObtenhaPorCodigo(produto.Codigo);

                produtoCadastrado.Quantidade -= produto.Quantidade;

                servico.Atualize(produtoCadastrado);
            }
        }
        #endregion
    }
}
