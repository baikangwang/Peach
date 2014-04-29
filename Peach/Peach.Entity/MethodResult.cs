namespace Peach.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The method result.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class MethodResult<T>
    {
        /// <summary>
        /// The value.
        /// </summary>
        private readonly bool value;

        /// <summary>
        /// The result.
        /// </summary>
        private T result;

        /// <summary>
        /// The message.
        /// </summary>
        private string message;

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

        /// <summary>
        /// Gets a value indicating whether result.
        /// </summary>
        public bool Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        public T Result
        {
            get
            {
                return this.result;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }
        }

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
    }
}
