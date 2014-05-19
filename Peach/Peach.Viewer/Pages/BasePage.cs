using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using Peach.Viewer.EventHandler;

namespace Peach.Viewer
{
    public abstract class BasePage:UserControl
    {
        private CancellationTokenSource _pagecancel = new CancellationTokenSource();
        public CancellationTokenSource PageCancellationToken { get { return _pagecancel; } }

        private ManualResetEvent _basicControlReady = new ManualResetEvent(false);
        protected ManualResetEvent BasicControlReady
        {
            get { return this._basicControlReady; }
        }

        protected virtual bool IsCompleted { get; set; }
        
        public event GalleryEventHandler GalleryClicked;

        protected virtual void OnGalleryClicked(GalleryEventArgs e)
        {
            GalleryEventHandler handler = GalleryClicked;
            if (handler != null) handler(this, e);
        }

        public event ImageEventHandler ImageClicked;

        protected virtual void OnImageClicked(ImageEventArgs e)
        {
            ImageEventHandler handler = ImageClicked;
            if (handler != null) handler(this,e);
        }
    }
}
