using FolderListenerTests.Builders;
using Moq;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace FolderListenerTests
{
    public static class DummyDirectories
    {

        private static IEnumerable<string> _documentFolderFileNames = new List<string>()
            {
                "Schedule (1).docx",
                "Schedule (3).docx",
                "AnnualReport.docx"
            };

        private static IEnumerable<string> _presentationFolderFileNames = new List<string>()
            {
                "AnnualReport.docx",
                "Arrays and Collection.pptx",
                "1.Arrays and Collections.pptx"
            };

        private static IEnumerable<string> _defaultFolderFileNames = new List<string>()
            {
                "Amigo.exe",
                "Amigo (1).exe",
                "Amigo (2).exe",
                "Amigo (3).exe",
                "Amigo (4).exe",
                "Unity Player.exe",
                "Reflections.pptx",
                "22.SocialNetwork.mdf"
            };

        public static DirectoryInfoBase documentsFolder;
        public static DirectoryInfoBase defaultsFolder;
        public static DirectoryInfoBase presentationsFolder;

        static DummyDirectories()
        {
            documentsFolder = CreateDirectory("documents", @"C:\Downloads\documents", _documentFolderFileNames);
            presentationsFolder = CreateDirectory("presentations", @"C:\Downloads\presentations", _presentationFolderFileNames);
            defaultsFolder = CreateDirectory("default", @"C:\Downloads\default" , _defaultFolderFileNames);
        }

        public static DirectoryInfoBase CreateDirectory(string name, string path, IEnumerable<string> files)
        {
            var folderFiles = files.Select(x => new FileInfoBaseBuilder().SetName(x).Build()).ToArray();
            return new DirectoryInfoBaseBuilder().SetName(name)
                                                 .SetFullName(path)
                                                 .SetInnerFiles(folderFiles)
                                                 .Build();
        }

        
    }

}
