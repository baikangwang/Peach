using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Peach.Entity;

namespace Peach.Core.Test
{
    [TestFixture]
    public class ThumbnailTest
    {
        [Test]
        public void LoadTest()
        {
            Thumbnail img = new Thumbnail("Test", "http://x2.fap.to/images/thumb/52/193/1936582325.jpg",string.Empty);
            img.ImageDownloading+= (sender, args) =>
                                       {
                                           Thumbnail imange = sender as Thumbnail;
                                           
                                           long lenght = args.TotalLength;
                                           long current = args.CurrentLength;
                                           Console.WriteLine("{0} -> {1}", imange.Title,
                                                             lenght == 0
                                                                 ? "N/A"
                                                                 : Math.Round((double)current*100 / (double)lenght, 2) + "%");
                                       };
            var t=Task.Factory.StartNew(img.Load);

            var top = t.ContinueWith(task =>
                                         {
                                             string fileName = "287720072.jpg";
                                             using (Image image = Image.FromStream(img.GetContent()))
                                             {
                                                 image.Save(fileName, ImageFormat.Jpeg);
                                             }

                                             Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                                         });

            top.Wait();
        }
    }
}
