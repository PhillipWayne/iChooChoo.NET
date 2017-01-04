using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Interface for Log Entry.
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// Gets or sets the timestamp of the entry.
        /// </summary>
        DateTime Stamp { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the address of the module.
        /// </summary>
        byte Address { get; set; }

        /// <summary>
        /// Gets or sets the success status.
        /// </summary>
        bool? Success { get; set; }

        /// <summary>
        /// Gets or sets the log level of the entry.
        /// </summary>
        LogLevel LogLevel { get; set; }
    }
}
