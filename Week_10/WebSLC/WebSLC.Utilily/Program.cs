using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSLC.Args;
using WebSLC.Interfaces;
using WebSLC.Models;

namespace WebSLC.Utilily
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultPath = @"D:\CDP\.net_tasks\Week_10\Downloads\";

            Uri[] resources = {
                                    new Uri("https://www.epam.com/"),
                                    new Uri("https://www.epam.com/what-we-do/"),
                                    new Uri("https://habr.com/company/goto/blog/345978/"),
                                    new Uri("https://msdn.microsoft.com/ru-ru/library/zdtaw1bw(v=vs.110).aspx")
                              };

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            HtmlLinkManager linkManager = new HtmlLinkManager(domainSwitchParameter: DomainSwitchParameter.WithoutRestrictions);
            FileSystemWebsiteSave resourceRecorder = new FileSystemWebsiteSave(defaultPath + "test_1\\");
            WebsiteDownloader downloader = new WebsiteDownloader(linkManager, resourceRecorder);
            downloader = AddHandlers(downloader);

            downloader.DownloadWebResourceAsync(resources[0], 0).Wait();

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            linkManager = new HtmlLinkManager(domainSwitchParameter: DomainSwitchParameter.BelowSourceUrlPath);

            resourceRecorder.DestinationPath = defaultPath + "test_2\\";
            downloader = new WebsiteDownloader(linkManager, resourceRecorder);
            downloader = AddHandlers(downloader);

            downloader.DownloadWebResourceAsync(resources[1], 1).Wait();

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            string[] excludedFormats = { ".jpg" };
            resourceRecorder.DestinationPath = defaultPath + "test_3\\";
            linkManager = new HtmlLinkManager(excludedFormats, DomainSwitchParameter.CurrentDomain);
            downloader = new WebsiteDownloader(linkManager, resourceRecorder);
            downloader = AddHandlers(downloader);

            downloader.DownloadWebResourceAsync(resources[2], 0).Wait();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;

            resourceRecorder.DestinationPath = defaultPath + "test_4\\";
            linkManager = new HtmlLinkManager(domainSwitchParameter: DomainSwitchParameter.WithoutRestrictions);
            downloader = new WebsiteDownloader(linkManager, resourceRecorder);
            downloader = AddHandlers(downloader);

            downloader.DownloadWebResourceAsync(resources[3], 1).Wait();

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
