using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations.UICulture
{
    public class UICultureElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UICultureElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UICultureElement)element).Name;
        }
    }
}
