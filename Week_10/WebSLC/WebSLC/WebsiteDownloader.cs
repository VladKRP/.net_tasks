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
using WebSLC.Interfaces;
using WebSLC.Models;

namespace WebSLC
{
    public class WebsiteDownloader
    {
        private readonly HtmlLinkManager _linkManager;

        private readonly ILinkStorage _resourceLinkStorage;

        private readonly IWebResourceSave _resourceRecorder;

        public EventHandler<DownloadArgs> WebpageDownloadStarted { get; set; }
        public EventHandler<DownloadArgs> WebpageDownloadCompleted { get; set; }
        public EventHandler<DownloadErrorArgs> DownloadError { get; set; }

        public WebsiteDownloader(HtmlLinkManager linkManager, IWebResourceSave resourceRecorder, ILinkStorage linkStorage = null)
        {
            _resourceLinkStorage = linkStorage ?? new MemoryLinkStorage();
            _resourceRecorder = resourceRecorder;
            _linkManager = linkManager;
        }

        public async Task DownloadWebResourceAsync(Uri url, int depth = 0)
        {
            if (depth >= 0 && !_resourceLinkStorage.Exists(url))
            {
                WebpageDownloadStarted.Invoke(this, new DownloadArgs() { Link = url, Depth = depth, Time = DateTime.Now });
                try
                {
                    byte[] downloadedResource = await DownloadResourceAsync(url);
                    WebpageDownloadCompleted.Invoke(this, new DownloadArgs() { Link = url, Depth = depth, Time = DateTime.Now });

                    _resourceLinkStorage.Add(url);

                    var webResource = WebResourceBaseFactory.Create(url, downloadedResource);
                    var webPage = webResource as WebPage;
                    if(webPage != null)
                        webResource = _linkManager.GetPageWithLocalLinks(webPage);

                    _resourceRecorder.Save(webResource);

                    if(webPage != null)
                    {
                        if (depth == 0)
                        {
                            var pageLinks = _linkManager.GetPageResourceLink(webPage);
                            await DownloadInnerWebResourcesAsync(pageLinks, url, 1);
                        }
                        else if (depth > 0)
                        {
                            var pageLinks = _linkManager.GetPageLinks(webPage);
                            await DownloadInnerWebResourcesAsync(pageLinks, url, depth);
                        }
                    }                
                }
                catch(HttpRequestException exc)
                {
                    DownloadError?.Invoke(this, new DownloadErrorArgs() { Link = url, ExceptionMessage = exc.Message });
                } 
            }
        }

        private async Task DownloadInnerWebResourcesAsync(IEnumerable<string> pageLinks, Uri baseUri, int depth)
        {
            if (pageLinks.Any())
            {
                foreach (var link in pageLinks)
                {
                    var linkUri = new Uri(link);
                    if (!_linkManager.IsLinkDomainForbidden(baseUri, linkUri))
                        await DownloadWebResourceAsync(new Uri(link), depth - 1);
                }
            }         
        }

        private async Task<byte[]> DownloadResourceAsync(Uri url)
        {
            byte[] page = { };

            using (HttpClient client = new HttpClient())
                page = await client.GetByteArrayAsync(url);

            return page;
        }

    }
}
