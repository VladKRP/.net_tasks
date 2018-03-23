using FolderListener.Configurations;
using FolderListener.Configurations.WatchFolders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace FolderListener
{
    class Program
    {

    static void Main(string[] args)
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            //CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
            var phrases = ConfigureProject.Cultures.FirstOrDefault(culture => culture.Name.Equals(CultureInfo.CurrentCulture.Name)).Phrases;
            var fileSystemWatchers = CreateMultipleFileSystemWatcher(ConfigureProject.WatchFoldersPathes);
            while (true)
            {
                
            }
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
            Console.WriteLine(entityInfo.CreationTime.ToLongDateString());
        }

        static void OnRename(object o, FileSystemEventArgs args)
        {
            var entityInfo = new FileInfo(args.FullPath);
            if(!entityInfo.Attributes.Equals(FileAttributes.Directory))
            {
                var passedRule = ConfigureProject.Rules.FirstOrDefault(rule => new Regex(rule.Template).Match(args.Name).Success);
                if (passedRule != null)
                {
                    
                    Console.WriteLine("\nFile name match rule: " + passedRule.Template);
                    try
                    {
                        File.Move(args.FullPath, passedRule.DestinationFolder + $"\\{args.Name}");
                        Console.WriteLine("Moved to " + passedRule.DestinationFolder);
                    }
                    catch (FileNotFoundException exc)
                    {

                    }
                }
                else
                {
                    Console.WriteLine("\nRules not matched");
                    try
                    {
                        File.Move(args.FullPath, ConfigureProject.DefaultFolderPath + $"\\{args.Name}");
                        Console.WriteLine("Moved to " + ConfigureProject.DefaultFolderPath);
                    }
                    catch (FileNotFoundException exc)
                    {

                    }
                }
            } 
        }

    }
}
