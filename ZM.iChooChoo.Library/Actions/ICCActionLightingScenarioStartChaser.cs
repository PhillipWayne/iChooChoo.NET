using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a lighting module start chaser scenario action.
    /// </summary>
    public class ICCActionLightingScenarioStartChaser : ICCActionLightingScenarioStart
    {
        /// <summary>
        /// Instantiates a new ICCActionLightingScenarioStartChaser.
        /// </summary>
        public ICCActionLightingScenarioStartChaser() : base()
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

            if (p.Length != 12)
                LogArgumentsError(12);
            else
            {
                if (biccp.RequestToModule(Address, answer, Group, Command, (byte)Data[0], (byte)p[0], (byte)p[1], (byte)p[2], (byte)p[3], (byte)p[4], (byte)p[5], (byte)p[6], (byte)p[7], (byte)p[8], (byte)(p[8] >> 8), (byte)p[9], (byte)(p[9] >> 8), (byte)p[10], (byte)(p[10] >> 8), (byte)p[11]))
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
            return string.Format("{0}, output1={1:x2}, output2={2:x2}, output3={3:x2}, output4={4:x2}, output5={5:x2}, output6={6:x2}, output7={7:x2}, output8={8:x2}, millisStep={9}, millisOn={10}, millisPause={11}, coexistence={12:x2}", base.ToString(), p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[7], p[8], p[9], p[10], p[11], p[12]);
        }
    }
}
