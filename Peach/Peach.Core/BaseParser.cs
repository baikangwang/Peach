// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseParser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The base parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Peach.Entity;

namespace Peach.Core
{
    using System;

    /// <summary>
    /// The base parser.
    /// </summary>
    public abstract class BaseParser : IDisposable
    {
        public event ParserStatusEventHandler ParserStatusChanged;

        public event ParserProcessEventHandler ParserGalleryProcessed;

        protected virtual void OnParserGalleryProcessed( ParserProcessEventArgs e)
        {
            ParserProcessEventHandler handler = ParserGalleryProcessed;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnParserStatusChanged(ParserStatusEventArgs e)
        {
            ParserStatusEventHandler handler = ParserStatusChanged;
            if (handler != null) handler(this, e);
        }

        #region Fields

        /// <summary>
        ///     The _input.
        /// </summary>
        private string _input;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParser"/> class. 
        /// Initializes a new instance of the <see cref="ViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        //protected BaseParser(string input)
        //{
        //    this._input = input;
        //}

        protected BaseParser()
        {
            this._input = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the input.
        /// </summary>
        protected virtual string Input
        {
            get
            {
                return this._input;
            }

            set
            {
                this._input = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The init.
        /// </summary>
        public virtual void Init(string input)
        {
            this._input = input;
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        #region Methods

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

        #endregion
    }
}