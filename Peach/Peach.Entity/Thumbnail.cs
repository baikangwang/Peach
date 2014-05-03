// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Thumbnail.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The thumbnail.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Entity
{
    /// <summary>
    /// The thumbnail.
    /// </summary>
    public class Thumbnail : Img
    {
        #region Fields

        /// <summary>
        /// The full url.
        /// </summary>
        private string fullUrl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Thumbnail"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="fullurl">
        /// The fullurl.
        /// </param>
        public Thumbnail(string title, string url, string fullurl)
            : base(title, url)
        {
            this.fullUrl = fullurl;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the full url.
        /// </summary>
        public string FullUrl
        {
            get
            {
                return this.fullUrl;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            string o = base.ToString();

            return string.Format("{0}, FullUrl: {1}", o, this.fullUrl);
        }

        #endregion

        protected override void Dispose(bool all)
        {
            base.Dispose(all);
            if (all)
            {
                this.fullUrl = null;
            }
        }
    }
}