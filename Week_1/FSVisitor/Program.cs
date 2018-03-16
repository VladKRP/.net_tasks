using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FSVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor((FileSystemInfo info) => (info is FileInfo));

            fileSystemVisitor.Start += FileSystemVisitor_ShowSearchProgress;
            fileSystemVisitor.Finish += FileSystemVisitor_ShowSearchProgress;
            fileSystemVisitor.FileFound += FileSystemVisitor_OnEntityFound;
            fileSystemVisitor.FilterFileFound += FileSystemVisitor_OnEntityFound;
            fileSystemVisitor.DirectoryFound += FileSystemVisitor_OnEntityFound;
            fileSystemVisitor.FilterDirectoryFound += FileSystemVisitor_OnEntityFound;

            var currentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var testFolderName = "TestFolder";
            var testFolderPath = Path.Combine(currentDirectory, testFolderName);

            foreach (var entity in fileSystemVisitor.SearchDirectoryInnerEntities(testFolderPath))
            {
                System.Console.WriteLine(entity.Name);
            }

            Console.ReadLine();
        }

        private static void FileSystemVisitor_ShowSearchProgress(object o, SearchProgressArgs args)
        {
            System.Console.WriteLine(args.Message);
        }

        private static void FileSystemVisitor_OnEntityFound(object o, EntityFoundArgs args)
        {
            //if (args.EntityInfo.Name.Equals("F2"))
            //{
            //    args.IsCancelled = true;
            //}
            Console.WriteLine(args.Message);
        }
    }
}
