using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace FolderListener.Configurations
{
    public class FolderElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)base["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("path", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { this["path"] = value; }
        }
        
    }
}
