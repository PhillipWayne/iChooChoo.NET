using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.CRC;
using ZM.iChooChoo.Library.Modules;
using ZM.iChooChoo.Server.BICCP;
using ZM.iChooChoo.Server.I2C;

namespace ZM.iChooChoo.Server.Test
{
    class Program
    {
        private static Dictionary<int, Module> Modules { get; set; }

        private static int[] Addresses { get; set; }

        public static int GetFDFromAddress(int address)
        {
            var m = new I2CManager();

            if (address < 0x08 || address > 0x77)
                return 0;

            if (Addresses[address] == 0)
                Addresses[address] = m.I2CSetup(address, 1);

            return Addresses[address];
        }

        static void Main(string[] args)
        {
            Modules = new Dictionary<int, Module>();
            Addresses = new int[0x78];

            byte[] b = new byte[0x78];
            var m = new I2CManager();
            int i = m.I2CScan(1, ref b);

            Console.WriteLine(string.Format("{0} modules detected.", i));
            for (int l = 0; l < 0x78; l++)
            {
                if (b[l] != 0)
                    Console.WriteLine(string.Format("Module detected at address {0}.", l));
            }

            System.Threading.Thread.Sleep(2000);

            var answer = new BICCPData();

//            if (_BICCPManager.RequestToModule(addr, answer, ICCConstants.BICCP_GRP_CONF, ICCConstants.BICCP_CMD_CONF_VERSION) != 0)


            //int fd = GetFDFromAddress(0x08);
            //Console.WriteLine(string.Format("File descriptor : {0}", fd));

            byte[] buffer = new byte[ICCConstants.PACKETSIZE];
            buffer[0] = ICCConstants.BICCP_GRP_CONF;
            buffer[1] = ICCConstants.BICCP_CMD_CONF_VERSION;
            for (int l = 2; l < ICCConstants.PACKETSIZE; l++)
                buffer[l] = 0;

            //for (int l = 0; l < datas.Length; i++)
            //{
            //    if (i < ICCConstants.DATASIZE)
            //        buffer[l + 2] = datas[l];
            //}

            PrintBuffer(buffer, "Before call");

            int j = RequestToModuleFromBuffer(0x08, ref buffer);
            if (j == 0)
                Console.WriteLine("RequestToModuleFromBuffer returned 0.");
            else if (buffer[0] != ICCConstants.BICCP_GRP_CONF || buffer[1] != ICCConstants.BICCP_CMD_CONF_VERSION)
                Console.WriteLine("RequestToModuleFromBuffer returned wrong group or command.");
            else
            {
                Console.WriteLine("Correct return.");
                //for (int i = 0; i < ICCConstants.DATASIZE; i++)
                //    data.Data[i] = buffer[i + 2];
                //return -1;
            }


            buffer = new byte[ICCConstants.PACKETSIZE];
            buffer[0] = ICCConstants.BICCP_GRP_CONF;
            buffer[1] = ICCConstants.BICCP_CMD_CONF_IDENT;
            for (int l = 2; l < ICCConstants.PACKETSIZE; l++)
                buffer[l] = 0;

            //for (int l = 0; l < datas.Length; i++)
            //{
            //    if (i < ICCConstants.DATASIZE)
            //        buffer[l + 2] = datas[l];
            //}

            PrintBuffer(buffer, "Before call");

            j = RequestToModuleFromBuffer(0x08, ref buffer);
            if (j == 0)
                Console.WriteLine("RequestToModuleFromBuffer returned 0.");
            else if (buffer[0] != ICCConstants.BICCP_GRP_CONF || buffer[1] != ICCConstants.BICCP_CMD_CONF_IDENT)
                Console.WriteLine("RequestToModuleFromBuffer returned wrong group or command.");
            else
            {
                Console.WriteLine("Correct return.");
                //for (int i = 0; i < ICCConstants.DATASIZE; i++)
                //    data.Data[i] = buffer[i + 2];
                //return -1;
            }
        }

        public static int RequestToModuleFromBuffer(int addr, ref byte[] buffer)
        {
            var m = new I2CManager();

            CRC16.PutCRC(ref buffer);
            int fd = GetFDFromAddress(addr);
            Console.WriteLine(string.Format("RequestToModuleFromBuffer : File descriptor : {0}", fd));
            int l = m.I2CWriteBlock(fd, ref buffer);
            //int l = m.I2CWriteBlockAddr(addr, ref buffer);

            PrintBuffer(buffer, "RequestToModuleFromBuffer");

            for (int i = 0; i < ICCConstants.PACKETSIZE; i++)
                buffer[i] = 0;

            return PollModuleForResponse(addr, ref buffer);
        }

        public static int PollModuleForResponse(int addr, ref byte[] bfrRecv)
        {
            var m = new I2CManager();

            bfrRecv[0] = 0xCC;
            int iCnt = 0;
            int fd = GetFDFromAddress(addr);
            Console.WriteLine(string.Format("PollModuleForResponse : File descriptor : {0}", fd));
            while ((bfrRecv[0] == 0xCC) && iCnt < 80) // 80x50ms = 4s timeout
            {
                int retVal = m.I2CReadBlock(fd, ref bfrRecv);
                //int retVal = m.I2CReadBlockAddr(addr, ref bfrRecv);

                PrintBuffer(bfrRecv, "PollModuleForResponse");

                if (bfrRecv[0] == 0xCC)
                {
                    // No data, waiting 50ms to retry.
                    System.Threading.Thread.Sleep(50);
                }

                iCnt++;
            }

            if (bfrRecv[0] == 0xCC)
                return 0;
            else
                return -1;
        }

        public static void PrintBuffer(byte[] data, string title = "")
        {
            Console.Write(title + " : ");
            Console.WriteLine(string.Join(" - ", data));
        }
    }
}
