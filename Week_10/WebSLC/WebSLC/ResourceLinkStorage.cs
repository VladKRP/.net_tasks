using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSLC
{
    public class ResourceLinkStorage
    {
        private ICollection<Uri> _downloadedLinks;

        public ResourceLinkStorage()
        {
            _downloadedLinks = new List<Uri>();
        }

        public void Add(Uri link)
        {
            _downloadedLinks.Add(link);
        }

        public void Clear()
        {
            _downloadedLinks = new List<Uri>();
        }

        public bool IsLinkDownloaded(Uri link)
        {
            bool isResourceAlreadyDownloaded = false;
            if (_downloadedLinks.Any(x => string.Equals(x.OriginalString, link.OriginalString)))
                isResourceAlreadyDownloaded = true;
            return isResourceAlreadyDownloaded;
        }
    }
}
