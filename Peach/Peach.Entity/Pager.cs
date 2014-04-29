namespace Peach.Entity
{
    using System.Collections.Generic;

    /// <summary>
    /// The pager.
    /// </summary>
    public class Pager
    {
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
        public Pager(SortedList<int, Page> pages, Page next, Page current)
        {
            this.Pages = pages;
            this.Current = current;
            this.Next = next;
        }

        /// <summary>
        /// Gets or sets the next.
        /// </summary>
        public Page Next { get; set; }

        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        public Page Current { get; set; }

        /// <summary>
        /// Gets or sets the pages.
        /// </summary>
        public SortedList<int, Page> Pages { get; set; }
    }
}
