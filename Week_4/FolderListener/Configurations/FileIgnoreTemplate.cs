using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations
{
    public class FileIgnoreTemplate : ConfigurationElement
    {

        [ConfigurationProperty("template", IsRequired = true)]
        public string Template {
            get { return (string)base["template"]; }
            set { base["template"] = value; }
        }

    }
}
