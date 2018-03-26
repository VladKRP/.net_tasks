using FolderListener.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using messages = FolderListener.Resources.Messages;

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
                    watchers[i].Created += OnCreate;
                    watchers[i].Renamed += OnRename;
                    watchers[i].EnableRaisingEvents = true;
                }
                fileSystemWatchers = watchers;
            }
            return fileSystemWatchers;
        }

        static void OnCreate(object o, FileSystemEventArgs args)
        {
            var entityInfo = new FileInfo(args.FullPath);
            Console.WriteLine($"\n{messages.FileCreated}\n{messages.FileName}:{entityInfo.Name}\n{messages.FileCreationDate}:{entityInfo.CreationTime}");
            if (!entityInfo.Attributes.Equals(FileAttributes.Directory))
            {
                var passedRule = FolderListenerConfigurations.Rules.FirstOrDefault(rule => new Regex(rule.Template).Match(args.Name).Success);
                if (passedRule != null)
                    MoveFileToSpecificFolder(entityInfo, passedRule);
                else
                    MoveFileToDefaultFolder(entityInfo);
            }
        }

        static void OnRename(object o, FileSystemEventArgs args)
        {
            var entityInfo = new FileInfo(args.FullPath);
        }

        static void MoveFileToSpecificFolder(FileInfo fileInfo, RuleElement rule)
        {
            CreateDirectoryIfNotExist(rule.DestinationFolder);

            Console.WriteLine($"\n{messages.RuleMatched} {rule.Template}\n");

            try
            {
                DirectoryInfo directory = new DirectoryInfo(rule.DestinationFolder);
                var resultFileName = ChangeResultFileName(fileInfo, rule);
                File.Move(fileInfo.FullName, rule.DestinationFolder + $"\\{resultFileName}");
                Console.WriteLine($"{messages.FileMoved} {rule.DestinationFolder}");
            }
            catch (FileNotFoundException exc)
            {
                Console.WriteLine(exc.Message);
            }
            catch(IOException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        static void MoveFileToDefaultFolder(FileInfo fileInfo)
        {
            CreateDirectoryIfNotExist(FolderListenerConfigurations.DefaultFolderPath);

            Console.WriteLine($"\n{messages.RuleNotMatched}");

            try
            {
                File.Move(fileInfo.FullName, FolderListenerConfigurations.DefaultFolderPath + $"\\{fileInfo.Name}");
                Console.WriteLine($"{messages.FileMoved} {FolderListenerConfigurations.DefaultFolderPath}");
            }
            catch (FileNotFoundException exc)
            {
                Console.WriteLine(exc.Message);
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        static void CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        static string ChangeResultFileName(FileInfo fileInfo, RuleElement rule)
        {
            string resultFileName = fileInfo.Name.Replace(fileInfo.Extension, "");
            if(rule != null)
            {
                if (rule.NameChangeRule.Equals(NameChangeRule.LastModifyDate))
                    resultFileName += DateTime.Now.ToShortDateString();
                else if (rule.NameChangeRule.Equals(NameChangeRule.SerialNumber))
                    throw new NotImplementedException();
            }
            resultFileName += fileInfo.Extension;
            return resultFileName;
        }

    }
}
