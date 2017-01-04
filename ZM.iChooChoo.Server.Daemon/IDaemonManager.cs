using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Server.Daemon
{
    /// <summary>
    /// Interface for Dameon Manager.
    /// </summary>
    public interface IDaemonManager
    {
        /// <summary>
        /// Initialisation steps.
        /// </summary>
        void Init();

        /// <summary>
        /// Lauches the network TCP listener.
        /// </summary>
        /// <returns>true when terminated.</returns>
        bool Start();
    }
}
