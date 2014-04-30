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
        public void ListGalleryTest()
        {
            string filename = "searchview.txt";

            string input = File.ReadAllText(filename);

            using (SearchParser p = new SearchParser(input))
            {
                IList<Gallery> gs = p.ListGalleries(true);

                foreach (Gallery g in gs)
                {
                    Console.WriteLine("[{0}] > {1}", gs.IndexOf(g), g);
                }
            }
        }
#if DEBUG
        [Test]
        public void GetGalleryTest()
        {
            string input = File.ReadAllText("searchsingle.txt");
            using (SearchParser p=new SearchParser(input))
            {
                Gallery g = p.GetGellery(input);
                Console.WriteLine(g.ToString());
            }
        }
#endif
    }
}
