using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peach.Entity
{
    public class Thumbnail:Img
    {
        private string fullUrl;
        public Thumbnail(string title, string url,string fullurl) : base(title, url)
        {
            this.fullUrl = fullurl;
        }

        public string FullUrl
        {
            get { return this.fullUrl; }
        }

        public override string ToString()
        {
            string o = base.ToString();

            return string.Format("{0}, FullUrl: {1}", o, this.fullUrl);
        }
    }
}
