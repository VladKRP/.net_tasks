using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListenerTests.Builders
{
    public class DirectoryInfoBaseBuilder
    {

        private Mock<DirectoryInfoBase> _directoryInfoMock;

        public DirectoryInfoBaseBuilder()
        {
            _directoryInfoMock = new Mock<DirectoryInfoBase>();
        }

        public DirectoryInfoBaseBuilder SetName(string name)
        {
            _directoryInfoMock.Setup(x => x.Name).Returns(name);
            return this;
        }

        public DirectoryInfoBaseBuilder SetFullName(string fullname)
        {
            _directoryInfoMock.Setup(x => x.FullName).Returns(fullname);
            return this;
        }

        public DirectoryInfoBaseBuilder SetInnerFiles(IEnumerable<FileInfoBase> files)
        {
            _directoryInfoMock.Setup(x => x.GetFiles()).Returns(files.ToArray());
            return this;
        }

        public DirectoryInfoBase Build()
        {
            _directoryInfoMock.Setup(x => x.Exists).Returns(true);
            return _directoryInfoMock.Object;
        }



    }
}
