using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations
{
    public class NameChangeRuleElement: ConfigurationElement
    {
        [ConfigurationProperty("hasSerialNumber")]
        public bool HasSerialNumber
        {
            get { return (bool)base["hasSerialNumber"]; }
        }

        [ConfigurationProperty("hasLastMovingDate")]
        public bool HasLastMovingDate
        {
            get { return (bool)base["hasLastMovingDate"]; }
        }
    }
}
