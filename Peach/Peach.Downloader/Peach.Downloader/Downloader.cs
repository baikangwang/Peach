using System;
using System.Collections.Generic;

namespace Peach.Downloader
{
    using System.IO;
    using System.Net;
    using System.Runtime.Remoting.Channels;
    using System.Text;
    using System.Threading;

    using Peach.Downloader.Models;

    public class Downloader
    {
        private static Ting56 _ting56 = new Ting56();

        public static Ting56 Ting56
        {
            get
            {
                return _ting56;
            }
        }

        protected Downloader()
        {

        }

        public virtual string GetContent(string url)
        {
            int i = 0;
            while (i<3)
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

        public void Download(ISeed seed,CancellationToken token)
        {
            seed.OnStatusChanged(0);

            string url = seed.Url;
            string file = this.GetFileName(seed);
            if(File.Exists(file)) File.Delete(file);

            using (WebClient wc=new WebClient())
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

            //HttpWebRequest request = HttpWebRequest.Create(seed.Url) as HttpWebRequest;
            //request.Method = "GET";
            ////request.ContentType = "text/html, application/xhtml+xml, */*";
            //request.KeepAlive = true;
            //request.Host = "vr.tudou.com";
            //request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)";
            //request.ProtocolVersion = Version.Parse("1.1");
            //request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            //request.Headers[HttpRequestHeader.AcceptLanguage] = "en-US";
            //request.Accept = "text/html, application/xhtml+xml, */*";
            //request.Headers["UA-CPU"] = "AMD64";

            //HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            //try
            //{
            //    using (Stream s = response.GetResponseStream())
            //    {
            //        string file = this.GetFileName(seed);
            //        long total = response.ContentLength;
            //        long read = 0;

            //        if(File.Exists(file)) File.Delete(file);

            //        using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //        {
            //            byte[] content = new byte[1024 * 1024];
            //            while (true)
            //            {
            //                read += s.Read(content, 0, content.Length);
            //                fs.Write(content, 0, content.Length);
            //                fs.Flush(true);
            //                if (read == total)
            //                {
            //                    seed.Status=Status.Complete;
            //                    seed.OnCompleted();
            //                    break;
            //                }

            //                int changed =(int)((read * 100) / total);
            //                seed.OnStatusChanged(changed);
            //            }
            //            fs.Close();
            //        }
            //        s.Close();
            //    }
            //}
            //catch (Exception)
            //{
            //    seed.Status = Status.Fail;
            //    seed.OnFail();
            //}
        }

        protected virtual string GetFileName(ISeed seed)
        {
            return string.Empty;
        }
    }

    public class Ting56 : Downloader
    {
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

        public string BaseSavePath = AppDomain.CurrentDomain.BaseDirectory;

        internal Ting56()
        {

        }

        protected override string GetFileName(ISeed seed)
        {
            string fileName = string.Format("{0}.flv", seed.Title.Replace(" ",string.Empty).Replace("-","_"));
            string chapterName = (seed as Seed).GetChapterName();
            string path = Path.Combine(this.BaseSavePath, chapterName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string file = Path.Combine(path, fileName);
            return file;
        }
    }


}
