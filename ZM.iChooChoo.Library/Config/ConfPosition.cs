using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public class ConfPosition : ConfElement, IConfPosition
    {
        public ConfPosition() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("cConfPosition()");         
#endif
        }
    }
}
