using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peach.Entity;

namespace Peach.Core
{
    public abstract class PagerViewParser:ViewParser
    {

        protected string PagerInput { get; set; }
        protected string ViewInput { get; set; }

        protected abstract void Init();

        protected PagerViewParser(string input) : base(input)
        {
            this.Init();
        }

        public virtual Pager GetPager(bool cleanup = false)
        {
            using (PagerParser p = new PagerParser(this.PagerInput))
            {
                Pager pr = p.GetPager(cleanup);
                return pr;
            }
        }
    }
}
