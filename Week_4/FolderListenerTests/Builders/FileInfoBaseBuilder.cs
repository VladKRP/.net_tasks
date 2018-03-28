using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListenerTests.Builders
{
    public class FileInfoBaseBuilder
    {
        private Mock<FileInfoBase> _fileinfoMock;

        public FileInfoBaseBuilder()
        {
            _fileinfoMock = new Mock<FileInfoBase>();
        }

        public FileInfoBaseBuilder SetName(string name) {
            _fileinfoMock.Setup(x => x.Name).Returns(name);
            return this;
        }

        public FileInfoBaseBuilder SetFullName(string fullName)
        {
            _fileinfoMock.Setup(x => x.FullName).Returns(fullName);
            return this;
        }

        public FileInfoBase Build()
        {
            _fileinfoMock.Setup(x => x.Extension).Returns(Path.GetExtension(_fileinfoMock.Object.Name));
            return _fileinfoMock.Object;
        }
    }
}
