using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSLC.Models;

namespace WebSLC.Tests
{
    [TestClass]
    public class LinkManagerTests
    {
        private readonly byte[] LayoutWithLinks;
        private readonly byte[] LayoutWithoutLinks;

        private readonly WebPage pageWithLinks;
        private readonly WebPage pageWithoutLinks;

        public LinkManagerTests()
        {
            LayoutWithLinks = Encoding.Default.GetBytes(@"<!DOCTYPE html>
                                                <html lang='en'>
                                                    <head>
                                                        <meta charset = 'UTF-8' >
                                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                                        <meta http-equiv='X-UA-Compatible' content='ie=edge'>
                                                        <link rel='stylesheet' href='https://linkmanagertests.com/index.css'>
                                                        <link rel='stylesheet' href='https://linkmanagertests.com/buttons.css'>
                                                        <title>Document</title>
                                                    </head>
                                                     <body>
                                                        <a href='https://linkmanagertests.com/home'>Home</a>
                                                        <a href='#pageanchor'>anchor</a>
                                                        <img src='https://linkmanagertests.com/img/hello/hello.png'/>
                                                        <img src='https://linkmanagertests.com/img/hello/hello.jpg'/>
                                                        <img src='https://linkmanagertests.com/img/hello/hello.ico'/>
                                                        <script src='https://linkmanagertests.com/scripts/script.js'></script>
                                                    </body>
                                                </html>");
            LayoutWithoutLinks = Encoding.Default.GetBytes(@"<!DOCTYPE html>
                                                <html lang='en'>
                                                    <head>
                                                        <meta charset = 'UTF-8' >
                                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                                        <meta http-equiv='X-UA-Compatible' content='ie=edge'>
                                                        <title>Document</title>
                                                    </head>
                                                     <body>  
                                                    </body>
                                                </html>");
            pageWithLinks = new WebPage() { Url = new Uri("https://linkmanagertests.com/"),  Data = LayoutWithLinks };
            pageWithoutLinks = new WebPage() { Url = new Uri("https://linkmanagertests.com/"), Data = LayoutWithoutLinks };
        }

        [TestMethod]
        public void GetPageLinks_PassNull_ArgumentNullExseption()
        {
            HtmlLinkManager linkManager = new HtmlLinkManager();

            Assert.ThrowsException<ArgumentNullException>(() => linkManager.GetPageLinks(null));
        }

        [TestMethod]
        public void GetPageLinks_PassLayoutWithLinks_ReturnLinkCollection()
        {
            var expectedLinks = new List<string>() {
                "https://linkmanagertests.com/index.css",
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/home",
                "https://linkmanagertests.com/img/hello/hello.png" ,
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actualLinks = linkManager.GetPageLinks(pageWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actualLinks);
        }


        [TestMethod]
        public void GetPageLinks_PassLayoutWithLinks_DoNotContainAnchor()
        {
            var anchor = "#pageanchor";
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actualLinks = linkManager.GetPageLinks(pageWithLinks).ToList();

            Assert.IsTrue(!actualLinks.Contains(anchor));
        }

        [TestMethod]
        public void GetPageLinks_PassLayoutWithoutLink_ReturnEmptyCollection()
        {
            var expectedLinks = new List<string>();
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actualLinks = linkManager.GetPageLinks(pageWithoutLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actualLinks);
        }

        [TestMethod]
        public void GetPageLinks_PassLayoutWithLinks_PngImageLinksRestricted_ReturnLinkCollectionWithoutPng()
        {
            var expectedLinks = new List<string>() {
                "https://linkmanagertests.com/index.css",
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/home",
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            var restrictedFormats = new List<string>() { ".png" };
            HtmlLinkManager linkManager = new HtmlLinkManager(restrictedFormats);

            var actualLinks = linkManager.GetPageLinks(pageWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actualLinks);
        }

        [TestMethod]
        public void GetPageLinks_PassLayoutWithLinks_PngAndCSSLinksRestricted_ReturnLinkCollectionWithoutRestricted()
        {
            var expectedLinks = new List<string>() {
                "https://linkmanagertests.com/home",
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            var restrictedFormats = new List<string>() { ".png", ".css" };
            HtmlLinkManager linkManager = new HtmlLinkManager(restrictedFormats);

            var actualLinks = linkManager.GetPageLinks(pageWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actualLinks);
        }


        [TestMethod]
        public void GetPageResourceLink_PassNull_ArgumentNullExseption()
        {
            HtmlLinkManager linkManager = new HtmlLinkManager();

            Assert.ThrowsException<ArgumentNullException>(() => linkManager.GetPageResourceLink(null));
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithLinks_ReturnResourceLinkCollection()
        {
            var expectedLinks = new List<string>() {
                "https://linkmanagertests.com/index.css",
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/img/hello/hello.png" ,
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actual = linkManager.GetPageResourceLink(pageWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actual);
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithoutLink_ReturnEmptyCollection()
        {
            var expected = new List<string>();
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actual = linkManager.GetPageResourceLink(pageWithoutLinks).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithLinks_PngImageLinksRestricted_ReturnResourceLinkCollectionWithoutPng()
        {
            var expectedLinks = new List<string>() {
                "https://linkmanagertests.com/index.css",
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            IEnumerable<string> forbiddenFormats = new List<string>() { ".png" };
            HtmlLinkManager linkManager = new HtmlLinkManager(forbiddenFormats);

            var actual = linkManager.GetPageResourceLink(pageWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actual);
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithLinks_PngAndCSSLinksRestricted_ReturnResourceLinkCollectionWithoutRestricted()
        {
            var expectedLinks = new List<string>() {
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            IEnumerable<string> forbiddenFormats = new List<string>() { ".png", ".css" };
            HtmlLinkManager linkManager = new HtmlLinkManager(forbiddenFormats);

            var actual = linkManager.GetPageResourceLink(pageWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actual);
        }


        [TestMethod]
        public void IsLinkFormatForbidden_HasForbiddenFormats_ReturnFilteredSequence()
        {
            var links = new List<string>() {
                "https://linkmanagertests.com/index.css",
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/home",
                "https://linkmanagertests.com/img/hello/hello.png" ,
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            bool[] expected = { false, false, false, true, false, true, true };
            IEnumerable<string> forbiddenFormats = new List<string>() { ".png", ".ico", ".js" };
            HtmlLinkManager linkManager = new HtmlLinkManager(forbiddenFormats);

            var actual = links.Select(link => linkManager.IsLinkFormatForbidden(link)).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLinkFormatForbidden_NoForbiddenFormats_ReturnAll()
        {
            var links = new List<string>() {
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/home",
                "https://linkmanagertests.com/img/hello/hello.png" ,
                "https://linkmanagertests.com/img/hello/hello.jpg",
                "https://linkmanagertests.com/img/hello/hello.ico",
                "https://linkmanagertests.com/scripts/script.js"
                };
            bool[] expected = { false, false, false, false, false, false };
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actual = links.Select(link => linkManager.IsLinkFormatForbidden(link)).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLinkAnchor_NoAnchors_AllFalse()
        {
            var links = new List<string>() {
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/home",
                "https://linkmanagertests.com/img/hello/hello.png" ,
                "https://linkmanagertests.com/scripts/script.js"
                };
            bool[] expected = { false, false, false, false };
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actual = links.Select(link => linkManager.IsLinkAnchor(link)).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLinkAnchor_Has2Anchors_2True()
        {
            var links = new List<string>() {
                "#about",
                "https://linkmanagertests.com/buttons.css",
                "https://linkmanagertests.com/home",
                "https://linkmanagertests.com/img/hello/hello.png" ,
                "#discounts",
                "https://linkmanagertests.com/scripts/script.js"
                };

            bool[] expected = { true, false, false, false, true, false };
            HtmlLinkManager linkManager = new HtmlLinkManager();

            var actual = links.Select(link => linkManager.IsLinkAnchor(link)).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLinkDomainForbidden_WithoutRestrictionsParam_ReturnAllLinks()
        {
            var links = new List<Uri>() {
                new Uri("https://linkmanagertests.com/buttons.css"),
                new Uri("https://linkmanagertests.com/home"),
                new Uri("https://linkmanagertests.com/img/hello/hello.png"),
                new Uri("https://habr.com"),
                new Uri("https://epam.com"),
                new Uri("https://linkmanagertests.com/scripts/script.js")
                };
            var currentDomain = new Uri("https://linkmanagertests.com");
            bool[] expected = { false, false, false, false, false, false };
            DomainSwitchParameter domainSwitchParameter = DomainSwitchParameter.WithoutRestrictions;
            HtmlLinkManager linkManager = new HtmlLinkManager(domainSwitchParameter: domainSwitchParameter);

            var actual = links.Select(link => linkManager.IsLinkDomainForbidden(currentDomain, link)).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLinkDomainForbidden_CurrentDomainParam_ReturnFilteredLinks()
        {
            var links = new List<Uri>() {
                new Uri("https://linkmanagertests.com/buttons.css"),
                new Uri("https://linkmanagertests.com/home"),
                new Uri("https://linkmanagertests.com/img/hello/hello.png"),
                new Uri("https://habr.com"),
                new Uri("https://epam.com"),
                new Uri("https://linkmanagertests.com/scripts/script.js")
                };
            var currentDomain = new Uri("https://linkmanagertests.com");
            bool[] expected = { false, false, false, true, true, false };
            DomainSwitchParameter domainSwitchParameter = DomainSwitchParameter.CurrentDomain;
            HtmlLinkManager linkManager = new HtmlLinkManager(domainSwitchParameter: domainSwitchParameter);

            var actual = links.Select(link => linkManager.IsLinkDomainForbidden(currentDomain, link)).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLinkDomainForbidden_BelowSourceUrlPath_ReturnFilteredLinks()
        {
            var links = new List<Uri>() {
                new Uri("https://linkmanagertests.com/users/index.html"),
                new Uri("https://linkmanagertests.com/home"),
                new Uri("https://linkmanagertests.com/"),//should it pass?
                new Uri("https://habr.com"),
                new Uri("https://epam.com"),
                new Uri("https://linkmanagertests.com/scripts/script.js")
                };
            var baseUrl = new Uri("https://linkmanagertests.com/users");
            bool[] expected = { false, true, true, true, true, true };

            DomainSwitchParameter domainSwitchParameter = DomainSwitchParameter.BelowSourceUrlPath;
            HtmlLinkManager linkManager = new HtmlLinkManager(domainSwitchParameter: domainSwitchParameter);

            var result = links.Select(link => linkManager.IsLinkDomainForbidden(baseUrl, link)).ToList();

            CollectionAssert.AreEqual(expected, result);
        }
    }
}