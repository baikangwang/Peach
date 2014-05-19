using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peach.Entity;

namespace Peach.Viewer.EventHandler
{
    public delegate void ImageEventHandler(object sender, ImageEventArgs e);

    public class ImageEventArgs:EventArgs
    {
        public int Index { get; private set; }

        public IImage Image { get; private set; }

        public ImageEventArgs(int index, IImage image)
        {
            this.Index = index;
            this.Image = image;
        }
    }


}
