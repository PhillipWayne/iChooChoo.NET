using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public class ConfSwitch : ConfModuleAttachedElement, IConfSwitch
    {
        public ConfSwitch() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("cConfSwitch()");         
#endif
        }

        public virtual int StraightValue { get; set; }
    }
}
