using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace OnvifAssistant
{
    public partial class VideoView : UserControl
    {
        Vlc.DotNet.Wpf.VlcControl myVlcControl;
        bool isPlaying = false;
        public VideoView()
        {
            InitializeComponent();
        }

        private void VideoView_Load(object sender, EventArgs e)
        {
            myVlcControl = new Vlc.DotNet.Wpf.VlcControl();
            myVlcControl.MediaPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                 | System.Windows.Forms.AnchorStyles.Left)
                 | System.Windows.Forms.AnchorStyles.Right)));
            myVlcControl.MediaPlayer.BackColor = System.Drawing.Color.Red;
            myVlcControl.MediaPlayer.Location = new System.Drawing.Point(12, 11);
            myVlcControl.MediaPlayer.Size = new System.Drawing.Size(564, 312);
            myVlcControl.MediaPlayer.VlcLibDirectoryNeeded += MediaPlayer_VlcLibDirectoryNeeded;
        
            this.Controls.Add(myVlcControl.MediaPlayer);
        }
        public void play(string rtspUrl)
        {
            if (isPlaying)
            {
                return;
            }
          
            Debug.WriteLine("play rtsp: " + rtspUrl);
            myVlcControl.MediaPlayer.Play(new Uri(rtspUrl));
            isPlaying = true;
        }
        public void stop()
        {
            if (myVlcControl.MediaPlayer == null)
            {
                return;
            }
            if (isPlaying)
            {
                myVlcControl.MediaPlayer.Stop();
                isPlaying = false;
            }
            
        }
        private void MediaPlayer_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
            {
                Debug.WriteLine("currentDir is null");
                return;
            }

            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
            {
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"vlc\x86\"));
                //e.VlcLibDirectory = new DirectoryInfo(currentDirectory);

            }
            else
            {
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"vlc\x64\"));
                //e.VlcLibDirectory = new DirectoryInfo(currentDirectory);
            }
            Debug.WriteLine("currentDir: " + e.VlcLibDirectory.FullName);
            if (!e.VlcLibDirectory.Exists)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.Description = "Select Vlc libraries folder.";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }
    }
}
