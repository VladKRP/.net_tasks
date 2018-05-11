using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebSLC.Tests
{
    [TestClass]
    public class LinkManagerTests
    {
        private const string LayoutWithLinks = @"<!DOCTYPE html>
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
                                                </html>";


        private const string LayoutWithoutLinks = @"<!DOCTYPE html>
                                                <html lang='en'>
                                                    <head>
                                                        <meta charset = 'UTF-8' >
                                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                                        <meta http-equiv='X-UA-Compatible' content='ie=edge'>
                                                        <title>Document</title>
                                                    </head>
                                                     <body>  
                                                    </body>
                                                </html>";


        [TestMethod]
        public void GetPageLinks_PassNull_ArgumentNullExseption()
        {
            LinkManager linkManager = new LinkManager();

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
            LinkManager linkManager = new LinkManager();

            var actualLinks = linkManager.GetPageLinks(LayoutWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actualLinks);
        }


        [TestMethod]
        public void GetPageLinks_PassLayoutWithLinks_DoNotContainAnchor()
        {
            var anchor = "#pageanchor";
            LinkManager linkManager = new LinkManager();

            var actualLinks = linkManager.GetPageLinks(LayoutWithLinks).ToList();

            Assert.IsTrue(!actualLinks.Contains(anchor));
        }

        [TestMethod]
        public void GetPageLinks_PassLayoutWithoutLink_ReturnEmptyCollection()
        {
            var expectedLinks = new List<string>();
            LinkManager linkManager = new LinkManager();

            var actualLinks = linkManager.GetPageLinks(LayoutWithoutLinks).ToList();

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
            LinkManager linkManager = new LinkManager(restrictedFormats);

            var actualLinks = linkManager.GetPageLinks(LayoutWithLinks).ToList();

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
            LinkManager linkManager = new LinkManager(restrictedFormats);

            var actualLinks = linkManager.GetPageLinks(LayoutWithLinks).ToList();

            CollectionAssert.AreEqual(expectedLinks, actualLinks);
        }


        [TestMethod]
        public void GetPageResourceLink_PassNull_ArgumentNullExseption()
        {
            LinkManager linkManager = new LinkManager();

            Assert.ThrowsException<ArgumentNullException>(() => linkManager.GetPageResourceLink(null));
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithLinks_ReturnResourceLinkCollection()
        {
            LinkManager linkManager = new LinkManager();

            throw new System.Exception();
            Assert.ThrowsException<ArgumentNullException>(() => linkManager.GetPageLinks(null));
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithoutLink_ReturnEmptyCollection()
        {
            LinkManager linkManager = new LinkManager();

            throw new System.Exception();
            Assert.ThrowsException<ArgumentNullException>(() => linkManager.GetPageLinks(null));
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithLinks_PngImageLinksRestricted_ReturnResourceLinkCollectionWithoutPng()
        {
            LinkManager linkManager = new LinkManager();

            throw new System.Exception();
            Assert.ThrowsException<ArgumentNullException>(() => linkManager.GetPageLinks(null));
        }

        [TestMethod]
        public void GetPageResourceLink_PassLayoutWithLinks_PngAndCSSLinksRestricted_ReturnResourceLinkCollectionWithoutRestricted()
        {
            LinkManager linkManager = new LinkManager();

            throw new System.Exception();
            Assert.ThrowsException<ArgumentNullException>(() => linkManager.GetPageLinks(null));
        }
    }
}