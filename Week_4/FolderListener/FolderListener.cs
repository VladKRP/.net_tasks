using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;
using messages = FolderListener.Resources.Messages;
using FolderListener.Configurations;
using FolderListener.Extensions;
using System.Threading;

namespace FolderListener
{

    class FolderListener
    {

        private FileInfoBase _changedFile;

        private IFileSystem _fileSystem;

        private IEnumerable<FileSystemWatcher> _fileSystemWatchers;

        private readonly FolderListenerConfigurationSection _folderListenerConfigurations;

        private readonly FileNameManager _fileNameManager;

        public EventHandler<FileListenerArgs> FileCreated;
        public EventHandler<FileListenerArgs> FileMoved;
        public EventHandler<FileListenerArgs> FileNameChanged;
        public EventHandler<FileListenerArgs> RuleMatch;
        public EventHandler<FileListenerArgs> RuleNotMatch;
        public EventHandler<FileListenerArgs> Error;
        public EventHandler<FileListenerArgs> ListeningStarted;


        public FolderListener(FolderListenerConfigurationSection folderListenerConfigurations, IFileSystem fileSystem = null, FileNameManager fileNameManager = null)
        {
            _folderListenerConfigurations = folderListenerConfigurations ?? throw new ArgumentNullException(messages.ConfigurationArgumentNullException);

            var foldersPathes = _folderListenerConfigurations.GetFolders().Select(x => x.Path);
            _fileSystemWatchers = CreateMultipleFileSystemWatcher(foldersPathes);

            _fileSystem = fileSystem ?? new FileSystem();
            _fileNameManager = fileNameManager ?? new FileNameManager(_fileSystem);
        }

        public void Listen()
        {
            if (!_fileSystemWatchers.Any())
                throw new FolderListenerException(messages.NoWatchersException, messages.NoWatchersExceptionDescription, messages.NoWatchersExceptionResolveCases);

            var watchers = _fileSystemWatchers.ToArray();

            for (int i = 0; i < watchers.Count(); i++)
            {
                watchers[i].Changed += new FileSystemEventHandler(OnChange);
                watchers[i].EnableRaisingEvents = true;
            }
            _fileSystemWatchers = watchers;

            ListeningStarted?.Invoke(this, new FileListenerArgs() { Message = messages.ListeningStarted });
        }

        protected virtual IEnumerable<FileSystemWatcher> CreateMultipleFileSystemWatcher(IEnumerable<string> folderPathes)
        {
            IEnumerable<FileSystemWatcher> fileSystemWatchers = Enumerable.Empty<FileSystemWatcher>();
            if (folderPathes?.Count() > 0)
                fileSystemWatchers = folderPathes.Where(path => Directory.Exists(path))
                                                 .Select(path => new FileSystemWatcher(path));
            return fileSystemWatchers;
        }

        protected virtual void OnChange(object o, FileSystemEventArgs args)
        {
            if(_changedFile == null)
            {
                var fileInfo = _fileSystem.FileInfo.FromFileName(args.FullPath);
                _changedFile = fileInfo;
                TimerCallback callback = new TimerCallback((object obj) => {
                    _changedFile = null;
                });
                Timer timer = new Timer(callback);
                if (!IsFileIgnored(fileInfo.Name) && IsNewlyCreatedFile(fileInfo))
                {
                    FileCreated?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.FileCreated}\n{messages.FileName}:{fileInfo.Name}\n{messages.FileCreationDate}:{fileInfo.CreationTime}" });
                    if (!fileInfo.Attributes.Equals(FileAttributes.Directory))
                    {
                        var passedRule = _folderListenerConfigurations.GetRules()
                                                                      .FirstOrDefault(rule => new Regex(rule.Template).Match(args.Name).Success);
                        if (passedRule != null)
                            MoveFileAccordingRules(fileInfo, passedRule, _fileNameManager.ChangeFileName);
                        else
                            MoveFileAccordingRules(fileInfo, changeFileNameFunc: _fileNameManager.ChangeFileName);
                    }
                }
                timer.Change(1000, 0);
            }
            

            bool IsFileIgnored(string filename)
            {
                bool isIgnored = false;
                var ignoreTemplates = _folderListenerConfigurations.GetIgnoreTemplates();
                if (ignoreTemplates.Any(ignoteTemplate => new Regex(ignoteTemplate.Template).Match(filename).Success))
                    isIgnored = true;
                return isIgnored;
            }

            /*This check is required for excel file.
             * When excel file created, onChange activates 3 time.
             * First with expected file, next with files with Date = 1.1.1601*/
            bool IsNewlyCreatedFile(FileInfoBase file)
            {
                TimeSpan maxResidual = new TimeSpan(0, 1, 0);
                bool isNewlyCreatedFile = false;
                var residualTime = DateTime.UtcNow.Date - file.CreationTimeUtc.Date;
                if (residualTime >= TimeSpan.Zero && residualTime <= maxResidual)
                    isNewlyCreatedFile = true;
                return isNewlyCreatedFile;
            }
        }

        private void MoveFileAccordingRules(FileInfoBase fileInfo, RuleElement rule = null, Func<FileInfoBase, FolderElement, RuleElement, string> changeFileNameFunc = null)
        {
            if (fileInfo.CreationTime.Year != DateTime.Now.Year)
                return;

            int numberOfRetries = 2;
            var defaultFolder = _folderListenerConfigurations.DefaultFolder;
            string destinationFolderPath = defaultFolder.Path;
            var isFileInProperDirectory = IsFileInProperDirectory(fileInfo, rule);

            if (rule == null)
            {
                if (isFileInProperDirectory)
                {
                    FileMoved?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.FileAlreadyInProperFolder}" });
                    return;
                }
                else
                    RuleMatch?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.RuleNotMatched}" });
            }
            else if (isFileInProperDirectory)
            {
                FileMoved?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.FileAlreadyInProperFolder}" });
                return;
            }
            else
            {
                RuleNotMatch?.Invoke(this, new FileListenerArgs() { Message = $"\n{messages.RuleMatched} {rule.Template}\n" });
                destinationFolderPath = rule.DestinationFolder;
            }

            if (!_fileSystem.Directory.Exists(destinationFolderPath))
                _fileSystem.Directory.CreateDirectory(destinationFolderPath);

            for (int i = 0; i <= numberOfRetries; i++)
            {
                try
                {
                    var resultFileName = changeFileNameFunc != null ? changeFileNameFunc(fileInfo, defaultFolder, rule) : fileInfo.Name;
                    var destinationFileName = destinationFolderPath + $"\\{resultFileName}";
                    Thread.Sleep(100);
                    File.Move(fileInfo.FullName, destinationFileName);
                    //Thread.Sleep(100);
                    if (!string.Equals(fileInfo?.Name, resultFileName))
                        FileNameChanged?.Invoke(this, new FileListenerArgs() { Message = $"{messages.FileNameChanged}: {resultFileName}" });
                    FileMoved?.Invoke(this, new FileListenerArgs() { Message = $"{messages.FileMoved} {destinationFolderPath}" });
                    return;
                }
                catch (IOException exc)//Error when file moved but listener try to find it in source directory
                {
                    if (numberOfRetries == i)
                        Error?.Invoke(this, new FileListenerArgs() { Message = exc.Message });
                }

            }

            bool IsFileInProperDirectory(FileInfoBase file, RuleElement ruleElement)
            {
                bool isInProperDirectory = false;
                if (!string.IsNullOrWhiteSpace(fileInfo?.DirectoryName))
                {
                    if (ruleElement == null && string.Equals(fileInfo?.DirectoryName, _folderListenerConfigurations.DefaultFolder.Path)
                        || string.Equals(fileInfo?.DirectoryName, rule?.DestinationFolder))
                        isInProperDirectory = true;
                }
                return isInProperDirectory;

            }  
        }

    }
}
