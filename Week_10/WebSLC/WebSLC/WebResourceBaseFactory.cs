using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebSLC.Models;

namespace WebSLC
{
    public static class  WebResourceBaseFactory
    {
        public static WebResourceBase Create(Uri url, byte[] resource)
        {
            if (Path.HasExtension(url.OriginalString))
                return new WebResource(url, resource);
            else
                return new WebPage(url, resource);
        }
    }
}
