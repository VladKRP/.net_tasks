using FolderListener.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener
{
    public static class FolderListenerConfigurations
    {
        public static string Culture { get; private set; }

        public static IEnumerable<string> WatchFoldersPathes { get;private set; }

        public static string DefaultFolderPath { get; private set; }

        public static IEnumerable<RuleElement> Rules { get; private set; }

        private static FolderListenerConfigurationSection _section;

        static FolderListenerConfigurations()
        {
            _section = ConfigurationManager.GetSection("folderListenerConfigurationSection") as FolderListenerConfigurationSection;
            Culture = _section?.ApplicationLanguage;
            Rules = GetRules();
            WatchFoldersPathes = GetWatchFolderPathes();
            DefaultFolderPath = _section?.DefaultFolder?.Path;
        }

        private static IEnumerable<string> GetWatchFolderPathes()
        {
            if (_section != null && _section.Folders != null)
            {
                foreach (FolderElement folder in _section.Folders)
                    yield return folder.Path;
            }
        }

        private static IEnumerable<RuleElement> GetRules()
        {
            if (_section != null && _section.Rules != null)
            {
                foreach (RuleElement rule in _section.Rules)
                    yield return rule;
            }
        }
    }
}