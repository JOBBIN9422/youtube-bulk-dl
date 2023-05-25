using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPIWrapper
{
    internal interface IYouTubeDownloader
    {
        //download a video from 'url' into 'dlPath', return the filename of the downloaded video
        Task<string> DownloadVideoAsync(string url, string dlPath);
    }
}
