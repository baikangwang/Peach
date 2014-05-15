using System;
using System.Collections.Generic;
using System.Text;
using Peach.Entity;

namespace Peach.Core
{
    public class ParserStatusEventArgs:EventArgs
    {
        public string Message { get; set; }

        public ParserStatusEventArgs(string message)
        {
            this.Message = message;
        }
    }

    public delegate void ParserStatusEventHandler(object sender, ParserStatusEventArgs e);

    public delegate void ParserProcessEventHandler(object sender, Gallery gallery);
}
