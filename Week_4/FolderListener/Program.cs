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

namespace FolderListener
{
    class Program
    {

    static void Main(string[] args)
        {
            //NotifyFilters filter = NotifyFilters.FileName | NotifyFilters.CreationTime;
            //var fileSystemWatchers = CreateMultipleFileSystemWatcher(ConfigureProject.WatchFoldersPathes, filter);
            
            FileSystemWatcher watcher = new FileSystemWatcher(@"D:\CDP\.net_tasks\Week_4\TestFolder\films");
            watcher.Created += OnCreate;
            watcher.Renamed += OnRename;
            watcher.EnableRaisingEvents = true;
            while (true)
            {
                
            }
        }

       

        //static IEnumerable<FileSystemWatcher> CreateMultipleFileSystemWatcher(IEnumerable<string> pathes, NotifyFilters filter)
        //{
        //    IEnumerable<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();
        //    if(pathes != null && pathes.Count() > 0)
        //    {
        //        var watchers = pathes.Where(path => Directory.Exists(path)).Select(path => new FileSystemWatcher(path)).ToArray();
        //        for (int i = 0; i < fileSystemWatchers.Count();i++)
        //        {
        //            watchers[i].NotifyFilter = filter;
        //            watchers[i].Created += OnCreate;
        //        }
        //        fileSystemWatchers = watchers;
        //    }
        //    return fileSystemWatchers;
        //}

        static void OnCreate(object o, FileSystemEventArgs args)
        {
            Console.WriteLine(args.Name);
        }

        static void OnRename(object o, FileSystemEventArgs args)
        {
            Console.WriteLine(args.Name);
            var passedRule = ConfigureProject.Rules.FirstOrDefault(rule => new Regex(rule.Template).Match(args.Name).Success);
            if (passedRule != null)
            {
                Console.WriteLine(passedRule.DestinationFolder);
            }
            else
            {
                Console.WriteLine(ConfigureProject.DefaultFolderPath);
            }
        }

    }
}
