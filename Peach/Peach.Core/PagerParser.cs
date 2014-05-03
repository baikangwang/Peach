using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Peach.Entity;

namespace Peach.Core
{
    public class PagerParser:BaseParser
    {
        public PagerParser(string input) : base(input)
        {
            
        }

        /// <summary>
        /// The get pager.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="cleanup">
        /// The cleanup.
        /// </param>
        /// <returns>
        /// The <see cref="Pager"/>.
        /// </returns>
        public Pager GetPager(string html, bool cleanup = false)
        {
            this.Input = html;
            return this.GetPager(cleanup);
        }

        public Pager GetPager(bool cleanup = false)
        {
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif

            if (string.IsNullOrEmpty(this.Input))
            {
                return null;
            }

            /* <center.*?>\s*\|\s*<b>\s*(?<current>\d+)\s*</b>(\s*\|\s*(?<page><a.*?href=".*?">\s*\d+\s*</a>))*\s*\|.*?(?<next><a.*?href=".*?">\s*::\s*next\s*::\s*</a>)\s*<BR><BR>\s*</center>
             */

            string pattern =
                "\\|\\s*<b>\\s*(?<current>\\d+)\\s*</b>(\\s*\\|\\s*(?<page><a.*?href=\".*?\">\\s*\\d+\\s*</a>))*\\s*\\|.*?(?<next><a.*?href=\".*?\">\\s*::\\s*next\\s*::\\s*</a>)";

            string input = cleanup ? this.Input.Replace("\\n", string.Empty).Replace("\\r", string.Empty) : this.Input;

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);


            Match match = r.Match(input);

#if DEBUG
            timer.Stop();
            Console.WriteLine(string.Format("Pager Match costed: {0}", timer.Elapsed));
            timer.Start();
#endif

            if (match.Success)
            {
                IList<Page> pages = new List<Page>();
                Page next=null;
                Page current = null;


                CaptureCollection cc = match.Groups["page"].Captures;

                Task[] tasks = new Task[cc.Count+2];

                for (int i = 0; i < cc.Count; i++)
                {
                    int index = i;
                    Task task = new Task(
                        () =>
                        {
                            Page p = this.GetPage(cc[index].Value);
                            pages.Add(p);
                        });
                    tasks[index] = task;
                    task.Start();
                }

                Task n=new Task(() =>
                                    {
                                        next = GetNext(match.Groups["next"].Value);
                                    });
                tasks[tasks.Length - 2] = n;

                Task c=new Task(() =>
                                    {
                                        current = GetCurrent(match.Groups["current"].Value);
                                    });
                tasks[tasks.Length - 1] = c;

                Task.WaitAll(tasks);

                Pager pr = new Pager(pages, next, current);

#if DEBUG
                timer.Stop();
                Console.WriteLine(string.Format("Pager Completed: {0}", timer.Elapsed));
#endif
                return pr;
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid pager tag, {0}", this.Input));
                return null;
            }
        }

        /// <summary>
        /// The get page.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="cleanup">
        /// The cleanup.
        /// </param>
        /// <returns>
        /// The <see cref="Page"/>.
        /// </returns>
        protected Page GetPage(string html, bool cleanup = false)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            string pattern ="<a.*?href=\"(?<url>.*?)\">\\s*(?<number>\\d+)\\s*</a>";

            string input = cleanup ? Regex.Replace(this.Input, "(\r|\n)", string.Empty) : html;

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            if (match.Success)
            {
                int number = int.Parse(match.Groups["number"].Value);
                string url = match.Groups["url"].Value;

                return new Page(number, url);
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid page tag, {0}", html));
                return null;
            }
        }

        protected Page GetCurrent(string html, bool cleanup = false)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            string pattern ="(?<number>\\d+)";

            string input = cleanup ? Regex.Replace(this.Input, "(\r|\n)", string.Empty) : html;

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            if (match.Success)
            {
                int number = int.Parse(match.Groups["number"].Value);
                string url = string.Empty;

                return new Page(number, url);
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid curent page tag, {0}", html));
                return null;
            }
        }

        protected Page GetNext(string html, bool cleanup = false)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            string pattern = "<a.*?href=\"(?<url>.*?page=(?<number>\\d+).*?)\">\\s*::\\s*next\\s*::\\s*</a>";

            string input = cleanup ? Regex.Replace(this.Input, "(\r|\n)", string.Empty) : html;

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            if (match.Success)
            {
                int number = int.Parse(match.Groups["number"].Value) + 1;
                string url = match.Groups["url"].Value;

                return new Page(number, url);
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid next page tag, {0}", html));
                return null;
            }
        }
    }
}
