using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Event handler for Log.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event Data.</param>
    public delegate void LogEventHandler(object sender, LogEventArgs e);

    /// <summary>
    /// Log levels.
    /// </summary>
    public enum LogLevel : int
    {
        /// <summary>
        /// Information log level. Used for all log entries providing useful informations for the user. Should always be logged.
        /// </summary>
        Information = 0,
        /// <summary>
        /// Warning log level. Used for non blocking errors.
        /// </summary>
        Warning = 1,
        /// <summary>
        /// Error log level. Used for blocking errors.
        /// </summary>
        Error = 2,
        /// <summary>
        /// Debug log level. Used to provide debugging information.
        /// </summary>
        Debug = 3
    }

    /// <summary>
    /// Interface for Logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets the maximum level to be logged.
        /// </summary>
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of entries to be kept in the log.
        /// </summary>
        int MaxLogEntries { get; set; }

        /// <summary>
        /// Event fired when a new entry is added to the log.
        /// </summary>
        event LogEventHandler EntryAdded;

        /// <summary>
        /// Adds a new Log Manager to the logger.
        /// </summary>
        /// <param name="log">Log Manager to be added.</param>
        void AddLogManager(ILogManager log);

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="entry">Log entry.</param>
        void Log(ILogEntry entry);

        /// <summary>
        /// Logs a text message associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="level">Log level.</param>
        void LogMessage(bool status, string message, byte address, LogLevel level = LogLevel.Information);

        /// <summary>
        /// Logs a text message associated to an integer data redered as Hexadecimal, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        /// <param name="level">Log level.</param>
        void LogMessageToHex(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information);

        /// <summary>
        /// Logs a text message associated to an integer data redered as Decimal, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        /// <param name="level">Log level.</param>
        void LogMessageToDec(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information);

        /// <summary>
        /// Logs a text message associated to some text, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        /// <param name="level">Log level.</param>
        void LogMessageText(bool status, string message, byte address, string data, LogLevel level = LogLevel.Information);

        /// <summary>
        /// Logs a text message.
        /// </summary>
        /// <param name="message">Text message.</param>
        /// <param name="level">Log level.</param>
        void LogText(string message, LogLevel level = LogLevel.Information);
    }
}
