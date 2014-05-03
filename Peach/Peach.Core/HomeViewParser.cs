// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeViewParser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The home parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Core
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Peach.Entity;
    using Peach.Log;

    /// <summary>
    ///     The home parser.
    /// </summary>
    public class HomeViewParser : ViewParser
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public HomeViewParser(string input)
            : base(input)
        {
        }

        #endregion

        #region Public Methods and Operators

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

            var r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            IList<Gallery> gs = new List<Gallery>();

            if (match.Success)
            {
                CaptureCollection cc = match.Groups["gallery"].Captures;

                var tasks = new Task[cc.Count];

                for (int i = 0; i < cc.Count; i++)
                {
                    int index = i;
                    var task = new Task(
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
                Logger.Current.Warn(string.Format("Invalid thumbnail tag, {0}", this.Input));
            }

            return gs;
        }

        #endregion

        #region Methods

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

            var r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            if (match.Success)
            {
                string title = match.Groups["title"].Value;
                string url = match.Groups["url"].Value;
                var g = new Gallery(title, url);

                CaptureCollection cc = match.Groups["thumbnail"].Captures;

                var tasks = new Task[cc.Count];

                for (int i = 0; i < cc.Count; i++)
                {
                    int index = i;
                    var task = new Task(
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
                Logger.Current.Warn(string.Format("Invalid gallery tag, {0}", html));
                return null;
            }
        }

        #endregion

        /*
        public IList<Thumbnail> ListThumbnails(string html)
        {
            return new List<Thumbnail>();
        }
         * */
    }
}