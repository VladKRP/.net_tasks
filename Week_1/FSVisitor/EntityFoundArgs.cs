using System.IO;

namespace FSVisitor
{
    public class EntityFoundArgs
    {
        public FileSystemInfo EntityInfo { get; set; }

        public string Message { get; set; }

        public bool IsCancelled { get; set; } = false;
    }
}