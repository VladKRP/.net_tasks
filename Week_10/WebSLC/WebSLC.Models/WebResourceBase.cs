using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSLC.Models
{
    public abstract class WebResourceBase
    {
        public Uri Url { get; set; }

        public byte[] Data { get; set; }

        public WebResourceBase()
        {

        }

        public WebResourceBase(Uri url, byte[] data)
        {
            Url = url;
            Data = data;
        }

        public WebResourceBase(string url, byte[] data)
        {
            Url = new Uri(url);
            Data = data;
        }

    }
}
