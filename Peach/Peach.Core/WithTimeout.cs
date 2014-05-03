namespace Peach.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// The util.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// The with timeout.
        /// </summary>
        /// <param name="proc">
        /// The proc.
        /// </param>
        /// <param name="duration">
        /// The duration.
        /// </param>
        /// <typeparam name="R">
        /// </typeparam>
        /// <returns>
        /// The <see cref="R"/>.
        /// </returns>
        /// <exception cref="TimeoutException">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public static R WithTimeout<R>(Func<R> proc, int duration)
        {
            var reset = new AutoResetEvent(false);
            var r = default(R);
            Exception ex = null;

            var t = new Thread(() =>
            {
                try
                {
                    r = proc();
                }
                catch (Exception e)
                {
                    ex = e;
                }

                reset.Set();
            });

            t.Start();

            // not sure if this is really needed in general
            while (t.ThreadState != ThreadState.Running)
            {
                Thread.Sleep(0);
            }

            if (!reset.WaitOne(duration))
            {
                t.Abort();
                throw new TimeoutException();
            }

            if (ex != null)
            {
                throw ex;
            }

            return r;
        }
    }
}
