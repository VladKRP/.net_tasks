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
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            string[] resources = { "https://www.epam.com/" };
            const string defaultPath = @"D:\CDP\.net_tasks\Week_10\Downloads\";



            HtmlLinkManager linkAnalyzer = new HtmlLinkManager(domainSwitchParameter: DomainSwitchParameter.WithoutRestrictions);


            WebsiteDownloader downloader = new WebsiteDownloader(defaultPath, linkAnalyzer);

            downloader.WebpageDownloadStarted += (object o, DownloadArgs arg) => System.Console.WriteLine($"\nDownload started\nLink: {arg.Link}\nTime: {arg.Time}\nDepth:{arg.Depth}");
            downloader.WebpageDownloadCompleted += (object o, DownloadArgs arg) => System.Console.WriteLine($"\nDownload ended\nLink: {arg.Link}\nTime: {arg.Time}\n");

            downloader.DownloadWebpageAsync(resources[0], 1).Wait();

            // downloader.DownloadWebpageAsync(resources[0], 1).Wait();

            //string[] excludedFormats = { ".jpg" };
            //LinkManager linkAnalyzerWithRestrictions = new LinkManager(excludedFormats, DomainSwitchParameter.CurrentDomain);

            //WebsiteDownloader downloaderWithRestrictions = new WebsiteDownloader(defaultPath, linkAnalyzerWithRestrictions);

            //downloaderWithRestrictions.WebpageDownloadStarted += (object o, DownloadArgs arg) => System.Console.WriteLine($"\nDownload started\nLink: {arg.Link}\nTime: {arg.Time}\nDepth:{arg.Depth}");
            //downloaderWithRestrictions.WebpageDownloadCompleted += (object o, DownloadArgs arg) => System.Console.WriteLine($"\nDownload ended\nLink: {arg.Link}\nTime: {arg.Time}\n");

            //downloaderWithRestrictions.DownloadWebpageAsync(resources[1], 1).Wait();

            Console.ReadLine();
        }
    }
}
