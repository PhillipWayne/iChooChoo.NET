using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.LogManagers
{
    /// <summary>
    /// Manages a log. This class is abstract and cannot be used directly. Please inherit from this class and implement 'void Write(ILogEntry entry, bool NewLineAtEnd)' method.
    /// </summary>
    public abstract class LogManager: ILogManager
    {
        /// <summary>
        /// Stores all log entries.
        /// </summary>
        protected virtual List<ILogEntry> _entries { get; private set; }

        /// <summary>
        /// Instanciates a new instance of the LogManager class.
        /// </summary>
        /// <param name="level"></param>
        public LogManager(LogLevel level = LogLevel.Information)
        {
            _entries = new List<ILogEntry>();
            MaxLogEntries = 100;
            LogLevel = level;
        }

        /// <summary>
        /// Gets or sets the maximum number of entries to be kept in the log.
        /// </summary>
        public virtual int MaxLogEntries { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public virtual LogLevel LogLevel { get; set; }

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
                if (_entries.Count == MaxLogEntries)
                    _entries.RemoveAt(0);

                _entries.Add(entry);
                WriteLine(entry);

                if (_entryAdded != null)
                    _entryAdded(this, new LogEventArgs() { Entry = entry });
            }
        }

        /// <summary>
        /// Writes a log entry and stays on same line.
        /// </summary>
        /// <param name="entry">Log entry to be written.</param>
        protected virtual void Write(ILogEntry entry)
        {
            Write(entry, false);
        }

        /// <summary>
        /// Writes a log entry to the output.
        /// </summary>
        /// <param name="entry">Log entry to be written.</param>
        /// <param name="NewLineAtEnd">If set to true, ends the current line and eventually write a newline char if supported by the underlying support.</param>
        protected virtual void Write(ILogEntry entry, bool NewLineAtEnd)
        {
            throw new NotImplementedException("LogManager.Write()");
        }

        /// <summary>
        /// Writes a log entry and stays on its own line.
        /// </summary>
        /// <param name="entry">Log entry to be written.</param>
        protected virtual void WriteLine(ILogEntry entry)
        {
            Write(entry, true);
        }

        /// <summary>
        /// Gets all log entries as an array of strings.
        /// </summary>
        /// <returns>String array containing all log entries.</returns>
        public virtual string[] GetLogContent()
        {
            return _entries.Select(x => x.ToString()).ToArray();
        }

        /// <summary>
        /// Logs a text message associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        public virtual void LogMessage(bool status, string message, byte address, LogLevel level = LogLevel.Information)
        {
            AddEntry(new LogEntry() { Success = status, Address = address, Text = message, LogLevel = level });
        }

        /// <summary>
        /// Logs a text message associated to an integer data redered as Hexadecimal, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        public virtual void LogMessageToHex(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information)
        {
            AddEntry(new LogEntry() { Success = status, Address = address, Text = string.Format("{0} to {1:x2}", message, data), LogLevel = level });
        }

        /// <summary>
        /// Logs a text message associated to an integer data redered as Decimal, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        public virtual void LogMessageToDec(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information)
        {
            AddEntry(new LogEntry() { Success = status, Address = address, Text = string.Format("{0} to {1}", message, data), LogLevel = level });
        }

        /// <summary>
        /// Logs a text message associated to some text, and associated to an address.
        /// </summary>
        /// <param name="status">true prints Success, false prints Error.</param>
        /// <param name="message">Text message.</param>
        /// <param name="address">Address of the module.</param>
        /// <param name="data">Data to be printed.</param>
        public virtual void LogMessageText(bool status, string message, byte address, string data, LogLevel level = LogLevel.Information)
        {
            AddEntry(new LogEntry() { Success = status, Address = address, Text = string.Format("{0} to {1}", message, data), LogLevel = level });
        }

        /// <summary>
        /// Logs a text message.
        /// </summary>
        /// <param name="message">Text message.</param>
        public virtual void LogText(string message, LogLevel level = LogLevel.Information)
        {
            AddEntry(new LogEntry(message) { LogLevel = level });
        }
    }
}
