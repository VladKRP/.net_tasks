using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebSLC.Tests
{
    [TestClass]
    public class HtmlAnalyzerTests
    {
        [TestMethod]
        public void IsLayoutContainHtmlTag_PassNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => HtmlAnalyzer.IsLayoutContainHtmlTag(null));
        }

        [TestMethod]
        public void IsLayoutContainHtmlTag_PassEmptyString_ReturnFalse()
        {
            bool expected = false;
            string layout = "";

            var actual = HtmlAnalyzer.IsLayoutContainHtmlTag(layout);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLayoutContainHtmlTag_PassHtmlLayout_HasHtmlTag_ReturnTrue()
        {
            bool expected = true;
            string layout = @"<!DOCTYPE html ><html>
                <head resURL = '/static/cde7a081' data-rooturl='' data-resurl='/static/cde7a081'>
                <title > Jenkins </ title >< link rel = 'stylesheet' href = '/static/cde7a081/css/layout-common.css' type = 'text/css' />
                <link rel='stylesheet' href='/static/cde7a081/css/style.css' type='text/css' />
                <link rel='shortcut icon' href='/static/cde7a081/favicon.ico' type='image/vnd.microsoft.icon' />
                <link color='black' rel='mask-icon' href='/images/mask-icon.svg' />
                <script> var isRunAsTest = false; var rootURL = ''; var resURL = '/static/cde7a081';</script >
                <script src = '/static/cde7a081/scripts/prototype.js' type='text/javascript'></script >
                <script src = '/static/cde7a081/scripts/behavior.js' type='text/javascript'></script >
                </head><body><a href = 'api/' > REST API</a></body></html>' ";

            var actual = HtmlAnalyzer.IsLayoutContainHtmlTag(layout);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsLayoutContainHtmlTag_PassHtmlLayout_HasNoHtmlTag_ReturnFalse()
        {
            bool expected = false;
            string layout = @"<!DOCTYPE html >
                <head resURL = '/static/cde7a081' data-rooturl='' data-resurl='/static/cde7a081'>
                <title > Jenkins </ title >< link rel = 'stylesheet' href = '/static/cde7a081/css/layout-common.css' type = 'text/css' />
                <link rel='stylesheet' href='/static/cde7a081/css/style.css' type='text/css' />
                <link rel='shortcut icon' href='/static/cde7a081/favicon.ico' type='image/vnd.microsoft.icon' />
                <link color='black' rel='mask-icon' href='/images/mask-icon.svg' />
                <script> var isRunAsTest = false; var rootURL = ''; var resURL = '/static/cde7a081';</script >
                <script src = '/static/cde7a081/scripts/prototype.js' type='text/javascript'></script >
                <script src = '/static/cde7a081/scripts/behavior.js' type='text/javascript'></script >
                </head><body><a href = 'api/' > REST API</a></body>' ";

            var actual = HtmlAnalyzer.IsLayoutContainHtmlTag(layout);

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetHtmlPageTitle_PassNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => HtmlAnalyzer.GetHtmlPageTitle(null));        
        }

        [TestMethod]
        public void GetHtmlPageTitle_PassHtmlLayout_WithTitle_ReturnTitle()
        {
            string expectedTitle = " Jenkins ";
            string layout = @"<!DOCTYPE html ><html>
                <head resURL = '/static/cde7a081' data-rooturl='' data-resurl='/static/cde7a081'>
                <title> Jenkins </title>< link rel = 'stylesheet' href = '/static/cde7a081/css/layout-common.css' type = 'text/css' />
                </head><body><a href = 'api/' > REST API</a></body></html> ";

            string actualTitle = HtmlAnalyzer.GetHtmlPageTitle(layout);

            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void GetHtmlPageTitle_PassHtmlLayout_WithoutTitle_ReturnNull()
        {
            string expectedTitle = null;
            string layout = @"<!DOCTYPE html ><html>
                <head resURL = '/static/cde7a081' data-rooturl='' data-resurl='/static/cde7a081'>
                <link rel = 'stylesheet' href = '/static/cde7a081/css/layout-common.css' type = 'text/css' />
                </head><body><a href = 'api/' > REST API</a></body></html> ";

            string actualTitle = HtmlAnalyzer.GetHtmlPageTitle(layout);
            Assert.AreEqual(expectedTitle, actualTitle);
        }
    }
}
