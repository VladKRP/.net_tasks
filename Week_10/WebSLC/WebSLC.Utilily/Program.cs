using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSLC.Utilily
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] resources = { "https://ru.wikipedia.org/wiki/Wget" };
            const string defaultPath = @"D:\CDP\.net_tasks\Week_10\Downloads";

            string[] excludedFormats = { ".png" };

            LinkAnalyzer linkAnalyzer = new LinkAnalyzer(excludedFormats);
            WebsiteDownloader downloader = new WebsiteDownloader(defaultPath, linkAnalyzer);

            downloader.DownloadStarted += (object o, EventArgs arg) => System.Console.WriteLine("Download started");
            downloader.DownloadEnded += (object o, EventArgs arg) => System.Console.WriteLine("Download ended");

            downloader.DownloadAsync(resources[0],1).Wait();

        }
    }
}
