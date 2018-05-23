using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSLC.Interfaces;

namespace WebSLC
{
    public class MemoryLinkStorage:ILinkStorage
    {
        private ICollection<Uri> _links;

        public MemoryLinkStorage()
        {
            _links = new List<Uri>();
        }

        public void Add(Uri link)
        {
            _links.Add(link);
        }

        public void Clear()
        {
            _links = new List<Uri>();
        }

        public bool Exists(Uri link)
        {
            bool exists = false;
            if (_links.Any(x => string.Equals(x.OriginalString, link.OriginalString)))
                exists = true;
            return exists;
        }
    }
}
