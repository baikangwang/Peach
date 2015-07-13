using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Downloader.Ting56
{
    using System.Threading;
    using System.Threading.Tasks;

    using Peach.Downloader.Models;

    class DownloadingQueue:Peach.Downloader.DownloadingQueue
    {
        private static DownloadingQueue _ting56 = new DownloadingQueue(PendingQueue.Ting56);

        public static DownloadingQueue Ting56
        {
            get
            {
                return _ting56;
            }
        }

        protected override void DownloadAsync(ISeed seed, CancellationToken token)
        {
            Task.Factory.StartNew(() => { Downloader.Ting56.Download(seed, token); }, token);
        }

        protected DownloadingQueue(Peach.Downloader.PendingQueue pendingQueue)
            : base(pendingQueue)
        {
        }
    }
}
