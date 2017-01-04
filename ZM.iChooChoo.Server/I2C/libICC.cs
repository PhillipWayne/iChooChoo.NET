using System.Runtime.InteropServices;

namespace ZM.iChooChoo.Server.I2C
{
    /// <summary>
    /// Wrapper to C library handling the actual I2C communications.
    /// </summary>
    internal static class libICC
    {
        [DllImport("libICC.so", EntryPoint = "I2CRead", SetLastError = true)]
        public static extern int I2CRead(int fd);

        [DllImport("libICC.so", EntryPoint = "I2CReadAddr", SetLastError = true)]
        public static extern int I2CReadAddr(int addr);

        [DllImport("libICC.so", EntryPoint = "I2CReadBlock", SetLastError = true)]
        public static extern int I2CReadBlock(int fd, byte[] myData);

        [DllImport("libICC.so", EntryPoint = "I2CReadBlockAddr", SetLastError = true)]
        public static extern int I2CReadBlockAddr(int addr, byte[] myData);

        [DllImport("libICC.so", EntryPoint = "I2CScan", SetLastError = true)]
        public static extern int I2CScan(byte busId, byte[] data);

        [DllImport("libICC.so", EntryPoint = "I2CSetup", SetLastError = true)]
        public static extern int I2CSetup(int addr, byte busId);

        [DllImport("libICC.so", EntryPoint = "I2CWriteBlock", SetLastError = true)]
        public static extern int I2CWriteBlock(int fd, byte[] bfr);

        [DllImport("libICC.so", EntryPoint = "I2CWriteBlockAddr", SetLastError = true)]
        public static extern int I2CWriteBlockAddr(int addr, byte[] bfr);

        //[DllImport("libnativei2c.so", EntryPoint = "openBus", SetLastError = true)]
        //public static extern int OpenBus(string busFileName);

        //[DllImport("libnativei2c.so", EntryPoint = "closeBus", SetLastError = true)]
        //public static extern int CloseBus(int busHandle);

        //[DllImport("libnativei2c.so", EntryPoint = "readBytes", SetLastError = true)]
        //public static extern int ReadBytes(int busHandle, int addr, byte[] buf, int len);

        //[DllImport("libnativei2c.so", EntryPoint = "writeBytes", SetLastError = true)]
        //public static extern int WriteBytes(int busHandle, int addr, byte[] buf, int len);
    }
}