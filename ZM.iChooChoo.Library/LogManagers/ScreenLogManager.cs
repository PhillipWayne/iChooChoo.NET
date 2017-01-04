using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.LogManagers
{
    public class ScreenLogManager : LogManager
    {
        public ScreenLogManager(LogLevel level = LogLevel.Information) : base(level)
        {

        }

        protected override void Write(ILogEntry entry, bool NewLineAtEnd)
        {
            if (NewLineAtEnd)
                Console.WriteLine(entry.ToString());
            else
                Console.Write(entry.ToString());
        }
    }
}
