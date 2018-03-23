using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FolderListener.Configurations.FolderListenerRules
{
    public class RuleElement:ConfigurationElement
    {
        [ConfigurationProperty("template", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public string Template
        {
            get { return (string)base["template"]; }
            set { this["template"] = value; }
        }

        [ConfigurationProperty("destinationFolder", IsRequired = true)]
        public string DestinationFolder
        {
            get { return (string)base["destinationFolder"]; }
            set { this["destinationFolder"] = value; }
        }

        //[ConfigurationProperty("fileNameChangeRule")]
        //public ResultFileNameParameters ResultFileName
        //{
        //    get { return (ResultFileNameParameters)base["fileNameChangeRule"]; }
        //    set { this["fileNameChangeRule"] = value; }
        //}
    }
}
