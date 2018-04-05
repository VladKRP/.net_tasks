using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;
using messages = FolderListener.Resources.Messages;
using FolderListener.Configurations;
using FolderListener.Extensions;

namespace FolderListener
{
    public class FileListenerArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class FolderListener
    {
        private IFileSystem _fileSystem;

        private IEnumerable<FileSystemWatcher> _fileSystemWatchers;

        private readonly FolderListenerConfigurationSection _folderListenerConfigurations;

        private readonly FileNameManager _fileNameManager;

        public EventHandler<FileListenerArgs> FileCreated;
        public EventHandler<FileListenerArgs> FileMoved;
        public EventHandler<FileListenerArgs> RuleMatch;
        public EventHandler<FileListenerArgs> RuleNotMatch;
        public EventHandler<FileListenerArgs> Error;


        public FolderListener(FolderListenerConfigurationSection folderListenerConfigurations, IFileSystem fileSystem = null, FileNameManager fileNameManager = null)
        {
            
            _folderListenerConfigurations = folderListenerConfigurations;

            var foldersPathes = _folderListenerConfigurations.GetFolders().Select(x => x.Path);
            _fileSystemWatchers = CreateMultipleFileSystemWatcher(foldersPathes);

            _fileSystem = fileSystem ?? new FileSystem();
            _fileNameManager = fileNameManager ?? new FileNameManager(_fileSystem);
        }

        public void Listen()
        {
            var watchers = _fileSystemWatchers.ToArray();
            for (int i = 0; i < watchers.Count(); i++)
            {
                watchers[i].Changed += OnChange;
                watchers[i].EnableRaisingEvents = true;
            }
            _fileSystemWatchers = watchers;
        }

        protected virtual IEnumerable<FileSystemWatcher> CreateMultipleFileSystemWatcher(IEnumerable<string> pathes)
        {
            IEnumerable<FileSystemWatcher> fileSystemWatchers = Enumerable.Empty<FileSystemWatcher>();
            if (pathes != null && pathes.Count() > 0)
                fileSystemWatchers = pathes.Where(path => Directory.Exists(path)).Select(path => new FileSystemWatcher(path));
            return fileSystemWatchers;
        }

        protected virtual void OnChange(object o, FileSystemEventArgs args)
        {
            
            var fileInfo = _fileSystem.FileInfo.FromFileName(args.FullPath);
            FileCreated?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.FileCreated}\n{messages.FileName}:{fileInfo.Name}\n{messages.FileCreationDate}:{fileInfo.CreationTime}" });
            if (!fileInfo.Attributes.Equals(FileAttributes.Directory))
            {
                var passedRule = _folderListenerConfigurations.GetRules()
                                                              .FirstOrDefault(rule => new Regex(rule.Template).Match(args.Name).Success);
                if (passedRule != null)
                    MoveFileAccordingRules(fileInfo, passedRule, _fileNameManager.ChangeResultFileName);
                else
                    MoveFileAccordingRules(fileInfo, changeFileNameFunc: _fileNameManager.ChangeResultFileName);
            }
        }

        private void MoveFileAccordingRules(FileInfoBase fileInfo, RuleElement rule = null, Func<FileInfoBase, FolderElement, RuleElement, string> changeFileNameFunc = null)
        {
            var defaultFolder = _folderListenerConfigurations.DefaultFolder;
            string destinationFolderPath = defaultFolder.Path;

            if (rule == null)
                RuleMatch?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.RuleNotMatched}" });
            else
            {
                RuleNotMatch?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.RuleMatched} {rule.Template}\n" });
                destinationFolderPath = rule.DestinationFolder;
            }

            if (!_fileSystem.Directory.Exists(destinationFolderPath))
                _fileSystem.Directory.CreateDirectory(destinationFolderPath);

            try
            {
                var resultFileName = changeFileNameFunc != null ? changeFileNameFunc(fileInfo, defaultFolder, rule) : fileInfo.Name;
                var destinationFileName = destinationFolderPath + $"\\{resultFileName}";
                File.Move(fileInfo.FullName, destinationFileName);
                FileMoved?.Invoke(this, new FileListenerArgs() { Message = $"{messages.FileMoved} {destinationFolderPath}" });
            }
            catch (IOException exc)
            {
                Error?.Invoke(this, new FileListenerArgs() { Message = exc.Message });
            }
        }
      
    }
}
