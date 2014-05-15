using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peach.View
{
    public class ViewException:ApplicationException
    {
        public ViewException(string message, Exception inner):base(message,inner)
        {
            
        }

        public ViewException(string message) : base(message)
        {
            
        }
    }
}
