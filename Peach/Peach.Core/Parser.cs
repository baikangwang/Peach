namespace Peach.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Peach.Entity;

    /// <summary>
    /// The parser.
    /// </summary>
    public abstract class Parser:IDisposable
    {
        private string _input;

        protected virtual string Input
        {
            get { return this._input; }
            set { this._input = value; }
        }

        protected Parser(string input)
        {
            this._input = input;
        }

        protected Parser()
        {
            
        }

        /// <summary>
        /// The list galleries.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="cleanup"></param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public virtual IList<Gallery> ListGalleries(string html, bool cleanup = false)
        {
            this._input = html;
            return this.ListGalleries(cleanup);
        }

        public abstract IList<Gallery> ListGalleries(bool cleanup = false);

        /// <summary>
        /// The get thumbnail.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="cleanup"></param>
        /// <returns>
        /// The <see cref="Thumbnail"/>.
        /// </returns>
        protected virtual Thumbnail GetThumbnail(string html, bool cleanup = false)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            /* <a title="View Horny Girls on Vacation - Kristyna - Part 1" href="http://www.imagefap.com/image.php?id=499149355">
             * <img border="0" 
             *      alt="Free porn pics of Horny Girls on Vacation - Kristyna - Part 1 1 of 4 pics" 
             *      src="http://x4.fap.to/images/thumb/52/499/499149355.jpg">
             * </a>*/

            string pattern =
                "<a.*?title\\s*?=\\s*?\"(?<gallery>.*?)\".*?href\\s*?=\\s*?\"(?<fullurl>.*?)\".*?><img.*?alt\\s*?=\\s*?\"(?<title>.*?)\".*?src\\s*?=\\s*?\"(?<thumbnailurl>.*?)\".*?>\\s*</a>";

            string input = cleanup ? html.Replace("\\n", string.Empty).Replace("\\r", string.Empty) : html;

            Match match = Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.Singleline);
            if (match.Success)
            {
                string fullurl = match.Groups["fullurl"].Value;
                string title = match.Groups["title"].Value;
                string thumbnailurl = match.Groups["thumbnailurl"].Value;

                return new Thumbnail(title, thumbnailurl, fullurl);
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid thumbnail tag, {0}", html));
                return null;
            }
        }

        /// <summary>
        /// The get gallery.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="cleanup"></param>
        /// <returns>
        /// The <see cref="Gallery"/>.
        /// </returns>
        protected abstract Gallery GetGallery(string html, bool cleanup = false);

        /*
        public IList<Thumbnail> ListThumbnails(string html)
        {
            return new List<Thumbnail>();
        }
         * */

        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                this._input = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
