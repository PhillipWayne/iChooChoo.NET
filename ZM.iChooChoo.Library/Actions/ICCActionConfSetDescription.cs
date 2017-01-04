using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a module configuration set description action.
    /// </summary>
    public class ICCActionConfSetDescription : ICCActionConf
    {
        /// <summary>
        /// Instantiates a new ICCActionConfSetDescription.
        /// </summary>
        public ICCActionConfSetDescription() : base()
        {
            Command = ICCConstants.BICCP_CMD_CONF_DESC;
            ICCPCommand = ICCConstants.ICCP_COMMANDS.SET_DESC;
        }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        public override string ToICCP()
        {
            return string.Format("SET_DESC {0:x2} {1}\n", Address, (string)Data[0]);
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

            string desc = (string)Data[0];

            byte[] data = new byte[ICCConstants.DESCSIZE];
            for (int i = 0; i < ICCConstants.DESCSIZE; i++)
            {
                if (i < desc.Length)
                    data[i] = (byte)desc[i];
                else
                    data[i] = 0;
            }

            if (biccp.RequestToModule(Address, answer, Group, Command, data[0], data[1], data[2], data[3], data[4], data[5], 
                data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13]))
                b = (answer.Data[0] == BICCPConstants.BICCP_SUCCESS);

            Log?.LogMessageText(b, "Set description", Address, (string)Data[0]);

            return b;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return string.Format("{0}, new description '{1}'", base.ToString(), (string)Data[0]);
        }
    }
}
