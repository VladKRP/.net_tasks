using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace FolderListener.Configurations
{
    public class FolderElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderElement)element).Path;
        }
    }
}
