using System;
using System.Collections.Generic;
using System.Text;

namespace Peach.Core
{
    public class BrowserEventArgs:EventArgs
    {
        public string Message { get; set; }

        public BrowserEventArgs(string message)
        {
            this.Message = message;
        }
    }

    public delegate void BrowserEventHandler(object sender, BrowserEventArgs e);
}
