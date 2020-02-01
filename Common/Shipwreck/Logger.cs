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
        public LogEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the log message
        /// </summary>
        public string Message { get; }
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
        public static void Log(string message)
        {
            OnLog?.Invoke(message, new LogEventArgs(message));
        }
    }
}
