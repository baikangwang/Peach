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

            /*
            this.OnViewStatusChanged(new ViewEventArgs(string.Format("Starting to draw all of galleries...")));

            //Task[] tgs = new Task[this._galleries.Count];

            for (int i = 0; i < this._galleries.Count; i++)
            {
                Gallery ig = this._galleries[i];
                Task tg = Task.Factory.StartNew(
                    () =>
                    {
                        this.OnViewStatusChanged(new ViewEventArgs(string.Format("Starting to get all of thumbnails of the gallery {0}...", ig.Title)));

                        Task[] tts = new Task[ig.Thumbnails.Count];

                        for (int j = 0; j < ig.Thumbnails.Count; j++)
                        {
                            IThumbnail it = ig.Thumbnails[j];
                            Task tt = new Task(
                                () =>
                                {
                                    this.OnViewStatusChanged(new ViewEventArgs(string.Format("Starting to get thumbnail {0}...", it.Url)));

                                    it.Load();
                                    this.OnViewStatusChanged(new ViewEventArgs(string.Format("Finish to get thumbnail {0}", it.Url)));
                                });

                            tts[j] = tt;
                            tt.Start();
                        }

                        Task.WaitAll(tts);

                        this.OnViewStatusChanged(new ViewEventArgs(string.Format("Finish to get all of thumbnails of the gallery {0}", ig.Title)));
                    });
                //tgs[i] = tg;
            }

            //Task.WaitAll(tgs);

            this.OnViewStatusChanged(new ViewEventArgs(string.Format("Finish to draw all of galleries.")));
        */
        }
    }
}
