using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a lighting module start traffic lights scenario action.
    /// </summary>
    public class ICCActionLightingScenarioStartTrafficLights : ICCActionLightingScenarioStart
    {
        /// <summary>
        /// Instantiates a new ICCActionLightingScenarioStartTrafficLights.
        /// </summary>
        public ICCActionLightingScenarioStartTrafficLights() : base()
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

            if (p.Length != 9)
                LogArgumentsError(9);
            else
            {
                if (biccp.RequestToModule(Address, answer, Group, Command, (byte)Data[0], (byte)p[0], (byte)p[1], (byte)p[2], (byte)p[3], (byte)p[4], (byte)p[5], (byte)p[6], (byte)(p[6] >> 8), (byte)p[7], (byte)(p[7] >> 8), (byte)p[8], (byte)(p[8] >> 8)))
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
            if ((byte)Data[0] == ICCConstants.BICCP_SCN_LIGHT_TRFCFRA)
                return "French";
            else
                return "German";
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            int[] p = (int[])Data[1];
            return string.Format("{0}, Mode={1}, outputGreen1={2:x2}, outputYellow1={3:x2}, outputRed1={4:x2}, outputGreen2={5:x2}, outputYellow2={6:x2}, outputRed2={7:x2}, millisGreen={8}, millisYellow={9}, millisRed={10}", base.ToString(), GetMode(), p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[7], p[8]);
        }
    }
}
