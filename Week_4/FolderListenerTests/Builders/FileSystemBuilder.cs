using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListenerTests.Builders
{
    public class FileSystemBuilder
    {
        private Mock<IFileSystem> _fileSystemMock;

        public FileSystemBuilder()
        {
            _fileSystemMock = new Mock<IFileSystem>();
        }

        public FileSystemBuilder AddDirectoryInfo(DirectoryInfoBase directoryInfo)
        {
            _fileSystemMock.Setup(x => x.DirectoryInfo.FromDirectoryName(directoryInfo.FullName)).Returns(() => directoryInfo);
            return this;
        }

        public FileSystemBuilder AddDirectoriesInfo(params DirectoryInfoBase[] directoryInfos)
        {
            foreach (var directoryInfo in directoryInfos)
                AddDirectoryInfo(directoryInfo);
            return this;
        }

        public IFileSystem Build()
        {
            return _fileSystemMock.Object;
        }
    }
}
