// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Page.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Peach.Entity
{
    /// <summary>
    ///     The page.
    /// </summary>
    public class Page:IDisposable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public Page(int number, string url)
        {
            this.Number = number;
            this.Url = url;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Format("Number: {0}, Url: {1}", this.Number, this.Url);
        }

        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                this.Url = null;
                this.Number = 0;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}