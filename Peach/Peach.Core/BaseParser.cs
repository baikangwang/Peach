// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseParser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The base parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Core
{
    using System;

    /// <summary>
    /// The base parser.
    /// </summary>
    public abstract class BaseParser : IDisposable
    {
        public event ParserEventHandler ParserStatusChanged;

        protected virtual void OnParserStatusChanged(ParserEventArgs e)
        {
            ParserEventHandler handler = ParserStatusChanged;
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
        protected BaseParser(string input)
        {
            this._input = input;
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