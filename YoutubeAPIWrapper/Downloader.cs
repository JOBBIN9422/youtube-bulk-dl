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

        private readonly YouTube _ytDownloader;

        public Downloader()
        {
            _ytService = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = ConfigurationManager.AppSettings["apiKey"],
                    ApplicationName = this.GetType().ToString()
                }
            );

            _ytDownloader = YouTube.Default;
        }

        public int ProcessedItems { get; private set; }

        private async Task<string> DownloadVideoAsync(string url, string dlPath)
        {
            string videoDataFilename;
            //download the youtube video data (usually .mp4 or .webm)
            var video = await _ytDownloader.GetVideoAsync(url);
            var videoData = await video.GetBytesAsync();

            //rename the video if it does not have a useful name
            string videoTitleName = video.FullName;
            if (string.IsNullOrEmpty(videoTitleName) || videoTitleName.ToLower() == "youtube.mp4" || videoTitleName.ToLower() == "youtube.webm")
            {
                videoTitleName = await GetVideoTitle(url);
            }
            videoTitleName = videoTitleName.Replace(" - YouTube", string.Empty);

            //write the downloaded media file to the temp assets dir
            videoDataFilename = Path.Combine(dlPath, videoTitleName);
            File.WriteAllBytes(videoDataFilename, videoData);

            return videoDataFilename;

        }

        public string ConvertToMp3(string videoFilePath)
        {
            string dlDirectory = Path.GetDirectoryName(videoFilePath);
            string baseFilename = Path.GetFileNameWithoutExtension(videoFilePath);
            string outputFilename = Path.Combine(dlDirectory, $"{baseFilename}.mp3");

            //prevent file collisions
            if (File.Exists(outputFilename))
            {
                outputFilename = Path.Combine(dlDirectory, $"{baseFilename}-{Guid.NewGuid().ToString().Substring(0, 8)}.mp3"); 
            }

            MediaFile videoFile = new MediaFile() { Filename = videoFilePath };
            MediaFile mp3File = new MediaFile() { Filename = outputFilename };

            using (var engine = new Engine())
            {
                engine.Convert(videoFile, mp3File);
            }
            if (File.Exists(videoFilePath))
            {
                //remove the source file 
                File.Delete(videoFilePath);
            }

            return outputFilename;
        }

        public async Task<DownloadItem> DownloadAudio(string videoUrl, string downloadDir, IProgress<DownloadItem> progress)
        {
            //define metadata params for the item to be returned 
            string mp3FilePath = string.Empty;
            string displayName = string.Empty;
            bool success = false;

            string videoFilePath = string.Empty;
            try
            {
                //attempt to download the video file and convert it to audio format
                videoFilePath = await DownloadVideoAsync(videoUrl, downloadDir);
                await Task.Run(() => mp3FilePath = ConvertToMp3(videoFilePath));
                displayName = Path.GetFileNameWithoutExtension(mp3FilePath);
            }
            catch (Exception)
            {
                //clean up files if necessary
                if (File.Exists(mp3FilePath))
                {
                    File.Delete(mp3FilePath);
                }
                if (File.Exists(videoFilePath))
                {
                    File.Delete(videoFilePath);
                }
                success = false;

                //on error, set title accordingly (don't have a local file to set name to)
                displayName = await GetVideoTitle(videoUrl);
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = "Invalid URL";
                }
            }

            DownloadItem item = new DownloadItem()
            {
                Path = mp3FilePath,
                SuccessfulDownload = success,
                DisplayName = displayName
            };

            progress.Report(item);
            return item;
        }

        public async Task<List<DownloadItem>> DownloadPlaylistAudio(string playlistUrl, string downloadDir, IProgress<ProgressItem> progress)
        {
            if (!(UrlIsPlaylist(playlistUrl)))
            {
                throw new ArgumentException("The given URL did not refer to a YouTube playlist.");
            }

            List<string> videoUrls = await GetPlaylistUrlsAsync(playlistUrl);
            List<DownloadItem> videoFiles = new List<DownloadItem>();

            ProcessedItems = 0;

            //attempt to download each video in the playlist
            foreach (string url in videoUrls)
            {
                //create metadata object for current download
                DownloadItem item = new DownloadItem
                {
                    Path = string.Empty,
                    SourceUrl = url,
                    SuccessfulDownload = false,
                    DisplayName = string.Empty
                };

                try
                {
                    //try to download the video and convert it to mp3 format
                    string videoFile = await DownloadVideoAsync(url, downloadDir);
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

                ProcessedItems++;
                videoFiles.Add(item);
                progress.Report(new ProgressItem()
                {
                    LastDownload = item,
                    Percentage = (int)(ProcessedItems / (double)videoUrls.Count * 100)
                });
            }

            return videoFiles;
        }

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
