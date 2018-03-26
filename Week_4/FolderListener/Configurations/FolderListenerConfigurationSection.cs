using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations
{
    public class FolderListenerConfigurationSection : ConfigurationSection
    {

        [ConfigurationProperty("applicationLanguage", DefaultValue = "en-US")]
        public string ApplicationLanguage
        {
            get { return (string)this["applicationLanguage"]; }
        }

        [ConfigurationCollection(typeof(RuleElement), AddItemName = "rule")]
        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules
        {
            get { return (RuleElementCollection)this["rules"]; }
        }

        [ConfigurationCollection(typeof(FolderElement), AddItemName = "folder")]
        [ConfigurationProperty("folders")]
        public FolderElementCollection Folders
        {
            get { return (FolderElementCollection)this["folders"]; }
        }

        [ConfigurationProperty("defaultFolder")]
        public FolderElement DefaultFolder
        {
            get { return (FolderElement)this["defaultFolder"]; }
        }

    }
}
