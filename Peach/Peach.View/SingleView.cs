using System;
using System.IO;
using System.Net;
using Peach.Core;

namespace Peach.View
{
    using Peach.Entity;

    /// <summary>
    /// The single view.
    /// </summary>
    public class SingleView:View
    {
        private FullImage _fullImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleView"/> class.
        /// </summary>
        /// <param name="fullImage">
        /// The full image.
        /// </param>
        public SingleView(string url):base(url)
        {
            this._fullImage = new FullImage(string.Empty, url);
        }

        /// <summary>
        /// Gets or sets the full image.
        /// </summary>
        public FullImage FullImage
        {
            get { return _fullImage; }
        }

        public override void GetView()
        {
            MethodResult<HttpWebResponse> r = Browser.Current.GetImage(this.Url);
            if (r)
            {
                        this._fullImage.Load(r.Result.GetResponseStream());
            }
            else
            {
                // no response, show error
                //throw exception
            }
        }
    }
}
