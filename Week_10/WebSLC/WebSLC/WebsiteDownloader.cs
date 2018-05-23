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
using WebSLC.Args;

namespace WebSLC
{
    public class WebsiteDownloader
    {
        private readonly HtmlLinkManager _linkManager;

        private readonly ResourceLinkStorage _resourceLinkStorage;

        private readonly FileSystemWebsiteSave _websiteSave;

        private readonly string _path;

        public EventHandler<DownloadArgs> WebpageDownloadStarted { get; set; }
        public EventHandler<DownloadArgs> WebpageDownloadCompleted { get; set; }
        public EventHandler<DownloadErrorArgs> DownloadError { get; set; }

        public WebsiteDownloader(string path, HtmlLinkManager linkManager, FileSystemWebsiteSave websiteSave = null, ResourceLinkStorage linkStorage = null)
        {
            _resourceLinkStorage = linkStorage ?? new ResourceLinkStorage();
            _websiteSave = websiteSave ?? new FileSystemWebsiteSave();
            _path = path;
            _linkManager = linkManager;
            
        }

        public async Task DownloadWebpageAsync(Uri url, int depth = 0)
        {
            if (depth >= 0 && !_resourceLinkStorage.IsLinkDownloaded(url))
            {
                WebpageDownloadStarted.Invoke(this, new DownloadArgs() { Link = url, Depth = depth, Time = DateTime.Now });
                try
                {
                    byte[] downloadedPage = await DownloadPageAsync(url);
                    WebpageDownloadCompleted.Invoke(this, new DownloadArgs() { Link = url, Depth = depth, Time = DateTime.Now });

                    _resourceLinkStorage.Add(url);

                    var localWebsite = _linkManager.GetPageWithLocalLinks(url, downloadedPage);
                    var localPath = _websiteSave.CreateLocalFileName(url, _path, downloadedPage);
                    _websiteSave.Save(localPath, localWebsite);

                    if (depth == 0)
                    {
                        var pageLinks = _linkManager.GetPageResourceLink(downloadedPage);
                        if (pageLinks.Any())
                        {
                            pageLinks = _linkManager.ProcessLinks(url.DnsSafeHost, pageLinks);
                            await DownloadWebpageResourcesAsync(pageLinks, url, 1);
                        }

                    }
                    else if (depth > 0)
                    {
                        var pageLinks = _linkManager.GetPageLinks(downloadedPage);
                        if (pageLinks.Any())
                        {
                            pageLinks = _linkManager.ProcessLinks(url.DnsSafeHost, pageLinks);
                            await DownloadWebpageResourcesAsync(pageLinks, url, depth);
                        }
                    }
                }
                catch(HttpRequestException exc)
                {
                    DownloadError?.Invoke(this, new DownloadErrorArgs() { Link = url, ExceptionMessage = exc.Message });
                } 
            }
        }

        private async Task DownloadWebpageResourcesAsync(IEnumerable<string> pageLinks, Uri baseUri, int depth)
        {
            foreach (var link in pageLinks)
            {
                var linkUri = new Uri(link);
                if (!_linkManager.IsLinkDomainForbidden(baseUri, linkUri))
                    await DownloadWebpageAsync(new Uri(link), depth - 1);
            }
        }

        private async Task<byte[]> DownloadPageAsync(Uri url)
        {
            byte[] page = { };

            using (HttpClient client = new HttpClient())
                page = await client.GetByteArrayAsync(url);

            return page;
        }

    }
}
