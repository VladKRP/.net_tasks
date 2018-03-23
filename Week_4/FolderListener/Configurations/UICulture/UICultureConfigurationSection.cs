using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations.UICulture
{
    public class UICultureConfigurationSection:ConfigurationSection
    {
        [ConfigurationCollection(typeof(UICultureElement), AddItemName = "culture")]
        [ConfigurationProperty("uiCultures")]
        public UICultureElementCollection UICultures
        {
            get { return (UICultureElementCollection)this["uiCultures"]; }
        }
    }
}
