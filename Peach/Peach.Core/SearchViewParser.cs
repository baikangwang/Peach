namespace Peach.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Peach.Entity;

    /// <summary>
    /// The search parser.
    /// </summary>
    public class SearchViewParser : PagerViewParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public SearchViewParser(string input) : base(input)
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
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif
            string input = cleanup ? Regex.Replace(this.ViewInput, "(\r|\n)", string.Empty) : this.ViewInput;

            string single =
                "(?<gallery>\\s*<tr\\s*id=\"\\w+\".*?>.*?<a\\s*title=\"View\\s*(?<title>.*?)\".*?href=\"(?<url>.*?)\".*?>\\s*<i>\\s*<b>\\s*\\k<title>\\s*</b>\\s*</i>\\s*</a>.*?</tr>\\s*<tr.*?>\\s*<td.*?>\\s*<table>\\s*<tr>(\\s*<!--<div.*?>-->\\s*<td.*?>\\s*<a.*?title=\"View\\s*\\k<title>\".*?>\\s*<img.*?>\\s*</a>\\s*</td>\\s*<!--</div>-->\\s*)+</tr>\\s*</table>\\s*</td>\\s*<td.*?>.*?</td>\\s*<td.*?>\\s*</td>\\s*</tr>\\s*)";
            string pattern = string.Format(
                "<div\\s*class=\"gallerylist\">\\s*<table.*?>.*?{0}+\\s*</table>\\s*</div>", single);

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

#if DEBUG
            timer.Stop();
            Console.WriteLine(string.Format("Galleries Match costed: {0}", timer.Elapsed));
            timer.Start();
#endif
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
#if DEBUG
            timer.Stop();
            Console.WriteLine(string.Format("Galleries Completed: {0}", timer.Elapsed));
#endif
            return gs;
        }

#if DEBUG

        /// <summary>
        /// The get gellery.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="Gallery"/>.
        /// </returns>
        public Gallery GetGellery(string input)
        {
            return this.GetGallery(input, true);
        }
#endif

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
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif
            
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

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);


            Match match = r.Match(input);

#if DEBUG
            timer.Stop();
            Console.WriteLine(string.Format("Gallery Match costed: {0}", timer.Elapsed));
            timer.Start();
#endif

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
#if DEBUG
                timer.Stop();
                Console.WriteLine(string.Format("Gallery Completed: {0}", timer.Elapsed));
#endif
                return g;
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid gallery tag, {0}", html));
                return null;
            }
        }

        protected override void Init()
        {
            string single =
                "(?<gallery>\\s*<tr\\s*id=\"\\w+\".*?>.*?<a\\s*title=\"View\\s*(?<title>.*?)\".*?href=\"(?<url>.*?)\".*?>\\s*<i>\\s*<b>\\s*\\k<title>\\s*</b>\\s*</i>\\s*</a>.*?</tr>\\s*<tr.*?>\\s*<td.*?>\\s*<table>\\s*<tr>(\\s*<!--<div.*?>-->\\s*<td.*?>\\s*<a.*?title=\"View\\s*\\k<title>\".*?>\\s*<img.*?>\\s*</a>\\s*</td>\\s*<!--</div>-->\\s*)+</tr>\\s*</table>\\s*</td>\\s*<td.*?>.*?</td>\\s*<td.*?>\\s*</td>\\s*</tr>\\s*)";
            string pattern = string.Format(
                "(?<galleries><div\\s*class=\"gallerylist\">\\s*<table.*?>.*?{0}+\\s*</table>\\s*</div>\\s*<BR>\\s*<center.*?>\\s*(?<pager>.*?)\\s*<BR><BR>\\s*</center>)", single);

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(this.Input);

            if (match.Success)
            {
                this.PagerInput = match.Groups["pager"].Value;
                this.ViewInput = match.Groups["galleries"].Value;
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid input, {0}", this.Input));
            }
        }
    }
}
