namespace Peach.Entity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class Gallery
    {
        private readonly string _title;

        private readonly IList<Thumbnail> _thumbnails;

        private readonly IList<FullImage> _fullImages;

        public string Title
        { get { return this._title; } }

        public Gallery(string title)
        {
            this._title = title;

            this._thumbnails=new List<Thumbnail>();
            this._fullImages=new List<FullImage>();
        }

        public IList<FullImage> FullImages
        {
            get { return this._fullImages; }
        }

        public IList<Thumbnail> Thumbnails
        {
            get { return this._thumbnails; }
        }

        public IEnumerator GetEnumerator<T>() where T:Img
        {
            Type t = typeof (T);
            
            if (t == typeof (Thumbnail))
            {
                return this._thumbnails.GetEnumerator();
            }
            else if (t == typeof (FullImage))
            {
                return this._fullImages.GetEnumerator();
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof (T));
            }
        }

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

        public int Add<T>(T value) where T : Img
        {
            Type t = typeof(T);

            if (t == typeof(Thumbnail))
            {
                Thumbnail img = value as Thumbnail;
                img.OwnerGallery = this;
                this._thumbnails.Add(img);
                return this._thumbnails.IndexOf(img);
            }
            else if (t == typeof(FullImage))
            {
                FullImage img = value as FullImage;
                img.OwnerGallery = this;
                this._fullImages.Add(img);
                return this._fullImages.IndexOf(img);
            }
            else
            {
                throw new NotSupportedException("Not Support the type of " + typeof(T));
            }
        }

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Title: {0}", this._title));
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
    }
}
