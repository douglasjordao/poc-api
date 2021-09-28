using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using System.Collections.Generic;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Enumeradores;
using LG.POC.InterfaceFabricas.Utilidades.Extensoes;
using LG.POC.InterfaceFabricas.Utilidades.Excecao;
using LG.POC.InterfaceFabricas.Fabricas.Negocio;

namespace LG.POC.ServicoMapeadores.Validacoes
{
    public static class ValidacaoProduto
    {
        public static void ValideDados<T>(this T dto, IList<DtoProduto> lista) where T : class
        {
            if (dto == null) throw new ExcecaoDadosInvalidos(MensagensExcecaoProduto.DADOS_NULOS);
            ValideDescricao(dto, lista);
            ValideValor(dto);
            ValideQuantidade(dto);
        }

        public static void ValidePendencias(this DtoProduto dto, IList<DtoPedido> pedidos)
        {
            var codigo = dto.Codigo;

            var codigosDeProdutos = new List<int>();

            foreach (var pedido in pedidos)
            {
                foreach (var produto in pedido.Produtos)
                {
                    codigosDeProdutos.Add(produto.Codigo);
                }
            }

            if (codigosDeProdutos.Contains(codigo))
            {
                throw new ExcecaoDadosInvalidos(MensagensExcecaoProduto.PRODUTO_COM_PENDENCIA);
            }
        }

        #region Métodos de Validação

        private static void ValideDescricao<T>(T dto, IList<DtoProduto> lista) where T : class
        {
            string descricao = (string)dto.GetType().GetProperty("Descricao").GetValue(dto);

            if (descricao.EhVazioOuNulo()) throw new ExcecaoDadosInvalidos(MensagensExcecaoProduto.DESCRICAO_OBRIGATORIA);

            descricao = descricao.RemovaEspacosDesnecessarios();

            if (descricao.Length < 5 || descricao.Length > 30) throw new ExcecaoDadosInvalidos(MensagensExcecaoProduto.DESCRICAO_TAMANHO_INVALIDO);

            if (dto.GetType().GetProperty("Codigo") != null) RemoveDtoDaLista(dto, lista);

            if (descricao.JaExiste(lista, EnumPropriedadeDto.Descricao)) throw new ExcecaoDadosDuplicados(MensagensExcecaoProduto.DESCRICAO_DUPLICADA);

            dto.GetType().GetProperty("Descricao").SetValue(dto, descricao);
        }

        private static void ValideValor<T>(T dto) where T : class
        {
            decimal valor = (decimal)dto.GetType().GetProperty("Valor").GetValue(dto);

            if (valor <= 0) dto.GetType().GetProperty("Valor").SetValue(dto, 0.01m);
        }

        private static void ValideQuantidade<T>(T dto)
        {
            int quantidade = (int)dto.GetType().GetProperty("Quantidade").GetValue(dto);

            if (quantidade < 0) throw new ExcecaoDadosInvalidos(MensagensExcecaoProduto.QUANTIDADE_INVALIDA);
        }

        #endregion

        #region Métodos Auxiliares

        private static void RemoveDtoDaLista<T>(T dto, IList<DtoProduto> lista) where T : class
        {
            int codigo = (int)dto.GetType().GetProperty("Codigo").GetValue(dto);

            var produto = FabricaDeDtoProduto.Crie();

            foreach (var item in lista)
            {
                if (codigo == item.Codigo)
                {
                    produto = item;
                }
            }

            lista.Remove(produto);
        }
        #endregion
    }
}
