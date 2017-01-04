using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public class ConfSection : ConfModuleAttachedElement, IConfRelay
    {
        public ConfSection() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("cConfSection()");         
#endif
        }
    }
}
