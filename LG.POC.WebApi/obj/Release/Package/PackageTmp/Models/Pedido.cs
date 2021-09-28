using System;
using System.Collections.Generic;

namespace LG.POC.WebApi.Models
{
    public class Pedido
    {
        public int Codigo { get; set; }
        public int CodigoCliente { get; set; }
        public string NomeCliente { get; set; }
        public IList<Produto> Produtos { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorPedido { get; set; }
        public bool EnviarPorEmail { get; set; }
    }
}