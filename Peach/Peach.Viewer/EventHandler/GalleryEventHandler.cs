using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peach.Entity;

namespace Peach.Viewer.EventHandler
{
    public delegate void GalleryEventHandler(object sender, GalleryEventArgs e);

    public class GalleryEventArgs:EventArgs
    {
        public int Index { get; private set; }
        public Gallery Gallery { get; private set; }

        public GalleryEventArgs(int index, Gallery gallery)
        {
            this.Index = index;
            this.Gallery = gallery;
        }
    }
}
