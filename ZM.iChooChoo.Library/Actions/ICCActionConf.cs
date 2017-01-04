using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Abstract class containing a module configuration action.
    /// </summary>
    public abstract class ICCActionConf : ICCAction
    {
        /// <summary>
        /// Instantiates a new ICCActionConf.
        /// </summary>
        public ICCActionConf() : base()
        {
            Group = ICCConstants.BICCP_GRP_CONF;
        }
    }
}
