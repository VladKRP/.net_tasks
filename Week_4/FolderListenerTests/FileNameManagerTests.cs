using System;
using System.Diagnostics;
using System.IO.Abstractions;
using FolderListener;
using FolderListener.Configurations;
using FolderListenerTests.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FolderListenerTests
{
    [TestClass]
    public class FileNameManagerTests
    {
        private readonly IFileSystem _fileSystem;
        private readonly FileNameManager _fileNameManager;

        private readonly FolderElement defaultsFolder = new FolderElement() { Path = @"C:\Downloads\default" };

        public FileNameManagerTests()
        {
            _fileSystem = new FileSystemBuilder().AddDirectoryInfo(DummyDirectories.defaultsFolder)
                                                 .AddDirectoryInfo(DummyDirectories.documentsFolder)
                                                 .AddDirectoryInfo(DummyDirectories.presentationsFolder)
                                                 .Build();
            _fileNameManager = new FileNameManager(_fileSystem);
        }


        [TestMethod]
        public void GetNextIndexOfFileInFolder_FilesNotFound_ReturnNull()
        {
            var result = _fileNameManager.GetNextIndexOfFileInFolder(DummyDirectories.documentsFolder, "Amigo.exe");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetNextIndexOfFileInFolder_HasOneFileWithoutIndex_Return1()
        {
            var result = _fileNameManager.GetNextIndexOfFileInFolder(DummyDirectories.defaultsFolder, "Unity Player.exe");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetNextIndexOfFileInFolder_FiveSameFiles_MaxIndex4_Return5()
        {
            var result = _fileNameManager.GetNextIndexOfFileInFolder(DummyDirectories.defaultsFolder, "Amigo.exe");

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void GetNextIndexOfFileInFolder_TwoSameFile_MaxIndex3_Return4()
        {
            var result = _fileNameManager.GetNextIndexOfFileInFolder(DummyDirectories.documentsFolder, "Schedule.docx");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void GetNextSerialNumberOfFileInFolder_FolderContainFileWithSerialNumber_Return2()
        {
            var result = _fileNameManager.GetNextSerialNumberOfFileInFolder(DummyDirectories.presentationsFolder);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetNextSerialNumberOfFileInFolder_FolderNotContainFilesWithSerialNumbers_Return1()
        {
            var result = _fileNameManager.GetNextSerialNumberOfFileInFolder(DummyDirectories.documentsFolder);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetNextSerialNumberOfFileInFolder_FolderContainFileWithSerialNumber22_Return23()
        {
            var result = _fileNameManager.GetNextSerialNumberOfFileInFolder(DummyDirectories.defaultsFolder);

            Assert.AreEqual(23, result);
        }

        [TestMethod]
        public void ChangeResultFileName_WithSerialNumber_ReturnNameWithSerialNumber2()
        {
            RuleElement rule = new RuleElement()
            {
                Template = @" ^.*\.(pptx)$",
                DestinationFolder = @"C:\Downloads\presentations",
                NameChangeRule = NameChangeRule.SerialNumber
            };
            var file = new FileInfoBaseBuilder().SetName("Arrays and Collections.pptx").Build();

            var result = _fileNameManager.ChangeResultFileName(file, defaultsFolder, rule);

            Assert.IsNotNull(result);
            Assert.AreEqual("2.Arrays and Collections.pptx", result);
        }

        [TestMethod]
        public void ChangeResultFileName_WithSerialNumber_ReturnNameWithSerialNumber1()
        {
            RuleElement rule = new RuleElement()
            {
                Template = @" ^.*\.(docx)$",
                DestinationFolder = @"C:\Downloads\documents",
                NameChangeRule = NameChangeRule.SerialNumber
            };
            var file = new FileInfoBaseBuilder().SetName("Ideas.docx").Build();

            var result = _fileNameManager.ChangeResultFileName(file, defaultsFolder, rule);

            Assert.IsNotNull(result);
            Assert.AreEqual("1.Ideas.docx", result);
        }

        [TestMethod]
        public void ChangeResultFileName_WithLastModifyDate_ReturnNameWithDate()
        {
            RuleElement rule = new RuleElement()
            {
                Template = @" ^.*\.(docx)$",
                DestinationFolder = @"C:\Downloads\documents",
                NameChangeRule = NameChangeRule.LastModifyDate
            };
            var file = new FileInfoBaseBuilder().SetName("Ideas.docx").Build();

            var result = _fileNameManager.ChangeResultFileName(file, defaultsFolder, rule);
            
            Assert.IsNotNull(result);
            Assert.AreEqual($"Ideas {DateTime.Now.ToShortDateString()}.docx", result);
        }

        [TestMethod]
        public void ChangeResultFileName_WithoutRule_ReturnName()
        {
            var file = new FileInfoBaseBuilder().SetName("Schedule (1).docx").Build();

            var result = _fileNameManager.ChangeResultFileName(file, defaultsFolder);

            Assert.IsNotNull(result);
            Assert.AreEqual($"Schedule .docx", result);
        }

    }
}
