// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerViewParser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The pager view parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Peach.Log;

namespace Peach.Core
{
    using Peach.Entity;

    /// <summary>
    /// The pager view parser.
    /// </summary>
    public abstract class PagerViewParser : ViewParser
    {
        //private bool _isInit;
        
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        protected PagerViewParser()
        {
            //this.Init();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pager input.
        /// </summary>
        protected string PagerInput { get; set; }

        /// <summary>
        /// Gets or sets the view input.
        /// </summary>
        protected string ViewInput { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get pager.
        /// </summary>
        /// <param name="cleanup">
        /// The cleanup.
        /// </param>
        /// <returns>
        /// The <see cref="Pager"/>.
        /// </returns>
        public virtual Pager GetPager(bool cleanup = false)
        {
            OnParserStatusChanged(
                new ParserStatusEventArgs("Starting to extract pager..."));
            
            using (var p = new PagerParser())
            {
                p.Init(this.PagerInput);
                
                Pager pr = p.GetPager(cleanup);

                OnParserStatusChanged(new ParserStatusEventArgs("Finish to extract pager..."));
                return pr;
            }
        }

        public override System.Collections.Generic.IList<Gallery> ListGalleries(bool cleanup = false)
        {
            return new List<Gallery>();
        }

        #endregion
    }
}