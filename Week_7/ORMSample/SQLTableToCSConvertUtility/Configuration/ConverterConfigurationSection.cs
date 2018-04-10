using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SQLTableToCSConvertUtility.Configuration
{
    public class ConverterConfigurationSection: ConfigurationSection
    {
        [ConfigurationProperty("domainNamespace")]
        public string DomainNamespace
        {
            get { return (string)base["domainNamespace"]; }
        }

        [ConfigurationProperty("sqlFilesPath")]
        public string SqlFilesPath
        {
            get { return (string)base["sqlFilesPath"]; }
        }

        [ConfigurationProperty("outputFilesPath")]
        public string OuputFilesPath
        {
            get { return (string)base["outputFilesPath"]; }
        }
    }
}
