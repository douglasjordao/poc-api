using System;
using System.Runtime.Serialization;

namespace LG.POC.InterfaceFabricas.Excecao
{
    [Serializable]
    public class ExcecaoDadosInvalidos : Exception
    {
        public ExcecaoDadosInvalidos()
        {
        }

        public ExcecaoDadosInvalidos(string message) : base(message)
        {
        }

        public ExcecaoDadosInvalidos(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExcecaoDadosInvalidos(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
