using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public class ConfSensor : ConfModuleAttachedElement, IConfSensor
    {
        public ConfSensor() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("cConfSensor()");         
#endif
        }

        public virtual int Type { get; set; }
    }
}
