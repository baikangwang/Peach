// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Orange">
//   
// </copyright>
// <summary>
//   The logger.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Peach.Log
{
    using System;

    using log4net;
    using log4net.Config;

    /// <summary>
    /// The logger.
    /// </summary>
    public class Logger
    {
        #region Static Fields

        /// <summary>
        /// The current.
        /// </summary>
        private static Logger _default; //= new Logger();
        private static Logger _dev;

        #endregion

        #region Fields

        /// <summary>
        /// The _logger.
        /// </summary>
        private ILog _logger;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        protected Logger(string name="default")
        {
            this._logger = LogManager.GetLogger(name);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current.
        /// </summary>
        public static Logger Default
        {
            get
            {
                if (_default == null)
                    _default = new Logger("default");
                return _default;
            }
        }

        public static Logger DEV
        {
            get
            {
                if(_dev==null)
                    _dev=new Logger("dev");
                return _dev;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is debug enabled.
        /// </summary>
        public bool IsDebugEnabled
        {
            get
            {
                return this._logger.IsDebugEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is error enabled.
        /// </summary>
        public bool IsErrorEnabled
        {
            get
            {
                return this._logger.IsErrorEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is fatal enabled.
        /// </summary>
        public bool IsFatalEnabled
        {
            get
            {
                return this._logger.IsFatalEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is info enabled.
        /// </summary>
        public bool IsInfoEnabled
        {
            get
            {
                return this._logger.IsInfoEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is warn enabled.
        /// </summary>
        public bool IsWarnEnabled
        {
            get
            {
                return this._logger.IsWarnEnabled;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Debug(object message)
        {
            this._logger.Debug(message);
        }

        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Debug(object message, Exception exception)
        {
            this._logger.Debug(message, exception);
        }

        /// <summary>
        /// The debug format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void DebugFormat(string format, params object[] args)
        {
            this._logger.DebugFormat(format, args);
        }

        /// <summary>
        /// The debug format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        public void DebugFormat(string format, object arg0)
        {
            this._logger.DebugFormat(format, arg0);
        }

        /// <summary>
        /// The debug format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            this._logger.DebugFormat(format, arg0, arg1);
        }

        /// <summary>
        /// The debug format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.DebugFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// The debug format.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.DebugFormat(provider, format, args);
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Error(object message)
        {
            this._logger.Error(message);
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Error(object message, Exception exception)
        {
            this._logger.Error(message, exception);
        }

        /// <summary>
        /// The error format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void ErrorFormat(string format, params object[] args)
        {
            this._logger.ErrorFormat(format, args);
        }

        /// <summary>
        /// The error format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        public void ErrorFormat(string format, object arg0)
        {
            this._logger.ErrorFormat(format, arg0);
        }

        /// <summary>
        /// The error format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            this._logger.ErrorFormat(format, arg0, arg1);
        }

        /// <summary>
        /// The error format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// The error format.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.ErrorFormat(provider, format, args);
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Fatal(object message)
        {
            this._logger.Fatal(message);
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Fatal(object message, Exception exception)
        {
            this._logger.Fatal(message, exception);
        }

        /// <summary>
        /// The fatal format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void FatalFormat(string format, params object[] args)
        {
            this._logger.FatalFormat(format, args);
        }

        /// <summary>
        /// The fatal format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        public void FatalFormat(string format, object arg0)
        {
            this._logger.FatalFormat(format, arg0);
        }

        /// <summary>
        /// The fatal format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            this._logger.FatalFormat(format, arg0, arg1);
        }

        /// <summary>
        /// The fatal format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.FatalFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// The fatal format.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.FatalFormat(provider, format, args);
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Info(object message)
        {
            this._logger.Info(message);
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Info(object message, Exception exception)
        {
            this._logger.Info(message, exception);
        }

        /// <summary>
        /// The info format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void InfoFormat(string format, params object[] args)
        {
            this._logger.InfoFormat(format, args);
        }

        /// <summary>
        /// The info format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        public void InfoFormat(string format, object arg0)
        {
            this._logger.InfoFormat(format, arg0);
        }

        /// <summary>
        /// The info format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            this._logger.InfoFormat(format, arg0, arg1);
        }

        /// <summary>
        /// The info format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.InfoFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// The info format.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.InfoFormat(provider, format, args);
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Warn(object message)
        {
            this._logger.Warn(message);
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Warn(object message, Exception exception)
        {
            this._logger.Warn(message, exception);
        }

        /// <summary>
        /// The warn format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void WarnFormat(string format, params object[] args)
        {
            this._logger.WarnFormat(format, args);
        }

        /// <summary>
        /// The warn format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        public void WarnFormat(string format, object arg0)
        {
            this._logger.WarnFormat(format, arg0);
        }

        /// <summary>
        /// The warn format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            this._logger.WarnFormat(format, arg0, arg1);
        }

        /// <summary>
        /// The warn format.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="arg0">
        /// The arg 0.
        /// </param>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            this._logger.WarnFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// The warn format.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this._logger.WarnFormat(provider, format, args);
        }

        #endregion
    }
}