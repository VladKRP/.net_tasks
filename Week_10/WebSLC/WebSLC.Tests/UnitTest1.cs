using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebSLC.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            LinkProcessingHelper linkAnalyzer = new LinkProcessingHelper();
            WebsiteDownloader downloader = new WebsiteDownloader("path", linkAnalyzer);
            
        }
    }
}
