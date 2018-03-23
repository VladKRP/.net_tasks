using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations.WatchFolders
{
    public class WatchFolderConfigurationSection : ConfigurationSection
    {
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