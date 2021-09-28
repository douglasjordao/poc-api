namespace LG.POC.InterfaceFabricas.Utilidades.Excecao
{
    public static class MensagensExcecaoCliente
    {
        public const string NAO_ENCONTRADO = "Cliente não encontrado na base de dados.";
        public const string NOME_OBRIGATORIO = "O campo Nome é obrigatório.";
        public const string NOME_CARACTERES_INVALIDOS = "O campo Nome não pode conter números ou caracteres especiais.";
        public const string NOME_DUAS_PALAVRAS = "O campo Nome deve conter pelo menos duas palavras.";
        public const string NOME_TAMANHO_INVALIDO = "O campo Nome deve ter entre 10 e 60 caracteres.";
        public const string EMAIL_INVALIDO = "E-mail inválido.";
        public const string EMAIL_DUPLICADO = "E-mail já cadastrado na base de dados.";
        public const string CONTATO_OBRIGATORIO = "O campo Contato é obrigatório.";
        public const string CONTATO_INVALIDO = "Contato inválido! O contato deve ser um número de telefone celular ou fixo válido (ddd+número) ex: (62998765432) ou (6239876543).";
        public const string CONTATO_DUPLICADO = "Contato já cadastrado na base de dados.";
        public const string DADOS_NULOS = "Nenhum dado enviado ao servidor.";
        public const string CLIENTE_COM_PENDENCIA = "Há um ou mais clientes selecionados com ocorrência à um ou mais pedidos.";
    }
}
