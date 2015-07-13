using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.FLV2MP3.WPF
{
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;

    using Peach.Downloader.Models;

    class PendingQueue:Peach.Downloader.PendingQueue
    {
        private static PendingQueue _flv2Mp3 = new PendingQueue();

        public static PendingQueue FLV2MP3
        {
            get
            {
                return _flv2Mp3;
            }
        }
        
        protected override IList<ISeed> RequestSeeds(CancellationToken token)
        {
            IList<ISeed> seeds = new List<ISeed>();
            IList<string> chapters = Downloader.FLV2MP3.GetChapters();
            if (chapters.Count == 0)
            {
                MessageBox.Show("Not found available flv folders");
                return seeds;
            }

            //IList<Task> tasks=new List<Task>();

            foreach (string cUrl in chapters)
            {
                this.OnStatusChanged(string.Format("Acquiring {0}...", cUrl));

                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                var url1 = cUrl;
                //Task t= Task.Factory.StartNew(() =>
                //{
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                int chapter = chapters.IndexOf(url1) + 1;

                IList<string> files = Directory.GetFiles(cUrl,"*.flv",SearchOption.TopDirectoryOnly);

                if (files.Count == 0)
                {
                    MessageBox.Show(string.Format("Not found available flvs in the {0}",cUrl));
                    this.OnStatusChanged(string.Format("Not found episodes -> {0}", url1));
                    return seeds;
                }

                foreach (var file in files)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    string httpUrl = file;
                    string title = Path.GetFileNameWithoutExtension(file);
                    string url = Path.Combine(Path.GetDirectoryName(httpUrl), string.Format("{0}.mp3", title));
                    int episode = 0;
                    if (!int.TryParse(Regex.Match(title, "\\d+").Value, out episode)) episode = 0;

                    ISeed seed = new Seed(title, chapter, episode, url, httpUrl);

                    this.OnStatusChanged(string.Format("Acquired {0}-第{1}集", (seed as Seed).GetChapterName(), seed.Episode));

                    seeds.Add(seed);

                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                }

                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
            }

            return seeds;
        }
    }
}
