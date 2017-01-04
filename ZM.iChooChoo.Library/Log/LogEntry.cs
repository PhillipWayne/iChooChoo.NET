using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Class containing a log entry.
    /// </summary>
    public class LogEntry : ILogEntry
    {
        /// <summary>
        /// Instantiates a new Log Entry.
        /// </summary>
        public LogEntry()
        {
            Stamp = DateTime.Now;
            Text = string.Empty;
            Address = 0x00;
            Success = null;
            LogLevel = LogLevel.Information;
        }

        /// <summary>
        /// Instantiates a new Log Entry.
        /// </summary>
        /// <param name="text">Text of the entry.</param>
        public LogEntry(string text)
        {
            Stamp = DateTime.Now;
            Text = text;
            Address = 0x00;
            Success = null;
            LogLevel = LogLevel.Information;
        }

        /// <summary>
        /// Gets or sets the timestamp of the entry.
        /// </summary>
        public virtual DateTime Stamp { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the address of the module.
        /// </summary>
        public virtual byte Address { get; set; }

        /// <summary>
        /// Gets or sets the success status.
        /// </summary>
        public virtual bool? Success { get; set; }

        /// <summary>
        /// Gets or sets the log level of the entry.
        /// </summary>
        public virtual LogLevel LogLevel { get; set; }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            string sSuccess = (Success == true ? ": Success" : (Success == false ? ": Error" : string.Empty));
            string sAddress = (Address == 0x00 ? string.Empty : string.Format(" on 0x{0:x2}", Address));
            return string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00} {6}{7}{8}", Stamp.Year, Stamp.Month, Stamp.Day, Stamp.Hour, Stamp.Minute, Stamp.Second, Text, sAddress, sSuccess);
        }
    }
}
