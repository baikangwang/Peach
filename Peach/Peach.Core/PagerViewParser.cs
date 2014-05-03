// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerViewParser.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The pager view parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Core
{
    using Peach.Entity;

    /// <summary>
    /// The pager view parser.
    /// </summary>
    public abstract class PagerViewParser : ViewParser
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerViewParser"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        protected PagerViewParser(string input)
            : base(input)
        {
            this.Init();
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
            using (var p = new PagerParser(this.PagerInput))
            {
                Pager pr = p.GetPager(cleanup);
                return pr;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The init.
        /// </summary>
        protected abstract void Init();

        #endregion
    }
}