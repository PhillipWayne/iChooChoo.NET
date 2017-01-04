using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public class ConfRelay : ConfModuleAttachedElement, IConfRelay
    {
        public ConfRelay() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("cConfRelay()");         
#endif
        }
    }
}
