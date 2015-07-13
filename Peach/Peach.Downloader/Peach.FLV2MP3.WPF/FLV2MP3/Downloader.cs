using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.FLV2MP3.WPF
{
    using System.IO;
    using System.Net;
    using System.Runtime.Remoting.Channels;
    using System.Threading;
    using System.Windows.Forms;

    using NReco.VideoConverter;

    using Peach.Downloader.Models;

    public delegate string DownloaderContentEventHandler(object sender, EventArgs e);

    public class Downloader : Peach.Downloader.Downloader
    {
        private static Downloader _flv2Mp3 = new Downloader();

        public static Downloader FLV2MP3
        {
            get
            {
                return _flv2Mp3;
            }
        }

        public event DownloaderContentEventHandler GettingContent;

        internal Downloader()
        {

        }

        public override string GetContent(string url)
        {
           return this.OnGettingContent();
        }

        public override void Download(ISeed seed, CancellationToken token)
        {
            seed.OnStatusChanged(0);
            string inputfile = seed.HttpUrl;
            string outputfile = seed.Url;
            FFMpegConverter converter = new FFMpegConverter();
            converter.FFMpegToolPath = AppDomain.CurrentDomain.BaseDirectory;
            converter.FFMpegExeName = "ffmpeg.exe";

            converter.ConvertProgress += (sender, e) =>
            {
                if (token.IsCancellationRequested)
                {
                    converter.Abort();
                    return;
                }

                int changed = (int)(e.Processed.TotalSeconds * 100 / e.TotalDuration.TotalSeconds);
                if (changed < 100)
                    seed.OnStatusChanged(changed);
                else
                    seed.OnCompleted();
            };

//#if DEBUG
//            converter.LogReceived += (sender, e) =>
//            {
//                MessageBox.Show(e.Data);
//            };
//#endif

            try
            {
                ConvertSettings cs = new ConvertSettings();
                cs.AudioCodec = "libmp3lame";
                cs.Seek = 0;
                cs.CustomOutputArgs = "-b:a 64k";
                converter.ConvertMedia(inputfile, "flv", outputfile, "mp3", cs);
            }
            catch (Exception)
            {
                converter.Stop();
            }
        }

        public IList<string> GetChapters()
        {
            string target = this.GetContent(AppDomain.CurrentDomain.BaseDirectory);
            if(string.IsNullOrEmpty(target))
                return new List<string>();

            try
            {
                var subs = Directory.GetDirectories(target);
                if(subs.Length==0) return new List<string>(){target};
                return subs.ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        protected virtual string OnGettingContent()
        {
            var handler = this.GettingContent;
            if (handler != null) { return handler(this, new EventArgs()); }

            return null;
        }
    }
}
