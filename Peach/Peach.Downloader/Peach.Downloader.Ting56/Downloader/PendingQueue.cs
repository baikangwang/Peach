using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Downloader.Ting56
{
    using System.Text.RegularExpressions;
    using System.Threading;

    using Peach.Downloader.Models;

    class PendingQueue:Peach.Downloader.PendingQueue
    {
        private static PendingQueue _ting56 = new PendingQueue();

        public static PendingQueue Ting56
        {
            get
            {
                return _ting56;
            }
        }
        
        protected override IList<ISeed> RequestSeeds(CancellationToken token)
        {
            IList<ISeed> seeds = new List<ISeed>();
            IList<string> chapters = Downloader.Ting56.CHAPTERS;

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

                MatchCollection ms = null;

                int i = 0;
                while (i < 10)
                {
                    string content = Downloader.Ting56.GetContent(url1);

                    Regex regex =
                        new Regex(
                            "\\<a\\s*title\\s*=\\s*'(?<title>.*?)'\\s*href\\s*=\\s*'(?<url>.*?)'\\s*target\\s*=\\s*\"_blank\"\\>",
                            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ms = regex.Matches(content);

                    if (ms.Count > 0) break;

                    this.Pause(5 * 1000);
                    i++;
                }

                if (ms == null || ms.Count == 0)
                {
                    this.OnStatusChanged(string.Format("Not found episodes -> {0}", url1));
                    return seeds;
                }

                foreach (Match match in ms)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    string httpUrl = "N/A";
                    string title = "N/A";
                    string url = "N/A";
                    int episode = 0;

                    if (match.Success)
                    {
                        httpUrl = string.Format("http://www.ting56.com{0}", match.Groups["url"].Value);
                        string shortTitle = match.Groups["title"].Value;
                        if (!int.TryParse(Regex.Match(shortTitle, "\\d+").Value, out episode)) episode = 0;

                        if (!string.IsNullOrEmpty(httpUrl))
                        {
                            string eContent = Downloader.Ting56.GetContent(httpUrl);
                            string pattern =
                                "\\<script\\>var\\s*datas\\s*=\\s*\\(FonHen_JieMa\\('(?<ma>.*)'\\)\\.split\\('&'\\)\\);\\s*var\\s*part\\s*=\\s*'(?<title>.*)';\\s*var\\s*play_vid\\s*=\\s*'.*';\\</script\\>";

                            Match m = Regex.Match(eContent, pattern);
                            if (m.Success)
                            {
                                title = m.Groups["title"].Value;
                                string ma = m.Groups["ma"].Value;
                                string p = "\\*";
                                Regex r = new Regex(p, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                                string[] mas = r.Split(ma).Where(a => !string.IsNullOrEmpty(a)).Select(
                                    s =>
                                    {
                                        int segament;
                                        if (!int.TryParse(s, out segament)) segament = 0;
                                        string c = Convert.ToChar(segament).ToString();
                                        return c;
                                    }).ToArray();
                                ma = string.Join(string.Empty, mas);
                                p = "&";
                                r = new Regex(p, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                                string[] paras = r.Split(ma).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                                if (paras.Length > 0)
                                    url = string.Format("http://vr.tudou.com/v2proxy/v2?it={0}", paras[0]);
                            }
                        }
                    }

                    ISeed seed = new Seed(title, chapter, episode, url, httpUrl);

                    this.OnStatusChanged(string.Format("Acquired {0}-第{1}集", (seed as Seed).GetChapterName(), seed.Episode));

                    seeds.Add(seed);

                    this.Pause(5 * 1000);
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
