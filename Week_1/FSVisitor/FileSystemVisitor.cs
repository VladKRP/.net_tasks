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
            OnSearchFinish(new SearchProgressArgs() { Message = "-> Stop <-" });
        }


        private IEnumerable<FileSystemInfo> GetDirectoryInnerEntities(string entryDirectoryPath)
        {
            DirectoryInfo entryDirectoryInfo = new DirectoryInfo(entryDirectoryPath);
            bool isCancelled = false;
            foreach (var entryDirectoryEntity in entryDirectoryInfo.GetFileSystemInfos())
            {
                if (isCancelled) break;

                if (entryDirectoryEntity is DirectoryInfo)
                {
                    OnDirectoryFound(new EntityFoundArgs() { EntityInfo = entryDirectoryInfo, Message = "-> Directory found " });
                    if (FilterAlgorithm(entryDirectoryEntity))
                    {
                        EntityFoundArgs entityFoundArgs = new EntityFoundArgs() { EntityInfo = entryDirectoryInfo, Message = "-> Directory pass filter" };
                        OnFilteredDirectoryFound(entityFoundArgs);
                        if (entityFoundArgs.IsCancelled == true) isCancelled = true;
                        else
                        {
                            yield return entryDirectoryEntity;
                            foreach (var innerElements in GetDirectoryInnerEntities(entryDirectoryEntity.FullName))
                                yield return innerElements;
                            continue;
                        }
                        break;
                    }
                }
                else
                {
                    OnFileFound(new EntityFoundArgs() { EntityInfo = entryDirectoryInfo, Message = "-> File found " });
                    if (FilterAlgorithm(entryDirectoryEntity))
                    {
                        EntityFoundArgs entityFoundArgs = new EntityFoundArgs() { EntityInfo = entryDirectoryInfo, Message = "-> File pass filter" };
                        OnFilteredFileFound(entityFoundArgs);
                        if (entityFoundArgs.IsCancelled == true) isCancelled = true;
                        else
                        {
                            yield return entryDirectoryEntity;
                            continue;
                        }
                        break;
                    }
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