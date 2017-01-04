using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.CRC;
using ZM.iChooChoo.Library.Log;
using ZM.iChooChoo.Server.I2C;

namespace ZM.iChooChoo.Server.BICCP
{
    /// <summary>
    /// Class handling BICCP (I2C) communications.
    /// </summary>
    public class BICCPManager : IBICCPManager
    {
        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        private ILogger Log { get; set; }

        /// <summary>
        /// Array of file descriptors.
        /// </summary>
        private int[] Addresses { get; set; }

        /// <summary>
        /// Instantiates a new BICCPManager.
        /// </summary>
        /// <param name="log">Logger.</param>
        public BICCPManager(ILogger log)
        {
            Addresses = new int[0x78];

            if (log == null)
                throw new ArgumentException("BICCPManager : argument log cannot be null.");
            else
                Log = log;
        }

        /// <summary>
        /// Gets a file descriptor for a specified address.
        /// </summary>
        /// <param name="address">I2C address.</param>
        /// <returns>File descriptor.</returns>
        protected virtual int GetFDFromAddress(int address)
        {
            var m = new I2CManager();

            if (address < 0x08 || address > 0x77)
                return 0;

            if (Addresses[address] == 0)
                Addresses[address] = m.I2CSetup(address, 1);

            return Addresses[address];
        }

        /// <summary>
        /// Sends a request to a module and then polls the module to get the response.
        /// </summary>
        /// <param name="addr">Module address.</param>
        /// <param name="buffer">I2C data to be sent, I2C data received after method returns.</param>
        /// <returns>Status of communication. True if response is valid, false otherwise.</returns>
        protected virtual bool RequestToModuleFromBuffer(int addr, ref byte[] buffer)
        {
            var m = new I2CManager();

            CRC16.PutCRC(ref buffer);

            LogBuffer(buffer, "I2C data sent");

            int fd = GetFDFromAddress(addr);
            int l = m.I2CWriteBlock(fd, ref buffer);

            for (int i = 0; i < ICCConstants.PACKETSIZE; i++)
                buffer[i] = 0;

            return PollModuleForResponse(addr, ref buffer);
        }

        /// <summary>
        /// Polls a module for response.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="bfrRecv">Buffer with received data after method returns.</param>
        /// <returns>Status of communication. True if response is valid, false otherwise.</returns>
        protected virtual bool PollModuleForResponse(int addr, ref byte[] bfrRecv)
        {
            var m = new I2CManager();

            bfrRecv[0] = 0xCC;
            int iCnt = 0;
            System.Threading.Thread.Sleep(10); // Needed on RPi3 because it was too fast
            int fd = GetFDFromAddress(addr);
            while ((bfrRecv[0] == 0xCC) && iCnt < 80) // 80x50ms = 4s timeout
            {
                int retVal = m.I2CReadBlock(fd, ref bfrRecv);

                if (bfrRecv[0] == 0xCC)
                {
                    // No data, waiting 50ms to retry.
                    System.Threading.Thread.Sleep(50);
                }

                iCnt++;
            }

            LogBuffer(bfrRecv, "I2C data recv");

            return (bfrRecv[0] != 0xCC);
        }

        /// <summary>
        /// Logs an I2C buffer.
        /// </summary>
        /// <param name="data">I2C buffer.</param>
        /// <param name="title">Title of the log text.</param>
        protected virtual void LogBuffer(byte[] data, string title)
        {
            Log.LogText(string.Format("{0}: {1}", title, string.Join("-", data.Select(x => x.ToString("X2")).ToArray())), LogLevel.Debug);
        }

        /// <summary>
        /// Scans the I2C bus.
        /// </summary>
        /// <param name="list">Array of bytes. Will contain status of module presence for each address (0 = no module, 1 = module found). It is mandaatory that the array has a size of 120 (0x78).</param>
        /// <returns>Quantity of modules found.</returns>
        public virtual int BICCP_ScanBus(ref byte[] list)
        {
            var m = new I2CManager();
            return m.I2CScan(1, ref list);
        }

        /// <summary>
        /// Sends a request to a module and gets the response.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <param name="data">Data to send.</param>
        /// <param name="group">Group of command to send.</param>
        /// <param name="command">Command to send.</param>
        /// <param name="datas">Response data.</param>
        /// <returns>Status of response. True if response acknowledges correctly group and command, false otherwise.</returns>
        public virtual bool RequestToModule(int addr, IBICCPData data, byte group, byte command, params byte[] datas)
        {
            var m = new I2CManager();

            int fd = GetFDFromAddress(addr);
            byte[] buffer = new byte[ICCConstants.PACKETSIZE];
            buffer[0] = group;
            buffer[1] = command;
            for (int i = 2; i < ICCConstants.PACKETSIZE; i++)
                buffer[i] = 0;

            for (int i = 0; i < datas.Length; i++)
            {
                if (i < ICCConstants.DATASIZE)
                    buffer[i + 2] = datas[i];
            }

            bool b = RequestToModuleFromBuffer(addr, ref buffer);
            if (!b)
                return false;
            else if (buffer[0] != group || buffer[1] != command)
                return false;
            else
            {
                for (int i = 0; i < ICCConstants.DATASIZE; i++)
                    data.Data[i] = buffer[i + 2];
                return true;
            }
        }
    }
}
