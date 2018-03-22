using System;
using System.IO;
using Xunit;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Moq;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions;
using System.Linq.Expressions;

namespace FSVisitor.Tests
{
    public class FileSystemVisitorTests
    {
        private const string _entryPath = @"D:\User";

        private readonly IFileSystem _fileSystem;
        private readonly Mock<IFileSystem> _fileSystemMock;

        private readonly DirectoryInfoBase _entryDirectoryInfo;


        public FileSystemVisitorTests()
        {
            _entryDirectoryInfo = EntryDirectoryConfigurator.ConfigureEntryDirectoryInfo();

            _fileSystemMock = new Mock<IFileSystem>();
            _fileSystemMock.Setup(x => x.DirectoryInfo.FromDirectoryName(_entryPath)).Returns(() => _entryDirectoryInfo);

            _fileSystem = _fileSystemMock.Object;
        }


        [Fact]
        public void SearchDirectoryInnerEntities_ReturnAllEntities()
        {
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem);

            var entitiesCount = fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).Count();

            Assert.Equal(4, entitiesCount);
        }

        [Fact]
        public void SearchDirectoryInnerEntities_WithNameLengthMoreThat5Character_Return2Entity()
        {
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem, (FileSystemInfoBase entityInfo) => entityInfo.Name.Length > 5);

            var entitiesCount = fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).Count();

            Assert.Equal(2, entitiesCount);
        }


        [Fact]
        public void SearchDirectoryInnerEntities_DirectoryFilter_ReturnDirectories()
        {
            var expected = new[] { "Films", "Music" };
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem, (FileSystemInfoBase entityInfo) => entityInfo is DirectoryInfoBase);

            var directories = fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToArray();

            Assert.NotEmpty(directories);
            Assert.Equal(expected.Length, directories.Count());
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], directories[i].Name);
        }

        [Fact]
        public void SearchDirectoryInnerEntities_FileFilter_ReturnFiles()
        {
            var expected = new[] { "schedule", "Rick & Morty"  };
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem, (FileSystemInfoBase entityInfo) => entityInfo is FileInfoBase);

            var directories = fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToArray();

            Assert.NotEmpty(directories);
            Assert.Equal(expected.Length, directories.Count());
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], directories[i].Name);
        }


        [Fact]
        public void SearchDirectoryInnerEntities_StartEventInvocationTest()
        {
            List<string> invokedEvents = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem);
            fileSystemVisitor.Start += (object o, SearchProgressArgs args) => invokedEvents.Add(args.Message);
            
           
            fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToList();

            Assert.NotEmpty(invokedEvents);
            Assert.Single(invokedEvents);
            Assert.Equal("-> Start <-", invokedEvents.FirstOrDefault());
        }


        [Fact]
        public void SearchDirectoryInnerEntities_FinishEventInvocationTest()
        {
            
            List<string> invokedEvents = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem);
            fileSystemVisitor.Finish += (object o, SearchProgressArgs args) => invokedEvents.Add(args.Message);


            fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToList();

            Assert.NotEmpty(invokedEvents);
            Assert.Single(invokedEvents);
            Assert.Equal("-> Finish <-", invokedEvents.FirstOrDefault());
        }

        [Fact]
        public void SearchDirectoryInnerEntities_DirectoryFoundEventInvocationTest()
        {
            var expectedInvocationAmount = 2;
            List<string> invokedEvents = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem);
            fileSystemVisitor.DirectoryFound += (object o, EntityFoundArgs args) => invokedEvents.Add(args.Message);


            fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToList();

            Assert.NotEmpty(invokedEvents);
            Assert.Equal(expectedInvocationAmount, invokedEvents.Count());
            Assert.Equal("Directory found", invokedEvents.FirstOrDefault());
        }

        [Fact]
        public void SearchDirectoryInnerEntities_FileFoundEventInvocationTest()
        {
            var expectedInvocationAmount = 2;
            List<string> invokedEvents = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem);
            fileSystemVisitor.FileFound += (object o, EntityFoundArgs args) => invokedEvents.Add(args.Message);


            fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToList();

            Assert.NotEmpty(invokedEvents);
            Assert.Equal(expectedInvocationAmount, invokedEvents.Count());
            Assert.Equal("File found", invokedEvents.FirstOrDefault());
        }

        [Fact]
        public void SearchDirectoryInnerEntities_FilteredFileFoundEventInvocationTest()
        {
            var expectedInvocationAmount = 1;
            List<string> invokedEvents = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem, (FileSystemInfoBase info) => info is FileInfoBase && info.Extension.Equals("mp4"));
            fileSystemVisitor.FilterFileFound += (object o, EntityFoundArgs args) => invokedEvents.Add(args.Message);


            var files = fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToArray();

            Assert.NotEmpty(invokedEvents);
            Assert.Equal(expectedInvocationAmount, invokedEvents.Count());
            Assert.Equal("Filtered file found", invokedEvents.FirstOrDefault());
            Assert.Equal("Rick & Morty", files[0].Name);
        }

        [Fact]
        public void SearchDirectoryInnerEntities_FilteredDirectoryFoundEventInvocationTest()
        {
            var expectedInvocationAmount = 1;
            List<string> invokedEvents = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem, (FileSystemInfoBase info) => info.Name.StartsWith("F"));
            fileSystemVisitor.FilterDirectoryFound += (object o, EntityFoundArgs args) => invokedEvents.Add(args.Message);


            var directories =fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToArray();

            Assert.NotEmpty(invokedEvents);
            Assert.Equal(expectedInvocationAmount, invokedEvents.Count());
            Assert.Equal("Filtered directory found", invokedEvents.FirstOrDefault());
            Assert.Equal("Films", directories[0].Name);
        }

        [Fact]
        public void SearchDirectoryInnerEntities_CancellationEventTest()
        {
            var expectedEntitiesAmount = 3;
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem);
            fileSystemVisitor.DirectoryFound += OnEntityFound_CancellWhileNameFound;
            fileSystemVisitor.FileFound += OnEntityFound_CancellWhileNameFound;

            var entities = fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToArray();

            Assert.NotEmpty(entities);
            Assert.Equal(expectedEntitiesAmount, entities.Length);
            Assert.True(!entities.Any(x => x.Name.Equals("Rick & Morty")));
            
            void OnEntityFound_CancellWhileNameFound(object o, EntityFoundArgs args)
            {
                if (args.EntityInfo.Name.Equals("schedule"))
                    args.IsCancelled = true;
            }
        }

        [Fact]
        public void SearchDirectoryInnerEntities_ExcludeEntitiesWithReadOnlyAttribute_ExcludeEventTest()
        {
            var expectedEntitiesAmount = 3;
            var fileSystemVisitor = new FileSystemVisitor(_fileSystem);
            fileSystemVisitor.FileFound += OnEntityFound_ExcludeReadonlyFiles;
            fileSystemVisitor.DirectoryFound += OnEntityFound_ExcludeReadonlyFiles;

            var entities = fileSystemVisitor.SearchDirectoryInnerEntities(_entryPath).ToArray();

            Assert.NotEmpty(entities);
            Assert.Equal(expectedEntitiesAmount, entities.Count());
            Assert.True(!entities.Any(x => x.Attributes.Equals(FileAttributes.ReadOnly)));

            void OnEntityFound_ExcludeReadonlyFiles(object o, EntityFoundArgs args){
                if (args.EntityInfo is FileInfoBase && args.EntityInfo.Attributes.Equals(FileAttributes.ReadOnly))
                    args.IsExcluded = true;
            }
        }

    }
}





