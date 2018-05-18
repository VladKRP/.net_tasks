using FolderListener.Configurations;
using System.Collections.Generic;

namespace FolderListener.Extensions
{
    public static class FolderListenerConfigurationSectionExtensions
    {
        public static IEnumerable<FolderElement> GetFolders(this FolderListenerConfigurationSection configurations)
        {
            var folders = configurations.Folders;
            if (folders != null)
            {
                foreach (FolderElement folder in folders)
                    yield return folder;
            }
        }

        public static IEnumerable<RuleElement> GetRules(this FolderListenerConfigurationSection configurations)
        {
            var rules = configurations.Rules;
            if (rules != null)
            {
                foreach (RuleElement rule in rules)
                    yield return rule;
            }
        }

        public static IEnumerable<FileIgnoreTemplate> GetIgnoreTemplates(this FolderListenerConfigurationSection configurations)
        {
            var ignoreTemplates = configurations.IgnoreTemplates;
            if (ignoreTemplates != null)
            {
                foreach (FileIgnoreTemplate ignoreTemplate in ignoreTemplates)
                    yield return ignoreTemplate;
            }
        }

    }
}
