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
    public class HomeView
    {
        /// <summary>
        /// Gets or sets the galleries.
        /// </summary>
        public IList<Gallery> Galleries { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeView"/> class.
        /// </summary>
        public HomeView()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeView"/> class.
        /// </summary>
        /// <param name="galleries">
        /// The galleries.
        /// </param>
        public HomeView(IList<Gallery> galleries)
        {
            this.Galleries = galleries;
        }
    }
}
