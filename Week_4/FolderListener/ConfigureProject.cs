using FolderListener.Configurations.FolderListenerRules;
using FolderListener.Configurations.UICulture;
using FolderListener.Configurations.WatchFolders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener
{
    public static class ConfigureProject
    {

        public static IEnumerable<string> WatchFoldersPathes { get;private set; }

        public static string DefaultFolderPath { get; private set; }

        public static IEnumerable<UICultureElement> Cultures { get;private set; }

        public static IEnumerable<RuleElement> Rules { get; private set; }

        static ConfigureProject()
        {
            DefaultFolderPath = GetDefaultFolderPath();
            WatchFoldersPathes = GetWatchFoldersPathes();
            Cultures = GetAvailableCultures();
            Rules = GetRules();
        }

        private static string GetDefaultFolderPath()
        {
            string folderPath = null;
            var section = ConfigurationManager.GetSection("watchFolderConfigurationSection") as WatchFolderConfigurationSection;
            if (section != null && section.DefaultFolder != null)
                folderPath = section.DefaultFolder.Path;
            return folderPath;

        }

        private static IEnumerable<string> GetWatchFoldersPathes()
        {
            var section = ConfigurationManager.GetSection("watchFolderConfigurationSection") as WatchFolderConfigurationSection;
            if(section != null && section.Folders != null)
            {
                foreach (FolderElement folder in section.Folders)
                    yield return folder.Path;
            }
        }

        private static IEnumerable<UICultureElement> GetAvailableCultures()
        {
            var section = ConfigurationManager.GetSection("uiCultureConfigurationSection") as UICultureConfigurationSection;
            if(section != null && section.UICultures != null)
            {
                foreach (UICultureElement culture in section.UICultures)
                    yield return culture;
            }
        }

        private static IEnumerable<RuleElement> GetRules()
        {
            var section = ConfigurationManager.GetSection("folderListenerConfigurationSection") as FolderListenerConfigurationSection;
            if (section != null && section.Rules != null)
            {
                foreach (RuleElement rule in section.Rules)
                    yield return rule;
            }
        }
    }
}
