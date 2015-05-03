﻿namespace Peach.Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    using Peach.Downloader.Models;

    public delegate void PendingQueueLoadEvent(object sender, IList<ISeed> seeds);

    public delegate void PendingQueueStatusEvent(object sender, string message);

    public class PendingQueue:IDisposable
    {
        private QueuePresenter _presenter;

        private IList<ISeed> _seeds; 

        private CancellationTokenSource _cancel;

        private CancellationTokenSource _stoper;

        public event PendingQueueLoadEvent Ready;

        public event PendingQueueStatusEvent StatusChanged;

        private static PendingQueue _default=new PendingQueue();

        public static PendingQueue Default
        {
            get
            {
                return _default;
            }
        }

        private static object _lock = new object();

        private Task _thread;

        protected QueuePresenter Presenter
        {
            get
            {
                return this._presenter ?? (this._presenter = new QueuePresenter());
            }
        }

        private Queue<ISeed> _queue;

        public PendingQueue()
        {
            this._queue = new Queue<ISeed>();
            this._cancel = new CancellationTokenSource();
            this._stoper=new CancellationTokenSource();
        }

        public void Start()
        {
           this._cancel=new CancellationTokenSource();
            this._stoper=new CancellationTokenSource();
            this._queue = new Queue<ISeed>();
            this._seeds=new List<ISeed>();

            this._thread = Task.Factory.StartNew(
                () =>
                {
                    if(this._cancel.IsCancellationRequested)
                        this._cancel.Token.ThrowIfCancellationRequested();

                    this.OnStatusChanged("Loading Seeds...");

                    this._seeds = this.LoadSeeds();

                    if (this._cancel.IsCancellationRequested)
                        this._cancel.Token.ThrowIfCancellationRequested();

                    this.OnStatusChanged("Not found Seeds in cache");

                    if (this._seeds.Count == 0)
                    {
                        this.OnStatusChanged("Requesting Seeds...");

                        this._seeds = this.RequestSeeds(this._cancel.Token);

                        this.OnStatusChanged(string.Format("Requested {0} Seeds...", this._seeds.Count));

                        this.OnStatusChanged(string.Format("Saving {0} Seeds...", this._seeds.Count));

                        this.Presenter.SaveSeeds(this._seeds);
                    }

                    if (this._cancel.IsCancellationRequested)
                        this._cancel.Token.ThrowIfCancellationRequested();

                    this.Push(this._seeds);

                    this.OnStatusChanged("Ready");

                    this.OnReady(this._seeds);

                    if (this._cancel.IsCancellationRequested)
                        this._cancel.Token.ThrowIfCancellationRequested();
                }, this._cancel.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        }

        private IList<ISeed> RequestSeeds(CancellationToken token)
        {
            IList<ISeed> seeds=new List<ISeed>();
            IList<string> chapters = Downloader.Ting56.CHAPTERS;

            //IList<Task> tasks=new List<Task>();

            foreach (string cUrl in chapters)
            {
                this.OnStatusChanged(string.Format("Acquiring {0}...",cUrl));

                if(token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                var url1 = cUrl;
                //Task t= Task.Factory.StartNew(() =>
                //{
                    if(token.IsCancellationRequested)
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
                        if(token.IsCancellationRequested)
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
                                    string[] mas = r.Split(ma).Where(a=>!string.IsNullOrEmpty(a)).Select(
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
                                    string[] paras = r.Split(ma).Where(s=>!string.IsNullOrEmpty(s)).ToArray();
                                    if (paras.Length > 0)
                                        url = string.Format("http://vr.tudou.com/v2proxy/v2?it={0}", paras[0]);
                                }
                            }
                        }

                        ISeed seed = new Seed(title, chapter, episode, url, httpUrl);

                        this.OnStatusChanged(string.Format("Acquired {0}-第{1}集", (seed as Seed).GetChapterName(),seed.Episode));

                        seeds.Add(seed);

                        this.Pause(5 * 1000);
                        if(token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();
                    }

                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                //}, token);

                //tasks.Add(t);
            }

            //Task.WaitAll(tasks.ToArray());

            return seeds;
        }

        private void Push(IList<ISeed> seeds)
        {
            lock (_lock)
            {
                foreach (ISeed seed in seeds)
                {
                    this._queue.Enqueue(seed);
                }
            }
        }

        public void Stop()
        {
            if(this._cancel!=null)
            this._cancel.Cancel();
            if(this._stoper!=null)
            this._stoper.Cancel();
            if (this._thread != null)
            {
                while (!this._thread.IsCanceled && !this._thread.IsCompleted && !this._thread.IsFaulted)
                {
                    this.Pause(1000);
                }
            }

            this.RefreshCache();

            this.Dispose();
        }

        public void RefreshCache()
        {
            lock (_lock)
            {
                if(this._seeds!=null)
                this.Presenter.SaveSeeds(this._seeds);
            }
        }

        private void Pause(int peroid)
        {
            this._stoper.Token.WaitHandle.WaitOne(peroid);
        }

        private IList<ISeed> LoadSeeds()
        {
            IList<ISeed> seeds = this.Presenter.ListAllSeeds();

            return seeds;
        }

        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                if (this._seeds != null)
                {
                    this._seeds.Clear();
                    this._seeds = null;
                }

                lock (_lock)
                {
                    if (this._queue != null)
                    {
                        this._queue.Clear();
                        this._queue = null;
                    }
                }
                this._presenter = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public ISeed Dequeue()
        {
            lock (_lock)
            {
                if (this._queue.Count > 0)
                {
                    ISeed seed = this._queue.Dequeue();
                    return seed;
                }

                return null;
            }
        }

        public bool IsEmpty()
        {
            lock (_lock)
            {
                return this._queue.Count == 0;
            }
        }

        protected virtual void OnReady(IList<ISeed> seeds)
        {
            var handler = this.Ready;
            if (handler != null)
            {
                handler(this, seeds);
            }
        }

        protected virtual void OnStatusChanged(string message)
        {
            var handler = this.StatusChanged;
            if (handler != null)
            {
                handler(this, message);
            }
        }
    }
}
