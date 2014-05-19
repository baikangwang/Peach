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

    public class ParserProcessEventArgs:EventArgs
    {
        public int Index { get; private set; }

        public Gallery Gallery { get; private set; }
        
        public ParserProcessEventArgs(int index, Gallery gallery)
        {
            this.Index = index;
            this.Gallery = gallery;
        }
    }

    public delegate void ParserStatusEventHandler(object sender, ParserStatusEventArgs e);

    public delegate void ParserProcessEventHandler(object sender, ParserProcessEventArgs e);
}
