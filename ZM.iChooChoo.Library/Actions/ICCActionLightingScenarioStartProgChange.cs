using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a lighting module start progressive change scenario action.
    /// </summary>
    public class ICCActionLightingScenarioStartProgChange : ICCActionLightingScenarioStart
    {
        /// <summary>
        /// Instantiates a new ICCActionLightingScenarioStartProgChange.
        /// </summary>
        public ICCActionLightingScenarioStartProgChange() : base()
        {
        }

        /// <summary>
        /// Generates the BICCP command for the current action.
        /// </summary>
        /// <param name="biccp">BICCP Manager used to send the command.</param>
        /// <param name="answer">BICCPData object to retrieve the answer from the module.</param>
        /// <returns>Success of the call to the module. True if command was successful, false otherwise.</returns>
        public override bool ToBICCP(IBICCPManager biccp, IBICCPData answer)
        {
            bool b = false;
            int[] p = (int[])Data[1];

            if (p.Length != 6)
                LogArgumentsError(6);
            else
            {
                if (biccp.RequestToModule(Address, answer, Group, Command, (byte)Data[0], (byte)p[0], (byte)p[1], (byte)(p[1] >> 8), (byte)p[2], (byte)p[3], (byte)p[4], (byte)(p[4] >> 8), (byte)p[5]))
                    b = (answer.Data[0] == BICCPConstants.BICCP_SUCCESS);
            }

            LogStart(b, p);

            return b;
        }
    }
}
