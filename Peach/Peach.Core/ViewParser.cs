﻿namespace Peach.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    using Peach.Entity;

    /// <summary>
    /// The parser.
    /// </summary>
    public abstract class ViewParser : BaseParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        protected ViewParser(string input):base(input)
        {
            if (string.IsNullOrEmpty(input))
            {
                this.Input = string.Empty;
            }
            else
            {
                Regex r = new Regex("\\r|\\n", RegexOptions.Compiled);
                this.Input = r.Replace(input, string.Empty);
            }
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
            this.Input = html;
            return this.ListGalleries(cleanup);
        }

        /// <summary>
        /// The list galleries.
        /// </summary>
        /// <param name="cleanup">
        /// The cleanup.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
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
                "<a.*?href\\s*=\\s*\"(?<fullurl>.*?)\".*?><img.*?alt\\s*=\\s*\"(?<title>.*?)\".*?src\\s*=\\s*\"(?<thumbnailurl>.*?)\".*?>\\s*</a>";

            string input = cleanup ? Regex.Replace(html, "(\r|\n)", string.Empty) : html;

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif

            Match match = r.Match(input);

#if DEBUG
            timer.Stop();
            Console.WriteLine(string.Format("Thumbnail Match costed: {0}", timer.Elapsed));
#endif
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
    }
}