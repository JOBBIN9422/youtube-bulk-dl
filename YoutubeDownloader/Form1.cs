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
                try
                {
                    string url = UrlTextBox.Text;
                    string dlPath = DownloadPathTextBox.Text;

                    DisableControls();

                    if (Downloader.UrlIsPlaylist(url))
                    {
                        List<DownloadItem> items = await _downloader.DownloadPlaylistAudio(url, dlPath, new Progress<ProgressItem>(i => ReportProgress(i)));
                    }

                    else
                    {
                        DownloadItem item = await _downloader.DownloadAudio(url, dlPath, new Progress<DownloadItem>(i => DisplayItem(i)));
                    }
                }
                catch (Exception ex)
                {
                    EnableControls();
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                    return;
                }

                EnableControls();
                MessageBox.Show("The operation completed. Check the appropriate download lists.", "Finished");
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
            DisplayItem(item.LastDownload);
            ProgressBar.Value = item.Percentage;
            //this.Text = $"Progress: {item.Percentage}%";
        }

        private void DisplayItem(DownloadItem item)
        {
            if (item.SuccessfulDownload)
            {
                SuccessListBox.Items.Add(item);
            }
            else
            {
                FailListBox.Items.Add(item);
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
