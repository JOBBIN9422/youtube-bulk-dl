using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPIWrapper
{
    public class DownloadItem
    {
        public bool SuccessfulDownload { get; set; }
        public string Path { get; set; }
        public string SourceUrl { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return DisplayName ?? Path;
        }
    }
}
