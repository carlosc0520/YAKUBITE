using System.Runtime.Serialization;

namespace YKT.CORE.Exceptions
{
    public class XMLHttpRequestException : Exception
    {
        public XMLHttpRequestException()
        {
        }

        public XMLHttpRequestException(string message) : base(message)
        {
        }

        public XMLHttpRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XMLHttpRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
