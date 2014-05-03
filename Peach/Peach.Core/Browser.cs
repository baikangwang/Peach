// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Browser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The browser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Browser" /> class.
        /// </summary>
        protected Browser()
        {
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

            Logger.Current.InfoFormat("Requesting url->{0}", url);

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
        public MethodResult<HttpWebResponse> GetImage(string url)
        {
            return this.Get(url);
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
                try
                {
                    Logger.Current.InfoFormat(
                        "Getting response of url, {0} requesting {1} times.", request.RequestUri, 3 - max + 1);

                    var response = (HttpWebResponse)request.GetResponse();

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
                }
            }

            // must not be here
            return new MethodResult<HttpWebResponse>("unknown error hanppened.");
        }

        #endregion
    }
}