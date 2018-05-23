using System;
using System.Collections.Generic;
using System.Text;

namespace WebSLC.Args
{
    public class DownloaderErrorArgs: EventArgs
    {
        public Uri Link { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
