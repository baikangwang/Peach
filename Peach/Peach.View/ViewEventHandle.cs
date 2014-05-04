using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.View
{
    public class ViewEventArgs:EventArgs
    {
        public string Message { get; set; }

        public ViewEventArgs(string message):base()
        {
            this.Message = message;
        }
    }

    public delegate void ViewEventHandle(object sender, ViewEventArgs e);

}
