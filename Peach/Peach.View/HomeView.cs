using System.IO;
using System.Net;
using Peach.Core;

namespace Peach.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Peach.Entity;

    /// <summary>
    /// The home view.
    /// </summary>
    public class HomeView:View<HomeViewParser>
    {
        private IList<Gallery> _galleries;

        /// <summary>
        /// Gets or sets the galleries.
        /// </summary>
        public IList<Gallery> Galleries
        {
            get { return _galleries; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeView"/> class.
        /// </summary>
        public HomeView(string url) : base(url)
        {
        }

        public override void GetView()
        {
            base.GetView();
            this._galleries = this.ViewParser.ListGalleries();
        }
    }
}
