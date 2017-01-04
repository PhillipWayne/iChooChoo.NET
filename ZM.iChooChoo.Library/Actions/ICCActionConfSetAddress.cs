using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a module configuration set address action.
    /// </summary>
    public class ICCActionConfSetAddress : ICCActionConf
    {
        /// <summary>
        /// Instantiates a new ICCActionConfSetAddress.
        /// </summary>
        public ICCActionConfSetAddress() : base()
        {
            Command = ICCConstants.BICCP_CMD_CONF_ADDR;
            ICCPCommand = ICCConstants.ICCP_COMMANDS.SET_ADDR;
        }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        public override string ToICCP()
        {
            return string.Format("SET_ADDR {0:x2}\n", (byte)Data[0]);
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

            if (biccp.RequestToModule(Address, answer, Group, Command, (byte)Data[0]))
                b = (answer.Data[0] == BICCPConstants.BICCP_SUCCESS);

            Log?.LogMessageToHex(b, "Set address", Address, (byte)Data[0]);

            return b;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return string.Format("{0}, new address {1:x2}", base.ToString(), (byte)Data[0]);
        }
    }
}
