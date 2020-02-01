using System;
using System.Collections.Generic;
using System.Text;

namespace Shipwreck
{
    /// <summary>
    /// Log event arguments
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the LogEventArgs class
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="data">Associated data</param>
        public LogEventArgs(string message, object data)
        {
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Gets the log message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the associated data
        /// </summary>
        public object Data { get; }
    }

    /// <summary>
    /// Shipwreck logger
    /// </summary>
    public static class Logger
    {
        public static event EventHandler<LogEventArgs> OnLog;

        /// <summary>
        /// Log activity
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="data">Associated data</param>
        public static void Log(string message, object data = null)
        {
            OnLog?.Invoke(message, new LogEventArgs(message, data));
        }
    }
}
