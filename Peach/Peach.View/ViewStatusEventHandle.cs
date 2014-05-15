using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peach.Entity;

namespace Peach.View
{
    public class ViewStatusEventArgs:EventArgs
    {
        public string Message { get; set; }

        public ViewStatusEventArgs(string message):base()
        {
            this.Message = message;
        }
    }

    public delegate void ViewStatusEventHandle(object sender, ViewStatusEventArgs e);

    public delegate void ViewProcessEventHandle(object sender, Gallery e);
}
