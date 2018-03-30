using FolderListener.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

namespace FolderListener
{
    public class FileNameManager
    {

        private readonly IFileSystem _fileSystem;

        public FileNameManager(IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public string ChangeResultFileName(FileInfoBase fileInfo, FolderElement defaultFolder, RuleElement rule = null)
        {
            DirectoryInfoBase destinationFolder;
            var resultFileName = GetFileNameWithoutExtensionAndIndex(fileInfo.Name);

            if (rule != null)
            {
                destinationFolder = _fileSystem.DirectoryInfo.FromDirectoryName(rule.DestinationFolder);
                if (rule.NameChangeRule.Equals(NameChangeRule.LastModifyDate))
                    resultFileName += $" {DateTime.Now.ToShortDateString()}";
                else if (rule.NameChangeRule.Equals(NameChangeRule.SerialNumber))
                    resultFileName = $"{GetNextSerialNumberOfFileInFolder(destinationFolder)}.{resultFileName}";
            }
            else destinationFolder = _fileSystem.DirectoryInfo.FromDirectoryName(defaultFolder.Path);

            var index = GetNextIndexOfFileInFolder(destinationFolder, resultFileName + fileInfo.Extension);
            resultFileName += (index != null ? $" ({index})" : "") + fileInfo.Extension;

            return resultFileName;

        }

        private string GetFileNameWithoutExtensionAndIndex(string filename)
        {
            var result = new Regex(@"\(\d+\)").Replace(filename, "");
            return Path.GetFileNameWithoutExtension(result);
        }


        public int GetNextSerialNumberOfFileInFolder(DirectoryInfoBase destinationFolder)
        {
            int nextSerialNumber = 1;

            var serialNumberRegExp = new Regex(@"^\d*[.]");
            var serialNumberExtractingRegExp = new Regex(@"(^\d*)");

            var folderFilesSerialNumbers = destinationFolder.GetFiles()
                                    .Where(file => serialNumberRegExp.Match(file.Name).Success)
                                    .Select(file => serialNumberExtractingRegExp.Match(file.Name).Value)
                                    .Where(snumber => snumber.All(c => char.IsDigit(c)))
                                    .Select(snumber => int.Parse(snumber));

            if (folderFilesSerialNumbers.Count() > 0)
                nextSerialNumber = folderFilesSerialNumbers.Max() + 1;

            return nextSerialNumber;
        }



        public int? GetNextIndexOfFileInFolder(DirectoryInfoBase folder, string filename)
        {
            int? nextIndex = null;
            var indexRegExp = new Regex(@"\((\d*)\)");

            var sameNameFiles = GetFilesWithSameNameAndExtensionInFolder(folder, filename);
            var sameNameFilesIndexes = GetIndexesOfFilesWithSameNameAndExtension(sameNameFiles, indexRegExp);

            if (sameNameFilesIndexes.Count() > 0)
                nextIndex = sameNameFilesIndexes.Max() + 1;
            else if (sameNameFiles.Count() > sameNameFilesIndexes.Count())
                nextIndex = 1;
            return nextIndex;
        }

        private IEnumerable<FileInfoBase> GetFilesWithSameNameAndExtensionInFolder(DirectoryInfoBase folder, string filename)
        {
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename);

            return folder.GetFiles().Where(file => file.Name.Contains(filenameWithoutExtension) && file.Extension.Equals(extension));
        }

        private IEnumerable<int> GetIndexesOfFilesWithSameNameAndExtension(IEnumerable<FileInfoBase> files, Regex regex)
        {
            return files.Select(file => regex.Match(file.Name)
                        .Groups[1]?.Value)
                        .Where(index => !string.IsNullOrWhiteSpace(index) && index.All(x => char.IsDigit(x)))
                        .Select(index => int.Parse(index));
        }

    }
}
