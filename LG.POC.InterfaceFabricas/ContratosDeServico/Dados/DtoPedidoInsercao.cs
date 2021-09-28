using System.Collections.Generic;

namespace LG.POC.InterfaceFabricas.ContratosDeServico.Dados
{
    public class DtoPedidoInsercao
    {
        public int CodigoCliente { get; set; }
        public IList<DtoProdutoInsercao> Produtos { get; set; }
        public bool EnviarPorEmail { get; set; }

        public DtoPedidoInsercao()
        {
        }

        public DtoPedidoInsercao(int codigoCliente, bool enviarPorEmail)
        {
            CodigoCliente = codigoCliente;
            EnviarPorEmail = enviarPorEmail;
        }

        public DtoPedidoInsercao(int codigoCliente, IList<DtoProdutoInsercao> produtos, bool enviarPorEmail)
        {
            CodigoCliente = codigoCliente;
            Produtos = produtos;
            EnviarPorEmail = enviarPorEmail;
        }
    }
}
