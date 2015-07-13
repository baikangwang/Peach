namespace Peach.MediaConverter.Sample
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        private Encoder _encoder = new Encoder();

        private IList<string> extList;

        private string outfolder;

        public Form1()
        {
            this.InitializeComponent();
            this.Load += this.Form1_Load;
        }

        public Encoder Encoder
        {
            get
            {
                return this._encoder;
            }
            set
            {
                if (this._encoder != null)
                {
                    this._encoder.Progress -= this.ConOut;
                    this._encoder.Status -= this.Status;
                }

                this._encoder = value;
                if (this._encoder != null)
                {
                    this._encoder.Progress += this.ConOut;
                    this._encoder.Status += this.Status;
                }
            }
        }

        private void ConOut(int prog, int tl)
        {
            this.ProgressBar.Value = prog;
            Application.DoEvents();
        }

        private void Status(string status)
        {
            this.StatusLbl.Text = status;
            Application.DoEvents();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var outpath = new FolderBrowserDialog();
            if (outpath.ShowDialog() == DialogResult.OK)
            {
                this.outfolder = outpath.SelectedPath;
                this.Label3.Text = outpath.SelectedPath;
            }
        }

        private void ListBox1_DragEnter(object sender, DragEventArgs e)
        {
            //makes sure its a file or folder
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                //copys the item to the "e" handler for later use 
                //in the drag drop event
            }
            else
            {
                e.Effect = DragDropEffects.None;
                //if its not a file or folder, do nothing
                //actually it will display a circle with line "not valid"
            }
        }

        private void ListBox1_DragDrop(object sender, DragEventArgs e)
        {
            var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
            //declares list of strings called "file_names". contains all the items dropped into list box       
            var listofiles = new List<string>();
            foreach (var fileName in fileNames)
            {
                if (File.Exists(fileName))
                {
                    //a file was dragged
                    listofiles.Add(fileName);
                }
                else if (Directory.Exists(fileName))
                {
                    //a folder was added..
                    try
                    {
                        listofiles.AddRange(Directory.GetFiles(fileName, "*.*", SearchOption.AllDirectories));
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            foreach (var file in listofiles)
            {
                //Create a file info object
                var fileInfo = new FileInfo(file);
                if (this.extList.Contains(fileInfo.Extension))
                {
                    //trim extension off of filename.
                    this.ListBox1.Items.Add(file);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.extList = new List<string>();
            this.extList.Add(".VOB");
            this.extList.Add(".wmv");
            this.extList.Add(".avi");
            this.extList.Add(".m2ts");
            this.extList.Add(".MP4");
            this.extList.Add(".ASF");
            this.extList.Add(".flac");
            this.extList.Add(".vob");
            this.extList.Add(".flv");
            this.extList.Add(".mp3");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            for (var i = 0; i <= this.ListBox1.Items.Count - 1; i++)
            {
                this.Encoder.OverWrite = false;
                this.Encoder.SourceFile = this.ListBox1.Items[i].ToString(); //.Item(i);
                this.Encoder.Format = Encoder.Format_MKV;
                this.Encoder.AudioCodec = Encoder.AudioCodec_mp3;
                this.Encoder.AudioBitrate = "128k";
                this.Encoder.Video_Codec = Encoder.Vcodec_h264;
                this.Encoder.Threads = "0";
                this.Encoder.OverWrite = true;
                //  Encoder.ConstantRateFactor = 22
                this.Encoder.RateControl = Encoder.RateControl_ABR;
                this.Encoder.VideoBitrate = "1000k";

                this.Encoder.Libx264_Preset_pass1 = Encoder.libx264_fast;

                this.Encoder.OutputPath = this.outfolder;
                this.Encoder.AnalyzeFile();

                this.Encoder.Encode();
                this.ListBox1.SetItemCheckState(i, CheckState.Checked);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            for (var i = 0; i <= this.ListBox1.Items.Count - 1; i++)
            {
                this.Encoder.OverWrite = false;
                this.Encoder.SourceFile = this.ListBox1.Items[i].ToString(); //.Item(i);
                this.Encoder.Format = Encoder.Format_MP3;
                this.Encoder.AudioCodec = Encoder.AudioCodec_mp3;
                this.Encoder.Video_Codec = Encoder.Vcodec_NONE;
                this.Encoder.Threads = "0";
                this.Encoder.OverWrite = true;
                this.Encoder.ConstantRateFactor = 17;
                this.Encoder.RateControl = Encoder.RateControl_CRF;
                this.Encoder.Libx264_Preset_pass1 = Encoder.libx264_fast;
                this.Encoder.OutputPath = this.outfolder;

                this.Encoder.Encode();
                this.ListBox1.SetItemCheckState(i, CheckState.Checked);
            }
        }
    }
}