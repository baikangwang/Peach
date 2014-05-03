// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodResult.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The method result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Entity
{
    /// <summary>
    /// The method result.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class MethodResult<T>
    {
        #region Fields

        /// <summary>
        ///     The message.
        /// </summary>
        private readonly string message;

        /// <summary>
        ///     The result.
        /// </summary>
        private readonly T result;

        /// <summary>
        ///     The value.
        /// </summary>
        private readonly bool value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodResult{T}"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public MethodResult(T result, bool value = true)
        {
            this.value = value;
            this.result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodResult{T}"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public MethodResult(string message, bool value = false)
        {
            this.value = value;
            this.result = default(T);
            this.message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the message.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }
        }

        /// <summary>
        ///     Gets the result.
        /// </summary>
        public T Result
        {
            get
            {
                return this.result;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether result.
        /// </summary>
        public bool Value
        {
            get
            {
                return this.value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Overloads false operator to return underlying Result
        /// </summary>
        /// <param name="mr">mr</param>
        /// <returns></returns>
        public static bool operator false(MethodResult<T> mr)
        {
            return !mr.value;
        }

        /// <summary>
        ///     Overloads negation (!) operator to return underlying Result
        /// </summary>
        /// <param name="mr">mr</param>
        /// <returns></returns>
        public static bool operator !(MethodResult<T> mr)
        {
            return !mr.value;
        }

        /// <summary>
        ///     Overloads true operator to return underlying Result
        /// </summary>
        /// <param name="mr">mr</param>
        /// <returns></returns>
        public static bool operator true(MethodResult<T> mr)
        {
            return mr.value;
        }

        #endregion
    }
}