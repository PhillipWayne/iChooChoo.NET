using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.LogManagers
{
    public class LogEntry : ILogEntry
    {
        public LogEntry()
        {
            Stamp = DateTime.Now;
            Text = string.Empty;
            Address = 0x00;
            Success = null;
            LogLevel = LogLevel.Information;
        }

        public LogEntry(string text)
        {
            Stamp = DateTime.Now;
            Text = text;
            Address = 0x00;
            Success = null;
            LogLevel = LogLevel.Information;
        }

        public virtual DateTime Stamp { get; set; }

        public virtual string Text { get; set; }

        public virtual byte Address { get; set; }

        public virtual bool? Success { get; set; }

        public virtual LogLevel LogLevel { get; set; }

        public override string ToString()
        {
            string sSuccess = (Success == true ? ": Success" : (Success == false ? ": Error" : string.Empty));
            string sAddress = (Address == 0x00 ? string.Empty : string.Format(" on 0x{0:x2}", Address));
            return string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00} {6}{7}{8}", Stamp.Year, Stamp.Month, Stamp.Day, Stamp.Hour, Stamp.Minute, Stamp.Second, Text, sAddress, sSuccess);
        }
    }
}
