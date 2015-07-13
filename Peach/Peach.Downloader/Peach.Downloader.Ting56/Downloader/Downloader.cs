using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Downloader.Ting56
{
    using System.IO;
    using System.Net;
    using System.Threading;

    using Peach.Downloader.Models;

    public class Downloader : Peach.Downloader.Downloader
    {
        private static Downloader _ting56 = new Downloader();

        public static Downloader Ting56
        {
            get
            {
                return _ting56;
            }
        }

        public IList<string> CHAPTERS = new List<string>()
                                               {
                                                   "http://www.ting56.com/mp3/218.html", // 第一季
                                                   "http://www.ting56.com/mp3/219.html", // 第二季
                                                   "http://www.ting56.com/mp3/220.html",
                                                   // 盗墓笔记 第三季
                                                   "http://www.ting56.com/mp3/899.html",
                                                   // 盗墓笔记 第四季
                                                   "http://www.ting56.com/mp3/1342.html",
                                                   // 盗墓笔记 第五季
                                                   "http://www.ting56.com/mp3/1748.html",
                                                   // 盗墓笔记 第六季
                                                   "http://www.ting56.com/mp3/1981.html",
                                                   // 盗墓笔记 第七季
                                                   "http://www.ting56.com/mp3/2568.html",
                                                   // 盗墓笔记 第八季
                                                   "http://www.ting56.com/mp3/2447.html",
                                                   // 盗墓笔记 藏海花
                                               };

        internal Downloader()
        {

        }

        public override string GetContent(string url)
        {
            int i = 0;
            while (i < 3)
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "text/html, application/xhtml+xml, */*";
                request.KeepAlive = true;
                request.Host = "www.ting56.com";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)";
                request.ProtocolVersion = Version.Parse("1.1");

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        using (Stream s = response.GetResponseStream())
                        {
                            byte[] content = new byte[response.ContentLength];
                            s.Read(content, 0, content.Length);
                            string result = Encoding.Default.GetString(content);
                            return result;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                Thread.Sleep(1000);
                i++;
            }

            return String.Empty;
        }

        public override void Download(ISeed seed, CancellationToken token)
        {
            seed.OnStatusChanged(0);

            string url = seed.Url;
            string file = this.GetFileName(seed);
            if (File.Exists(file)) File.Delete(file);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                wc.Headers[HttpRequestHeader.AcceptLanguage] = "en-US";
                wc.Headers[HttpRequestHeader.Accept] = "text/html, application/xhtml+xml, */*";
                wc.Headers[HttpRequestHeader.Host] = "vr.tudou.com";
                wc.Headers[HttpRequestHeader.UserAgent] =
                    "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)";
                wc.Headers["UA-CPU"] = "AMD64";

                try
                {
                    wc.DownloadProgressChanged += (sender, e) =>
                    {
                        int changed = (int)(e.BytesReceived * 100 / e.TotalBytesToReceive);
                        seed.OnStatusChanged(changed);
                    };
                    wc.DownloadFileCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            seed.OnFail();
                        }
                        else if (!e.Cancelled) seed.OnCompleted();
                    };
                    wc.DownloadFileAsync(new Uri(url), file);

                    while (wc.IsBusy)
                    {
                        if (token.IsCancellationRequested)
                        {
                            wc.CancelAsync();
                            break;
                        }

                        Thread.Sleep(1000);
                    }
                }
                catch (Exception)
                {
                    seed.OnFail();
                }
            }
        }
    }
}
