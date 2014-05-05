using System;
using System.Collections.Generic;
using System.Text;

namespace Peach.Core
{
    public class ParserEventArgs:EventArgs
    {
        public string Message { get; set; }

        public ParserEventArgs(string message)
        {
            this.Message = message;
        }
    }

    public delegate void ParserEventHandler(object sender, ParserEventArgs e);
}
