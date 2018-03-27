using FolderListener.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using messages = FolderListener.Resources.Messages;
using System.Threading;

namespace FolderListener
{
    class Program
    {

        static void Main(string[] args)
        {
            var culture = new CultureInfo(FolderListenerConfigurations.Culture);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var fileSystemWatchers = CreateMultipleFileSystemWatcher(FolderListenerConfigurations.WatchFoldersPathes);
            while (true){}
        }

        static IEnumerable<FileSystemWatcher> CreateMultipleFileSystemWatcher(IEnumerable<string> pathes)
        {
            IEnumerable<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();
            if (pathes != null && pathes.Count() > 0)
            {
                var watchers = pathes.Where(path => Directory.Exists(path)).Select(path => new FileSystemWatcher(path)).ToArray();
                for (int i = 0; i < watchers.Count(); i++)
                {
                    watchers[i].Changed += OnChange;
                    watchers[i].EnableRaisingEvents = true;
                }
                fileSystemWatchers = watchers;
            }
            return fileSystemWatchers;
        }

        static void OnChange(object o, FileSystemEventArgs args)
        {
            var entityInfo = new FileInfo(args.FullPath);
            Console.WriteLine($"\n{messages.FileCreated}\n{messages.FileName}:{entityInfo.Name}\n{messages.FileCreationDate}:{entityInfo.CreationTime}");
            if (!entityInfo.Attributes.Equals(FileAttributes.Directory))
            {
                var passedRule = FolderListenerConfigurations.Rules.FirstOrDefault(rule => new Regex(rule.Template).Match(args.Name).Success);
                if (passedRule != null)
                    MoveFileAccordingRules(entityInfo, passedRule, ChangeResultFileName);
                else
                    MoveFileAccordingRules(entityInfo, changeFileNameFunc: ChangeResultFileName);
            }
        }

        static void MoveFileAccordingRules(FileInfo fileInfo, RuleElement rule = null, Func<FileInfo, RuleElement, string> changeFileNameFunc = null)
        {
            var destinationFolder = FolderListenerConfigurations.DefaultFolderPath;
            if (rule == null) Console.WriteLine($"\n{messages.RuleNotMatched}");
            else
            {
                Console.WriteLine($"\n{messages.RuleMatched} {rule.Template}\n");
                destinationFolder = rule.DestinationFolder;
            }

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            try
            {
                var resultFileName = changeFileNameFunc != null ? changeFileNameFunc(fileInfo, rule) : fileInfo.Name;
                File.Move(fileInfo.FullName, destinationFolder + $"\\{resultFileName}");
                Console.WriteLine($"{messages.FileMoved} {destinationFolder}");
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        static string ChangeResultFileName(FileInfo fileInfo, RuleElement rule = null)
        {
            DirectoryInfo destinationFolder; 
            var fileName = new Regex(@"\(\d+\)").Replace(fileInfo.Name, "");
            fileName = Path.GetFileNameWithoutExtension(fileName);

            if (rule != null)
            {
                destinationFolder = new DirectoryInfo(rule.DestinationFolder); 
                if (rule.NameChangeRule.Equals(NameChangeRule.LastModifyDate))
                    fileName += $" {DateTime.Now.ToShortDateString()}";
                else if (rule.NameChangeRule.Equals(NameChangeRule.SerialNumber))
                    fileName = $"{GetNextSerialNumberInFolder(destinationFolder)}." + fileName;
            }
            else destinationFolder = new DirectoryInfo(FolderListenerConfigurations.DefaultFolderPath);

            var maxIndex = GetMaxIndexOfSameNameFileInFolder(fileName, destinationFolder);
            fileName += (maxIndex != null ? $"({maxIndex})" : "") + fileInfo.Extension;

            return fileName;
        }

        static int GetNextSerialNumberInFolder(DirectoryInfo destinationFolder)
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

        static int? GetMaxIndexOfSameNameFileInFolder(string filename, DirectoryInfo destinationFolder)
        {
            int? maxIndex = null;
            var indexRegExp = new Regex(@"\((\d*)\)");
            var sameNameFiles = destinationFolder.GetFiles().Where(file => file.Name.Contains(filename));

            var sameNameFilesIndexes = sameNameFiles.Select(file => indexRegExp.Match(file.Name).Groups[1]?.Value)
                                                               .Where(index => !string.IsNullOrWhiteSpace(index) && index.All(x => char.IsDigit(x)))
                                                               .Select(index => int.Parse(index));
            if (sameNameFilesIndexes.Count() > 0)
                maxIndex = sameNameFilesIndexes.Max();
            else if (sameNameFiles.Count() > sameNameFilesIndexes.Count())
                maxIndex = 1;

            return maxIndex;
        }

    }
}
