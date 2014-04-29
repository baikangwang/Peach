using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Peach.Entity;

namespace Peach.Core
{
    public class HomeParser:Parser
    {
        public HomeParser(string input) : base(input)
        {
        }

        public HomeParser():base()
        {
            
        }

        /// <summary>
        /// The list galleries.
        /// </summary>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public override IList<Gallery> ListGalleries(bool cleanup = false)
        {
            string input = cleanup ? this.Input.Replace("\n", string.Empty).Replace("\r", string.Empty) : this.Input;

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
                Peach.Log.Logger.Current.Warn(string.Format("Invalid thumbnail tag, {0}", this.Input));
            }

            return gs;
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
        protected override Gallery GetGallery(string html, bool cleanup = false)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            /* (?<gallery><tr\s*?id='(\w*?)'.*?>\s*<td.*?>.*?<a.*?>View gallery</a>.*?<b>(?<title>.+?)</b>.*?</td>\s*<td.*?>.*?</td>\s*<td.*?>.*?</td>\s*</tr>\s*<tr>\s*<td.*?>\s*<table>\s*<tr.*?>\s*<td.*?>\s*</td>(\s*<td.*?>\s*(?<thumbnail><a.*?>\s*<img.*?>\s*</a>).*?</td>\s*)+?</tr>\s*</table>\s*</tr>) */

            string pattern =
                "(?<gallery><tr\\s*?id='(\\w*?)'.*?>\\s*<td.*?>.*?<a.*?href=\"(?<url>.*?)\".*?>View gallery</a>.*?<b>\\s*?(?<title>.+?)\\s*?</b>.*?</td>\\s*<td.*?>.*?</td>\\s*<td.*?>.*?</td>\\s*</tr>\\s*<tr>\\s*<td.*?>\\s*<table>\\s*<tr.*?>\\s*<td.*?>\\s*</td>(\\s*<td.*?>\\s*(?<thumbnail><a.*?>\\s*<img.*?>\\s*</a>).*?</td>\\s*)+?</tr>\\s*</table>\\s*</tr>)";

            string input = cleanup ? html.Replace("\\n", string.Empty).Replace("\\r", string.Empty) : html;

            Match match = Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            if (match.Success)
            {
                string title = match.Groups["title"].Value;
                string url = match.Groups["url"].Value;
                Gallery g = new Gallery(title, url);

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

        /*
        public IList<Thumbnail> ListThumbnails(string html)
        {
            return new List<Thumbnail>();
        }
         * */
    }
}
