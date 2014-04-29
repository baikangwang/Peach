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
    public class Parser
    {
        /// <summary>
        /// The list galleries.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public IList<Gallery> ListGalleries(string html)
        {
            string input = html.Replace("\n", string.Empty).Replace("\r", string.Empty);
            
            string single =
                "(?<gallery><tr\\s*?id='(\\w*?)'.*?>.*?<a.*?>View gallery</a>.*?</tr>\\s*<tr>.*?<table>\\s*<tr.*?>*?(\\s*<td.*?>\\s*<a.*?>\\s*<img.*?>\\s*</a>.*?</td>\\s*)+?</tr>\\s*</table>\\s*</tr>)";

            string pattern = string.Format("<table.*?>.*?(?<galleries>{0}+).*?</table>", single);

            Match match = Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.Singleline);
            
            IList<Gallery> gs = new List<Gallery>();
            
            if (match.Success)
            {
                foreach (Capture c in match.Groups["gallery"].Captures)
                {
                    Gallery g = this.GetGallery(c.Value);

                    gs.Add(g);
                }
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid thumbnail tag, {0}", html));
            }

            return gs;
        }

        /// <summary>
        /// The get thumbnail.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <returns>
        /// The <see cref="Thumbnail"/>.
        /// </returns>
        public Thumbnail GetThumbnail(string html)
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
            
            string input = html.Replace("\\n", string.Empty).Replace("\\r", string.Empty);

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
        /// <returns>
        /// The <see cref="Gallery"/>.
        /// </returns>
        public Gallery GetGallery(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }
            
            /* (?<gallery><tr\s*?id='(\w*?)'.*?>\s*<td.*?>.*?<a.*?>View gallery</a>.*?<b>(?<title>.+?)</b>.*?</td>\s*<td.*?>.*?</td>\s*<td.*?>.*?</td>\s*</tr>\s*<tr>\s*<td.*?>\s*<table>\s*<tr.*?>\s*<td.*?>\s*</td>(\s*<td.*?>\s*(?<thumbnail><a.*?>\s*<img.*?>\s*</a>).*?</td>\s*)+?</tr>\s*</table>\s*</tr>) */
            
            string pattern =
                "(?<gallery><tr\\s*?id='(\\w*?)'.*?>\\s*<td.*?>.*?<a.*?>View gallery</a>.*?<b>\\s*?(?<title>.+?)\\s*?</b>.*?</td>\\s*<td.*?>.*?</td>\\s*<td.*?>.*?</td>\\s*</tr>\\s*<tr>\\s*<td.*?>\\s*<table>\\s*<tr.*?>\\s*<td.*?>\\s*</td>(\\s*<td.*?>\\s*(?<thumbnail><a.*?>\\s*<img.*?>\\s*</a>).*?</td>\\s*)+?</tr>\\s*</table>\\s*</tr>)";

            Match match = Regex.Match(html, pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            if (match.Success)
            {
                string title = match.Groups["title"].Value;
                Gallery g = new Gallery(title);

                foreach (Capture c in match.Groups["thumbnail"].Captures)
                {
                    Thumbnail t = this.GetThumbnail(c.Value);
                    g.Add(t);
                }

                return g;
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid gallery tag, {0}", html));
                return null;
            }
        }

        public IList<Thumbnail> ListThumbnails(string html)
        {
            return new List<Thumbnail>();
        }
    }
}
