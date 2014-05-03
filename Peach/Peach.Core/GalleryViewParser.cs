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
    public class GalleryViewParser:PagerViewParser
    {
        
        public GalleryViewParser(string input) : base(input)
        {
            
        }

        public override IList<Gallery> ListGalleries(bool cleanup = false)
        {
            IList<Gallery> gs=new List<Gallery>();
            Gallery g = this.GetGallery(this.ViewInput);
            if (g != null)
            {
                gs.Add(g);
            }

            return gs;
        }

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

            // <table.*?>\s*<tr>\s*<td.*?>\s*<div\s*id="menubar".*?>.*?</div>.*?<table.*?id="gal_desc"\s*>.*?</table>.*?<div\s*id="gallery">\s*(?<ptemp><font.*?>\s*<span.*?>\s*(?<pager>\|\s*<b>\s*\w+\s*</b>.*?::\s*next\s*::\s*</a>)\s*</span>\s*</font>)\s*<BR>\s*<form.*?>\s*<table.*?>(?<row>\s*<tr>(?<thumbnal>\s*<td.*?>.*?</td>\s*){1,4}</tr>\s*)+</table>\s*</form>\s*<BR>\s*\k<ptemp>\s*<br\s*/><br\s*/>\s*</div>.*?<center>\s*<a.*?>\s*Report\s*this\s*gallery\s*</a>\s*<br>\s*<br>\s*<br>\s*<br>\s*</center>\s*<!--.*?-->\s*</td>\s*</tr>\s*</table>
            

            string pattern =
                "<table.*?>\\s*<tr>\\s*<td.*?>\\s*<div\\s*id=\"menubar\".*?>.*?</div>.*?<table.*?id=\"gal_desc\"\\s*>.*?</table>.*?<div\\s*id=\"gallery\">\\s*(?<ptemp><font.*?>\\s*<span.*?>\\s*(?<pager>\\|\\s*<b>\\s*\\w+\\s*</b>.*?::\\s*next\\s*::\\s*</a>)\\s*</span>\\s*</font>)\\s*<BR>\\s*<form.*?>\\s*<table.*?>(?<row>\\s*<tr>(?<thumbnail>\\s*<td.*?>.*?</td>\\s*){1,4}</tr>\\s*)+</table>\\s*</form>\\s*<BR>\\s*\\k<ptemp>\\s*<br\\s*/><br\\s*/>\\s*</div>.*?<center>\\s*<a.*?>\\s*Report\\s*this\\s*gallery\\s*</a>\\s*<br>\\s*<br>\\s*<br>\\s*<br>\\s*</center>\\s*<!--.*?-->\\s*</td>\\s*</tr>\\s*</table>";

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
                string title = string.Empty;//match.Groups["title"].Value;
                string url = string.Empty; //match.Groups["url"].Value;
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

        protected override Thumbnail GetThumbnail(string html, bool cleanup = false)
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

            /*
             <td\s*?id="(?<name>\w+)".*?>\s*<table>\s*<tr>\s*<td.*?>\s*<a\s*name="\k<name>"\s*href="(?<fullurl>.*?)">\s*<img.*?alt="(?<title>.*?)".*?src="(?<thumbnailurl>.*?)">\s*</a>\s*</td>\s*</tr>\s*<tr.*?>\s*<td.*?>.*?<span\s*id="img_\k<name>_desc">\s*</span>.*?<i>(?<filename>.*?)</i>.*?<b>\d+</b>&nbsp;x&nbsp;<b>\d+</b>.*?<span.*?>.*?\d+\s*Views.*?</span>.*?</td>\s*</tr>\s*</table>\s*</td>
             */

            string pattern =
                "<td\\s*?id=\"(?<name>\\w+)\".*?>\\s*<table>\\s*<tr>\\s*<td.*?>\\s*<a\\s*name=\"\\k<name>\"\\s*href=\"(?<fullurl>.*?)\">\\s*<img.*?alt=\"(?<title>.*?)\".*?src=\"(?<thumbnailurl>.*?)\">\\s*</a>\\s*</td>\\s*</tr>\\s*<tr.*?>\\s*<td.*?>.*?<span\\s*id=\"img_\\k<name>_desc\">\\s*</span>.*?<i>.*?</i>.*?<b>\\d+</b>&nbsp;x&nbsp;<b>\\d+</b>.*?<span.*?>.*?\\d+\\s*Views.*?</span>.*?</td>\\s*</tr>\\s*</table>\\s*</td>";

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

        protected override void Init()
        {
            string pattern =
                "(?<gallery><table.*?>\\s*<tr>\\s*<td.*?>\\s*<div\\s*id=\"menubar\".*?>.*?</div>.*?<table.*?id=\"gal_desc\"\\s*>.*?</table>.*?<div\\s*id=\"gallery\">\\s*(?<ptemp><font.*?>\\s*<span.*?>\\s*(?<pager>\\|\\s*<b>\\s*\\w+\\s*</b>.*?::\\s*next\\s*::\\s*</a>)\\s*</span>\\s*</font>)\\s*<BR>\\s*<form.*?>\\s*<table.*?>(?<row>\\s*<tr>(?<thumbnail>\\s*<td.*?>.*?</td>\\s*){1,4}</tr>\\s*)+</table>\\s*</form>\\s*<BR>\\s*\\k<ptemp>\\s*<br\\s*/><br\\s*/>\\s*</div>.*?<center>\\s*<a.*?>\\s*Report\\s*this\\s*gallery\\s*</a>\\s*<br>\\s*<br>\\s*<br>\\s*<br>\\s*</center>\\s*<!--.*?-->\\s*</td>\\s*</tr>\\s*</table>)";

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(this.Input);

            if (match.Success)
            {
                this.PagerInput = match.Groups["pager"].Value;
                this.ViewInput = match.Groups["gallery"].Value;
            }
            else
            {
                Peach.Log.Logger.Current.Warn(string.Format("Invalid input, {0}", this.Input));
            }
        }
    }
}
