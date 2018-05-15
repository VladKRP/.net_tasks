using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;

namespace WebSLC
{
    public class WebsiteDownloader
    {
        private readonly string _destinationPath;

        private readonly HtmlLinkManager _linkManager;

        private readonly ResourceLinkStorage _resourceLinkStorage;

        public EventHandler<DownloadArgs> WebpageDownloadStarted { get; set; }
        public EventHandler<DownloadArgs> WebpageDownloadCompleted { get; set; }

        public WebsiteDownloader(string destinationPath, HtmlLinkManager linkManager)
        {
            _resourceLinkStorage = new ResourceLinkStorage();
            _destinationPath = destinationPath;
            _linkManager = linkManager;
        }

        public async Task DownloadWebpageAsync(string url, int depth = 0)
        {
            if (depth >= 0 && !_resourceLinkStorage.IsResourceAlreadyDownloaded(url))
            {
                var baseUri = new Uri(url);

                WebpageDownloadStarted.Invoke(this, new DownloadArgs() { Link = url, Depth = depth, Time = DateTime.Now });
                var downloadedPage = await DownloadPageAsync(url);
                WebpageDownloadCompleted.Invoke(this, new DownloadArgs() { Link = url, Depth = depth, Time = DateTime.Now });

                _resourceLinkStorage.AddResourceLinkToDownloaded(url);

                var pageLayoutStringRepresentation = Encoding.UTF8.GetString(downloadedPage);

                var localPage = ResolveLocalLinks(url, downloadedPage);
                var resourceFullPath = new LocalPathResolver().CreateLocalPath(_destinationPath, url, pageLayoutStringRepresentation);
                SaveWebResourceOnDevice(resourceFullPath, localPage);

                if (depth == 0)
                {
                    var pageLinks = _linkManager.GetPageResourceLink(pageLayoutStringRepresentation);
                    if (pageLinks.Any())
                    {
                        pageLinks = _linkManager.ProcessLinks(baseUri.DnsSafeHost, pageLinks);
                        await DownloadWebpageResourcesAsync(pageLinks, baseUri, 1);
                    }

                }
                else if (depth > 0)
                {
                    var pageLinks = _linkManager.GetPageLinks(pageLayoutStringRepresentation);
                    if (pageLinks.Any())
                    {
                        pageLinks = _linkManager.ProcessLinks(baseUri.DnsSafeHost, pageLinks);
                        await DownloadWebpageResourcesAsync(pageLinks, baseUri, depth);
                    }
                }
            }
        }

        

        private byte[] ResolveLocalLinks(string url, byte[] resource)
        {
            if (!Path.HasExtension(url))
            {
               resource = Encoding.UTF8.GetBytes(_linkManager.ReplacePageLinksToLocal(resource));
            }
                
            return resource;
        }

        private void SaveWebResourceOnDevice(string path, byte[] resource)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                fileStream.Write(resource, 0, resource.Length);
        }

        private async Task DownloadWebpageResourcesAsync(IEnumerable<string> pageLinks, Uri baseUri, int depth)
        {
            foreach (var link in pageLinks)
            {
                var linkUri = new Uri(link);
                if (!_linkManager.IsLinkDomainForbidden(baseUri, linkUri))
                    await DownloadWebpageAsync(link, depth - 1);
            }
        }

        private async Task<byte[]> DownloadPageAsync(string url)
        {
            byte[] page = { };

            using (HttpClient client = new HttpClient())
                page = await client.GetByteArrayAsync(url);

            return page;
        }

    }
}
