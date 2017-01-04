using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a bus scan action.
    /// </summary>
    public class ICCActionConfScanBus : ICCActionConf
    {
        /// <summary>
        /// Gets or sets the Configuration Manager.
        /// </summary>
        public virtual IConfManager Conf { get; set; }

        /// <summary>
        /// Instantiates a new ICCActionConfScanBus.
        /// </summary>
        public ICCActionConfScanBus() : base()
        {
            Command = ICCConstants.BICCP_CMD_CONF_IDENT;
            ICCPCommand = ICCConstants.ICCP_COMMANDS.DO_RESCAN;
        }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        public override string ToICCP()
        {
            return "DO_RESCAN\n";
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

            lock (Conf)
            {
                if (Conf != null)
                    if (Conf.ScanBus() != 0)
                        b = true;
            }

            Log?.LogMessage(b, "ScanBus", Address);

            return b;
        }
    }
}
