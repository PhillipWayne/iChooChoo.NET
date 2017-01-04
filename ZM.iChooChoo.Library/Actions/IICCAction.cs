using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Interface for ICCAction.
    /// </summary>
    public interface IICCAction
    {
        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        ILogger Log { get; set; }

        /// <summary>
        /// Gets or sets the module address.
        /// </summary>
        byte Address { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        ICCConstants.ICCP_COMMANDS ICCPCommand { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        object[] Data { get; set; }

        /// <summary>
        /// Gets or sets the Group of the module.
        /// </summary>
        byte Group { get; }

        /// <summary>
        /// Gets or sets the Command to send to the module.
        /// </summary>
        byte Command { get; }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        string ToICCP();

        /// <summary>
        /// Generates the BICCP command for the current action.
        /// </summary>
        /// <param name="biccp">BICCP Manager used to send the command.</param>
        /// <param name="answer">BICCPData object to retrieve the answer from the module.</param>
        /// <returns>Success of the call to the module. True if command was successful, false otherwise.</returns>
        bool ToBICCP(IBICCPManager biccp, IBICCPData answer);
    }
}
