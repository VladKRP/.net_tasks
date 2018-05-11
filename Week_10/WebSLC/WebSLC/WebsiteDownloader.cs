using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace WebSLC
{
    public class WebsiteDownloader
    {
        private readonly string _destinationPath;

        private readonly LinkManager _linkManager;

        private readonly Regex fileNameCorrectingRegEx = new Regex("[/\\:?*\"<>|]+");

        public EventHandler<DownloadArgs> WebpageDownloadStarted { get; set; }
        public EventHandler<DownloadArgs> WebpageDownloadCompleted { get; set; }

        public EventHandler<EventArgs> LinkFound { get; set; }

        public EventHandler<RestrictionArgs> FormatRestrictionFound { get; set; }
        public EventHandler<RestrictionArgs> DomainSwitchRestrictionFound { get; set; }

        public WebsiteDownloader(string destinationPath, LinkManager linkManager)
        {
            _destinationPath = destinationPath;
            _linkManager = linkManager;
        }

        public async Task DownloadWebpageAsync(string url, int depth = 0)
        {
            var baseUri = new Uri(url);
            WebpageDownloadStarted.Invoke(this, new DownloadArgs() { Link = url, Time = DateTime.Now });
            var bytePage = await DownloadPageAsync(url);
            WebpageDownloadCompleted.Invoke(this, new DownloadArgs() { Link = url, Time = DateTime.Now });

            var pageLayout = System.Text.Encoding.UTF8.GetString(bytePage);

            byte[] pageLayoutWithLocalLinks;
            if (Path.HasExtension(url))
                pageLayoutWithLocalLinks = bytePage;
            else
                pageLayoutWithLocalLinks = System.Text.Encoding.UTF8.GetBytes(_linkManager.ReplacePageLinksToLocal(_destinationPath, pageLayout));


            var downloadFilePath = GenerateLocalFullFilePath(url, pageLayout);

            using (FileStream fileStream = new FileStream(downloadFilePath, FileMode.Create, FileAccess.Write))
                fileStream.Write(pageLayoutWithLocalLinks, 0, pageLayoutWithLocalLinks.Length);

            if (depth == 0)
            {
                var pageLinks = _linkManager.GetPageResourceLink(pageLayout);
                if (pageLinks.Any())
                {
                    //pageLinks = _linkManager.ProcessLinks(baseUri.DnsSafeHost, pageLinks);
                    await DownloadWebpageResourcesAsync(pageLinks, baseUri, 0);
                }

            }
            else if (depth > 0)
            {
                var pageLinks = _linkManager.GetPageLinks(pageLayout);
                if (pageLinks.Any())
                {
                    //pageLinks = _linkManager.ProcessLinks(baseUri.DnsSafeHost, pageLinks);
                    await DownloadWebpageResourcesAsync(pageLinks, baseUri, depth);
                }
            }

        }

        private async Task DownloadWebpageResourcesAsync(string pageLayout, Uri baseUri, Func<string, IEnumerable<string>> resourceFilterFunction, int depth)//??
        {
            var pageLinks = _linkManager.GetPageLinks(pageLayout);
            if (pageLinks.Any())
            {
                //pageLinks = _linkManager.ProcessLinks(baseUri.DnsSafeHost, pageLinks);
                await DownloadWebpageResourcesAsync(pageLinks, baseUri, depth);
            }
        }

        private async Task DownloadWebpageResourcesAsync(IEnumerable<string> pageLinks, Uri baseUri, int depth)
        {
            foreach (var link in pageLinks)
            {
                var linkUri = new Uri(link);

                LinkFound.Invoke(this, new EventArgs());
                if (_linkManager.IsLinkFormatForbidden(link))
                    FormatRestrictionFound?.Invoke(this, new RestrictionArgs() { Entity = Path.GetExtension(link) });
                else if (_linkManager.IsLinkDomainForbidden(baseUri, linkUri))
                    DomainSwitchRestrictionFound?.Invoke(this, new RestrictionArgs() { Entity = linkUri.DnsSafeHost });
                else
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

        private string GenerateLocalFullFilePath(string url, string pageLayout)
        {
            var localPath = _destinationPath;
            var processedFileName = "";
            if (HtmlAnalyzer.IsLayoutContainHtmlTag(pageLayout))
                processedFileName = HtmlAnalyzer.GetHtmlPageTitle(pageLayout) + ".html";
            else
            {
                var fileNameWithExtension = Path.GetFileName(url);
                if (string.IsNullOrWhiteSpace(fileNameWithExtension) || fileNameWithExtension == ".")
                    processedFileName = url;
                else
                    processedFileName = fileNameWithExtension;
            }
            localPath += fileNameCorrectingRegEx.Replace(processedFileName, "_");
            return localPath;
        }
    }
}
