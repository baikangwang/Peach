using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Peach.Core
{
    public abstract class BaseParser:IDisposable
    {
        /// <summary>
        /// The _input.
        /// </summary>
        private string _input;

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        protected virtual string Input
        {
            get { return this._input; }
            set { this._input = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        protected BaseParser(string input)
        {
            this._input = input;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewParser"/> class.
        /// </summary>
        protected BaseParser()
        {
            
        }
        
        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="all">
        /// The all.
        /// </param>
        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                this._input = null;
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
