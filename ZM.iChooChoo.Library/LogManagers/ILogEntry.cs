using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.LogManagers
{
    public interface ILogEntry
    {
        DateTime Stamp { get; set; }

        string Text { get; set; }

        byte Address { get; set; }

        bool? Success { get; set; }

        LogLevel LogLevel { get; set; }
    }
}
