using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener
{
    [Serializable]
    public class FolderListenerException : ApplicationException
    {
        public string Description { get;  }

        public string ResolveCases { get; }

        public FolderListenerException()
        {
        }

        public FolderListenerException(string message, string description, string resolveCases = null):base(message)
        {
            Description = description;
            ResolveCases = resolveCases;
        }

        public FolderListenerException(string message) : base(message)
        {
        }

        public FolderListenerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FolderListenerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
