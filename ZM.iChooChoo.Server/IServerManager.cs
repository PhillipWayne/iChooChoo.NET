using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Server
{
    /// <summary>
    /// Interface for Server Manager.
    /// </summary>
    public interface IServerManager
    {
        /// <summary>
        /// Gets or sets the client socket.
        /// </summary>
        TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets the BICCP Manager.
        /// </summary>
        IBICCPManager Biccp { get; set; }

        /// <summary>
        /// Gets or sets the Configuration Manager.
        /// </summary>
        IConfManager Conf { get; set; }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        ILogger Log { get; set; }

        /// <summary>
        /// Gets or sets the BICCP Communicator.
        /// </summary>
        IBICCPCommunicator Communicator { get; set; }

        /// <summary>
        /// Gets or sets the Session ID.
        /// </summary>
        int SessionID { get; set; }

        /// <summary>
        /// Loop managing the client connection and communication.
        /// </summary>
        void Loop();
    }
}
