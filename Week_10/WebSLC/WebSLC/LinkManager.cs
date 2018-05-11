using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace WebSLC
{
    public class LinkManager
    {
        //local href

        private readonly IEnumerable<string> _linkElements = new List<string>() { "link", "script", "img", "a", };

        private readonly IEnumerable<string> _linkAttributes = new List<string>() { "src", "href" };

        private readonly IEnumerable<string> _excludedExtensions;

        private readonly DomainSwitchParameter _domainSwitchParameter;

        public LinkManager() { }

        public LinkManager(IEnumerable<string> excludedFromSearchExtensions,
            DomainSwitchParameter domainSwitchParameter = DomainSwitchParameter.WithoutRestrictions)
        {
            _excludedExtensions = excludedFromSearchExtensions;
            _domainSwitchParameter = domainSwitchParameter;
        }

        private IEnumerable<HtmlNode> GetPageLinkNodes(string siteLayout)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(siteLayout);
            return document.DocumentNode.Descendants()
                                        .Where(desc => _linkElements.Any(lelem => lelem == desc.Name));
        }

        public IEnumerable<string> GetPageLinks(string siteLayout)
        {
            return GetPageLinkNodes(siteLayout).SelectMany(link => link.Attributes)
                                               .Where(attribute => _linkAttributes.Any(lattr => lattr == attribute.Name))
                                               .Select(attribute => attribute.Value)
                                               .Distinct()
                                               .Where(link => !IsLinkFormatForbidden(link) && !IsLinkAnchor(link));
        }

        public IEnumerable<string> GetPageResourceLink(string siteLayout)
        {
            return GetPageLinkNodes(siteLayout).Where(link => link.Name != "a")
                                                  .SelectMany(link => link.Attributes)
                                                  .Where(attribute => _linkAttributes.Any(lattr => lattr == attribute.Name))
                                                  .Select(attribute => attribute.Value).Distinct();
        }

        public bool IsLinkFormatForbidden(string link)
        {
            var linkExtension = Path.GetExtension(link);
            if (linkExtension == null)
                return false;
            return _excludedExtensions.Any(extension => extension == linkExtension);
        }

        public bool IsLinkAnchor(string link)
        {
            bool isAnchor = false;
            if (link.StartsWith("#"))
                isAnchor = true;
            return isAnchor;
        }

        public bool IsLinkDomainForbidden(Uri baseLink, Uri innerLink)
        {
            bool isLinkDomainForbidden = false;

            if (DomainSwitchParameter.CurrentDomain.Equals(_domainSwitchParameter) && !string.Equals(baseLink.DnsSafeHost, innerLink.DnsSafeHost))
                isLinkDomainForbidden = true;

            return isLinkDomainForbidden;

        }

        public string ReplaceLinkWebPathToLocalPath(string path, string link)
        {
            return path + Path.GetFileName(link);
        }

        public string ReplacePageLinksToLocal(string path, string siteLayout)
        {
            var document = new HtmlDocument();
            document.LoadHtml(siteLayout);
            var descendants = document.DocumentNode
                                        .Descendants()
                                        .Where(desc => _linkElements.Any(lelem => lelem == desc.Name));
            foreach (var descendant in descendants)
            {
                if (descendant.HasAttributes)
                {
                    if (descendant.Attributes["src"] != null)
                        descendant.Attributes["src"].Value = ReplaceLinkWebPathToLocalPath(path, descendant.Attributes["src"].Value);
                    else if (descendant.Attributes["href"] != null)
                        descendant.Attributes["href"].Value = ReplaceLinkWebPathToLocalPath(path, descendant.Attributes["href"].Value);
                }
            }
            return document.DocumentNode.OuterHtml;
        }

        //public string ProcessLink(string domain, string link)
        //{
        //    string result = null;
        //    if (!string.IsNullOrEmpty(link))
        //    {
        //        if (link.StartsWith("//"))
        //            result = "http:" + link;
        //        else if (link.StartsWith("/"))
        //            result = "http://" + domain + link;
        //        else if (link.StartsWith("http"))
        //            result = link;
        //    }
        //    return result;
        //}

        //public IEnumerable<string> ProcessLinks(string domain, IEnumerable<string> links)
        //{
        //    IEnumerable<string> processedLinks = Enumerable.Empty<string>();
        //    if(links.Any())
        //        processedLinks = links.Select(link => ProcessLink(domain, link)).Where(link => link != null);
        //    return processedLinks;
        //}
    }
}
