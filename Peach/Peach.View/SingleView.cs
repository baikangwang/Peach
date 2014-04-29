namespace Peach.View
{
    using Peach.Entity;

    /// <summary>
    /// The single view.
    /// </summary>
    public class SingleView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleView"/> class.
        /// </summary>
        /// <param name="fullImage">
        /// The full image.
        /// </param>
        public SingleView(FullImage fullImage)
        {
            this.FullImage = fullImage;
        }

        /// <summary>
        /// Gets or sets the full image.
        /// </summary>
        public FullImage FullImage { get; set; }
    }
}
