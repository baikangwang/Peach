using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Peach.Core;
using Peach.Entity;

namespace Peach.View
{
    public abstract class View<T> : View where T : BaseParser
    {
        public event ParserStatusEventHandler ViewStatusChanged
        {
            add { ViewParser.ParserStatusChanged += value; }
            remove { ViewParser.ParserStatusChanged -= value; }
        }

        public event ParserProcessEventHandler ViewGalleryProcessed
        {
            add
            {
                ViewParser.ParserGalleryProcessed += value;
            }
            remove
            {
                ViewParser.ParserGalleryProcessed -= value;
            }
        }

        protected T ViewParser { get; private set; }
        
        public override void GetView()
        {
            MethodResult<HttpWebResponse> r = Browser.Current.GetPage(this.Url);
            if (r)
            {
                try
                {
                    using (Stream s = r.Result.GetResponseStream())
                    {
                        if (s != null)
                        {
                            using (StreamReader sr = new StreamReader(s))
                            {
                                string input = sr.ReadToEnd();
                                ViewParser.Init(input);
                            }
                        }
                        else
                        {
                            // error
                            throw new ViewException(string.Format("{0} -> fail to get page stream.", this.Url));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ViewException)
                    {
                        throw;
                    }
                    
                    Peach.Log.Logger.Current.ErrorFormat(string.Format("{0} -> fail to get page stream", this.Url), ex);
                    throw new ViewException(string.Format("{0} -> fail to get page stream. {1}", this.Url, ex.Message));
                }
                
            }
            else
            {
                // no response, show error
                throw new ViewException(string.Format("{0} -> no response. {1}",this.Url, r.Message));
            }
        }

        protected View(string url):base(url)
        {
            ViewParser = Activator.CreateInstance(typeof(T)) as T;
        }
        
        protected override void Dispose(bool all)
        {
            base.Dispose(all);
            
            if (all)
            {
                this.ViewParser.Dispose();
            }
        }
    }

    public abstract class View:IDisposable
    {
        private string _url;
        protected virtual string Url
        {
            get { return _url; }
        }

        protected View(string url)
        {
            this._url = url;
        }

        public abstract void GetView();
        
        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                this._url = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
