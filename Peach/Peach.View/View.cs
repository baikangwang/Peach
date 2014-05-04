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
        public event ViewEventHandle ViewStatusChanged;

        protected virtual void OnViewStatusChanged(ViewEventArgs e)
        {
            ViewEventHandle handler = ViewStatusChanged;
            if (handler != null) handler(this, e);
        }

        protected T ViewParser { get; set; }
        
        public override void GetView()
        {
            MethodResult<HttpWebResponse> r = Browser.Current.GetPage(this.Url);
            if (r)
            {
                using (Stream s=r.Result.GetResponseStream())
                {
                    if (s != null)
                    {
                        using (StreamReader sr=new StreamReader(s))
                        {
                            string input = sr.ReadToEnd();
                            ViewParser = Activator.CreateInstance(typeof (T), input) as T;
                            EventInfo e = typeof (T).GetEvent("ParserStatusChanged");
                            if (e != null)
                            {
                                e.AddEventHandler(ViewParser, new ParserEventHandler(ParserStatusChanged));
                            }
                        }
                    }
                    else
                    {
                        // error
                        // throw exception
                    }
                }
            }
            else
            {
                // no response, show error
                //throw exception
            }
        }

        protected View(string url):base(url)
        {
        }
        
        protected override void Dispose(bool all)
        {
            base.Dispose(all);
            
            if (all)
            {
                this.ViewParser.Dispose();
            }
        }

        protected void ParserStatusChanged(object sender, ParserEventArgs e)
        {
            this.OnViewStatusChanged(new ViewEventArgs(e.Message));
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
