using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Represents a Traction Module.
    /// </summary>
    public class TractionModule : Module, ITractionModule
    {
        /// <summary>
        /// Instantiates a new Traction Module.
        /// </summary>
        public TractionModule() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("TractionModule()");         
#endif
            Type = ICCConstants.BICCP_GRP_TRACTION;
            TypeDescription = ICCConstants.GetTypeDescription((byte)Type);
        }

        /// <summary>
        /// Writes the status of the module.
        /// </summary>
        /// <returns>Status of the module.</returns>
        public override string writeStatus()
        {
            string s = string.Empty;

            return s;
        }
    }
}
