using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSLC.Models
{
    public class WebPage:WebResourceBase
    {

        public WebPage()
        {

        }

        public WebPage(Uri url, byte[] data) : base(url, data)
        {

        }

        public string Layout {
            get {
                return Encoding.Default.GetString(Data);
            }            
        }

    }
}
