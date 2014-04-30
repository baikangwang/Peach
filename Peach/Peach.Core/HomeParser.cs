namespace Peach.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Peach.Entity;

    /// <summary>
    /// The home parser.
    /// </summary>
    public class HomeParser : Parser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public HomeParser(string input) : base(input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeParser"/> class.
        /// </summary>
        public HomeParser()
            : base()
        {
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
        public override IList<Gallery> ListGalleries(bool cleanup = false)
        {
            string input = cleanup ? Regex.Replace(this.Input, "(\r|\n)", string.Empty) : this.Input;

            string single =
                "(?<gallery><tr\\s*id='(\\w*?)'.*?>.*?<a.*?>View gallery</a>.*?</tr>\\s*<tr>.*?<table>\\s*<tr.*?>*?(\\s*<td.*?>\\s*<a.*?>\\s*<img.*?>\\s*</a>.*?</td>\\s*)+?</tr>\\s*</table>\\s*</tr>)";

            string pattern = string.Format("<table.*?>.*?(?<galleries>{0}+).*?</table>", single);

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            IList<Gallery> gs = new List<Gallery>();

            if (match.Success)
            {
                CaptureCollection cc = match.Groups["gallery"].Captures;

                Task[] tasks = new Task[cc.Count];

                for (int i = 0; i < cc.Count; i++)
                {
                    int index = i;
                    Task task = new Task(
                        () =>
                        {
                            Gallery g = this.GetGallery(cc[index].Value);

                            gs.Add(g);
                        });
                    tasks[index] = task;
                    task.Start();
                }

                Task.WaitAll(tasks);
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
        /// <param name="cleanup">
        /// The cleanup.
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
                "(?<gallery><tr\\s*id='(\\w*?)'.*?>\\s*<td.*?>.*?<a.*?href=\"(?<url>.*?)\".*?>View gallery</a>.*?<b>\\s*(?<title>.+?)\\s*</b>.*?</td>\\s*<td.*?>.*?</td>\\s*<td.*?>.*?</td>\\s*</tr>\\s*<tr>\\s*<td.*?>\\s*<table>\\s*<tr.*?>\\s*<td.*?>\\s*</td>(\\s*<td.*?>\\s*(?<thumbnail><a.*?>\\s*<img.*?>\\s*</a>).*?</td>\\s*)+?</tr>\\s*</table>\\s*</tr>)";

            string input = cleanup ? Regex.Replace(html, "(\r|\n)", string.Empty) : html;

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            if (match.Success)
            {
                string title = match.Groups["title"].Value;
                string url = match.Groups["url"].Value;
                Gallery g = new Gallery(title, url);


                CaptureCollection cc = match.Groups["thumbnail"].Captures;

                Task[] tasks = new Task[cc.Count];

                for (int i = 0; i < cc.Count; i++)
                {
                    int index = i;
                    Task task = new Task(
                        () =>
                        {
                            Thumbnail t = this.GetThumbnail(cc[index].Value);
                            g.Add(t);
                        });
                    tasks[index] = task;
                    task.Start();
                }

                Task.WaitAll(tasks);

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
