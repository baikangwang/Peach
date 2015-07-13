using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Downloader.TingShu520
{
    using System.Threading;
    using System.Threading.Tasks;

    using Peach.Downloader.Models;

    public class DownloadingQueue:Peach.Downloader.DownloadingQueue
    {
        private static DownloadingQueue _tingshu520 = new DownloadingQueue(PendingQueue.TingShu520);

        public static DownloadingQueue TingShu520
        {
            get
            {
                return _tingshu520;
            }
        }

        protected override void DownloadAsync(ISeed seed, CancellationToken token)
        {
            Task.Factory.StartNew(() => { Downloader.TingShu520.Download(seed, token); }, token);
        }

        protected DownloadingQueue(Peach.Downloader.PendingQueue pendingQueue)
            : base(pendingQueue)
        {
        }
    }
}
