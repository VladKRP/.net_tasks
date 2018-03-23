using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations.UICulture
{
    public class PhraseElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsKey = true)]
        public string Type
        {
            get { return (string)base["type"]; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)base["value"]; }
        }
    }
}
