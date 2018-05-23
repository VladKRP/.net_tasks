using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebSLC.Models;

namespace WebSLC
{
    public class FileSystemWebsiteSave
    {

        public string CreateLocalFileName(Uri url, byte[] resource)
        {
            Regex fileNameCorrectingRegEx = new Regex("[/\\:?*\"<>|]+");
            var layout = Encoding.Default.GetString(resource);

            var filename = "";
            if (HtmlAnalyzer.IsLayoutContainHtmlTag(layout))
                filename = CreateLocalFileNameForWebpage(url, layout);
            else
                filename = CreateLocalFileNameForResource(url);
            return DestinationPath + fileNameCorrectingRegEx.Replace(filename, "_");
        }

        private string CreateLocalFileNameForWebpage(Uri url, string layout)
        {
            string htmlPageFormat = ".html";
            string filename = "";

            var pageName = url.Segments.Last().Trim('\\', '.', ',', ' ', '/');
            if (!string.IsNullOrEmpty(pageName) && pageName != "/")
                filename = pageName;
            else
                filename = HtmlAnalyzer.GetHtmlPageTitle(layout);
            return filename + htmlPageFormat;
        }

        private string CreateLocalFileNameForResource(Uri url)
        {
            string filename = "";
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(url.OriginalString);

            if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
                filename = url.OriginalString;
            else
                filename = fileNameWithoutExtension.Trim('\\', '.', ',', ' ', '/') + Path.GetExtension(url.OriginalString);
            return filename;
        }

        public void Save(byte[] resource)
        {
            using (FileStream fileStream = new FileStream(DestinationPath, FileMode.Create, FileAccess.Write))
                fileStream.Write(resource, 0, resource.Length);
        }

    }
}
