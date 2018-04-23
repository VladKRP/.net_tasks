using System;

namespace WebSLC
{
    public class DownloadArgs: EventArgs
    {
        public string Link { get; set; }

        public DateTime Time { get; set; }
    }
}