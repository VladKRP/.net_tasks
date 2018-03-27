using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener
{
    public class FolderListener
    {
        private readonly IFileSystem _fileSystem;

        private readonly IEnumerable<FileSystemWatcherBase> _fileSystemWatchers;

        public FolderListener(IEnumerable<FileSystemWatcherBase> fileSystemWatchers, IFileSystem fileSystem = null)
        {
            _fileSystemWatchers = fileSystemWatchers;
            _fileSystem = fileSystem ?? new FileSystem();
        }

    }
}
