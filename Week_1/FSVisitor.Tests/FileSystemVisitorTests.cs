using System;
using System.IO;
using Xunit;
using System.Linq;
using System.Diagnostics;

namespace FSVisitor.Tests
{
    public class FileSystemVisitorTests
    {

        private readonly string testFolderPath;
        private FileSystemVisitor fileSystemVisitor;

        public FileSystemVisitorTests()
        {
            testFolderPath = GenerateTestDirectoryPath();
        }

        [Fact]
        public void SearchDirectoryInnerEntities_ReturnCountOfEntities()
        {
            fileSystemVisitor = new FileSystemVisitor();
            Assert.Equal(5, fileSystemVisitor.SearchDirectoryInnerEntities(testFolderPath).Count());
        }

        [Fact]
        public void SearchDirectoryInnerEntities_ReturnCountOfEntities_WithNameLengthLessThan15()
        {
            fileSystemVisitor = new FileSystemVisitor((FileSystemInfo entityInfo) => entityInfo.Name.Length < 15);
            Assert.Equal(5, fileSystemVisitor.SearchDirectoryInnerEntities(testFolderPath).Count());
        }

        [Fact]
        public void SearchDirectoryInnerEntities_ReturnCountOfEntities_StartsFromNameF()
        {
            fileSystemVisitor = new FileSystemVisitor((FileSystemInfo entityInfo) => entityInfo.Name.StartsWith("F"));
            Assert.Equal(3, fileSystemVisitor.SearchDirectoryInnerEntities(testFolderPath).Count());
        }

        [Fact]
        public void SearchDirectoryInnerEntities_ReturnCountOfEntities_WithTypeFile()
        {
            fileSystemVisitor = new FileSystemVisitor((FileSystemInfo entityInfo) => entityInfo is FileInfo);
            Assert.Single(fileSystemVisitor.SearchDirectoryInnerEntities(testFolderPath));
        }

        [Fact]
        public void SearchDirectoryInnerEntities_ReturnCountOfEntities_WithTypeDirectory()
        {
            fileSystemVisitor = new FileSystemVisitor((FileSystemInfo entityInfo) => entityInfo is DirectoryInfo);
            Assert.Equal(3, fileSystemVisitor.SearchDirectoryInnerEntities(testFolderPath).Count());
        }

        [Theory]
        [InlineData("F1", 0)]
        [InlineData("F2",1)]
        [InlineData("F21", 2)]
        public void SearchDirectoryInnerEntities_IsDirectoryHasRightName(string expectedName, int index)
        {
            fileSystemVisitor = new FileSystemVisitor();
            Assert.Equal(expectedName, fileSystemVisitor.SearchDirectoryInnerEntities(testFolderPath).ElementAtOrDefault(index).Name);
        }


        private string GenerateTestDirectoryPath()
        {
            var projectRootPath = GetBackFromDirectory(Environment.CurrentDirectory, 4);
            return Path.Combine(projectRootPath, "TestFolder");
        }

        private string GetBackFromDirectory(string path, int depth)
        {
            for (int i = 0; i < depth; i++)
                path = Directory.GetParent(path).FullName;
            return path;
        }
    }
}
