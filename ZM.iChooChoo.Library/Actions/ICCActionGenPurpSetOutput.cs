using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a general purpose module set output action.
    /// </summary>
    public class ICCActionGenPurpSetOutput : ICCActionGenPurp
    {
        /// <summary>
        /// Instantiates a new ICCActionGenPurpSetOutput.
        /// </summary>
        public ICCActionGenPurpSetOutput() : base()
        {
            Command = ICCConstants.BICCP_CMD_GENPURP_OUT_STR;
            ICCPCommand = ICCConstants.ICCP_COMMANDS.DO_SETOUT;
        }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        public override string ToICCP()
        {
            return string.Format("DO_SETOUT {0:X2} {1:X2} {2}\n", Address, Data[0], (bool)Data[1] ? 1 : 0);
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
            byte outValue = (byte)((bool)Data[1] ? 0xFF : 0x00);

            if (biccp.RequestToModule(Address, answer, Group, (byte)(Command + (byte)Data[0]), outValue))
                b = (answer.Data[0] == BICCPConstants.BICCP_SUCCESS);

            Log?.LogMessage(b, string.Format("GenPurpModule: Set output {0:X} to {1:X2}", (byte)Data[0], outValue), Address);

            return b;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return string.Format("{0}, Output {1:x2}, value {2}", base.ToString(), (byte)Data[0], (bool)Data[1]);
        }
    }
}
