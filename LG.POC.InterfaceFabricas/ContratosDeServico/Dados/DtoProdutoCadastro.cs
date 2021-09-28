namespace LG.POC.InterfaceFabricas.ContratosDeServico.Dados
{
    public class DtoProdutoCadastro
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }

        public DtoProdutoCadastro()
        {
        }

        public DtoProdutoCadastro(string descricao, decimal valor, int quantidade)
        {
            Descricao = descricao;
            Valor = valor;
            Quantidade = quantidade;
        }
    }
}
