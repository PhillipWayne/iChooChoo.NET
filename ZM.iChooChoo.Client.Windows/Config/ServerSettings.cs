using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;

namespace ZM.iChooChoo.Client.Windows.Config
{
    /// <summary>
    /// Class containing the server settings.
    /// </summary>
    public class ServerSettings
    {
        /// <summary>
        /// Instantiates a new Server Settings.
        /// </summary>
        public ServerSettings()
        {
            ServerName = string.Empty;
            ServerCommandPort = ICCConstants.TCP_CMD_PORT;
            ServerLogPort = ICCConstants.TCP_LOG_PORT;
        }

        /// <summary>
        /// Gets or sets the server name or IP.
        /// </summary>
        public virtual string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the server command TCP port.
        /// </summary>
        public virtual int ServerCommandPort { get; set; }

        /// <summary>
        /// Gets or sets the server log TCP port.
        /// </summary>
        public virtual int ServerLogPort { get; set; }
    }
}
