using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Interface for Server Log Manager.
    /// </summary>
    public interface IServerLogManager
    {
        /// <summary>
        /// Event fired when a new Log Entry is added.
        /// </summary>
        event LogEventHandler EntryAdded;
    }
}
