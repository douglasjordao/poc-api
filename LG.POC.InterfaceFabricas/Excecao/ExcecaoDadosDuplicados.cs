using System;
using System.Runtime.Serialization;

namespace LG.POC.InterfaceFabricas.Excecao
{
    [Serializable]
    public class ExcecaoDadosDuplicados : Exception
    {
        public ExcecaoDadosDuplicados()
        {
        }

        public ExcecaoDadosDuplicados(string message) : base(message)
        {
        }

        public ExcecaoDadosDuplicados(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExcecaoDadosDuplicados(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
