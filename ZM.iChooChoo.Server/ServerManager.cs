using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Actions;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Log;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Server
{
    /// <summary>
    /// Class handling a connection with a client. An instance is required for each client.
    /// </summary>
    public class ServerManager : IServerManager
    {
        /// <summary>
        /// Gets or sets the client socket.
        /// </summary>
        public virtual TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets the BICCP Manager.
        /// </summary>
        public virtual IBICCPManager Biccp { get; set; }

        /// <summary>
        /// Gets or sets the Configuration Manager.
        /// </summary>
        public virtual IConfManager Conf { get; set; }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        public virtual ILogger Log { get; set; }

        /// <summary>
        /// Gets or sets the BICCP Communicator.
        /// </summary>
        public virtual IBICCPCommunicator Communicator { get; set; }

        /// <summary>
        /// Gets or sets the Session ID.
        /// </summary>
        public virtual int SessionID { get; set; }

        /// <summary>
        /// Gets or sets the exiting status. If true, the connection will be closed and the instance will terminate.
        /// </summary>
        protected virtual bool Exiting { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last communication with the client. Used to track the awakeness of the connection.
        /// </summary>
        protected virtual DateTime LastRequest { get; set; }

        /// <summary>
        /// Instantiates a new Server Manager.
        /// </summary>
        public ServerManager()
        {
            Exiting = false;
            LastRequest = DateTime.Now;
        }

        /// <summary>
        /// Loop managing the client connection and communication.
        /// </summary>
        public virtual void Loop()
        {
            LastRequest = DateTime.Now;

            var networkStream = Client.GetStream();

            var sData = string.Empty;

            // Loop while thread is alive and Existing property is false.
            while (Thread.CurrentThread.IsAlive && Client.Connected && !Exiting)
            {
                try
                {
                    // Loop until we get data in the stream, or Exiting becomes true (if nothing received since more than the timeout).
                    while (!networkStream.DataAvailable && !Exiting)
                    {
                        if (DateTime.Now.Subtract(LastRequest).TotalSeconds > ICCConstants.TCP_TIMEOUT)
                            Exiting = true;
                    }

                    // If not exiting, receive the data and process the message.
                    if (!Exiting)
                    {
                        byte[] bytesFrom = new byte[Client.Available];
                        networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                        sData += System.Text.Encoding.ASCII.GetString(bytesFrom);

                        if (sData[sData.Length - 1] == ICCConstants.EOL)
                        {
                            sData = sData.Trim();

                            if (sData.Length > 0)
                            {
                                Log.LogText(string.Format("Recv ({0}): {1}", SessionID, sData), LogLevel.Debug);

                                // Update the timestamp of last data received, process the data, then send the response to the client.
                                LastRequest = DateTime.Now;
                                string serverResponse = ProcessMessage(sData);
                                SendMessage(networkStream, serverResponse);

                                // If the client sent "QUIT", actually end the connection.
                                if (sData.ToUpper() == "QUIT")
                                    break;

                                sData = string.Empty;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.LogText(ex.ToString());
                    break;
                }
            }

            Client.Close();

            Log.LogText(string.Format("Closed ({0}): client disconnected", SessionID));
        }

        /// <summary>
        /// Sends a message thru the TCP connection.
        /// </summary>
        /// <param name="ns">Network Stream to use.</param>
        /// <param name="message">Message to be sent.</param>
        protected virtual void SendMessage(NetworkStream ns, string message)
        {
            byte[] sendBytes = Encoding.ASCII.GetBytes(message + '\n');
            ns.Write(sendBytes, 0, sendBytes.Length);
            ns.Flush();
            Log.LogText(string.Format("Sent ({0}): {1}", SessionID, message), LogLevel.Debug);
        }

        //protected virtual string CheckTerms(string[] terms, int ArgsCount, ref int iAddress, params Type[] types)
        //{
        //    string s = "+OK";

        //    if (terms.Length != ArgsCount)
        //        s = string.Format("-KO Wrong command format, {0} takes {1} argument(s)", terms[0], ArgsCount);
        //    else if (!int.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out iAddress))
        //        s = string.Format("-KO Error parsing address {0}", terms[1]);
        //    else if (iAddress < ICCConstants.ADDR_MIN || iAddress > ICCConstants.ADDR_MAX)
        //        s = string.Format("-KO Wrong address {0:x2}", iAddress);
        //    else
        //    {
        //        for (int i = 0; i < types.Length && s[0] == '+'; i++)
        //        {
        //            if (types[i] == typeof(int))
        //            {
        //                int iTest = 0;
        //                if (!int.TryParse(terms[i + 2], out iTest))
        //                    s = string.Format("-KO Error parsing argument {0}, expected int for value {1}", i + 2, terms[i + 2]);
        //            }
        //            else if (types[i] == typeof(byte))
        //            {
        //                byte bTest = 0;
        //                if (!byte.TryParse(terms[i + 2], out bTest))
        //                    s = string.Format("-KO Error parsing argument {0}, expected byte for value {1}", i + 2, terms[i + 2]);
        //            }
        //            else if (types[i] != typeof(string))
        //                throw new ArgumentException(string.Format("ServerManager.CheckTerms : type {0} not supported.", types[i].Name));
        //        }
        //    }

        //    return s;
        //}

        #region Proceed
        
        private string Proceed_DO_HARDRESET(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;

            if (terms.Length != 2)
                sReturn = string.Format("-KO Wrong command format, {0} takes 1 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as IModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No module at address {0:x2}", bAddr);
                    else
                    {
                        mod.HardReset();
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_DO_RESCAN(string[] terms)
        {
            string sReturn = string.Empty;

            if (terms.Length != 1)
                sReturn = string.Format("-KO Wrong command format, {0} takes 0 argument(s)", terms[0]);
            else
            {
                lock (Conf)
                {
                    Conf.RescanPending = true;
                }
                Communicator.AddMessage(new ICCActionConfScanBus() { Address = 0x00, Conf = Conf });
                sReturn = "+OK";
            }

            return sReturn;
        }

        private string Proceed_DO_SCENSTART(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;
            byte bAction = 0xFF;
            int iTmp;

            for (int i = 3; i < terms.Length; i++)
            {
                if (!int.TryParse(terms[i], NumberStyles.Integer, new NumberFormatInfo(), out iTmp))
                    sReturn = terms[i];
            }

            if (sReturn != string.Empty)
                sReturn = string.Format("-KO Error parsing action arguments, expecting integer value for '{0}'", sReturn);
            else if (terms.Length < 4)
                sReturn = string.Format("-KO Wrong command format, action takes at least 3 argument(s)");
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!byte.TryParse(terms[2], NumberStyles.HexNumber, new NumberFormatInfo(), out bAction))
                sReturn = string.Format("-KO Error parsing action ID {0}", terms[2]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as IScenariosModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No scenarios capable module at address {0:x2}", bAddr);
                    else
                    {
                        var args = terms.ToList().GetRange(3, terms.Length - 3).Select(x => int.Parse(x)).ToArray();
                        mod.StartScenario(bAction, args);
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_DO_SCENSTOP(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;
            byte bOutput = 0xFF;

            if (terms.Length != 3)
                sReturn = string.Format("-KO Wrong command format, stop action takes 2 argument(s)");
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!byte.TryParse(terms[2], NumberStyles.HexNumber, new NumberFormatInfo(), out bOutput))
                sReturn = string.Format("-KO Error parsing output {0}", terms[2]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as IScenariosModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No scenarios capable module at address {0:x2}", bAddr);
                    else
                    {
                        mod.StopScenario(bOutput);
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_DO_SETDIMOUT(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;
            byte bOutput = 0xFF;
            byte bValue = 0;

            if (terms.Length != 4)
                sReturn = string.Format("-KO Wrong command format, {0} takes 3 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!byte.TryParse(terms[2], NumberStyles.HexNumber, new NumberFormatInfo(), out bOutput))
                sReturn = string.Format("-KO Error parsing output {0}", terms[2]);
            else if (!byte.TryParse(terms[3], NumberStyles.Integer, new NumberFormatInfo(), out bValue))
                sReturn = string.Format("-KO Error parsing value {0}", terms[3]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as IDimmableOutputsModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No module at address {0:x2}", bAddr);
                    else
                    {
                        mod.setDimmableOutput(bOutput, bValue);
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_DO_SETOUT(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;
            byte bOutput = 0xFF;
            byte bValue = 0;

            if (terms.Length != 4)
                sReturn = string.Format("-KO Wrong command format, {0} takes 3 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!byte.TryParse(terms[2], NumberStyles.HexNumber, new NumberFormatInfo(), out bOutput))
                sReturn = string.Format("-KO Error parsing output {0}", terms[2]);
            else if (!byte.TryParse(terms[3], NumberStyles.Integer, new NumberFormatInfo(), out bValue))
                sReturn = string.Format("-KO Error parsing value {0}", terms[3]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as IOnOffOutputsModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No module at address {0:x2}", bAddr);
                    else
                    {
                        mod.setOutput(bOutput, (bValue != 0));
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_DO_SOFTRESET(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;

            if (terms.Length != 2)
                sReturn = string.Format("-KO Wrong command format, {0} takes 1 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as IModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No module at address {0:x2}", bAddr);
                    else
                    {
                        mod.SoftReset();
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_GET_MODULE(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;

            if (terms.Length != 2)
                sReturn = string.Format("-KO Wrong command format, {0} takes 1 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr];
                    if (mod == null)
                        sReturn = string.Format("-KO No module at address {0:x2}", bAddr);
                    else
                        sReturn = string.Format("+OK 1 {0:x2} {1}.{2}.{3} {4:x2} {5}", bAddr, mod.Major, mod.Minor, mod.Build, mod.Type, (mod.Description == string.Empty ? "?" : mod.Description));
                }
            }

            return sReturn;
        }

        private string Proceed_GET_MODULELIST(string[] terms)
        {
            string sReturn = string.Empty;

            if (terms.Length != 1)
                sReturn = string.Format("-KO Wrong command format, {0} takes 0 argument(s)", terms[0]);
            else
            {
                lock (Conf)
                {
                    if (Conf.RescanPending)
                        sReturn = "-KO Please wait until bus has been rescanned";
                    else
                        sReturn = string.Format("+OK {0} {1}", Conf.Modules.Count,
                            string.Join(" ", Conf.Modules.Select(x => string.Format("{0:x2} {1}.{2}.{3} {4:x2} {5}", x.Value.ID, x.Value.Major, x.Value.Minor, x.Value.Build, x.Value.Type, (x.Value.Description == string.Empty ? "?" : x.Value.Description))).ToArray())
                            );
                }
            }

            return sReturn;
        }

        private string Proceed_GET_STATUS(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;

            if (terms.Length != 2)
                sReturn = string.Format("-KO Wrong command format, {0} takes 1 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as Module;
                    if (mod == null)
                        sReturn = string.Format("-KO No module at address {0:x2}", bAddr);
                    else
                        sReturn = string.Format("+OK {0}", mod.writeStatus().Trim());
                }
            }

            return sReturn;
        }

        private string Proceed_SET_ADDR(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;

            if (terms.Length != 2)
                sReturn = string.Format("-KO Wrong command format, {0} takes 1 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[0x77] as IModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No new module at address 0x77");
                    else
                    {
                        mod.SetAddress(bAddr);
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_SET_DESC(string[] terms)
        {
            string sReturn = string.Empty;
            byte bAddr = 0xFF;

            if (terms.Length != 3)
                sReturn = string.Format("-KO Wrong command format, {0} takes 2 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bAddr))
                sReturn = string.Format("-KO Error parsing address {0}", terms[1]);
            else if (!Module.IsAddressValid(bAddr))
                sReturn = string.Format("-KO Wrong address {0:x2}", bAddr);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[bAddr] as IModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No module at address {0:x2}", bAddr);
                    else
                    {
                        mod.SetDescription(terms[2]);
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        private string Proceed_SET_TYPE(string[] terms)
        {
            string sReturn = string.Empty;
            byte bType = 0;

            if (terms.Length != 2)
                sReturn = string.Format("-KO Wrong command format, {0} takes 1 argument(s)", terms[0]);
            else if (!byte.TryParse(terms[1], NumberStyles.HexNumber, new NumberFormatInfo(), out bType))
                sReturn = string.Format("-KO Error parsing type {0}", terms[1]);
            else if (!ICCConstants.CheckIfTypeExists(bType))
                sReturn = string.Format("-KO Wrong type {0:x2}", bType);
            else
            {
                lock (Conf)
                {
                    var mod = Conf.Modules[0x77] as IModule;
                    if (mod == null)
                        sReturn = string.Format("-KO No new module at address 0x77");
                    else
                    {
                        mod.SetType(bType);
                        sReturn = "+OK";
                    }
                }
            }

            return sReturn;
        }

        #endregion

        /// <summary>
        /// Process a message received from the client. Does actually call sub methods "Proceed_{COMMAND}".
        /// </summary>
        /// <param name="message">Message received.</param>
        /// <returns>Response to send back to the client.</returns>
        protected virtual string ProcessMessage(string message)
        {
            var sReturn = string.Empty;
            var nfi = new System.Globalization.NumberFormatInfo();

            if (message == null)
                return "-KO Empty message";
            if (message.Trim().Length == 0)
                return "-KO Empty message";

            try
            {
                var terms = message.Trim().Split(' ');

                switch (terms[0])
                {
                    case "NOOP":
                        sReturn = "+OK";
                        break;
                    case "EXIT":
                        sReturn = "+OK";
                        Exiting = true;
                        break;
                    case "DO_HARDRESET":
                        sReturn = Proceed_DO_HARDRESET(terms);
                        break;
                    case "DO_RESCAN":
                        sReturn = Proceed_DO_RESCAN(terms);
                        break;
                    case "DO_SCENSTART":
                        sReturn = Proceed_DO_SCENSTART(terms);
                        break;
                    case "DO_SCENSTOP":
                        sReturn = Proceed_DO_SCENSTOP(terms);
                        break;
                    case "DO_SETDIMOUT":
                        sReturn = Proceed_DO_SETDIMOUT(terms);
                        break;
                    case "DO_SETOUT":
                        sReturn = Proceed_DO_SETOUT(terms);
                        break;
                    case "DO_SOFTRESET":
                        sReturn = Proceed_DO_SOFTRESET(terms);
                        break;
                    case "GET_MODULE":
                        sReturn = Proceed_GET_MODULE(terms);
                        break;
                    case "GET_MODULELIST":
                        sReturn = Proceed_GET_MODULELIST(terms);
                        break;
                    case "GET_STATUS":
                        sReturn = Proceed_GET_STATUS(terms);
                        break;
                    case "SET_ADDR":
                        sReturn = Proceed_SET_ADDR(terms);
                        break;
                    case "SET_DESC":
                        sReturn = Proceed_SET_DESC(terms);
                        break;
                    case "SET_TYPE":
                        sReturn = Proceed_SET_TYPE(terms);
                        break;
                    default:
                        sReturn = "-KO Unknown command";
                        break;
                }
            }
            catch (Exception ex)
            {
                sReturn = string.Format("-KO {0}", ex.ToString().Replace("\r", "").Replace("\n", ", "));
            }

            return sReturn;
        }
    }
}
