using System;
using System.IO;

namespace FSVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor();
            fileSystemVisitor.Start += ShowOnConsole;
            fileSystemVisitor.Stop += ShowOnConsole;
            foreach(var entity in fileSystemVisitor.GetDirectoryInnerEntities(@"D:\CDP")){
                System.Console.WriteLine(entity.Name);
            }
        }

        static void ShowOnConsole(string message) => System.Console.WriteLine(message);
    }
}
