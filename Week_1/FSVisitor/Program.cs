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
            Func<FileSystemInfo, bool> filterAlgorithm = (FileSystemInfo entity) => entity.Name.Length < 8;
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(filterAlgorithm);
            
            fileSystemVisitor.Start += ShowOnConsole;
            fileSystemVisitor.Stop += ShowOnConsole;
            fileSystemVisitor.FileFinded += ShowOnConsole;
            fileSystemVisitor.DirectoryFinded += ShowOnConsole;
            fileSystemVisitor.FilterFileFinded += ShowOnConsole;
            fileSystemVisitor.FilterDirectoryFinded += ShowOnConsole;
            
            
            foreach(var entity in fileSystemVisitor.SearchDirectoryInnerEntities(@"D:\CDP\DOTNET")){
                System.Console.WriteLine(entity.Name);
            }
        }

        static void ShowOnConsole(object o, SearchProgressArgs args) => System.Console.WriteLine(args.Message);

    }
}
