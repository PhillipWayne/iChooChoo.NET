using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZM.iChooChoo.Library.LogManagers
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs() : base()
        {

        }

        public virtual ILogEntry Entry { get; set; }
    }
}
