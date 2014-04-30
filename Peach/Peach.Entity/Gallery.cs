// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Gallery.cs" company="">
//   
// </copyright>
// <summary>
//   The gallery.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Entity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The gallery.
    /// </summary>
    public class Gallery
    {
        #region Fields

        /// <summary>
        /// The _full images.
        /// </summary>
        private readonly IList<FullImage> _fullImages;

        /// <summary>
        /// The _thumbnails.
        /// </summary>
        private readonly IList<Thumbnail> _thumbnails;

        /// <summary>
        /// The _title.
        /// </summary>
        private readonly string _title;

        /// <summary>
        /// The _url.
        /// </summary>
        private readonly string _url;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Gallery"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public Gallery(string title, string url)
        {
            this._title = title;
            this._url = url;

            this._thumbnails = new List<Thumbnail>();
            this._fullImages = new List<FullImage>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the full images.
        /// </summary>
        public IList<FullImage> FullImages
        {
            get
            {
                return this._fullImages;
            }
        }

        /// <summary>
        /// Gets the thumbnails.
        /// </summary>
        public IList<Thumbnail> Thumbnails
        {
            get
            {
                return this._thumbnails;
            }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public int Add<T>(T value) where T : Img
        {
            Type t = typeof(T);

            if (t == typeof(Thumbnail))
            {
                var img = value as Thumbnail;
                img.OwnerGallery = this;
                this._thumbnails.Add(img);
                return this._thumbnails.IndexOf(img);
            }
            else if (t == typeof(FullImage))
            {
                var img = value as FullImage;
                img.OwnerGallery = this;
                this._fullImages.Add(img);
                return this._fullImages.IndexOf(img);
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof(T));
            }
        }

        /// <summary>
        /// The clear.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public void Clear<T>() where T : Img
        {
            Type t = typeof(T);

            if (t == typeof(Thumbnail))
            {
                this._thumbnails.Clear();
            }
            else if (t == typeof(FullImage))
            {
                this._fullImages.Clear();
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof(T));
            }
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public bool Contains<T>(object value) where T : Img
        {
            Type t = typeof(T);

            if (t == typeof(Thumbnail))
            {
                return this._thumbnails.Contains(value);
            }
            else if (t == typeof(FullImage))
            {
                return this._fullImages.Contains(value);
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof(T));
            }
        }

        /// <summary>
        /// The count.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public int Count<T>() where T : Img
        {
            Type t = typeof(T);

            if (t == typeof(Thumbnail))
            {
                return this._thumbnails.Count;
            }
            else if (t == typeof(FullImage))
            {
                return this._fullImages.Count;
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof(T));
            }
        }

        /*
        public int IndexOf<T>(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt<T>(int index)
        {
            throw new NotImplementedException();
        }
         * */

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public T Get<T>(int index) where T : Img
        {
            Type t = typeof(T);

            if (t == typeof(Thumbnail))
            {
                return this._thumbnails[index] as T;
            }
            else if (t == typeof(FullImage))
            {
                return this._fullImages[index] as T;
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof(T));
            }
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public IEnumerator GetEnumerator<T>() where T : Img
        {
            Type t = typeof(T);

            if (t == typeof(Thumbnail))
            {
                return this._thumbnails.GetEnumerator();
            }
            else if (t == typeof(FullImage))
            {
                return this._fullImages.GetEnumerator();
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof(T));
            }
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Title: {0}, Url: {1}", this._title, this._url));
            sb.AppendLine(string.Format("---{0} of Thumbnails---", this._thumbnails.Count));
            foreach (Thumbnail t in this._thumbnails)
            {
                sb.AppendLine(string.Format("[{0}] > {1}", this._thumbnails.IndexOf(t), t));
            }

            sb.AppendLine(string.Format("---{0} of FullImages---", this._fullImages.Count));
            foreach (FullImage f in this._fullImages)
            {
                sb.AppendLine(string.Format("[{0}] > {1}", this._fullImages.IndexOf(f), f));
            }

            return sb.ToString();
        }

        #endregion
    }
}