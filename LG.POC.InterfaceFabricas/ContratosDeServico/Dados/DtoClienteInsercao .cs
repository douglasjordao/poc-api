namespace LG.POC.InterfaceFabricas.ContratosDeServico.Dados
{
    public class DtoClienteInsercao
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Contato { get; set; }

        public DtoClienteInsercao()
        {
        }

        public DtoClienteInsercao(string nome, string email, string contato)
        {
            Nome = nome;
            Email = email;
            Contato = contato;
        }
    }
}
