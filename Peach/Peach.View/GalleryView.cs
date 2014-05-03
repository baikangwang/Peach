using System.Linq;
using Peach.Core;

namespace Peach.View
{
    using Peach.Entity;

    /// <summary>
    /// The gallery view.
    /// </summary>
    public class GalleryView:View<GalleryViewParser>
    {
        /// <summary>
        /// The _current.
        /// </summary>
        private Gallery _current;

        private Pager _pager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryView"/> class.
        /// </summary>
        public GalleryView(string url):base(url)
        {
        }

        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        public Gallery Current
        {
            get
            {
                return this._current;
            }
        }

        /// <summary>
        /// Gets or sets the pager.
        /// </summary>
        public Pager Pager
        {
            get { return _pager; }
        }

        public override void GetView()
        {
            base.GetView();
            this._current = this.ViewParser.ListGalleries().FirstOrDefault();
            this._pager = this.ViewParser.GetPager();
        }
    }
}
