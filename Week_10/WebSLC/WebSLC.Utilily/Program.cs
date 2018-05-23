using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSLC.Args;

namespace WebSLC.Utilily
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Uri[] resources = { new Uri("https://www.epam.com/"),
                                new Uri("https://www.epam.com/what-we-do/"),
                                new Uri("https://habr.com/company/goto/blog/345978/") };
            const string defaultPath = @"D:\CDP\.net_tasks\Week_10\Downloads\";

            HtmlLinkManager linkManager = new HtmlLinkManager(domainSwitchParameter: DomainSwitchParameter.WithoutRestrictions);
            WebsiteDownloader downloader = new WebsiteDownloader(defaultPath + "test_1\\", linkManager);
            downloader = AddHandlers(downloader);

            downloader.DownloadWebpageAsync(resources[0], 0).Wait();

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            linkManager = new HtmlLinkManager(domainSwitchParameter: DomainSwitchParameter.BelowSourceUrlPath);

            downloader = new WebsiteDownloader(defaultPath + "test_2\\", linkManager);
            downloader = AddHandlers(downloader);

            downloader.DownloadWebpageAsync(resources[1], 1).Wait();

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            string[] excludedFormats = { ".jpg" };
            linkManager = new HtmlLinkManager(excludedFormats, DomainSwitchParameter.CurrentDomain);
            downloader = new WebsiteDownloader(defaultPath + "test_3\\", linkManager);
            downloader = AddHandlers(downloader);

            downloader.DownloadWebpageAsync(resources[2], 0).Wait();

            Console.ReadLine();
        }

        static WebsiteDownloader AddHandlers(WebsiteDownloader downloader)
        {
            downloader.WebpageDownloadStarted += (object o, DownloadArgs arg) =>
                Console.WriteLine($"\nDownload started\nLink: {arg.Link.OriginalString}\nTime: {arg.Time}\nDepth:{arg.Depth}");
            downloader.WebpageDownloadCompleted += (object o, DownloadArgs arg) =>
                Console.WriteLine($"\nDownload ended\nLink: {arg.Link.OriginalString}\nTime: {arg.Time}\n");
            downloader.DownloadError += (object o, DownloadErrorArgs arg) =>
                Console.WriteLine($"\nError - '{arg.ExceptionMessage}' occured during downloading from:\n {arg.Link}");
            return downloader;
        }
    }
}
