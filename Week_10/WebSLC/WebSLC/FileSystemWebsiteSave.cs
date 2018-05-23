using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebSLC.Interfaces;
using WebSLC.Models;

namespace WebSLC
{
    public class FileSystemWebsiteSave: IWebResourceSave
    {
        public string DestinationPath { get; set; }

        public FileSystemWebsiteSave(string path)
        {
            DestinationPath = path;
        }

        public void Save(WebResourceBase entity)
        {
            var localpath = CreateLocalPath(entity);
            using (FileStream fileStream = new FileStream(localpath, FileMode.Create, FileAccess.Write))
                fileStream.Write(entity.Data, 0, entity.Data.Length);
        }

        private string CreateLocalPath(WebResourceBase entity)
        {
            Regex fileNameCorrectingRegEx = new Regex("[/\\:?*\"<>|]+");
            var filename = "";
            var webEntity = entity as WebPage;

            if(webEntity != null)
                filename = CreateLocalFileNameForWebpage(webEntity);
            else
                filename = CreateLocalFileNameForResource(entity.Url);
  
            return DestinationPath + fileNameCorrectingRegEx.Replace(filename, "_");   
        }

        private string CreateLocalFileNameForWebpage(WebPage page)
        {
            string htmlPageFormat = ".html";
            string filename = "";

            var pageName = page.Url.Segments.Last().Trim('\\', '.', ',', ' ', '/');
            if (!string.IsNullOrEmpty(pageName) && pageName != "/")
                filename = pageName;
            else
                filename = HtmlAnalyzer.GetHtmlPageTitle(page.Layout);
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
    }
}
