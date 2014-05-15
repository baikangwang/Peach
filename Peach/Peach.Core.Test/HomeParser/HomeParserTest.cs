using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Core.Test
{
    using System.Collections;
    using System.IO;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    using Peach.Entity;

    [TestFixture]
    public class HomeParserTest
    {
        [Test]
        [Ignore]
        public void ParseTest()
        {
            string input = File.ReadAllText("HomeParser\\homeview.txt").Replace("\n", string.Empty).Replace("\r", string.Empty);

            string single =
                //"(?<gallery><tr\\s*?id='(\\w*?)'.*?>\\s*<td((?!colspan=2).)*?>.*?<a.*?>View gallery</a>.*?</td>\\s*<td.*?>.*?</td>\\s*<td.*?>.*?</td>\\s*</tr>\\s*<tr>\\s*<td.*?>\\s*<table>\\s*<tr.*?>\\s*<td.*?>\\s*</td>(\\s*<td.*?>\\s*<a.*?>\\s*<img.*?>\\s*</a>.*?</td>\\s*)+?</tr>\\s*</table>\\s*</tr>)";
                
                "(?<gallery><tr\\s*?id='(\\w*?)'.*?>.*?<a.*?>View gallery</a>.*?</tr>\\s*<tr>.*?<table>\\s*<tr.*?>*?(\\s*<td.*?>\\s*<a.*?>\\s*<img.*?>\\s*</a>.*?</td>\\s*)+?</tr>\\s*</table>\\s*</tr>)";
            
            string pattern = string.Format("<table.*?>.*?(?<galleries>{0}+).*?</table>", single);
                
            Regex r = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Match match = r.Match(input);

            do
            {
                for (int i = 0; i < match.Groups["gallery"].Captures.Count; i++)
                {
                    Console.WriteLine(match.Groups["gallery"].Captures[i]);
                    Console.WriteLine();
                }
                
                match = match.NextMatch();
            }
            while (match.Success && match.Index != (input.Length - 1));

        }

        [Test]
        public void ListGalleryTest()
        {
            string filename = "HomeParser\\homeview.txt";//"searchview.txt"; //;

            string input = File.ReadAllText(filename);//.Replace("\n", string.Empty).Replace("\r", string.Empty);

            ViewParser p=new HomeViewParser();
            p.Init(input);
            IList<Gallery> gs = p.ListGalleries(input);

            foreach (Gallery g in gs)
            {
                Console.WriteLine("[{0}] > {1}", gs.IndexOf(g), g);
            }

            p.Dispose();
        }
    }
}
