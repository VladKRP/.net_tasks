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

        public string ChangeFileName(FileInfoBase fileInfo, FolderElement defaultFolder, RuleElement rule = null)
        {
            DirectoryInfoBase destinationDirectory = GetFileDestinationDirectory(rule);
            var changedFileName = GetFileNameWithoutExtensionAndIndex(fileInfo.Name).Trim();

            if (rule != null)
                changedFileName = ChangeFileNameAccordingRule(changedFileName, destinationDirectory, rule);

            var index = GetNextIndexOfFileInDirectory(destinationDirectory, changedFileName + fileInfo.Extension);
            changedFileName += (index != null ? $" ({index})" : "") + fileInfo.Extension;

            return changedFileName;

            DirectoryInfoBase GetFileDestinationDirectory(RuleElement ruleElement)
            {
                DirectoryInfoBase directory = _fileSystem.DirectoryInfo.FromDirectoryName(defaultFolder.Path);
                if (rule != null)
                   directory = _fileSystem.DirectoryInfo.FromDirectoryName(rule.DestinationFolder);
                return directory;
            }

            string GetFileNameWithoutExtensionAndIndex(string filename)
            {
                var result = new Regex(@"\(\d+\)").Replace(filename, "");
                return Path.GetFileNameWithoutExtension(result);
            }

            string ChangeFileNameAccordingRule(string filename, DirectoryInfoBase directory, RuleElement ruleElement)
            {
                if (ruleElement.NameChangeRule.Equals(NameChangeRule.LastModifyDate))
                    filename += $" {DateTime.Now.ToShortDateString()}";
                else if (ruleElement.NameChangeRule.Equals(NameChangeRule.SerialNumber))
                    filename = $"{GetNextSerialNumberOfFileInDirectory(directory)}.{filename}";
                return filename;
            }
        }

        public int GetNextSerialNumberOfFileInDirectory(DirectoryInfoBase directory)
        {
            int nextSerialNumber = 1;

            var filesSerialNumbers = GetFilesSerialNumbersInDirectory(directory);

            if (filesSerialNumbers.Any())
                nextSerialNumber = filesSerialNumbers.Max() + 1;

            return nextSerialNumber;

            IEnumerable<int> GetFilesSerialNumbersInDirectory(DirectoryInfoBase filesDirectory)
            {
                var serialNumberExtractingRegExp = new Regex(@"(^\d+)");

                var directoryFilesSerialNumbers = filesDirectory.GetFiles()
                    .Select(file => serialNumberExtractingRegExp.Match(file.Name))
                    .Where(match => match.Success)
                    .Select(match => match.Value)
                    .Select(snumber => int.Parse(snumber));
                return directoryFilesSerialNumbers;
            }
        }

        public int? GetNextIndexOfFileInDirectory(DirectoryInfoBase directory, string filename)
        {
            int? nextIndex = null;
            var indexRegExp = new Regex(@"\((\d*)\)");
            IEnumerable<int> sameNameFilesIndexes;

            var sameNameFiles = GetFilesWithSameNameAndExtensionInFolder(directory, filename);
            if (sameNameFiles.Any())
            {
                sameNameFilesIndexes = GetIndexesOfFilesWithSameNameAndExtension(sameNameFiles, indexRegExp);

                if (sameNameFilesIndexes.Any())
                    nextIndex = sameNameFilesIndexes.Max() + 1;
                else
                    nextIndex = 1;
            }
                
            return nextIndex;


            IEnumerable<FileInfoBase> GetFilesWithSameNameAndExtensionInFolder(DirectoryInfoBase folder, string fileName)
            {
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var extension = Path.GetExtension(fileName);

                return folder.GetFiles().Where(file => file.Name.Contains(filenameWithoutExtension) && file.Extension.Equals(extension));
            }

            IEnumerable<int> GetIndexesOfFilesWithSameNameAndExtension(IEnumerable<FileInfoBase> files, Regex regex)
            {
                return files.Select(file => regex.Match(file.Name)
                            .Groups[1]?.Value)
                            .Where(index => !string.IsNullOrWhiteSpace(index) && index.All(x => char.IsDigit(x)))
                            .Select(index => int.Parse(index));
            }
        }
    }
}
