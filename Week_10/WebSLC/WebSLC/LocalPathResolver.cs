using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebSLC
{
    public class LocalPathResolver
    {
        private readonly Regex fileNameCorrectingRegEx = new Regex("[/\\:?*\"<>|]+");

        public string CreateLocalPath(string destinationPath, string url, string pageLayout)
        {
            var localPath = destinationPath;
            var htmlPageFormat = ".html";

            var processedFileName = "";
            if (HtmlAnalyzer.IsLayoutContainHtmlTag(pageLayout))
            {
                var uri = new Uri(url);
                var pageName = uri.Segments.Last();
                if (!string.IsNullOrEmpty(pageName) && pageName != "/")
                    processedFileName = pageName;
                else
                    processedFileName = HtmlAnalyzer.GetHtmlPageTitle(pageLayout);
                processedFileName += htmlPageFormat;
            }
  
            else
            {
                var fileNameWithExtension = Path.GetFileName(url);
                if (string.IsNullOrWhiteSpace(fileNameWithExtension))
                    processedFileName = url;
                else
                    processedFileName = fileNameWithExtension;
            }
            localPath += fileNameCorrectingRegEx.Replace(processedFileName, "_");
            return localPath;
        }

    }
}
