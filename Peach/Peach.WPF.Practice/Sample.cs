using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.WPF.Practice
{
    public class Sample
    {
        public string Title { get; set; }

        public IList<SampleItem> Items { get; set; } 

        public Sample(string title,IList<SampleItem> items )
        {
            this.Title = title;
            this.Items = items;
        }
    }

    public class SampleItem
    {
        public string Title { get; set; }

        public SampleItem(string title)
        {
            this.Title = title;
        }
    }
}
