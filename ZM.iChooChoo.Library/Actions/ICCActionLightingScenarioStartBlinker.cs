using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a lighting module start blinker scenario action.
    /// </summary>
    public class ICCActionLightingScenarioStartBlinker : ICCActionLightingScenarioStart
    {
        /// <summary>
        /// Instantiates a new ICCActionLightingScenarioStartBlinker.
        /// </summary>
        public ICCActionLightingScenarioStartBlinker() : base()
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

            if (p.Length != 4)
                LogArgumentsError(4);
            else
            {
                if (biccp.RequestToModule(Address, answer, Group, Command, (byte)Data[0], (byte)p[0], (byte)p[1], (byte)p[2], (byte)(p[2] >> 8), (byte)p[3], (byte)(p[3] >> 8)))
                    b = (answer.Data[0] == BICCPConstants.BICCP_SUCCESS);
            }

            LogStart(b, p);

            return b;
        }

        /// <summary>
        /// Get the blinker mode description.
        /// </summary>
        /// <returns></returns>
        private string GetMode()
        {
            if ((byte)Data[0] == ICCConstants.BICCP_SCN_LIGHT_BLKLED)
                return "Led";
            else
                return "Old";
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            int[] p = (int[])Data[1];
            return string.Format("{0}, Mode={1}, output1={2:x2}, output2={3:x2}, millis1={4}, millis2={5}", base.ToString(), GetMode(), p[0], p[1], p[2], p[3]);
        }
    }
}
