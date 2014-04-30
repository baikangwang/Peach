﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Page.cs" company="">
//   
// </copyright>
// <summary>
//   The page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Entity
{
    /// <summary>
    ///     The page.
    /// </summary>
    public class Page
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
    }
}