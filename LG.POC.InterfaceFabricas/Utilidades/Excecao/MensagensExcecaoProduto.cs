namespace LG.POC.InterfaceFabricas.Utilidades.Excecao
{
    public static class MensagensExcecaoProduto
    {
        public const string NAO_ENCONTRADO = "Produto não encontrado na base de dados.";
        public const string DESCRICAO_OBRIGATORIA = "O campo Descrição é obrigatório.";
        public const string DESCRICAO_TAMANHO_INVALIDO = "A Descrição deve conter entre 5 e 30 caracteres.";
        public const string DESCRICAO_DUPLICADA = "Descrição já existe na base de dados.";
        public const string QUANTIDADE_INVALIDA = "Quantidade deve ser um valor positivo.";
        public const string DADOS_NULOS = "Nenhum dado enviado ao servidor.";
        public const string PRODUTO_COM_PENDENCIA = "Há um ou mais produtos selecionados com ocorrências em um ou mais pedidos";
    }
}
