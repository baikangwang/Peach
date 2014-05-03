// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Img.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The img.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Entity
{
    using System;
    using System.IO;

    using Peach.Log;

    /// <summary>
    ///     The img.
    /// </summary>
    public abstract class Img:IDisposable
    {
        #region Fields

        /// <summary>
        ///     The title.
        /// </summary>
        private string _title;

        /// <summary>
        ///     The url.
        /// </summary>
        private string _url;

        /// <summary>
        /// The _file name.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The _stream.
        /// </summary>
        private Stream _stream;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Img"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        protected Img(string title, string url)
        {
            this._title = title;
            this._url = url;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public virtual string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(this._fileName))
                {
                    if (string.IsNullOrEmpty(this._url))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        try
                        {
                            var uri = new Uri(this._url);
                            this._fileName = uri.Segments[uri.Segments.Length - 1];
                        }
                        catch (Exception ex)
                        {
                            Logger.Current.Error(string.Format("Resolving invalid image url: {0}", this._url), ex);
                            this._fileName = string.Empty;
                        }
                    }
                }

                return this._fileName;
            }
        }

        /// <summary>
        /// Gets the owner gallery.
        /// </summary>
        public Gallery OwnerGallery { get; internal set; }

        /// <summary>
        ///     Gets the title.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return this._title;
            }
        }

        /// <summary>
        ///     Gets the url.
        /// </summary>
        public virtual string Url
        {
            get
            {
                return this._url;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Img)obj);
        }

        /// <summary>
        ///     The get content.
        /// </summary>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        public virtual Stream GetContent()
        {
            return this._stream;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this._title != null ? this._title.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this._url != null ? this._url.GetHashCode() : 0);

                /*hashCode = (hashCode*397) ^ (_stream != null ? _stream.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_fileName != null ? _fileName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (OwnerGallery != null ? OwnerGallery.GetHashCode() : 0);*/
                return hashCode;
            }
        }

        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                this._stream.Dispose();
                this._fileName = null;
                this._url = null;
                this._title = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public virtual void Load(Stream response)
        {
            this._stream = response;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Title: {0},Url: {1}", this._title, this._url);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Equals(Img other)
        {
            return string.Equals(this._title, other._title) && string.Equals(this._url, other._url);
        }

        #endregion

        /*
        /// <summary>
        /// The resolve url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        //protected abstract string ResolveUrl(string url);
         **/
    }
}