using System.Collections.Generic;

namespace LG.POC.InterfaceFabricas.ContratosDeServico.Dados
{
    public class DtoPedido
    {
        public int Codigo { get; set; }
        public int CodigoCliente { get; set; }
        public string NomeCliente { get; set; }
        public IList<DtoProduto> Produtos { get; set; }
        public string DataPedido { get; set; }
        public decimal ValorPedido { get; set; }
        public bool EnviarPorEmail { get; set; }

        public DtoPedido()
        {
        }

        public DtoPedido(int codigo, int codigoCliente, string nomeCliente, string dataPedido, decimal valorPedido, bool enviarPorEmail)
        {
            Codigo = codigo;
            CodigoCliente = codigoCliente;
            NomeCliente = nomeCliente;
            DataPedido = dataPedido;
            ValorPedido = valorPedido;
            EnviarPorEmail = enviarPorEmail;
        }

        public DtoPedido(int codigo, int codigoCliente, string nomeCliente, IList<DtoProduto> produtos, string dataPedido, decimal valorPedido, bool enviarPorEmail)
        {
            Codigo = codigo;
            CodigoCliente = codigoCliente;
            NomeCliente = nomeCliente;
            Produtos = produtos;
            DataPedido = dataPedido;
            ValorPedido = valorPedido;
            EnviarPorEmail = enviarPorEmail;
        }
    }
}
