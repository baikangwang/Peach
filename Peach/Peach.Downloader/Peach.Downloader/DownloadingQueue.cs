using System;
using System.Collections.Generic;

namespace Peach.Downloader
{
    using System.Runtime.Remoting.Channels;
    using System.Threading;
    using System.Threading.Tasks;

    using Peach.Downloader.Models;

    public abstract class DownloadingQueue:IDisposable
    {
        private IList<ISeed> _queue;

        const int MAX_THREAD = 10;

        private CancellationTokenSource _cancel;

        private CancellationTokenSource _stoper;

        private Task _thread;

        private static object _lock=new object();

        public event SeedDownloadEvent SeedCompleted;

        public event SeedDownloadEvent SeedFail;

        public event SeedStatusEvent SeedStatusChanged;

        public event EventHandler Finished;

        private PendingQueue _pendingQueue;

        protected DownloadingQueue(PendingQueue pendingQueue)
        {
            this._cancel = new CancellationTokenSource();
            this._stoper = new CancellationTokenSource();
            this._queue = new List<ISeed>(MAX_THREAD);
            this._pendingQueue = pendingQueue;
        }

        public void Start()
        {
            this._cancel=new CancellationTokenSource();
            this._stoper=new CancellationTokenSource();
            this._queue = new List<ISeed>(MAX_THREAD);
            this._thread = Task.Factory.StartNew(
                () =>
                {
                    CancellationToken token = this._cancel.Token;

                    while (true)
                    {
                        if(token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        if (this.IsEmpty())
                        {
                            if (this._pendingQueue.IsEmpty()) this.OnFinished();
                            else
                            {
                                while (!this.IsFull())
                                {
                                    if (token.IsCancellationRequested) token.ThrowIfCancellationRequested();

                                    ISeed seed = this._pendingQueue.Dequeue();

                                    if (seed != null)
                                    {
                                        this.Enqueue(seed);
                                    }
                                    else break;
                                }
                            }
                        }
                        else if (this.IsFull())
                        {
                            this.Process(token);
                        }
                        else
                        {
                            if (this._pendingQueue.IsEmpty())
                            {
                                this.Process(token);
                                this.Pause(3*1000);
                            }
                            else
                            {
                                while (!this.IsFull())
                                {
                                    if (token.IsCancellationRequested)
                                        token.ThrowIfCancellationRequested();

                                    ISeed seed = this._pendingQueue.Dequeue();
                                    if (seed != null) this.Enqueue(seed);
                                    else break;
                                }
                            }
                        }
                    }
                },
                this._cancel.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void Process(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();

            lock (_lock)
            {
                for (int i = this._queue.Count-1; i >= 0; i--)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                    
                    ISeed seed = this._queue[i];
                    switch (seed.Status)
                    {
                            case Status.Complete:
                            case Status.Fail:
                            this._queue.RemoveAt(i);
                            break;
                        case Status.Waiting:
                            seed.Status=Status.Downloading;
                            this.DownloadAsync(seed,token);
                            break;
                        case Status.Downloading:
                        default:
                            break;
                    }
                }
            }
        }

        protected abstract void DownloadAsync(ISeed seed, CancellationToken token);

        private void Enqueue(ISeed seed)
        {
            if (seed == null) return;

            seed.Completed += this.Seed_Completed;
            seed.Fail += this.Seed_Fail;
            seed.StatusChanged += this.Seed_StatusChanged;

            this._queue.Add(seed);
        }

        private void Seed_StatusChanged(ISeed sender, int changed)
        {
            this.OnSeedStatusChanged(sender, changed);
        }

        private void Seed_Fail(ISeed sender)
        {
            this.OnSeedFail(sender);
        }

        private void Seed_Completed(ISeed sender)
        {
            this.OnSeedCompleted(sender);
        }

        private bool IsEmpty()
        {
            lock (_lock)
            {
                return this._queue.Count == 0;
            }
        }

        private void Pause(int period)
        {
            this._stoper.Token.WaitHandle.WaitOne(period);
        }

        private bool IsFull()
        {
            lock (_lock)
            {
                return this._queue.Count == MAX_THREAD;
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
            this.Dispose();
        }

        protected void Dispose(bool all)
        {
            if (all)
            {
                if (this._queue != null)
                {
                    this._queue.Clear();
                    this._queue = null;
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void OnSeedCompleted(ISeed sender)
        {
            Task.Factory.StartNew(
                () =>
                {
                    var handler = this.SeedCompleted;
                    if (handler != null)
                    {
                        handler(sender);
                    }
                });
        }

        protected virtual void OnSeedFail(ISeed sender)
        {
            Task.Factory.StartNew(
                () =>
                {
                    var handler = this.SeedFail;
                    if (handler != null)
                    {
                        handler(sender);
                    }
                });
        }

        protected virtual void OnSeedStatusChanged(ISeed sender, int changed)
        {
            Task.Factory.StartNew(
                () =>
                {
                    var handler = this.SeedStatusChanged;
                    if (handler != null)
                    {
                        handler(sender, changed);
                    }
                });
        }

        protected virtual void OnFinished()
        {
            var handler = this.Finished;
            if (handler != null) { handler(this, EventArgs.Empty); }
        }
    }
}
