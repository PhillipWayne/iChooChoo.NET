using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Main logger class. Handles all logging for the application.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Instantiates a new Logger.
        /// </summary>
        /// <param name="level"></param>
        public Logger(LogLevel level = ICCConstants.DEFAULT_LOG_LEVEL)
        {
            Entries = new List<ILogEntry>();
            LogManagers = new List<ILogManager>();
            MaxLogEntries = 100;
            LogLevel = level;
        }

        /// <summary>
        /// Stores all log entries.
        /// </summary>
        protected virtual List<ILogEntry> Entries { get; private set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public virtual LogLevel LogLevel { get; set; }

        /// <summary>
        /// Holds all log managers.
        /// </summary>
        protected virtual List<ILogManager> LogManagers { get; private set; }

        /// <summary>
        /// Gets or sets the maximum number of entries to be kept in the log.
        /// </summary>
        public virtual int MaxLogEntries { get; set; }

        /// <summary>
        /// Actual handler storing subscribers
        /// </summary>
        protected virtual event LogEventHandler _entryAdded;
        /// <summary>
        /// Event fired when a new entry is added to the log.
        /// </summary>
        public virtual event LogEventHandler EntryAdded
        {
            // Prevents a delegate to subscribe to the same event twice
            add
            {
                bool exists = false;
                if (_entryAdded != null)
                {
                    foreach (Delegate existingHandler in _entryAdded.GetInvocationList())
                    {
                        if (Delegate.Equals(existingHandler, value))
                            exists = true;
                    }
                }
                if (!exists)
                    _entryAdded += value;
            }
            remove { _entryAdded -= value; }
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        /// <param name="entry">Log entry to be added.</param>
        protected virtual void AddEntry(ILogEntry entry)
        {
            if (entry.LogLevel <= LogLevel)
            {
                if (Entries.Count == MaxLogEntries)
                    Entries.RemoveAt(0);

                Entries.Add(entry);

                if (_entryAdded != null)
                    _entryAdded(this, new LogEventArgs() { Entry = entry });
            }
        }

        /// <summary>
        /// Adds a new Log Manager to the logger.
        /// </summary>
        /// <param name="log">Log Manager to be added.</param>
        public virtual void AddLogManager(ILogManager log)
        {
            log.Init(this);
            LogManagers.Add(log);
        }

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="entry">Log entry.</param>
        public virtual void Log(ILogEntry entry)
        {
            AddEntry(entry);
        }

        /// <summary>
        /// Logs a text message associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="level">Log level.</param>
        public virtual void LogMessage(bool status, string message, byte address, LogLevel level = LogLevel.Information)
        {
            Log(new LogEntry() { Success = status, Address = address, Text = message, LogLevel = level });
        }

        /// <summary>
        /// Logs a text message associated to an integer data redered as Hexadecimal, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        /// <param name="level">Log level.</param>
        public virtual void LogMessageToHex(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information)
        {
            Log(new LogEntry() { Success = status, Address = address, Text = string.Format("{0} to {1:x2}", message, data), LogLevel = level });
        }

        /// <summary>
        /// Logs a text message associated to an integer data redered as Decimal, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        /// <param name="level">Log level.</param>
        public virtual void LogMessageToDec(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information)
        {
            Log(new LogEntry() { Success = status, Address = address, Text = string.Format("{0} to {1}", message, data), LogLevel = level });
        }

        /// <summary>
        /// Logs a text message associated to some text, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        /// <param name="level">Log level.</param>
        public virtual void LogMessageText(bool status, string message, byte address, string data, LogLevel level = LogLevel.Information)
        {
            Log(new LogEntry() { Success = status, Address = address, Text = string.Format("{0} to {1}", message, data), LogLevel = level });
        }

        /// <summary>
        /// Logs a text message.
        /// </summary>
        /// <param name="message">Text message.</param>
        /// <param name="level">Log level.</param>
        public virtual void LogText(string message, LogLevel level = LogLevel.Information)
        {
            Log(new LogEntry(message) { LogLevel = level });
        }
    }
}
