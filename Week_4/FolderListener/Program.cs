using System;
using System.IO;

namespace FolderListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
            while (true)
            {
                
            }
            
        }

        static void OnChanged(object o, FileSystemEventArgs args)
        {
           
        }
    }
}
