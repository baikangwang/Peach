using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Peach.Entity;

namespace Peach.Core.Test
{
    [TestFixture]
    public class SearchParserTest
    {
        [Test]
        public void ListHomeGalleryTest()
        {
            string filename = "searchview.txt";

            string input = File.ReadAllText(filename);//.Replace("\n", string.Empty).Replace("\r", string.Empty);

            using (Parser p = new SearchParser(input))
            {
                IList<Gallery> gs = p.ListGalleries(true);

                foreach (Gallery g in gs)
                {
                    Console.WriteLine("[{0}] > {1}", gs.IndexOf(g), g);
                }
            }
        }
    }
}
