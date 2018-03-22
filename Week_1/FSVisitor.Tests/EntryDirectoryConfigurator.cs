using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace FSVisitor.Tests
{
    class EntryDirectoryConfigurator
    {
        public static DirectoryInfoBase ConfigureEntryDirectoryInfo()
        {

            var entryDirectory = CreateMockedDirectoryInfo(new DirectotySetupInfo()
            {
                Name = "User",
                FullName = @"D:\User"
            });

            var filmDirectory = CreateMockedDirectoryInfo(new DirectotySetupInfo()
            {
                Name = "Films",
                FullName = @"D:\User\Films",
                Parent = entryDirectory.Object
            });

            var musicDirectory = CreateMockedDirectoryInfo(new DirectotySetupInfo()
            {
                Name = "Music",
                FullName = @"D:\User\Music",
                Parent = entryDirectory.Object
            });

            var filmFile = CreateMockedFileInfo(new FileSetupInfo()
            {
                Name = "Rick & Morty",
                Extension = "mp4",
                Directory = filmDirectory.Object,
                FileAttributes = FileAttributes.Normal
            });

            var textFile = CreateMockedFileInfo(new FileSetupInfo()
            {
                Name = "schedule",
                Extension = "txt",
                Directory = entryDirectory.Object,
                FileAttributes = FileAttributes.ReadOnly
            });

            filmDirectory.Setup(x => x.GetFileSystemInfos()).Returns(() => new List<FileSystemInfoBase>() { filmFile.Object }.ToArray());
            filmDirectory.Setup(x => x.GetDirectories()).Returns(() => new List<DirectoryInfoBase>() { }.ToArray());

            musicDirectory.Setup(x => x.GetDirectories()).Returns(() => new List<DirectoryInfoBase>() { }.ToArray());

            entryDirectory.Setup(x => x.GetFileSystemInfos()).Returns(() => new List<FileSystemInfoBase>() {
                filmDirectory.Object,
                musicDirectory.Object,
                textFile.Object
            }.ToArray());
            entryDirectory.Setup(x => x.GetDirectories()).Returns(() => new List<DirectoryInfoBase>() { filmDirectory.Object, musicDirectory.Object }.ToArray());

            return entryDirectory.Object;
        }

        private static Mock<FileInfoBase> CreateMockedFileInfo(FileSetupInfo fileSetupInfo)
        {
            var fileInfoMock = new Mock<FileInfoBase>();
            fileInfoMock.Setup(x => x.Name).Returns(fileSetupInfo.Name);
            fileInfoMock.Setup(x => x.FullName).Returns(fileSetupInfo.FullName);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.Directory).Returns(fileSetupInfo.Directory);
            fileInfoMock.Setup(x => x.Extension).Returns(fileSetupInfo.Extension);
            fileInfoMock.Setup(x => x.Attributes).Returns(fileSetupInfo.FileAttributes);
            return fileInfoMock;
        }

        private static Mock<DirectoryInfoBase> CreateMockedDirectoryInfo(DirectotySetupInfo directorySetupInfo)
        {
            var directoryInfoMock = new Mock<DirectoryInfoBase>();
            directoryInfoMock.Setup(x => x.Name).Returns(directorySetupInfo.Name);
            directoryInfoMock.Setup(x => x.FullName).Returns(directorySetupInfo.FullName);
            directoryInfoMock.Setup(x => x.Exists).Returns(true);
            directoryInfoMock.Setup(x => x.Parent).Returns(directorySetupInfo.Parent);
            directoryInfoMock.Setup(x => x.Root).Returns(directorySetupInfo.Root);
            return directoryInfoMock;
        }


        class FileSetupInfo
        {
            public string Name { get; set; }

            public string FullName { get; set; }

            public DirectoryInfoBase Directory { get; set; }

            public string Extension { get; set; }

            public FileAttributes FileAttributes { get; set; }
        }

        class DirectotySetupInfo
        {

            public string Name { get; set; }

            public string FullName { get; set; }

            public DirectoryInfoBase Parent { get; set; }

            public DirectoryInfoBase Root { get; }
        }
    }
}


