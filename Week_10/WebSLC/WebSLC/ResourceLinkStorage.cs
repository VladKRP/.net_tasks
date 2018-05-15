using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSLC
{
    public class ResourceLinkStorage
    {
        private ICollection<string> _downloadedLinks;

        public ResourceLinkStorage()
        {
            _downloadedLinks = new List<string>();
        }

        public void AddResourceLinkToDownloaded(string link)
        {
            _downloadedLinks.Add(link);
        }

        public bool IsResourceAlreadyDownloaded(string link)
        {
            bool isResourceAlreadyDownloaded = false;
            if (_downloadedLinks.Any(x => x == link))
                isResourceAlreadyDownloaded = true;
            return isResourceAlreadyDownloaded;
        }

        public void ClearResourceLinkStorage()
        {
            _downloadedLinks = new List<string>();
        }
    }
}
