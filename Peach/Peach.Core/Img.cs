using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.Core
{
    using System.IO;

    /// <summary>
    /// The img.
    /// </summary>
    public abstract class Img
    {
        /// <summary>
        /// The title.
        /// </summary>
        private string title;

        /// <summary>
        /// The url.
        /// </summary>
        private string url;

        private Stream stream;

        /// <summary>
        /// Gets the title.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return this.title;
            }
        }

        /// <summary>
        /// Gets the url.
        /// </summary>
        public virtual string Url
        {
            get
            {
                return this.url;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Img"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        protected Img(string title,string url)
        {
            this.title = title;
            this.url = url;
        }

        /// <summary>
        /// The get content.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public virtual Stream GetContent()
        {
            return this.stream;
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public virtual void Read(Stream response)
        {
            this.stream = response;
        }

        /// <summary>
        /// The resolve url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected abstract string ResolveUrl(string url);
    }
}
