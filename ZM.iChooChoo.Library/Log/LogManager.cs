using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Manages a log. This class is abstract and cannot be used directly. Please inherit from this class and implement 'void Write(ILogEntry entry, bool NewLineAtEnd)' method.
    /// </summary>
    public abstract class LogManager: ILogManager
    {
        /// <summary>
        /// Instanciates a new instance of the LogManager class.
        /// </summary>
        public LogManager()
        {
        }

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <param name="logger"></param>
        public virtual void Init(ILogger logger)
        {
            logger.EntryAdded += Logger_EntryAdded;
        }

        /// <summary>
        /// Handles a new entry in the log.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event Data.</param>
        protected virtual void Logger_EntryAdded(object sender, LogEventArgs e)
        {
            WriteLine(e.Entry);
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
    }
}
