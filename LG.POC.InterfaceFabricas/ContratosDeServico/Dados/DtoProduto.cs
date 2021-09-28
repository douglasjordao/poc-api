using System.Dynamic;

namespace LG.POC.InterfaceFabricas.ContratosDeServico.Dados
{
    public class DtoProduto
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }

        public DtoProduto()
        {
        }

        public DtoProduto(int codigo, string descricao, decimal valor, int quantidade)
        {
            Codigo = codigo;
            Descricao = descricao;
            Valor = valor;
            Quantidade = quantidade;
        }
    }
}