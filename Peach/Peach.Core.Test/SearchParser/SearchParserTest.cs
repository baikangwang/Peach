﻿using System;
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
            string filename = "SearchParser\\searchview.txt";

            string input = File.ReadAllText(filename);

            using (SearchViewParser p = new SearchViewParser())
            {
                p.Init(input);
                IList<Gallery> gs = p.ListGalleries();

                Pager pr = p.GetPager();

                foreach (Gallery g in gs)
                {
                    Console.WriteLine("[{0}] > {1}", gs.IndexOf(g), g);
                }

                Console.WriteLine(pr);
            }
        }
    }
}
