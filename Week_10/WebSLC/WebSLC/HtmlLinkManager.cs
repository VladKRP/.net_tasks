using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using WebSLC.Models;

namespace WebSLC
{
    public class HtmlLinkManager
    {

        protected readonly IEnumerable<string> _linkElements = new List<string>() { "link", "script", "img", "a", };

        protected readonly IEnumerable<string> _linkAttributes = new List<string>() { "src", "href" };

        private readonly IEnumerable<string> _excludedExtensions;

        private readonly DomainSwitchParameter _domainSwitchParameter;

        public HtmlLinkManager()
        {
            _excludedExtensions = new List<string>();
            _domainSwitchParameter = DomainSwitchParameter.WithoutRestrictions;
        }

        public HtmlLinkManager(IEnumerable<string> excludedFromSearchExtensions = null,
            DomainSwitchParameter domainSwitchParameter = DomainSwitchParameter.WithoutRestrictions)
        {
            _excludedExtensions = excludedFromSearchExtensions ?? new List<string>();
            _domainSwitchParameter = domainSwitchParameter;
        }

        private IEnumerable<HtmlNode> GetPageLinkNodes(WebPage page)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(page.Layout);
            return document.DocumentNode.Descendants()
                                        .Where(desc => _linkElements.Any(lelem => lelem == desc.Name));
        }

        private IEnumerable<string> GetUniqueLinksThatPassRestrictions(IEnumerable<HtmlNode> links)
        {
            return links.SelectMany(link => link.Attributes)
                        .Where(attribute => _linkAttributes.Any(lattr => lattr == attribute.Name))
                        .Select(attribute => attribute.Value)
                        .Distinct()
                        .Where(link => !IsLinkFormatForbidden(link) && !IsLinkAnchor(link));
        }

        public IEnumerable<string> GetPageLinks(WebPage page)
        {
            if (page == null)
                throw new ArgumentNullException("Page must be not null.");

            var pageLinks = GetPageLinkNodes(page);
            var uniqueLinks = GetUniqueLinksThatPassRestrictions(pageLinks);
            return ProcessLinks(page.Url.DnsSafeHost, uniqueLinks);
        }

        public IEnumerable<string> GetPageResourceLink(WebPage page)
        {
            if (page == null)
                throw new ArgumentNullException("Page must be not null.");

            var pageLinks = GetPageLinkNodes(page).Where(link => link.Name != "a");
            var uniqueLinks = GetUniqueLinksThatPassRestrictions(pageLinks);
            return ProcessLinks(page.Url.DnsSafeHost, uniqueLinks);
        }

        public bool IsLinkFormatForbidden(string link)
        {
            bool isLinkFormatForbidden = false;
            var linkExtension = Path.GetExtension(link);
            if (linkExtension != null)
                isLinkFormatForbidden = _excludedExtensions.Any(extension => extension == linkExtension);
            return isLinkFormatForbidden;
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

            else if (DomainSwitchParameter.BelowSourceUrlPath.Equals(_domainSwitchParameter))
            {
                if (!string.Equals(baseLink.DnsSafeHost, innerLink.DnsSafeHost))
                    isLinkDomainForbidden = true;
                else if (string.Equals(baseLink.DnsSafeHost, innerLink.DnsSafeHost) && !innerLink.AbsolutePath.StartsWith(baseLink.AbsolutePath))
                    isLinkDomainForbidden = true;
            }

            return isLinkDomainForbidden;
        }

        private string ReplaceLinkWebPathToLocalPath(string link, string path = null)
        {
            if (IsLinkAnchor(link))
                return link;

            var filename = Path.GetFileName(link);
            if (!Path.HasExtension(filename))
                filename = Path.ChangeExtension(filename, "html");
            return path + filename;
        }

        private string ReplacePageLinksToLocal(byte[] siteLayout, string path = null)
        {
            var document = new HtmlDocument();
            var stringLayout = Encoding.UTF8.GetString(siteLayout);
            document.LoadHtml(stringLayout);
            var descendants = document.DocumentNode
                                        .Descendants()
                                        .Where(desc => _linkElements.Any(lelem => lelem == desc.Name));
            foreach (var descendant in descendants)
            {
                if (descendant.HasAttributes)
                {
                    if (descendant.Attributes["src"] != null)
                        descendant.Attributes["src"].Value = ReplaceLinkWebPathToLocalPath(descendant.Attributes["src"].Value, path);
                    else if (descendant.Attributes["href"] != null)
                        descendant.Attributes["href"].Value = ReplaceLinkWebPathToLocalPath(descendant.Attributes["href"].Value, path);
                }
            }
            return document.DocumentNode.OuterHtml;
        }

        public WebPage GetPageWithLocalLinks(WebPage page)
        {
            if (!Path.HasExtension(page.Url.OriginalString))
                page.Data = Encoding.UTF8.GetBytes(ReplacePageLinksToLocal(page.Data));
            return page;
        }

        private IEnumerable<string> ProcessLinks(string domain, IEnumerable<string> links)
        {
            IEnumerable<string> processedLinks = Enumerable.Empty<string>();
            if (links.Any())
                processedLinks = links.Select(link => ProcessLink(domain, link)).Where(link => link != null);
            return processedLinks;
        }
        
        private string ProcessLink(string domain, string link)
        {
            string httpPrefix = "http";
            string result = null;
            if (!string.IsNullOrEmpty(link))
            {
                if (link.StartsWith("//"))
                    result = $"{httpPrefix}:" + link;
                else if (link.StartsWith(httpPrefix))
                    result = link;
                else
                    result = $"{httpPrefix}://{domain}/{link}";
            }
            return result;
        } 
    }
}
