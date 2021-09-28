using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;
using LG.POC.InterfaceFabricas.Utilidades.Excecao;
using LG.POC.InterfaceFabricas.Utilidades.Extensoes;
using System.Collections.Generic;

namespace LG.POC.ServicoMapeadores.Validacoes
{
    public static class ValidacaoPedido
    {
        public static void ValideDados<T>(this T dto) where T : class
        {
            if (dto == null) throw new ExcecaoDadosInvalidos(MensagensExcecaoPedido.DADOS_NULOS);
            ValideCodigoCliente(dto);
            ValideProdutos(dto);
            ValideEnviarPorEmail(dto);
        }

        #region Métodos De Validação

        private static void ValideCodigoCliente<T>(T dto) where T : class
        {
            var clientes = FabricaDeServicoDeClientes.Crie().ObtenhaTodos();

            IList<int> codigos = new List<int>();

            int codigoCliente = (int)dto.GetType().GetProperty("CodigoCliente").GetValue(dto);

            foreach (var cliente in clientes)
            {
                codigos.Add(cliente.Codigo);
            }

            if (!codigos.Contains(codigoCliente)) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.NAO_ENCONTRADO);
        }

        private static void ValideProdutos<T>(T dto) where T : class
        {
            IList<DtoProdutoInsercao> produtos = (IList<DtoProdutoInsercao>)dto.GetType().GetProperty("Produtos").GetValue(dto);

            IList<DtoProduto> produtosCadastrados = FabricaDeServicoDeProduto.Crie().ObtenhaTodos();

            if (produtos == null || produtos.Count == 0) throw new ExcecaoDadosInvalidos(MensagensExcecaoPedido.PRODUTO_OBRIGATORIO);

            IList<int> codigosProdutos = new List<int>();

            foreach (var produto in FabricaDeServicoDeProduto.Crie().ObtenhaTodos())
            {
                codigosProdutos.Add(produto.Codigo);
            }

            foreach (var produto in produtos)
            {
                if (!codigosProdutos.Contains(produto.Codigo)) throw new ExcecaoDadosInvalidos(MensagensExcecaoProduto.NAO_ENCONTRADO);

                if (produto.Quantidade <= 0) throw new ExcecaoDadosInvalidos(MensagensExcecaoPedido.QUANTIDADE_INVALIDA);

                foreach (var produtoCadastrado in produtosCadastrados)
                {
                    if (produto.Codigo == produtoCadastrado.Codigo)
                    {
                        if ((produtoCadastrado.Quantidade - produto.Quantidade) < 0)
                        {
                            throw new ExcecaoDadosInvalidos(MensagensExcecaoPedido.QUANTIDADE_DE_VENDA_INVALIDA + $" Produto: {produtoCadastrado.Codigo} - {produtoCadastrado.Descricao}");
                        }
                    }
                }
            }
        }

        private static void ValideEnviarPorEmail<T>(T dto) where T : class
        {
            bool enviarPorEmail = (bool)dto.GetType().GetProperty("EnviarPorEmail").GetValue(dto);

            if (enviarPorEmail)
            {
                var cliente = FabricaDeServicoDeClientes.Crie().ObtenhaPorCodigo((int)dto.GetType().GetProperty("CodigoCliente").GetValue(dto));

                if (cliente.Email.EhVazioOuNulo()) throw new ExcecaoDadosInvalidos(MensagensExcecaoPedido.CLIENTE_SEM_EMAIL);
            }
        }
        #endregion
    }
}
