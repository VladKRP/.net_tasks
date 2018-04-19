using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace WebSLC
{
    public class WebsiteDownloader
    {
        private readonly string _destinationPath;

        private readonly LinkAnalyzer _linkAnalyzer;

        public EventHandler<EventArgs> DownloadStarted { get; set; }//change EventArg to custom Arg
        public EventHandler<EventArgs> DownloadEnded { get; set; }

        public EventHandler<EventArgs> ReferenceFound { get; set; }//change EventArg to custom Arg

        public EventHandler<EventArgs> DepthRestrictionFound { get; set; }
        public EventHandler<EventArgs> FormatRestrictionFound { get; set; }
        public EventHandler<EventArgs> DomainSwitchRestrictionFound { get; set; }

        public EventHandler<EventArgs> WebsiteDownloaded { get; set; }

        public WebsiteDownloader(string destinationPath, LinkAnalyzer linkAnalyzer)
        {
            _destinationPath = destinationPath;
            _linkAnalyzer = linkAnalyzer;
        }

        public async Task DownloadAsync(string url, int depth = 0) //tracing ?
        {
            DownloadStarted.Invoke(this, new EventArgs() { });
            var uri = new Uri(url);

            var page = await DownloadPage(url);
            var pageStringRepresentation = System.Text.Encoding.UTF8.GetString(page);
            var destinationFile = string.Format("{0}\\{1}", _destinationPath, uri.Segments.Last());
            if (_linkAnalyzer.IsHtmlPage(pageStringRepresentation))
                destinationFile += ".html";

            using (FileStream fileStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
            {
                await fileStream.WriteAsync(page, 0, page.Length);
            }


            if (depth > 0)//??
            {
                var links = _linkAnalyzer.GetWebsiteLinks(pageStringRepresentation);
                if (links.Any())
                {
                    links = links.Select(link => _linkAnalyzer.CorrectLink(uri.IdnHost, link));
                    foreach (var link in links)
                        await DownloadAsync(link, depth - 1);
                }
               
            }



            DownloadEnded.Invoke(this, new EventArgs() { });
        }

        public async Task<byte[]> DownloadPage(string url)
        {
            byte[] page = { };

            using(HttpClient client = new HttpClient())
            {
                page = await client.GetByteArrayAsync(url);
            }

            return page;
        }
    }
}
