﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FolderListener.Configurations
{
    public class RuleElement:ConfigurationElement
    {
        [ConfigurationProperty("template", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public string Template
        {
            get { return (string)base["template"]; }
            set { base["template"] = value; }
        }

        [ConfigurationProperty("destinationFolder", IsRequired = true)]
        public string DestinationFolder
        {
            get { return (string)base["destinationFolder"]; }
            set { base["destinationFolder"] = value; }
        }

        [ConfigurationProperty("nameChangeRule")]
        public NameChangeRule NameChangeRule
        {
            get { return (NameChangeRule)base["nameChangeRule"]; }
            set { base["nameChangeRule"] = value; }
        }
    }

    public enum NameChangeRule
    {
        SerialNumber,
        LastModifyDate
    }
}
