using System.IO;
using System.IO.Abstractions;

namespace FSVisitor
{
    public class EntityFoundArgs
    {
        public FileSystemInfoBase EntityInfo { get; set; }

        public string Message { get; set; }

        public bool IsCancelled { get; set; } = false;

        public bool IsExcluded { get; set; } = false;
    }
}