using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace WebSLC
{
    public class LinkAnalyzer
    {

        private readonly IEnumerable<string> _linkElements;

        private readonly IEnumerable<string> _linkAttributes;

        private readonly IEnumerable<string> _excludedFormats;
        //DomainSwitchParameter domainSwitchParameter = DomainSwitchParameter.CurrentDomain,
        //    , IEnumerable<string> formatRestrictions = null 

        public LinkAnalyzer()
        {
            _linkElements = new List<string>() { "link", "script", "image" };
            _linkAttributes = new List<string>() { "src", "href" };
        }

        public LinkAnalyzer(IEnumerable<string> excludedFromSearchFormats)
        {
            _linkElements = new List<string>() { "link", "script", "image" };
            _linkAttributes = new List<string>() { "src", "href" };
            _excludedFormats = excludedFromSearchFormats;
        }

        public bool IsHtmlPage(string siteLayout)
        {
            var document = new HtmlDocument();
            document.LoadHtml(siteLayout);
            return document.DocumentNode.ChildNodes.Any(child => child.Name == "html");
        }

        public string PageTitle(string siteLayout)
        {
            var document = new HtmlDocument();
            document.LoadHtml(siteLayout);
            return document.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
        }

        public IEnumerable<string> GetWebsiteLinks(string siteLayout)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(siteLayout);
            var documentLinks = document.DocumentNode
                                        .Descendants()
                                        .Where(desc => _linkElements.Any(lelem => lelem == desc.Name))
                                        .SelectMany(link => link.Attributes)
                                        .Where(attribute => _linkAttributes.Any(lattr => lattr == attribute.Name))
                                        .Select(attribute => attribute.Value).Distinct();
            if(_excludedFormats != null)
                documentLinks = documentLinks.Where(link => _excludedFormats.All(format => Path.GetExtension(link) != format));

            return documentLinks;
        }

        public string CorrectLink(string site, string link)
        {
            string result = null;
            if (!string.IsNullOrEmpty(link))
            {
                if (link.StartsWith("//"))
                    result = "http:" + link;
                else if (link.StartsWith("/"))
                    result = site + link;
            }
            return result;
        }
    }
}
