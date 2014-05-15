using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Peach.Entity;

namespace Peach.Core.Test
{
    [TestFixture]
    public class PagerParserTest
    {
        [Test]
        public void GetPagerTest()
        {
            string input = File.ReadAllText("SearchParser\\searchview.txt");

            input = Regex.Replace(input, "\r|\n", string.Empty);
            
            using (PagerParser p=new PagerParser())
            {
                p.Init(input);
                Pager pr= p.GetPager(false);
                Console.WriteLine(pr);
            }
        }
    }
}
