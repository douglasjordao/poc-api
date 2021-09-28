namespace LG.POC.InterfaceFabricas.ContratosDeServico.Dados
{
    public class DtoCliente
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Contato { get; set; }

        public DtoCliente()
        {
        }

        public DtoCliente(int codigo, string nome, string email, string contato)
        {
            Codigo = codigo;
            Nome = nome;
            Email = email;
            Contato = contato;
        }
    }
}
