namespace Peach.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    using Peach.Entity;

    /// <summary>
    /// The browser.
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// The _current.
        /// </summary>
        private static Browser _current = new Browser();

        /// <summary>
        /// Gets the current.
        /// </summary>
        public static Browser Current
        {
            get
            {
                return _current;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser"/> class.
        /// </summary>
        protected Browser()
        {
        }
        
        /// <summary>
        /// The get image.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="MethodResult"/>.
        /// </returns>
        public MethodResult<HttpWebResponse> GetImage(string url)
        {
            Uri uri = new Uri(url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ProtocolVersion = new Version(1, 1);
            request.Method = "GET";
            request.Host = uri.Host;
            request.KeepAlive = true;
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Maxthon/3.0 Chrome/22.0.1229.79 Safari/537.1";
            request.Accept = "*/*";
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            request.Headers[HttpRequestHeader.AcceptLanguage] = "en-US";
            request.Headers[HttpRequestHeader.AcceptCharset] = "GBK,utf-8;q=0.7,*;q=0.3";

            return this.TryGetResponse(request);
        }

        /// <summary>
        /// The get page.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="MethodResult"/>.
        /// </returns>
        public MethodResult<HttpWebResponse> GetPage(string url)
        {
            Uri uri = new Uri(url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ProtocolVersion = new Version(1, 1);
            request.Method = "GET";
            request.Host = uri.Host;
            request.KeepAlive = true;
            request.UserAgent=
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Maxthon/3.0 Chrome/22.0.1229.79 Safari/537.1";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            request.Headers[HttpRequestHeader.AcceptLanguage] = "en-US";
            request.Headers[HttpRequestHeader.AcceptCharset] = "GBK,utf-8;q=0.7,*;q=0.3";

            return this.TryGetResponse(request);
        }

        private MethodResult<HttpWebResponse> TryGetResponse(HttpWebRequest request)
        {
            int max = 3;

            //try 3 times
            while (max > 0)
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new MethodResult<HttpWebResponse>(response);
                }
                else
                {
                    max--;
                }

                if (max == 0)
                {
                    return new MethodResult<HttpWebResponse>(response.StatusDescription);
                }
                else
                {
                    // nothing to do
                }
            }

            //must not be here
            return new MethodResult<HttpWebResponse>("unknown error hanppened.");
        }
    }
}
