// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Browser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The browser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;

namespace Peach.Core
{
    using System;
    using System.Net;

    using Peach.Entity;
    using Peach.Log;

    /// <summary>
    ///     The browser.
    /// </summary>
    public class Browser
    {
        #region Static Fields

        /// <summary>
        ///     The _current.
        /// </summary>
        private static readonly Browser _current = new Browser();

        public event BrowserEventHandler Requesting;

        private void OnRequesting(BrowserEventArgs e)
        {
            BrowserEventHandler handler = Requesting;
            if (handler != null) handler(null, e);
        }

        public event BrowserEventHandler Responsed;

        private void OnResponsed(BrowserEventArgs e)
        {
            BrowserEventHandler handler = Responsed;
            if (handler != null) handler(null, e);
        }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Browser" /> class.
        /// </summary>
        protected Browser()
        {
            this.Requesting += Browser_Requesting;
            this.Responsed += Browser_Responsed;
        }

        void Browser_Responsed(object sender, BrowserEventArgs e)
        {
            Logger.Current.Debug(e.Message);
        }

        void Browser_Requesting(object sender, BrowserEventArgs e)
        {
            Logger.Current.Debug(e.Message);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current.
        /// </summary>
        public static Browser Current
        {
            get
            {
                return _current;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="MethodResult"/>.
        /// </returns>
        public MethodResult<HttpWebResponse> Get(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Logger.Current.ErrorFormat("Empty requesting url->{0}", url);
                return new MethodResult<HttpWebResponse>("Empty requesting url");
            }

            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch (Exception ex)
            {
                Logger.Current.ErrorFormat("Invalid requesting url->{0}, Error:{1}", url, ex);
                return
                    new MethodResult<HttpWebResponse>(
                        string.Format("Invalid requesting url->{0}, Error:{1}", url, ex.Message));
            }

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 5 * 1000;

            return this.TryGetResponse(request);
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
        public MethodResult<Stream> GetImage(string url)
        {
            MethodResult<HttpWebResponse> r = this.Get(url);
            if (r)
            {
                try
                {
                    Stream s = Util.WithTimeout<Stream>(() => r.Result.GetResponseStream(), 10 * 1000);
                    return new MethodResult<Stream>(s);
                }
                catch (Exception ex)
                {
                    Logger.Current.Warn(string.Format("Fail to get image {0}. Error:{1}", url, ex));
                    return new MethodResult<Stream>(string.Format("Fail to get image {0}. Error:{1}", url, ex.Message));
                }
                
            }
            else
            {
                return new MethodResult<Stream>(r.Message);
            }
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
            return this.Get(url);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The try get response.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="MethodResult"/>.
        /// </returns>
        private MethodResult<HttpWebResponse> TryGetResponse(HttpWebRequest request)
        {
            int max = 3;

            // try 3 times
            while (max > 0)
            {
                HttpWebResponse response;
                
                try
                {
                    OnRequesting(
                        new BrowserEventArgs(string.Format("The {0} time to getting response of {1}", ToLabel(3 - max + 1),
                                                           request.RequestUri)));

                    response = Util.WithTimeout<HttpWebResponse>(() => (HttpWebResponse)request.GetResponse(), 10 * 1000);

                    OnResponsed(
                        new BrowserEventArgs(string.Format("Got response of {1} at {0} time.", ToLabel(3 - max + 1),
                                                           request.RequestUri)));

                }
                catch (Exception ex)
                {
                    Logger.Current.WarnFormat(
                        "Fail to getting response of url, {0} {1} times. error: {2}", 
                        request.RequestUri, 
                        3 - max + 1, 
                        ex.Message);

                    max--;

                    if (max == 0)
                    {
                        return new MethodResult<HttpWebResponse>(ex.Message);
                    }

                    continue;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new MethodResult<HttpWebResponse>(response);
                }
                else
                {
                    Logger.Current.InfoFormat(
                        "Fail to getting response of url, {0} {1} times. Status Code: {2}",
                        request.RequestUri,
                        3 - max + 1,
                        response.StatusCode);
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

            // must not be here
            return new MethodResult<HttpWebResponse>("unknown error hanppened.");
        }

        private string ToLabel(int sequence)
        {
            switch (sequence)
            {
                case 1:
                    return "First";
                case 2:
                    return "Second";
                case 3:
                    return "Third";
                default:
                    return "Last";
            }
        }

        #endregion
    }
}