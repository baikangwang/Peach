namespace Peach.Entity
{
    /// <summary>
    /// The page.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// The number.
        /// </summary>
        private int number;

        /// <summary>
        /// The url.
        /// </summary>
        private string url;

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public Page(int number, string url)
        {
            this.number = number;
            this.url = url;
        }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.number = value;
            }
        }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url
        {
            get
            {
                return this.url;
            }

            set
            {
                this.url = value;
            }
        }
    }
}
