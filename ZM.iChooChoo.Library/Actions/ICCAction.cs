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
    /// Abstract class containing a module action.
    /// </summary>
    public abstract class ICCAction : IICCAction
    {
        /// <summary>
        /// Instantiates a new ICCAction.
        /// </summary>
        public ICCAction()
        {
        }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        public virtual ILogger Log { get; set; }

        /// <summary>
        /// Gets or sets the module address.
        /// </summary>
        public virtual byte Address { get; set; }

        /// <summary>
        /// Gets or sets the ICCP command.
        /// </summary>
        public virtual ICCConstants.ICCP_COMMANDS ICCPCommand { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public virtual object[] Data { get; set; }

        /// <summary>
        /// Gets or sets the Group of the module.
        /// </summary>
        public virtual byte Group { get; protected set; }

        /// <summary>
        /// Gets or sets the Command to send to the module.
        /// </summary>
        public virtual byte Command { get; protected set; }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        public virtual string ToICCP()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates the BICCP command for the current action.
        /// </summary>
        /// <param name="biccp">BICCP Manager used to send the command.</param>
        /// <param name="answer">BICCPData object to retrieve the answer from the module.</param>
        /// <returns>Success of the call to the module. True if command was successful, false otherwise.</returns>
        public virtual bool ToBICCP(IBICCPManager biccp, IBICCPData answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
