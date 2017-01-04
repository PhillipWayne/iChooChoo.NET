using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Server.I2C
{
    /// <summary>
    /// Class embedding calls to the C library for I2C communications.
    /// </summary>
    public class I2CManager : II2CManager
    {
        /// <summary>
        /// Instantiates a new I2CManager.
        /// </summary>
        public I2CManager()
        {

        }

        /// <summary>
        /// Reads I2C bus for one byte.
        /// </summary>
        /// <param name="fd">File descriptor.</param>
        /// <returns>Byte received.</returns>
        public int I2CRead(int fd)
        {
            return libICC.I2CRead(fd);
        }

        /// <summary>
        /// Reads I2C bus for one byte.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <returns>Byte received.</returns>
        public int I2CReadAddr(int addr)
        {
            return libICC.I2CReadAddr(addr);
        }

        /// <summary>
        /// Reads I2C bus for a block of data.
        /// </summary>
        /// <param name="fd">File descriptor.</param>
        /// <param name="myData">Byte array to contain received data.</param>
        /// <returns>C function return value.</returns>
        public int I2CReadBlock(int fd, ref byte[] myData)
        {
            return libICC.I2CReadBlock(fd, myData);
        }

        /// <summary>
        /// Reads I2C bus for a block of data.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="myData">Byte array to contain received data.</param>
        /// <returns>C function return value.</returns>
        public int I2CReadBlockAddr(int addr, ref byte[] myData)
        {
            return libICC.I2CReadBlockAddr(addr, myData);
        }

        /// <summary>
        /// Scans the I2C bus.
        /// </summary>
        /// <param name="busId">I2C bus ID.</param>
        /// <param name="data">Array of bytes. Will contain status of module presence for each address (0 = no module, 1 = module found). It is mandaatory that the array has a size of 120 (0x78).</param>
        /// <returns>Quantity of modules found.</returns>
        public int I2CScan(byte busId, ref byte[] data)
        {
            return libICC.I2CScan(busId, data);
        }

        /// <summary>
        /// Gets a file descriptor for a module address.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="busId">I2C bus ID.</param>
        /// <returns>File descriptor.</returns>
        public int I2CSetup(int addr, byte busId)
        {
            return libICC.I2CSetup(addr, busId);
        }

        /// <summary>
        /// Write a block of data to I2C bus.
        /// </summary>
        /// <param name="fd">File descriptor.</param>
        /// <param name="bfr">Buffer of data to write.</param>
        /// <returns>C function return value.</returns>
        public int I2CWriteBlock(int fd, ref byte[] bfr)
        {
            return libICC.I2CWriteBlock(fd, bfr);
        }

        /// <summary>
        /// Write a block of data to I2C bus.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="bfr">Buffer of data to write.</param>
        /// <returns>C function return value.</returns>
        public int I2CWriteBlockAddr(int addr, ref byte[] bfr)
        {
            return libICC.I2CWriteBlockAddr(addr, bfr);
        }
    }
}
