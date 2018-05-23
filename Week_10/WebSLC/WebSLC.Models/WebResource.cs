using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSLC.Models
{
    public class WebResource : WebResourceBase
    {
        public WebResource()
        {

        }

        public WebResource(Uri url, byte[] data) : base(url, data)
        {
        }
    }
}
