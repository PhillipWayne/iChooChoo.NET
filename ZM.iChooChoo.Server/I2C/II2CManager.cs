using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Server.I2C
{
    /// <summary>
    /// Interface for I2C Manager.
    /// </summary>
    public interface II2CManager
    {
        /// <summary>
        /// Reads I2C bus for one byte.
        /// </summary>
        /// <param name="fd">File descriptor.</param>
        /// <returns>Byte received.</returns>
        int I2CRead(int fd);

        /// <summary>
        /// Reads I2C bus for one byte.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <returns>Byte received.</returns>
        int I2CReadAddr(int addr);

        /// <summary>
        /// Reads I2C bus for a block of data.
        /// </summary>
        /// <param name="fd">File descriptor.</param>
        /// <param name="myData">Byte array to contain received data.</param>
        /// <returns>C function return value.</returns>
        int I2CReadBlock(int fd, ref byte[] myData);

        /// <summary>
        /// Scans the I2C bus.
        /// </summary>
        /// <param name="busId">I2C bus ID.</param>
        /// <param name="data">Array of bytes. Will contain status of module presence for each address (0 = no module, 1 = module found). It is mandaatory that the array has a size of 120 (0x78).</param>
        /// <returns>Quantity of modules found.</returns>
        int I2CScan(byte busId, ref byte[] data);

        /// <summary>
        /// Gets a file descriptor for a module address.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="busId">I2C bus ID.</param>
        /// <returns>File descriptor.</returns>
        int I2CSetup(int addr, byte busId);

        /// <summary>
        /// Write a block of data to I2C bus.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="bfr">Buffer of data to write.</param>
        /// <returns>C function return value.</returns>
        int I2CWriteBlock(int fd, ref byte[] bfr);

        /// <summary>
        /// Reads I2C bus for a block of data.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="myData">Byte array to contain received data.</param>
        /// <returns>C function return value.</returns>
        int I2CReadBlockAddr(int addr, ref byte[] myData);
    }
}
