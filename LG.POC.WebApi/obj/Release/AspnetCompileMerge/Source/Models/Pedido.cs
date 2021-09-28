using System.Collections.Generic;

namespace LG.POC.WebApi.Models
{
    public class Pedido
    {
        public int Codigo { get; set; }
        public Cliente Cliente { get; set; }
        public IList<Produto> Produtos { get; set; }
        public bool EnviarPorEmail { get; set; }
    }
}