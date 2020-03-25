using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPIWrapper
{
    public class ProgressItem
    {
        public DownloadItem LastDownload { get; set; }
        public int Percentage { get; set; }
    }
}
