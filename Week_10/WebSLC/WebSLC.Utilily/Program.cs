using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebSLC.Utilily
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            string[] resources = { "https://habrahabr.ru/company/infopulse/blog/261097/" };
            const string defaultPath = @"D:\CDP\.net_tasks\Week_10\Downloads\";

           string[] excludedFormats = {};
        
            HtmlLinkProcessingHelper linkAnalyzer = new HtmlLinkProcessingHelper(excludedFormats, DomainSwitchParameter.WithoutRestrictions);
            WebsiteDownloader downloader = new WebsiteDownloader(defaultPath, linkAnalyzer);

            //string[] excludedFormats = { ".png", ".jpg" };
            //LinkProcessingHelper linkAnalyzerDomainRestriction = new LinkProcessingHelper(excludedFormats, DomainSwitchParameter.CurrentDomain);
            //WebsiteDownloader downloader = new WebsiteDownloader(defaultPath, linkAnalyzerDomainRestriction);

            downloader.WebpageDownloadStarted += (object o, DownloadArgs arg) => System.Console.WriteLine($"\nDownload started\nLink: {arg.Link}\nTime: {arg.Time}\n");
            downloader.WebpageDownloadCompleted += (object o, DownloadArgs arg) => System.Console.WriteLine($"\nDownload ended\nLink: {arg.Link}\nTime: {arg.Time}\n");
            downloader.FormatRestrictionFound += (object o, RestrictionArgs arg) => System.Console.WriteLine($"Link extension restricted: {arg.Entity}");
            downloader.LinkFound += (object o, EventArgs arg) => Console.WriteLine("Link found");
            downloader.DomainSwitchRestrictionFound += (object o, RestrictionArgs arg) => Console.WriteLine($"Domain restricted: {arg.Entity}");

            downloader.DownloadWebpageAsync(resources[0], 2).Wait();

        }
    }
}
