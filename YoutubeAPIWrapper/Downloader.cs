using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoLibrary;

namespace YoutubeAPIWrapper
{
    public class Downloader
    {
        private readonly YouTubeService _ytService;

        private readonly IYouTubeDownloader _ytDownloader;

        public Downloader()
        {
            //create YouTube API client w/ key from config
            _ytService = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = ConfigurationManager.AppSettings["apiKey"],
                    ApplicationName = this.GetType().ToString()
                }
            );

            _ytDownloader = new YouTubeExplodeDownloader();
        }

        //convert the file indicated by 'videoFilePath' to mp3 (return the output filename)
        public string ConvertToMp3(string videoFilePath)
        {
            //determine destination dir and I/O filenames
            string dlDirectory = Path.GetDirectoryName(videoFilePath);
            string baseFilename = Path.GetFileNameWithoutExtension(videoFilePath);
            string outputFilename = Path.Combine(dlDirectory, $"{baseFilename}.mp3");

            //prevent file collisions
            if (File.Exists(outputFilename))
            {
                outputFilename = Path.Combine(dlDirectory, $"{baseFilename}-{Guid.NewGuid().ToString().Substring(0, 8)}.mp3"); 
            }

            //convert from source to mp3 via MediaToolkit
            MediaFile videoFile = new MediaFile() { Filename = videoFilePath };
            MediaFile mp3File = new MediaFile() { Filename = outputFilename };
            using (var engine = new Engine())
            {
                engine.Convert(videoFile, mp3File);
            }

            //clean up the original video file
            if (File.Exists(videoFilePath))
            {
                File.Delete(videoFilePath);
            }

            return outputFilename;
        }

        //download an MP3 file for the video indicated by videoUrl into downloadDir
        public async Task<DownloadItem> DownloadAudio(string videoUrl, string downloadDir, IProgress<DownloadItem> progress)
        {
            DownloadItem item = new DownloadItem
            {
                SourceUrl = videoUrl,
            };

            string videoFilePath = string.Empty;
            try
            {
                //attempt to download the video file and convert it to audio format
                videoFilePath = await _ytDownloader.DownloadVideoAsync(videoUrl, downloadDir);
                await Task.Run(() => item.Path = ConvertToMp3(videoFilePath));
                item.DisplayName = Path.GetFileNameWithoutExtension(item.Path);
                item.SuccessfulDownload = true;
            }
            catch (Exception)
            {
                //clean up files if necessary
                if (File.Exists(item.Path))
                {
                    File.Delete(item.Path);
                }
                if (File.Exists(videoFilePath))
                {
                    File.Delete(videoFilePath);
                }
                item.SuccessfulDownload = false;

                //on error, set title accordingly (don't have a local file to set name to)
                item.DisplayName = await GetVideoTitle(videoUrl);
                if (string.IsNullOrEmpty(item.DisplayName))
                {
                    item.DisplayName = "Invalid URL";
                }
            }

            progress.Report(item);
            return item;
        }

        //Download all of the videos indicated by 'playlistUrl' to 'downloadDir'
        public async Task<List<DownloadItem>> DownloadPlaylistAudio(string playlistUrl, string downloadDir, IProgress<ProgressItem> progress)
        {
            //break if not a playlist 
            if (!(UrlIsPlaylist(playlistUrl)))
            {
                throw new ArgumentException("The given URL did not refer to a YouTube playlist.");
            }

            //fetch the video URLs for the given playlist via YouTube API
            List<string> videoUrls = await GetPlaylistUrlsAsync(playlistUrl);
            List<DownloadItem> videoFiles = new List<DownloadItem>();

            int processedItems = 0;

            //attempt to download each video in the playlist
            foreach (string url in videoUrls)
            {
                //create metadata object for current download
                DownloadItem item = new DownloadItem
                {
                    SourceUrl = url,
                };

                try
                {
                    //try to download the video and convert it to mp3 format
                    string videoFile = await _ytDownloader.DownloadVideoAsync(url, downloadDir);
                    await Task.Run(() => item.Path = ConvertToMp3(videoFile));

                    item.SuccessfulDownload = true;
                    item.DisplayName = Path.GetFileNameWithoutExtension(item.Path);
                }
                catch (Exception)
                {
                    //clean up and mark the item as failed
                    if (!string.IsNullOrEmpty(item.Path))
                    {
                        File.Delete(item.Path);
                    }

                    item.Path = string.Empty;
                    item.DisplayName = await GetVideoTitle(url);
                    item.SuccessfulDownload = false;
                }

                videoFiles.Add(item);

                //report current download progress
                processedItems++;
                progress.Report(new ProgressItem()
                {
                    LastDownload = item,
                    Percentage = (int)(processedItems / (double)videoUrls.Count * 100)
                });
            }

            return videoFiles;
        }

        //get the title for the YouTube video indicated by 'url' via the YouTube API
        public async Task<string> GetVideoTitle(string url)
        {
            var videoRequest = _ytService.Videos.List("snippet");
            videoRequest.Id = GetVideoIdFromUrl(url);

            var searchResponse = await videoRequest.ExecuteAsync();

            string title = string.Empty;
            foreach (var item in searchResponse.Items)
            {
                if (!string.IsNullOrEmpty(item.Snippet.Title))
                {
                    title = item.Snippet.Title;
                    break;
                }
            }

            return title;
        }

        
        //retrieve a list of video URLs for the playlist indicated by 'playlistUrl' via the YouTube API
        public async Task<List<string>> GetPlaylistUrlsAsync(string playlistUrl)
        {
            List<string> urls = new List<string>();

            string nextPageToken = "";

            //iterate over paginated playlist results from youtube and extract video URLs
            while (nextPageToken != null)
            {
                //prepare a paged playlist request for the given playlist URL
                var playlistRequest = _ytService.PlaylistItems.List("snippet,contentDetails");
                playlistRequest.PlaylistId = GetPlaylistIdFromUrl(playlistUrl);
                playlistRequest.MaxResults = 50;
                playlistRequest.PageToken = nextPageToken;

                var searchListResponse = await playlistRequest.ExecuteAsync();

                //iterate over the results and build each video URL
                foreach (var item in searchListResponse.Items)
                {
                    string videoUrl = $"http://www.youtube.com/watch?v={item.Snippet.ResourceId.VideoId}";
                    urls.Add(videoUrl);
                }

                //index to the next page of results
                nextPageToken = searchListResponse.NextPageToken;
            }

            return urls;
        }

        public static bool UrlIsPlaylist(string url)
        {
            return Regex.IsMatch(url, @"https:\/\/www.youtube.com\/playlist\?list=.+");
        }

        private static string GetPlaylistIdFromUrl(string url)
        {
            string Id = Regex.Match(url, "list=.+").Value.Replace("list=", string.Empty);
            return Id;
        }
        private static string GetVideoIdFromUrl(string url)
        {
            string Id = Regex.Match(url, "v=.[A-Za-z0-9-_]+").Value.Replace("v=", string.Empty);
            return Id;
        }
    }
}
