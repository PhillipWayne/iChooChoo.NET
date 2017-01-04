using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.Actions;

namespace ZM.iChooChoo.Library.Modules
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
