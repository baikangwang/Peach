// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GalleryViewParser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The gallery view parser.
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
    /// The gallery view parser.
    /// </summary>
    public class GalleryViewParser : PagerViewParser
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public GalleryViewParser(string input)
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
            IList<Gallery> gs = new List<Gallery>();
            Gallery g = this.GetGallery(this.ViewInput);
            if (g != null)
            {
                gs.Add(g);
            }
            else
            {
                Logger.Current.WarnFormat("Cannot extract Gallery info");
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
                Logger.Current.DebugFormat("Empty input of gallery");
                return null;
            }

            // <table.*?>\s*<tr>\s*<td.*?>\s*<div\s*id="menubar".*?>.*?</div>.*?<table.*?id="gal_desc"\s*>.*?</table>.*?<div\s*id="gallery">\s*(?<ptemp><font.*?>\s*<span.*?>\s*(?<pager>\|\s*<b>\s*\w+\s*</b>.*?::\s*next\s*::\s*</a>)\s*</span>\s*</font>)\s*<BR>\s*<form.*?>\s*<table.*?>(?<row>\s*<tr>(?<thumbnal>\s*<td.*?>.*?</td>\s*){1,4}</tr>\s*)+</table>\s*</form>\s*<BR>\s*\k<ptemp>\s*<br\s*/><br\s*/>\s*</div>.*?<center>\s*<a.*?>\s*Report\s*this\s*gallery\s*</a>\s*<br>\s*<br>\s*<br>\s*<br>\s*</center>\s*<!--.*?-->\s*</td>\s*</tr>\s*</table>
            string pattern =
                "<table.*?>\\s*<tr>\\s*<td.*?>\\s*<div\\s*id=\"menubar\".*?>.*?</div>.*?<table.*?id=\"gal_desc\"\\s*>.*?</table>.*?<div\\s*id=\"gallery\">\\s*(?<ptemp><font.*?>\\s*<span.*?>\\s*(?<pager>\\|\\s*<b>\\s*\\w+\\s*</b>.*?::\\s*next\\s*::\\s*</a>)\\s*</span>\\s*</font>)\\s*<BR>\\s*<form.*?>\\s*<table.*?>(?<row>\\s*<tr>(?<thumbnail>\\s*<td.*?>.*?</td>\\s*){1,4}</tr>\\s*)+</table>\\s*</form>\\s*<BR>\\s*\\k<ptemp>\\s*<br\\s*/><br\\s*/>\\s*</div>.*?<center>\\s*<a.*?>\\s*Report\\s*this\\s*gallery\\s*</a>\\s*<br>\\s*<br>\\s*<br>\\s*<br>\\s*</center>\\s*<!--.*?-->\\s*</td>\\s*</tr>\\s*</table>";

            string input = cleanup ? Regex.Replace(this.Input, "(\r|\n)", string.Empty) : html;

            var r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            //todo: add try-catch block
            Match match = Util.WithTimeout(() => r.Match(input), 5 * 1000);

            if (match.Success)
            {
                string title = string.Empty;
                string url = string.Empty;
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
                                if (t != null)
                                {
                                    g.Add(t);
                                }
                                else
                                {
                                    // nothing to do
                                }
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
        /// The get thumbnail.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="cleanup">
        /// The cleanup.
        /// </param>
        /// <returns>
        /// The <see cref="Thumbnail"/>.
        /// </returns>
        protected override Thumbnail GetThumbnail(string html, bool cleanup = false)
        {
            if (string.IsNullOrEmpty(html))
            {
                Logger.Current.DebugFormat("Empty input of thumbnail");
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

            var r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(input);

            if (match.Success)
            {
                string fullurl = match.Groups["fullurl"].Value;
                string title = match.Groups["title"].Value;
                string thumbnailurl = match.Groups["thumbnailurl"].Value;

                return new Thumbnail(title, thumbnailurl, fullurl);
            }
            else
            {
                Logger.Current.Warn(string.Format("Invalid thumbnail tag, {0}", html));
                return null;
            }
        }

        /// <summary>
        /// The init.
        /// </summary>
        protected override void Init()
        {
            string pattern =
                "(?<gallery><table.*?>\\s*<tr>\\s*<td.*?>\\s*<div\\s*id=\"menubar\".*?>.*?</div>.*?<table.*?id=\"gal_desc\"\\s*>.*?</table>.*?<div\\s*id=\"gallery\">\\s*(?<ptemp><font.*?>\\s*<span.*?>\\s*(?<pager>\\|\\s*<b>\\s*\\w+\\s*</b>.*?::\\s*next\\s*::\\s*</a>)\\s*</span>\\s*</font>)\\s*<BR>\\s*<form.*?>\\s*<table.*?>(?<row>\\s*<tr>(?<thumbnail>\\s*<td.*?>.*?</td>\\s*){1,4}</tr>\\s*)+</table>\\s*</form>\\s*<BR>\\s*\\k<ptemp>\\s*<br\\s*/><br\\s*/>\\s*</div>.*?<center>\\s*<a.*?>\\s*Report\\s*this\\s*gallery\\s*</a>\\s*<br>\\s*<br>\\s*<br>\\s*<br>\\s*</center>\\s*<!--.*?-->\\s*</td>\\s*</tr>\\s*</table>)";

            var r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);

            Match match = r.Match(this.Input);

            if (match.Success)
            {
                this.PagerInput = match.Groups["pager"].Value;
                this.ViewInput = match.Groups["gallery"].Value;
            }
            else
            {
                Logger.Current.Warn(string.Format("Invalid input, {0}", this.Input));
            }
        }

        #endregion
    }
}