using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations
{
    public class FileIgnoreTemplateCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileIgnoreTemplate();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileIgnoreTemplate)element).Template;
        }
    }
}
