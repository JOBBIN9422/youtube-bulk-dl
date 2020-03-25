using System;

namespace YoutubeDownloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DownloadPathFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.SuccessListBox = new System.Windows.Forms.ListBox();
            this.ChooseDownloadPathButton = new System.Windows.Forms.Button();
            this.UrlTextBox = new System.Windows.Forms.TextBox();
            this.DownloadPathTextBox = new System.Windows.Forms.TextBox();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.FailListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.DownloadWorker = new System.ComponentModel.BackgroundWorker();
            this.SuccessCountTextBox = new System.Windows.Forms.TextBox();
            this.FailCountTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.ChooseDownloadPathButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.UrlTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.DownloadPathTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.DownloadButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ProgressBar, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.SuccessListBox, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.SuccessCountTextBox, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.FailListBox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.FailCountTextBox, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 478);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(541, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Failed Downloads";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SuccessListBox
            // 
            this.SuccessListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SuccessListBox.FormattingEnabled = true;
            this.SuccessListBox.Location = new System.Drawing.Point(3, 129);
            this.SuccessListBox.Name = "SuccessListBox";
            this.SuccessListBox.Size = new System.Drawing.Size(394, 346);
            this.SuccessListBox.TabIndex = 9;
            this.SuccessListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SuccessListBox_MouseDoubleClick);
            // 
            // ChooseDownloadPathButton
            // 
            this.ChooseDownloadPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChooseDownloadPathButton.Location = new System.Drawing.Point(256, 3);
            this.ChooseDownloadPathButton.Name = "ChooseDownloadPathButton";
            this.ChooseDownloadPathButton.Size = new System.Drawing.Size(141, 23);
            this.ChooseDownloadPathButton.TabIndex = 8;
            this.ChooseDownloadPathButton.Text = "Choose Download Path";
            this.ChooseDownloadPathButton.UseVisualStyleBackColor = true;
            this.ChooseDownloadPathButton.Click += new System.EventHandler(this.ChooseDownloadPathButton_Click);
            // 
            // UrlTextBox
            // 
            this.UrlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UrlTextBox.Location = new System.Drawing.Point(403, 32);
            this.UrlTextBox.Name = "UrlTextBox";
            this.UrlTextBox.Size = new System.Drawing.Size(394, 20);
            this.UrlTextBox.TabIndex = 4;
            // 
            // DownloadPathTextBox
            // 
            this.DownloadPathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DownloadPathTextBox.Location = new System.Drawing.Point(403, 3);
            this.DownloadPathTextBox.Name = "DownloadPathTextBox";
            this.DownloadPathTextBox.ReadOnly = true;
            this.DownloadPathTextBox.Size = new System.Drawing.Size(394, 20);
            this.DownloadPathTextBox.TabIndex = 0;
            // 
            // DownloadButton
            // 
            this.DownloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadButton.Location = new System.Drawing.Point(256, 32);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(141, 23);
            this.DownloadButton.TabIndex = 3;
            this.DownloadButton.Text = "Download from URL";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // FailListBox
            // 
            this.FailListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FailListBox.FormattingEnabled = true;
            this.FailListBox.Location = new System.Drawing.Point(403, 129);
            this.FailListBox.Name = "FailListBox";
            this.FailListBox.Size = new System.Drawing.Size(394, 346);
            this.FailListBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(128, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Successful Downloads";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgressBar
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ProgressBar, 2);
            this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressBar.Location = new System.Drawing.Point(3, 61);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(794, 20);
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressBar.TabIndex = 13;
            // 
            // DownloadWorker
            // 
            this.DownloadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DownloadWorker_DoWork);
            // 
            // SuccessCountTextBox
            // 
            this.SuccessCountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SuccessCountTextBox.Location = new System.Drawing.Point(3, 103);
            this.SuccessCountTextBox.Name = "SuccessCountTextBox";
            this.SuccessCountTextBox.ReadOnly = true;
            this.SuccessCountTextBox.Size = new System.Drawing.Size(394, 20);
            this.SuccessCountTextBox.TabIndex = 14;
            // 
            // FailCountTextBox
            // 
            this.FailCountTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FailCountTextBox.Location = new System.Drawing.Point(403, 103);
            this.FailCountTextBox.Name = "FailCountTextBox";
            this.FailCountTextBox.ReadOnly = true;
            this.FailCountTextBox.Size = new System.Drawing.Size(394, 20);
            this.FailCountTextBox.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 478);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "YouTube Downloader";
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FailListBox_MouseDoubleClick);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.FolderBrowserDialog DownloadPathFolderBrowser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox DownloadPathTextBox;
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.TextBox UrlTextBox;
        private System.ComponentModel.BackgroundWorker DownloadWorker;
        private System.Windows.Forms.ListBox SuccessListBox;
        private System.Windows.Forms.Button ChooseDownloadPathButton;
        private System.Windows.Forms.ListBox FailListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.TextBox SuccessCountTextBox;
        private System.Windows.Forms.TextBox FailCountTextBox;
    }
}

