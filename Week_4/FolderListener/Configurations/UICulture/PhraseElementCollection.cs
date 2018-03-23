using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderListener.Configurations.UICulture
{
    public class PhraseElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PhraseElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PhraseElement)element).Type;
        }
    }
}
