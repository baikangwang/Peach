using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.FLV2MP3.WPF
{
    using System.Threading;
    using System.Threading.Tasks;

    using Peach.Downloader.Models;

    class DownloadingQueue:Peach.Downloader.DownloadingQueue
    {
        private static DownloadingQueue _flv2Mp3 = new DownloadingQueue(PendingQueue.FLV2MP3);

        public static DownloadingQueue FLV2MP3
        {
            get
            {
                return _flv2Mp3;
            }
        }

        protected override void DownloadAsync(ISeed seed, CancellationToken token)
        {
            Task.Factory.StartNew(() => { Downloader.FLV2MP3.Download(seed, token); }, token);
        }

        protected DownloadingQueue(Peach.Downloader.PendingQueue pendingQueue)
            : base(pendingQueue)
        {
        }
    }
}
