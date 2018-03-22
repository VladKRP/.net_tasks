using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.IO.Abstractions;

namespace FSVisitor
{
    public class FileSystemVisitor
    {
        public Func<FileSystemInfoBase, bool> FilterAlgorithm { get; set; }
        public IFileSystem FileSystem { get; private set; }
        

        public event EventHandler<SearchProgressArgs> Start;
        public event EventHandler<SearchProgressArgs> Finish;

        public event EventHandler<EntityFoundArgs> FileFound;
        public event EventHandler<EntityFoundArgs> DirectoryFound;
        public event EventHandler<EntityFoundArgs> FilterFileFound;
        public event EventHandler<EntityFoundArgs> FilterDirectoryFound;

        public FileSystemVisitor(IFileSystem fileSystem = null, Func<FileSystemInfoBase, bool> filterAlgorithm = null)
        {
            FileSystem = fileSystem ?? new FileSystem();
            FilterAlgorithm = filterAlgorithm;
           // FilterAlgorithm = filterAlgorithm ?? ((FileSystemInfoBase info) => { return true; });
        }

        public IEnumerable<FileSystemInfoBase> SearchDirectoryInnerEntities(string entryDirectoryPath)
        {
            OnSearchStart(new SearchProgressArgs() { Message = "-> Start <-" });
            var entryDirectory = FileSystem.DirectoryInfo.FromDirectoryName(entryDirectoryPath);
            foreach (var entity in GetDirectoryInnerEntities(entryDirectory))
                yield return entity;
            OnSearchFinish(new SearchProgressArgs() { Message = "-> Finish <-" });
        }

        public IEnumerable<FileSystemInfoBase> GetDirectoryInnerEntities(DirectoryInfoBase entryDirectoryInfo)
        {
            bool isCancelled = false;
            foreach (var entryDirectoryEntity in entryDirectoryInfo.GetFileSystemInfos())
            {
                if (isCancelled) break;

                EntityFoundArgs entityFoundArgs = new EntityFoundArgs() { EntityInfo = entryDirectoryEntity };

                if (entryDirectoryEntity is DirectoryInfoBase)
                {
                    entityFoundArgs.Message = "Directory found";
                    OnDirectoryFound(entityFoundArgs);
                    if((FilterAlgorithm == null || FilterAlgorithm(entryDirectoryEntity)) && !entityFoundArgs.IsExcluded)
                    {
                        entityFoundArgs.Message = "Filtered directory found";
                        OnFilteredDirectoryFound(entityFoundArgs);
                        yield return entryDirectoryEntity;
                    }
                }
                else
                {
                    entityFoundArgs.Message = "File found";
                    OnFileFound(entityFoundArgs);
                    if ((FilterAlgorithm == null || FilterAlgorithm(entryDirectoryEntity)) && !entityFoundArgs.IsExcluded)
                    {
                        entityFoundArgs.Message = "Filtered file found";
                        OnFilteredFileFound(entityFoundArgs);
                        yield return entryDirectoryEntity;
                    }
                }

                if (entityFoundArgs.IsCancelled) isCancelled = true;
            }
            if (!isCancelled)
            {
                foreach (var entryDirectoryEntity in entryDirectoryInfo.GetDirectories())
                {
                    foreach (var entity in GetDirectoryInnerEntities(entryDirectoryEntity))
                        yield return entity;
                }
            }           
        }

        protected virtual void OnSearchStart(SearchProgressArgs args)
        {
            var temporary = Start;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnSearchFinish(SearchProgressArgs args)
        {
            var temporary = Finish;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnFileFound(EntityFoundArgs args)
        {
            var temporary = FileFound;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnDirectoryFound(EntityFoundArgs args)
        {
            var temporary = DirectoryFound;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnFilteredFileFound(EntityFoundArgs args)
        {
            var temporary = FilterFileFound;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnFilteredDirectoryFound(EntityFoundArgs args)
        {
            var temporary = FilterDirectoryFound;
            temporary?.Invoke(this, args);
        }

    }
}


//Enumerator version

//:IEnumerable<FileSystemInfo>

//public string EntryDirectoryPath { get; set; }

//public FileSystemVisitor(string entryDirectoryPath,IFileSystem fileSystem = null, Func<FileSystemInfo, bool> filterAlgorithm = null)
//{
//    EntryDirectoryPath = entryDirectoryPath;
//    FileSystem = fileSystem ?? new FileSystem();
//    FilterAlgorithm = filterAlgorithm;
//}

//public IEnumerator<FileSystemInfo> GetEnumerator()
//{
//    //throw new NotImplementedException();
//    OnSearchStart(new SearchProgressArgs() { Message = "-> Start <-" });
//    var entryDirectory = FileSystem.DirectoryInfo.FromDirectoryName(EntryDirectoryPath);
//    foreach (var entity in GetDirectoryInnerEntities(entryDirectory))
//        yield return entity;
//    OnSearchFinish(new SearchProgressArgs() { Message = "-> Finish <-" });
//}

//IEnumerator IEnumerable.GetEnumerator()
//{
//    return GetEnumerator();
//}
