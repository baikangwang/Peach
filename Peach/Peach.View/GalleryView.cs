namespace Peach.View
{
    using Peach.Entity;

    /// <summary>
    /// The gallery view.
    /// </summary>
    public class GalleryView
    {
        /// <summary>
        /// The _current.
        /// </summary>
        private Gallery _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryView"/> class.
        /// </summary>
        public GalleryView()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryView"/> class.
        /// </summary>
        /// <param name="gallery">
        /// The gallery.
        /// </param>
        public GalleryView(Gallery gallery)
        {
            this._current = gallery;
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
            set
            {
                this._current = value;
            }
        }
    }
}
