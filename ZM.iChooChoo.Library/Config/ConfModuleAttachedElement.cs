using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public class ConfModuleAttachedElement : ConfElement, IConfModuleAttachedElement
    {
        public ConfModuleAttachedElement()
        {
#if VERBOSEDEBUG
            Console.WriteLine("cConfModuleAttachedElement()\n");
#endif
            ModuleID = 0;
            IOPort = 0;
        }

        public virtual int IOPort { get; set; }

        public virtual int ModuleID { get; set; }
    }
}
