using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Event data for Actions.
    /// </summary>
    public class ActionEventArgs : EventArgs
    {
        /// <summary>
        /// Action.
        /// </summary>
        public IICCAction Action { get; set; }
    }
}
