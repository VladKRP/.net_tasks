using HtmlAgilityPack;
using System.Linq;

namespace WebSLC
{
    public class HtmlAnalyzer
    {
        public static bool IsLayoutContainHtmlTag(string siteLayout)
        {
            var document = new HtmlDocument();
            document.LoadHtml(siteLayout);
            return document.DocumentNode.ChildNodes.Any(child => child.Name == "html");
        }

        public static string GetHtmlPageTitle(string siteLayout)
        {
            var document = new HtmlDocument();
            document.LoadHtml(siteLayout);
            return document.DocumentNode?.SelectSingleNode("/html/head/title")?.InnerText;
        }  
    }
}
