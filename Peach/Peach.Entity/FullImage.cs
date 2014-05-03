// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FullImage.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The full image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Entity
{
    /// <summary>
    /// The full image.
    /// </summary>
    public class FullImage : Img
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FullImage"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public FullImage(string title, string url)
            : base(title, url)
        {
        }

        #endregion
    }
}