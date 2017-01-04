using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Represents a New Module.
    /// </summary>
    public class NewModule : Module, INewModule
    {
        /// <summary>
        /// Instantiates a new New Module.
        /// </summary>
        public NewModule() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("Newmodule()");         
#endif
            Type = ICCConstants.BICCP_GRP_NEW;
            TypeDescription = ICCConstants.GetTypeDescription((byte)Type);
        }
    }
}
