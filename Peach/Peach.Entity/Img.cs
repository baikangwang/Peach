namespace Peach.Entity
{
    using System;
    using System.IO;
    using Peach.Log;
    
    /// <summary>
    /// The img.
    /// </summary>
    public abstract class Img
    {
        protected bool Equals(Img other)
        {
            return string.Equals(this._title,other._title) && string.Equals(this._url, other._url);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_title != null ? _title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_url != null ? _url.GetHashCode() : 0);
                /*hashCode = (hashCode*397) ^ (_stream != null ? _stream.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_fileName != null ? _fileName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (OwnerGallery != null ? OwnerGallery.GetHashCode() : 0);*/
                return hashCode;
            }
        }

        /// <summary>
        /// The title.
        /// </summary>
        private readonly string _title;

        /// <summary>
        /// The url.
        /// </summary>
        private readonly string _url;

        private Stream _stream;

        private string _fileName;

        public Gallery OwnerGallery { get; internal set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return this._title;
            }
        }

        /// <summary>
        /// Gets the url.
        /// </summary>
        public virtual string Url
        {
            get
            {
                return this._url;
            }
        }

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
                            Uri uri = new Uri(this._url);
                            this._fileName = uri.Segments[uri.Segments.Length - 1];
                        }
                        catch (Exception ex)
                        {
                            Logger.Current.Error(string.Format("Resolving invalid image url: {0}", this._url),ex);
                            this._fileName = string.Empty;
                        }
                    }
                }

                return this._fileName;
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
            this._title = title;
            this._url = url;
        }

        /// <summary>
        /// The get content.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public virtual Stream GetContent()
        {
            return this._stream;
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public virtual void Read(Stream response)
        {
            this._stream = response;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Img) obj);
        }

        public override string ToString()
        {
            return string.Format("Title: {0},Url: {1}", this._title, this._url);
        }

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
