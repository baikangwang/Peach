using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Core.Test
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;

    using NUnit.Framework;

    using Peach.Entity;

    [TestFixture]
    public class BrowserTest
    {
        [Test]
        public void GetImageTest()
        {
            // string url = "http://www.imagefap.com/image.php?id=287720072"; //string.Empty;
            string url = "http://x1.fap.to/images/thumb/52/287/287720072.jpg";

            Stream response = Browser.Current.GetImage(url).Result;

            string contentType = "image/jpeg";

            if (contentType.Contains("image"))
            {
                string type = contentType.Split(new char[] { '/' }, StringSplitOptions.None)[1];

                using (Stream s = response)
                {
                    using (Image image = Image.FromStream(s))
                    {
                        string fileName = "287720072.jpg";
                        image.Save(fileName, GetImageFormat(type));
                    }
                }
            }

            response.Close();
        }
        
        [Test]
        public void GetPageTest()
        {
            string url = "http://www.imagefap.com"; //"http://www.imagefap.com/image.php?id=851926088"; //string.Empty;
            // string thumb = "http://x1.fap.to/images/thumb/52/851/851926088.jpg";

            // send request
            HttpWebResponse response = Browser.Current.GetPage(url).Result;

                using (Stream s = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        byte[] content = Encoding.Default.GetBytes(sr.ReadToEnd());
                        using (FileStream fs = File.Create("response_test.txt"))
                        {
                            fs.Write(content, 0, content.Length);
                            fs.Flush(true);
                        }
                    }
                }

            Process.Start("response_test.txt");

            // recieve response

            // show image
        }

        private ImageFormat GetImageFormat(string type)
        {
            switch (type)
            {
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        private string GetFileExtension(string type)
        {
            switch (type)
            {
                case "jpeg":
                    return ".jpg";
                case "gif":
                    return ".gif";
                default:
                    return ".jpg";
            }
        }
    }
}
