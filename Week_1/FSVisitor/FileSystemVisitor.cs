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
        public event EventHandler<SearchProgressArgs> Stop;
        public event EventHandler<SearchProgressArgs> FileFinded;
        public event EventHandler<SearchProgressArgs> DirectoryFinded;
        public event EventHandler<SearchProgressArgs> FilterFileFinded;
        public event EventHandler<SearchProgressArgs> FilterDirectoryFinded;

        public FileSystemVisitor(Func<FileSystemInfo, bool> algorithm = null)
        {
            FilterAlgorithm = algorithm;
        }

        public IEnumerable<FileSystemInfo> SearchDirectoryInnerEntities(string entryDirectoryPath)
        {
            OnSearchStart(new SearchProgressArgs() { Message = "-> Start searching <-" });
            foreach (var entity in GetDirectoryInnerEntities(entryDirectoryPath))
                yield return entity;
            OnSearchStop(new SearchProgressArgs() { Message = "-> Stop searching <-" });
        }


        public IEnumerable<FileSystemInfo> GetDirectoryInnerEntities(string entryDirectoryPath)
        {
            DirectoryInfo entryDirectoryInfo = new DirectoryInfo(entryDirectoryPath);
            foreach (var entryDirectoryEntity in entryDirectoryInfo.GetFileSystemInfos())
            {
                if (entryDirectoryEntity is DirectoryInfo)
                {
                    OnDirectoryFinded(new SearchProgressArgs() { Message = "-> Directory finded " });
                    if (FilterAlgorithm(entryDirectoryEntity))
                    {
                        OnFilteredDirectoryFinded(new SearchProgressArgs() { Message = "-> Directory pass filter" });
                        System.Threading.Thread.Sleep(1000);
                        yield return entryDirectoryEntity;
                        foreach (var innerElements in GetDirectoryInnerEntities(entryDirectoryEntity.FullName))
                            yield return innerElements;               
                    }
                }
                else
                {
                    OnFileFinded(new SearchProgressArgs() { Message = "-> File finded " });
                    if (FilterAlgorithm(entryDirectoryEntity))
                    {
                        OnFilteredFileFinded(new SearchProgressArgs() { Message = "-> File pass filter" });
                        System.Threading.Thread.Sleep(1000);
                        yield return entryDirectoryEntity;
                    }
                }
            }
        }
        
        protected virtual void OnSearchStart(SearchProgressArgs args)
        {
            var temporary = Start;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnSearchStop(SearchProgressArgs args)
        {
            var temporary = Stop;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnFileFinded(SearchProgressArgs args)
        {
            var temporary = FileFinded;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnDirectoryFinded(SearchProgressArgs args)
        {
            var temporary = DirectoryFinded;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnFilteredFileFinded(SearchProgressArgs args)
        {
            var temporary = FilterFileFinded;
            temporary?.Invoke(this, args);
        }

        protected virtual void OnFilteredDirectoryFinded(SearchProgressArgs args)
        {
            var temporary = FilterDirectoryFinded;
            temporary?.Invoke(this, args);
        }

    }
}