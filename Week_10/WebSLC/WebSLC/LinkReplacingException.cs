using System;
using System.Runtime.Serialization;

namespace WebSLC
{
    [Serializable]
    internal class LinkReplacingException : Exception
    {
        public LinkReplacingException()
        {
        }

        public LinkReplacingException(string message) : base(message)
        {
        }

        public LinkReplacingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LinkReplacingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}