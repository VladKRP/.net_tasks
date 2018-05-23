using System;

namespace WebSLC.Args
{
    public class DownloadArgs: EventArgs
    {
        public Uri Link { get; set; }

        public int Depth { get; set; }

        public DateTime Time { get; set; }
    }
}