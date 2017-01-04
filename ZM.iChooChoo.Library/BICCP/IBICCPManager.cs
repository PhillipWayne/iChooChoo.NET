using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.BICCP
{
    /// <summary>
    /// Interface for BICCP Manager.
    /// </summary>
    public interface IBICCPManager
    {
        /// <summary>
        /// Scans the I2C bus.
        /// </summary>
        /// <param name="list">Array of bytes. Will contain status of module presence for each address (0 = no module, 1 = module found). It is mandaatory that the array has a size of 120 (0x78).</param>
        /// <returns>Quantity of modules found.</returns>
        int BICCP_ScanBus(ref byte[] list);

        /// <summary>
        /// Sends a request to a module and gets the response.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="data">Data to send.</param>
        /// <param name="group">Group of command to send.</param>
        /// <param name="command">Command to send.</param>
        /// <param name="datas">Response data.</param>
        /// <returns>Status of response. True if response acknowledges correctly group and command, false otherwise.</returns>
        bool RequestToModule(int addr, IBICCPData data, byte group, byte command, params byte[] datas);
    }
}
