using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Actions;

namespace ZM.iChooChoo.Library.BICCP
{
    /// <summary>
    /// Interface for BICCP Communicator.
    /// </summary>
    public interface IBICCPCommunicator
    {
        /// <summary>
        /// Add an action to the list.
        /// </summary>
        /// <param name="action">Action to be added.</param>
        void AddMessage(IICCAction message);

        /// <summary>
        /// Start the Communicator. Creates a new thread for BICCP communication handling.
        /// </summary>
        void Start();
    }
}
