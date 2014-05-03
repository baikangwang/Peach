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
            this._galleries = new List<Gallery>() {this.ViewParser.ListGalleries().FirstOrDefault()};

            Task[] tgs = new Task[this._galleries.Count];

            for (int i = 0; i < this._galleries.Count;i++ )
            {
                Gallery ig = this._galleries[i];
                Task tg = new Task(() =>
                                    {
                                        Task[] tts = new Task[ig.Thumbnails.Count];

                                        for (int j = 0; j < ig.Thumbnails.Count; j++)
                                        {
                                            Thumbnail it = ig.Thumbnails[j];
                                            Task tt = new Task(() =>
                                                                 {
                                                                     MethodResult<HttpWebResponse> r =
                                                                         Browser.Current.GetImage(it.Url);

                                                                     it.Load(r.Result.GetResponseStream());
                                                                 });
                                            tts[j] = tt;
                                            tt.Start();
                                        }

                                        Task.WaitAll(tts);
                                    });
                tgs[i] = tg;
                tg.Start();
            }

            Task.WaitAll(tgs);
        }
    }
}
