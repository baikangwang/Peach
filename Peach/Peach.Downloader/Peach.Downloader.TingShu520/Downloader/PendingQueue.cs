using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Downloader.TingShu520
{
    using System.Text.RegularExpressions;
    using System.Threading;

    using Peach.Downloader.Models;

    public class PendingQueue:Peach.Downloader.PendingQueue
    {
        private static PendingQueue _tingshu520 = new PendingQueue();

        public static PendingQueue TingShu520
        {
            get
            {
                return _tingshu520;
            }
        }
        
        protected override IList<ISeed> RequestSeeds(CancellationToken token)
        {
            IList<ISeed> seeds = new List<ISeed>();
            IList<string> chapters = Downloader.TingShu520.CHAPTERS;

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

                //int chapter = chapters.IndexOf(url1) + 1;

                MatchCollection ms = null;

                int i = 0;
                while (i < 10)
                {
                    string content = Downloader.TingShu520.GetContent(url1);

                    Regex regex =
                        new Regex("\\<a\\s*title='(?<title>.*?)'\\s*href='(?<url>/video/\\?\\d*-\\d*-(?<index>\\d*)\\.html)'\\s*target=\"_blank\"\\>\\k<title>\\</a\\>",
                            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    ms = regex.Matches(content);

                    if (ms.Count == 421) break;

                    this.Pause(5 * 1000);
                    i++;
                }

                if (ms == null || ms.Count == 0)
                {
                    this.OnStatusChanged(string.Format("Not found audioes -> {0}", url1));
                    return seeds;
                }

                foreach (Match match in ms)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    string httpUrl = "N/A";
                    string title = "N/A";
                    string url = "N/A";
                    int chapter = 0;
                    int episode = 0;
                        string episodePattern = "\\d+";
                    string chapterPattern = "[一二三四五六七]|番外篇";

                    if (match.Success)
                    {
                        httpUrl = string.Format("http://www.520tingshu.com{0}", match.Groups["url"].Value);
                        string shortTitle = match.Groups["title"].Value;
                        int index;
                        if (!int.TryParse(match.Groups["index"].Value, out index))
                            index = 0;
                        int? id = Downloader.TingShu520.GetSeedId(index);
                        if(id==null)
                            this.OnStatusChanged(string.Format("The seed of {0} -> not existing", shortTitle));

                        //http://vr.tudou.com/v2proxy/v2?it=46481378
                        url = string.Format("http://vr.tudou.com/v2proxy/v2?it={0}", id);
                        title = shortTitle;

                        if (!int.TryParse(Regex.Match(shortTitle, episodePattern).Value, out episode)) episode = 0;

                        string chapterStr = Regex.Match(shortTitle, chapterPattern).Value;
                        switch (chapterStr)
                        {
                            case "一":
                                chapter = 1;
                                break;
                            case "二":
                                chapter = 2;
                                break;
                            case "三":
                                chapter = 3;
                                break;
                            case "四":
                                chapter = 4;
                                break;
                            case "五":
                                chapter = 5;
                                break;
                            case "六":
                                chapter = 6;
                                break;
                            case "七":
                                chapter = 7;
                                break;
                            case "番外篇":
                                chapter = 10;
                                break;
                        }
                    }

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
