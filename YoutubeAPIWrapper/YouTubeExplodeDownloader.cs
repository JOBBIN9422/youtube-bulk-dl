using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;


namespace YoutubeAPIWrapper
{
    internal class YouTubeExplodeDownloader : IYouTubeDownloader
    {
        private readonly YoutubeClient _ytClient = new YoutubeClient();

        public async Task<string> DownloadVideoAsync(string url, string dlPath)
        {
            var videoInfo = await _ytClient.Videos.GetAsync(url);
            //get all streams for the video
            var streamManifest = await _ytClient.Videos.Streams.GetManifestAsync(url);

            //get highest quality audio-only stream
            var streamInfo = streamManifest.GetAudioOnlyStreams().OrderBy(s => s.Bitrate).Last();

            //generate filename (remove invalid filename chars)
            string validVidName = string.Join("-", videoInfo.Title.Split(Path.GetInvalidFileNameChars()));
            string downloadFilename = Path.Combine(dlPath, $"{validVidName}.{streamInfo.Container}");

            //download to file
            await _ytClient.Videos.Streams.DownloadAsync(streamInfo, downloadFilename);

            return downloadFilename;
        }
    }
}
