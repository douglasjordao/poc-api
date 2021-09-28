namespace LG.POC.InterfaceFabricas.ContratosDeServico.Dados
{
    public class DtoProdutoInsercao
    {
        public int Codigo { get; set; }
        public int Quantidade { get; set; }

        public DtoProdutoInsercao()
        {
        }

        public DtoProdutoInsercao(int codigo, int quantidade)
        {
            Codigo = codigo;
            Quantidade = quantidade;
        }
    }
}
