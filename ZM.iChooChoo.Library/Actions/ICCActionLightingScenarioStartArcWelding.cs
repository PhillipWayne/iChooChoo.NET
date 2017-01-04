using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a lighting module start arc welding scenario action.
    /// </summary>
    public class ICCActionLightingScenarioStartArcWelding : ICCActionLightingScenarioStart
    {
        /// <summary>
        /// Instantiates a new ICCActionLightingScenarioStartArcWelding.
        /// </summary>
        public ICCActionLightingScenarioStartArcWelding() : base()
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

            if (p.Length != 7)
                LogArgumentsError(7);
            else
            {
                if (biccp.RequestToModule(Address, answer, Group, Command, (byte)Data[0], (byte)p[0], (byte)p[1], (byte)(p[1] >> 8), (byte)p[2], (byte)(p[2] >> 8), (byte)p[3], (byte)(p[3] >> 8), (byte)p[4], (byte)(p[4] >> 8), (byte)p[5], (byte)p[6]))
                    b = (answer.Data[0] == BICCPConstants.BICCP_SUCCESS);
            }

            LogStart(b, p);

            return b;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            int[] p = (int[])Data[1];
            return string.Format("{0}, Output {1:x2}, millisOn={2}, millisOff={3}, pauseMin={4}, pauseMax={5}, flashMin={6}, flashMax={7}", base.ToString(), p[0], p[1], p[2], p[3], p[4], p[5], p[6]);
        }
    }
}
