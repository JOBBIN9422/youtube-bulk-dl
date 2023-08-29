using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeAPIWrapper;

namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        private Downloader _downloader = new Downloader();
        private int _videosDownloaded = 0;

        public Form1()
        {
            InitializeComponent();
            DownloadWorker.WorkerSupportsCancellation = true;
            DownloadWorker.WorkerReportsProgress = true;
        }

        private void ChooseDownloadPathButton_Click(object sender, EventArgs e)
        {
            if (DownloadPathFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                DownloadPathTextBox.Text = DownloadPathFolderBrowser.SelectedPath;
                LogToStatusBox($"Download path set to {DownloadPathFolderBrowser.SelectedPath}");
            }
        }

        private async void DownloadButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UrlTextBox.Text))
            {
                MessageBox.Show("Please enter a valid YouTube URL.", "Invalid or Empty URL");
            }
            else if (string.IsNullOrEmpty(DownloadPathTextBox.Text))
            {
                MessageBox.Show("Please select a download path.", "Invalid or Empty Download Path");
            }
            else
            {
                string url;
                string dlPath;
                try
                {
                    url = UrlTextBox.Text;
                    dlPath = DownloadPathTextBox.Text;

                    DisableControls();

                    LogToStatusBox($"Beginning download for {url}...");
                    if (Downloader.UrlIsPlaylist(url))
                    {
                        List<DownloadItem> items = await _downloader.DownloadPlaylistAudio(url, dlPath, new Progress<ProgressItem>(i => ReportProgress(i)));
                    }

                    else
                    {
                        DownloadItem item = await _downloader.DownloadAudio(url, dlPath, new Progress<DownloadItem>(i => DisplayDownloadItem(i)));
                    }
                }
                catch (Exception ex)
                {
                    EnableControls();
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                    LogToStatusBox($"Error: {ex.Message}");
                    return;
                }

                EnableControls();
            }
        }

        private void DisableControls()
        {
            DownloadButton.Enabled = false;
            ChooseDownloadPathButton.Enabled = false;
            DownloadButton.Text = "Downloading...";
        }

        public void EnableControls()
        {
            DownloadButton.Enabled = true;
            ChooseDownloadPathButton.Enabled = true;
            DownloadButton.Text = "Download from URL";
        }

        private void ReportProgress(ProgressItem item)
        {
            DisplayDownloadItem(item.LastDownload);
            ProgressBar.Value = item.Percentage;
            //this.Text = $"Progress: {item.Percentage}%";
        }

        private void LogToStatusBox(string text)
        {
            string dateTimeStr = DateTime.Now.ToString("HH:mm:ss");
            StatusTextBox.AppendText($"[{dateTimeStr}] {text}{Environment.NewLine}");
        }

        private void DisplayDownloadItem(DownloadItem item)
        {
            if (item.SuccessfulDownload)
            {
                LogToStatusBox($"Successfully downloaded {item.DisplayName}.");

                SuccessListBox.Items.Add(item);
                SuccessCountTextBox.Text = SuccessListBox.Items.Count.ToString();
            }
            else
            {
                LogToStatusBox($"Download failed for {item.DisplayName}.");

                FailListBox.Items.Add(item);
                FailCountTextBox.Text = FailListBox.Items.Count.ToString();
            }
        }

        private void SuccessListBox_MouseDoubleClick(object sender, EventArgs e)
        {
            DownloadItem selectedItem = SuccessListBox.SelectedItem as DownloadItem;
            if (!string.IsNullOrEmpty(selectedItem.SourceUrl))
            {
                System.Diagnostics.Process.Start(selectedItem.SourceUrl);
            }
        }

        private void FailListBox_MouseDoubleClick(object sender, EventArgs e)
        {
            DownloadItem selectedItem = FailListBox.SelectedItem as DownloadItem;
            if (!string.IsNullOrEmpty(selectedItem.SourceUrl))
            {
                System.Diagnostics.Process.Start(selectedItem.SourceUrl);
            }
        }

        private void DownloadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
