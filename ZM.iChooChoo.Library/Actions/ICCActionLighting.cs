using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Abstract class containing a lighting module action.
    /// </summary>
    public abstract class ICCActionLighting : ICCAction
    {
        /// <summary>
        /// Instantiates a new ICCActionLighting.
        /// </summary>
        public ICCActionLighting() : base()
        {
            Group = ICCConstants.BICCP_GRP_LIGHTING;
        }
    }
}
