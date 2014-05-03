// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchViewParser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The search parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Peach.Entity;
    using Peach.Log;

    /// <summary>
    ///     The search parser.
    /// </summary>
    public class SearchViewParser : PagerViewParser
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public SearchViewParser(string input)
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
            string input = cleanup ? Regex.Replace(this.ViewInput, "(\r|\n)", string.Empty) : this.ViewInput;

            string single =
                "(?<gallery>\\s*<tr\\s*id=\"\\w+\".*?>.*?<a\\s*title=\"View\\s*(?<title>.*?)\".*?href=\"(?<url>.*?)\".*?>\\s*<i>\\s*<b>\\s*\\k<title>\\s*</b>\\s*</i>\\s*</a>.*?</tr>\\s*<tr.*?>\\s*<td.*?>\\s*<table>\\s*<tr>(\\s*<!--<div.*?>-->\\s*<td.*?>\\s*<a.*?title=\"View\\s*\\k<title>\".*?>\\s*<img.*?>\\s*</a>\\s*</td>\\s*<!--</div>-->\\s*)+</tr>\\s*</table>\\s*</td>\\s*<td.*?>.*?</td>\\s*<td.*?>\\s*</td>\\s*</tr>\\s*)";
            string pattern = string.Format(
                "<div\\s*class=\"gallerylist\">\\s*<table.*?>.*?{0}+\\s*</table>\\s*</div>", single);

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

            /* <div\s*class="gallerylist">\s*<table.*?>.*?(?<galleries>(?<gallery>\s*<tr\s*id="\w+".*?>.*?<a\s*title="View\s*(?<title>.*?)".*?href="(?<url>.*?)".*?>\s*<i>\s*<b>\s*\1\s*</b>\s*</i>\s*</a>.*?</tr>\s*<tr.*?>\s*<td.*?>\s*<table>\s*<tr>(?:\s*<!--<div.*?>-->\s*<td.*?>\s*<a.*?title="View\s*\1".*?>\s*<img.*?>\s*</a>\s*</td>\s*<!--</div>-->\s*)+</tr>\s*</table>\s*</td>\s*<td.*?>.*?</td>\s*<td.*?>\s*</td>\s*</tr>\s*)+)\s*</table>\s*</div> 
             (?<gallery>\s*<tr\s*id="\w+".*?>.*?<a\s*title="View\s*(?<title>.*?)".*?href="(?<url>.*?)".*?>\s*<i>\s*<b>\s*\1\s*</b>\s*</i>\s*</a>.*?</tr>\s*<tr.*?>\s*<td.*?>\s*<table>\s*<tr>(?:\s*<!--<div.*?>-->\s*<td.*?>\s*<a.*?title="View\s*\1".*?>\s*<img.*?>\s*</a>\s*</td>\s*<!--</div>-->\s*)+</tr>\s*</table>\s*</td>\s*<td.*?>.*?</td>\s*<td.*?>\s*</td>\s*</tr>\s*)
             */
            string pattern =
                "<tr\\s*id=\"\\w+\".*?>.*?<a\\s*title=\"View\\s*(?<title>.*?)\".*?href=\"(?<url>.*?)\".*?>.*?</a>.*?</tr>\\s*<tr.*?>\\s*<td.*?>\\s*<table>\\s*<tr>(\\s*<!--<div.*?>-->\\s*<td.*?>\\s*(?<thumbnail><a.*?title=\"View\\s*\\k<title>\".*?>\\s*<img.*?>\\s*</a>)\\s*</td>\\s*<!--</div>-->\\s*)+</tr>";

            string input = cleanup ? Regex.Replace(this.Input, "(\r|\n)", string.Empty) : html;

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

        /// <summary>
        /// The init.
        /// </summary>
        protected override void Init()
        {
            string single =
                "(?<gallery>\\s*<tr\\s*id=\"\\w+\".*?>.*?<a\\s*title=\"View\\s*(?<title>.*?)\".*?href=\"(?<url>.*?)\".*?>\\s*<i>\\s*<b>\\s*\\k<title>\\s*</b>\\s*</i>\\s*</a>.*?</tr>\\s*<tr.*?>\\s*<td.*?>\\s*<table>\\s*<tr>(\\s*<!--<div.*?>-->\\s*<td.*?>\\s*<a.*?title=\"View\\s*\\k<title>\".*?>\\s*<img.*?>\\s*</a>\\s*</td>\\s*<!--</div>-->\\s*)+</tr>\\s*</table>\\s*</td>\\s*<td.*?>.*?</td>\\s*<td.*?>\\s*</td>\\s*</tr>\\s*)";
            string pattern =
                string.Format(
                    "(?<galleries><div\\s*class=\"gallerylist\">\\s*<table.*?>.*?{0}+\\s*</table>\\s*</div>\\s*<BR>\\s*<center.*?>\\s*(?<pager>.*?)\\s*<BR><BR>\\s*</center>)", 
                    single);

            var r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(this.Input);

            if (match.Success)
            {
                this.PagerInput = match.Groups["pager"].Value;
                this.ViewInput = match.Groups["galleries"].Value;
            }
            else
            {
                Logger.Current.Warn(string.Format("Invalid input, {0}", this.Input));
            }
        }

        #endregion
    }
}