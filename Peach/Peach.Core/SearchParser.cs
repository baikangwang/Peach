using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Peach.Entity;

namespace Peach.Core
{
    public class SearchParser:Parser
    {
        public SearchParser(string input) : base(input)
        {
        }

        public override IList<Gallery> ListGalleries(bool cleanup = false)
        {
            string input = cleanup ? this.Input.Replace("\n", string.Empty).Replace("\r", string.Empty) : this.Input;

            string single =
                "(?<gallery><tr\\s*?id=\"\\w+\".*?>.*?<a\\s*?title=\"View\\s*?(?<title>.*?)\".*?>\\s*?<i>\\s*?<b>.*?</b>\\s*?</i>\\s*?</a>.*?</tr>\\s*<tr.*?>.*?<table>\\s*?<tr>(.*?<td.*?>\\s*?<a.*?>\\s*?<img.*?>\\s*?</a>\\s*?</td>\\s*?)+.*?</tr>\\s*?</table>*.?</tr>)";

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

        protected override Gallery GetGallery(string html, bool cleanup = false)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            /* <tr\s*?id="\w+".*?>.*?<a\s*?title="View\s*?(?<title>.*?)".*?>\s*?<i>\s*?<b>\s*?\1\s*?</b>\s*?</i>\s*?</a>.*?</tr>.*?<table>.*?(<td.*?>\s*?<a.*?>\s*?<img.*?>\s*?</a>\s*?</td>)+.*?</table>*.?</tr> */

            string pattern =
                "(?<gallery><tr\\s*?id=\"\\w+\".*?>.*?<a\\s*?title=\"View\\s*?(?<title>.*?)\".*?href=\"(?<url>.*?)\".*?>\\s*?<i>\\s*?<b>\\2</b>\\s*?</i>\\s*?</a>.*?</tr>\\s*<tr.*?>.*?<table>\\s*?<tr>(\\s*?<td.*?>\\s*?(?<thumbnail><a.*?>\\s*?<img.*?>\\s*?</a>)\\s*?</td>\\s*?)+</tr>\\s*?</table>*.?</tr>)";

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
    }
}
