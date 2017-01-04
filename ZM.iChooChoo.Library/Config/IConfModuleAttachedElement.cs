using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public interface IConfModuleAttachedElement
    {
        int ModuleID { get; set; }
        int IOPort { get; set; }
    }
}
