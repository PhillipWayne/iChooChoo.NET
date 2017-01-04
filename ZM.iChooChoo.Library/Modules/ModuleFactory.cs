using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library.Log;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Provides helper methods to instantiate the right Modules.
    /// </summary>
    public static class ModuleFactory
    {
        /// <summary>
        /// Instantiates a Module depending on its type.
        /// </summary>
        /// <param name="type">Module type (see ICCConstants).</param>
        /// <param name="log">Log Manager.</param>
        /// <returns>A new Module instance.</returns>
        public static IModule CreateInstance(byte type, ILogger log = null)
        {
            IModule ret = null;

            if (type == ICCConstants.BICCP_GRP_NEW)
                ret = new NewModule();
            else if (type == ICCConstants.BICCP_GRP_TRACTION)
                ret = new TractionModule();
            else if (type == ICCConstants.BICCP_GRP_GENPURP)
                ret = new GenPurpModule();
            else if (type == ICCConstants.BICCP_GRP_LIGHTING)
                ret = new LightingModule();
            else if (type == ICCConstants.BICCP_GRP_UNKNOWN)
                ret = new UnknownModule();

            ret.Log = log;

            return ret;
        }
    }
}
