using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

namespace FSVisitor
{
    public class FileSystemVisitor
    {
        

        public delegate bool EntityFinded();
        public delegate bool FilteredFindedEntity();
        public delegate void SearchProgress(string message);
        public Func<FileSystemInfo> FilterAlgorithm {get;private set;}

        public event SearchProgress Start;
        public event SearchProgress Stop;

        public event EntityFinded FileFinded;
        public event EntityFinded DirectoryFinded;

        public event FilteredFindedEntity FilterFileFinded;
        public event FilteredFindedEntity FilterDirectoryFinded; 

        public FileSystemVisitor(Func<FileSystemInfo> algorithm = null)
        {
            FilterAlgorithm = algorithm;
        }

        public IEnumerable<FileSystemInfo> GetDirectoryInnerEntities(string entryDirectoryPath)
        {
            Start("Start search");
            DirectoryInfo entryDirectoryInfo = new DirectoryInfo(entryDirectoryPath);
            foreach(var entryDirectoryEntity in entryDirectoryInfo.GetFileSystemInfos()){
                yield return entryDirectoryEntity;
                if(entryDirectoryEntity is DirectoryInfo){
                    foreach(var innerElements in GetDirectoryInnerEntities(entryDirectoryEntity.FullName))
                    {
                        yield return innerElements;
                    }  
                }
            }  
            Stop("Stop search");
        }
    }
}