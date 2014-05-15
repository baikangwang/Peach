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
    public class GalleryParserTest
    {
        [Test]
        public void ListGalleriesTest()
        {
            string input = File.ReadAllText("GalleryParser\\galleryview.txt");

            using (GalleryViewParser gp=new GalleryViewParser())
            {
                gp.Init(input);
                
                IList<Gallery> gs = gp.ListGalleries();

                Pager pr = gp.GetPager();

                foreach (Gallery g in gs)
                {
                    Console.WriteLine(g);
                }

                Console.WriteLine(pr);
            }
        }
    }
}
