using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace YoutubeAPIWrapper
{
    internal class LibVideoDownloader : IYouTubeDownloader
    {
        private readonly YouTube _ytDownloader = YouTube.Default;

        public async Task<string> DownloadVideoAsync(string url, string dlPath)
        {
            string videoDataFilename;

            //download the youtube video data (usually .mp4 or .webm)
            var video = await _ytDownloader.GetVideoAsync(url);
            var videoData = await video.GetBytesAsync();

            //remove the suffix added by the library
            string videoTitleName = video.FullName.Replace(" - YouTube", string.Empty);

            //write the downloaded media file to the temp assets dir
            videoDataFilename = Path.Combine(dlPath, videoTitleName);
            File.WriteAllBytes(videoDataFilename, videoData);

            return videoDataFilename;
        }
    }
}
