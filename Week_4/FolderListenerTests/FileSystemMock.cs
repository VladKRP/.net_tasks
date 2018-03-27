using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListenerTests
{
    public class FileSystemMock
    {

        public void CreateMockEntities()
        { 
            var documentFolder = CreatedDirectoryInfoMock(new DirectotySetupInfo()
            {
                Name = "documents",
                FullName = @"C:\Downloads\documents"
            });

            var presentationFolder = CreatedDirectoryInfoMock(new DirectotySetupInfo()
            {
                Name = "presentations",
                FullName = @"C:\Downloads\presentations"
            });

            var defaultFolder = CreatedDirectoryInfoMock(new DirectotySetupInfo()
            {
                Name = "default",
                FullName = @"C:\Downloads\default"
            });

            var documentFile = CreateFileInfoMock(new FileSetupInfo()
            {
                Name = "AnnualVasts.docx",
                Extension = "docx",
                Directory = documentFolder.Object,
            });

            var textFile = CreateFileInfoMock(new FileSetupInfo()
            {
                Name = "ToReadList.txt",
                Extension = "txt",
                Directory = documentFolder.Object,
            });

            var presentationFile = CreateFileInfoMock(new FileSetupInfo()
            {
                Name = "DiplomaPresentation.pptx",
                Extension = "pptx",
                Directory = presentationFolder.Object,
            });

            var presentationFileCopy = CreateFileInfoMock(new FileSetupInfo()
            {
                Name = "DiplomaPresentation (2).pptx",
                Extension = "pptx",
                Directory = presentationFolder.Object,
            });

        }

        private static Mock<FileInfoBase> CreateFileInfoMock(FileSetupInfo fileSetupInfo)
        {
            var fileInfoMock = new Mock<FileInfoBase>();
            fileInfoMock.Setup(x => x.Name).Returns(fileSetupInfo.Name);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.Directory).Returns(fileSetupInfo.Directory);
            fileInfoMock.Setup(x => x.Extension).Returns(fileSetupInfo.Extension);
            return fileInfoMock;
        }

        private static Mock<DirectoryInfoBase> CreatedDirectoryInfoMock(DirectotySetupInfo directorySetupInfo)
        {
            var directoryInfoMock = new Mock<DirectoryInfoBase>();
            directoryInfoMock.Setup(x => x.Name).Returns(directorySetupInfo.Name);
            directoryInfoMock.Setup(x => x.FullName).Returns(directorySetupInfo.FullName);
            directoryInfoMock.Setup(x => x.Exists).Returns(true);
            return directoryInfoMock;
        }

    }

    class FileSetupInfo
    {
        public string Name { get; set; }

        public DirectoryInfoBase Directory { get; set; }

        public string Extension { get; set; }
    }

    class DirectotySetupInfo
    {

        public string Name { get; set; }

        public string FullName { get; set; }

    }
}
