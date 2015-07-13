namespace Peach.Downloader
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Peach.Downloader.Models;

    public delegate void PendingQueueLoadEvent(object sender, IList<ISeed> seeds);

    public delegate void PendingQueueStatusEvent(object sender, string message);

    public abstract class PendingQueue:IDisposable
    {
        private QueuePresenter _presenter;

        private IList<ISeed> _seeds; 

        private CancellationTokenSource _cancel;

        private CancellationTokenSource _stoper;

        public event PendingQueueLoadEvent Ready;

        public event PendingQueueStatusEvent StatusChanged;

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

        protected abstract IList<ISeed> RequestSeeds(CancellationToken token);

        private void Push(IList<ISeed> seeds)
        {
            lock (_lock)
            {
                foreach (ISeed seed in seeds)
                {
                    if (seed.Status != Status.Complete)
                    {
                    //    Where(s => s.Status != Status.Complete).Select(
                    //s =>
                    //{
                    //    s.Status = Status.Waiting;
                    //    return s;
                    //})
                        seed.Status=Status.Waiting;
                        this._queue.Enqueue(seed);
                    }
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

        protected void Pause(int peroid)
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

        public string GetCachePath()
        {
            return this.Presenter.GetCachePath();
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
