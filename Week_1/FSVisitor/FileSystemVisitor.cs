using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FSVisitor
{
    public class FileSystemVisitor
    {
        public Func<FileSystemInfo, bool> FilterAlgorithm { get; private set; }

        public event EventHandler<SearchProgressArgs> Start;
        public event EventHandler<SearchProgressArgs> Finish;

        public event EventHandler<EntityFoundArgs> FileFound;
        public event EventHandler<EntityFoundArgs> DirectoryFound;
        public event EventHandler<EntityFoundArgs> FilterFileFound;
        public event EventHandler<EntityFoundArgs> FilterDirectoryFound;

        public FileSystemVisitor(Func<FileSystemInfo, bool> algorithm = null)
        {
            FilterAlgorithm = algorithm ?? ((FileSystemInfo entityInfo) => { return true; });
        }

        public IEnumerable<FileSystemInfo> SearchDirectoryInnerEntities(string entryDirectoryPath)
        {
            OnSearchStart(new SearchProgressArgs() { Message = "-> Start <-" });
            foreach (var entity in GetDirectoryInnerEntities(entryDirectoryPath))
                yield return entity;
            OnSearchFinish(new SearchProgressArgs() { Message = "-> Finish <-" });
        }


        private IEnumerable<FileSystemInfo> GetDirectoryInnerEntities(string entryDirectoryPath)
        {
            bool isCancelled = false;
            DirectoryInfo entryDirectoryInfo = new DirectoryInfo(entryDirectoryPath); 
            foreach (var entryDirectoryEntity in entryDirectoryInfo.GetFileSystemInfos())
            {
                if (isCancelled) break;

                EntityFoundArgs entityFoundArgs = new EntityFoundArgs() { EntityInfo = entryDirectoryEntity };

                if (entryDirectoryEntity is DirectoryInfo)
                {
                    entityFoundArgs.Message = "Directory found";
                    OnDirectoryFound(entityFoundArgs);
                    if(FilterAlgorithm(entryDirectoryEntity))
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
                    if (FilterAlgorithm(entryDirectoryEntity))
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
                    foreach (var entity in GetDirectoryInnerEntities(entryDirectoryEntity.FullName))
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

