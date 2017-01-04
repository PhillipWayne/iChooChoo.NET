using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Abstract class containing a general purpose module action.
    /// </summary>
    public abstract class ICCActionGenPurp : ICCAction
    {
        /// <summary>
        /// Instantiates a new ICCActionGenPurp.
        /// </summary>
        public ICCActionGenPurp() : base()
        {
            Group = ICCConstants.BICCP_GRP_GENPURP;
        }
    }
}
