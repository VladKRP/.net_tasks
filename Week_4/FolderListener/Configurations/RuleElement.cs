using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace FolderListener.Configurations
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("template", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public Regex Template {
            get { return (Regex)base["template"]; }
            set { this["template"] = value; } }

        [ConfigurationProperty("destinationFolder", IsRequired = true)]
        public string DestinationFolder{
            get { return (string)base["destinationFolder"]; }
            set { this["destinationFolder"] = value; }
        }

        [ConfigurationProperty("fileNameChangeRule")]
        public ResultFileNameParameters ResultFileName
        {
            get { return (ResultFileNameParameters)base["fileNameChangeRule"]; }
            set { this["fileNameChangeRule"] = value; }
        }

    }
}
