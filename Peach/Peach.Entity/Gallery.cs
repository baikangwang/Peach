// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Gallery.cs" company="Orange">
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
    public class Gallery:IDisposable
    {
        #region Fields

        /// <summary>
        /// The _full images.
        /// </summary>
        private IList<IFullImage> _fullImages;

        /// <summary>
        /// The _thumbnails.
        /// </summary>
        private IList<IThumbnail> _thumbnails;

        /// <summary>
        /// The _title.
        /// </summary>
        private string _title;

        /// <summary>
        /// The _url.
        /// </summary>
        private string _url;

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

            this._thumbnails = new List<IThumbnail>();
            this._fullImages = new List<IFullImage>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the full images.
        /// </summary>
        public IList<IFullImage> FullImages
        {
            get
            {
                return this._fullImages;
            }
        }

        /// <summary>
        /// Gets the thumbnails.
        /// </summary>
        public IList<IThumbnail> Thumbnails
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
        public int Add<T>(T value) where T : IImage
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            if (interfaces.Contains(typeof(IThumbnail)))
            {
                var img = value as IThumbnail;
                img.OwnerGallery = this;
                this._thumbnails.Add(img);
                return this._thumbnails.IndexOf(img);
            }
            else if (interfaces.Contains(typeof(IFullImage)))
            {
                var img = value as IFullImage;
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
        public void Clear<T>() where T : IImage
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            if (interfaces.Contains(typeof(IThumbnail)))
            {
                this._thumbnails.Clear();
            }
            else if (interfaces.Contains(typeof(IFullImage)))
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
        public bool Contains<T>(object value) where T : IImage
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            if (interfaces.Contains(typeof(IThumbnail)))
            {
                return this._thumbnails.Contains(value);
            }
            else if (interfaces.Contains(typeof(IFullImage)))
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
        public int Count<T>() where T : IImage
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            if (interfaces.Contains(typeof(IThumbnail)))
            {
                return this._thumbnails.Count;
            }
            else if (interfaces.Contains(typeof(IFullImage)))
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
        public T Get<T>(int index) where T : IImage
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            if (interfaces.Contains(typeof(IThumbnail)))
            {
                return (T)this._thumbnails[index];
            }
            else if (interfaces.Contains(typeof(IFullImage)))
            {
                return (T) this._fullImages[index];
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
        public IEnumerator GetEnumerator<T>() where T : IImage
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            if (interfaces.Contains(typeof(IThumbnail)))
            {
                return this._thumbnails.GetEnumerator();
            }
            else if (interfaces.Contains(typeof(IFullImage)))
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
            foreach (IThumbnail t in this._thumbnails)
            {
                sb.AppendLine(string.Format("[{0}] > {1}", this._thumbnails.IndexOf(t), t));
            }

            sb.AppendLine(string.Format("---{0} of FullImages---", this._fullImages.Count));
            foreach (IFullImage f in this._fullImages)
            {
                sb.AppendLine(string.Format("[{0}] > {1}", this._fullImages.IndexOf(f), f));
            }

            return sb.ToString();
        }

        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                this._title = null;
                this._url = null;

                foreach (IThumbnail img in _thumbnails)
                {
                    img.Dispose();
                }

                this._thumbnails.Clear();
                this._thumbnails = null;

                foreach (IFullImage img in this._fullImages)
                {
                    img.Dispose();
                }
                this._fullImages.Clear();
                this._fullImages = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion
    }
}