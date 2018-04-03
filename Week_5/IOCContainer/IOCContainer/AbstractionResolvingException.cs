using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public class AbstractionResolvingException : ApplicationException
    {
        public Type Type { get; private set; }

        public AbstractionResolvingException()
        {
        }

        public AbstractionResolvingException(Type type, string message):base(message)
        {
            Type = type;
        }

        public AbstractionResolvingException(string message) : base(message)
        {
        }

        public AbstractionResolvingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AbstractionResolvingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
