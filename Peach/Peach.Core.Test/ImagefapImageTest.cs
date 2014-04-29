using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;

namespace Peach.Core.Test
{
    [TestFixture]
    public class ImagefapImageTest
    {
        [Test]
        public void ViewImageTest()
        {
            string url = "http://www.imagefap.com/image.php?id=851926088"; //string.Empty;
            // string thumb = "http://x1.fap.to/images/thumb/52/851/851926088.jpg";

            // send request
            WebRequest request = WebRequest.Create(new Uri(url));

            WebResponse response = request.GetResponse();

            if(response.Headers[HttpRequestHeader.ContentType].Contains("Image"))

            using (Stream s=response.GetResponseStream())
            {
                using (StreamReader sr=new StreamReader(s))
                {
                    byte[] content = Encoding.Default.GetBytes(sr.ReadToEnd());
                    using (FileStream fs=File.Create("response.txt"))
                    {
                        fs.Write(content, 0, content.Length);
                        fs.Flush(true);
                    } 
                }
            }


            // recieve response

            // show image
        }

        [Test]
        public void SaveImageTest()
        {
            // string url = "http://www.imagefap.com/image.php?id=287720072"; //string.Empty;
             string url = "http://x1.fap.to/images/thumb/52/287/287720072.jpg";

            WebRequest request = WebRequest.Create(new Uri(url));

            WebResponse response = request.GetResponse();

            string contentType = response.Headers[HttpResponseHeader.ContentType];

            if (contentType.Contains("image"))
            {
                string type = contentType.Split(new char[] {'/'}, StringSplitOptions.None)[1];
                
                using (Stream s = response.GetResponseStream())
                {
                    using (Image image = Image.FromStream(s))
                    {
                        string fileName = request.RequestUri.Segments[request.RequestUri.Segments.Length - 1];
                        image.Save(fileName,GetImageFormat(type));
                    }
                }
            }

            response.Close();
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
