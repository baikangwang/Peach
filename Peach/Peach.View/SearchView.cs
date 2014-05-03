using Peach.Core;

namespace Peach.View
{
    using System.Collections.Generic;

    using Peach.Entity;

    /// <summary>
    /// The search view.
    /// </summary>
    public class SearchView:View<SearchViewParser>
    {
        private Pager _pager;
        private IList<Gallery> _galleries;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchView"/> class.
        /// </summary>
        /// <param name="pager">
        /// The pager.
        /// </param>
        /// <param name="galleries">
        /// The galleries.
        /// </param>
        public SearchView(string url):base(url)
        {
        }
        
        /// <summary>
        /// Gets or sets the pager.
        /// </summary>
        public Pager Pager
        {
            get { return _pager; }
        }

        /// <summary>
        /// Gets or sets the galleries.
        /// </summary>
        public IList<Gallery> Galleries
        {
            get { return _galleries; }
        }

        public override void GetView()
        {
            base.GetView();
            this._galleries = this.ViewParser.ListGalleries();
            this._pager = this.ViewParser.GetPager();
        }
    }
}
