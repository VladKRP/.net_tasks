using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations.UICulture
{
    public class UICultureElement: ConfigurationElement
    {
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationCollection(typeof(PhraseElement), AddItemName = "phrase")]
        [ConfigurationProperty("phrases")]
        public PhraseElementCollection Phrases
        {
            get
            {
                return (PhraseElementCollection)this["phrases"];
            }
        }

    }
}
