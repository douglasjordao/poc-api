namespace LG.POC.InterfaceFabricas.Utilidades.Excecao
{
    public static class MensagensExcecaoPedido
    {
        public const string NAO_ENCONTRADO = "Pedido não encontrado na base de dados.";
        public const string QUANTIDADE_DE_VENDA_INVALIDA = "Quantidade de venda maior que a quantidade de estoque.";
        public const string QUANTIDADE_INVALIDA = "Quantidade do produto precisa ser um valor maior que zero.";
        public const string PRODUTO_OBRIGATORIO = "É necessário no mínimo 1 produto para incluir um pedido";
        public const string CLIENTE_SEM_EMAIL = "Não é possível enviar pedido por email. O Cliente informado não possui email cadastrado.";
        public const string DADOS_NULOS = "Nenhum dado enviado ao servidor.";
    }
}
