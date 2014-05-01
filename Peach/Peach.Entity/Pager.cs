// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pager.cs" company="">
//   
// </copyright>
// <summary>
//   The pager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;

namespace Peach.Entity
{
    using System.Collections.Generic;

    /// <summary>
    ///     The pager.
    /// </summary>
    public class Pager
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Pager"/> class.
        /// </summary>
        /// <param name="pages">
        /// The pages.
        /// </param>
        /// <param name="next">
        /// The next.
        /// </param>
        /// <param name="current">
        /// The current.
        /// </param>
        public Pager(IList<Page> pages, Page next, Page current)
        {
            this.Pages = pages;
            this.Current = current;
            this.Next = next;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the current.
        /// </summary>
        public Page Current { get; set; }

        /// <summary>
        ///     Gets or sets the next.
        /// </summary>
        public Page Next { get; set; }

        /// <summary>
        ///     Gets or sets the pages.
        /// </summary>
        public IList<Page> Pages { get; set; }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Current Page-> {0}", this.Current));
            foreach (Page p in this.Pages)
            {
                sb.AppendLine(string.Format("Other Page-> {0}", p));
            }
            sb.AppendLine(string.Format("Next page-> {0}", this.Next));
            return sb.ToString();
        }
    }
}