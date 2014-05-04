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
            this._galleries = new List<Gallery>() { this.ViewParser.ListGalleries().FirstOrDefault() };

            this.OnViewStatusChanged(new ViewEventArgs(string.Format("Starting to draw all of galleries...")));
            
            Task[] tgs = new Task[this._galleries.Count];

            for (int i = 0; i < this._galleries.Count; i++)
            {
                Gallery ig = this._galleries[i];
                Task tg = new Task(
                    () =>
                        {
                            this.OnViewStatusChanged(new ViewEventArgs(string.Format("Starting to get all of thumbnails of the gallery {0}...", ig.Title)));
                            
                            Task[] tts = new Task[ig.Thumbnails.Count];

                            for (int j = 0; j < ig.Thumbnails.Count; j++)
                            {
                                Thumbnail it = ig.Thumbnails[j];
                                Task tt = new Task(
                                    () =>
                                        {
                                            this.OnViewStatusChanged(new ViewEventArgs(string.Format("Starting to get thumbnail {0}...", it.Url)));
                                            
                                            MethodResult<Stream> r = Browser.Current.GetImage(it.Url);
                                            if (r)
                                            {
                                                it.Load(r.Result);
                                                this.OnViewStatusChanged(new ViewEventArgs(string.Format("Finish to get thumbnail {0}", it.Url)));
                                            }
                                            else
                                            {
                                                this.OnViewStatusChanged(new ViewEventArgs(r.Message));
                                            }
                                        });

                                tts[j] = tt;
                                tt.Start();
                            }

                            Task.WaitAll(tts);

                            this.OnViewStatusChanged(new ViewEventArgs(string.Format("Finish to get all of thumbnails of the gallery {0}", ig.Title)));
                        });
                tgs[i] = tg;
                tg.Start();
            }

            Task.WaitAll(tgs);

            this.OnViewStatusChanged(new ViewEventArgs(string.Format("Finish to draw all of galleries.")));
        }
    }
}
