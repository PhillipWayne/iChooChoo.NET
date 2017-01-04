using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Data for LogEventHandler.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Instantiates a new LogEventArgs.
        /// </summary>
        public LogEventArgs() : base()
        {

        }

        /// <summary>
        /// Gets or sets the Log Entry.
        /// </summary>
        public virtual ILogEntry Entry { get; set; }
    }
}
