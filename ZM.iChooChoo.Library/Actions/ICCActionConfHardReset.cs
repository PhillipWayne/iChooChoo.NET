using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a module configuration Hard Reset action.
    /// </summary>
    public class ICCActionConfHardReset : ICCActionConf
    {
        /// <summary>
        /// Instantiates a new ICCActionConfHardReset.
        /// </summary>
        public ICCActionConfHardReset() : base()
        {
            Command = ICCConstants.BICCP_CMD_CONF_HARDRST;
            ICCPCommand = ICCConstants.ICCP_COMMANDS.DO_SOFTRESET;
        }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        public override string ToICCP()
        {
            return string.Format("DO_HARDRESET {0:x2}\n", Address);
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

            if (biccp.RequestToModule(Address, answer, Group, Command))
                b = (answer.Data[0] == BICCPConstants.BICCP_SUCCESS);

            Log?.LogMessage(b, "HardReset", Address);

            return b;
        }
    }
}
