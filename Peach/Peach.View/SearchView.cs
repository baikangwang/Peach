namespace Peach.View
{
    using System.Collections.Generic;

    using Peach.Entity;

    /// <summary>
    /// The search view.
    /// </summary>
    public class SearchView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchView"/> class.
        /// </summary>
        /// <param name="pager">
        /// The pager.
        /// </param>
        /// <param name="galleries">
        /// The galleries.
        /// </param>
        public SearchView(Pager pager, IList<Gallery> galleries)
        {
            this.Pager = pager;
            this.Galleries = galleries;
        }
        
        /// <summary>
        /// Gets or sets the pager.
        /// </summary>
        public Pager Pager { get; set; }

        /// <summary>
        /// Gets or sets the galleries.
        /// </summary>
        public IList<Gallery> Galleries { get; set; }
    }
}
