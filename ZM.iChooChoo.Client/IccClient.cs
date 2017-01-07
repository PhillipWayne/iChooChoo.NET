using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Actions;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Client
{
    /// <summary>
    /// iChooChoo Client provides methods to communicate with iChooChoo server.
    /// </summary>
    public partial class IccClient : IDisposable
    {
        /// <summary>
        /// Gets the iChooChoo server name or IP address.
        /// </summary>
        public virtual string iChooChoo_Server { get; private set; }

        /// <summary>
        /// Gets the iChooChoo server Command TCP port.
        /// </summary>
        public virtual int iChooChoo_CommandPort { get; private set; }

        /// <summary>
        /// Gets the iChooChoo server Log TCP port.
        /// </summary>
        public virtual int iChooChoo_LogPort { get; private set; }

        /// <summary>
        /// Gets the connection state to the server.
        /// </summary>
        public virtual bool Connected { get; private set; }

        /// <summary>
        /// Internal variable for ConnectionKeepAlive property.
        /// </summary>
        private bool _bConnectionKeepAlive;
        /// <summary>
        /// Gets or sets the KeepAlive setting. If KeepAlive is activated (default), server will be polled on a regular interval to keep connection alive.
        /// </summary>
        public virtual bool ConnectionKeepAlive
        {
            get { return _bConnectionKeepAlive; }
            set
            {
                _bConnectionKeepAlive = value;
                if (Timer != null)
                {
                    if (_bConnectionKeepAlive)
                        Timer.Start();
                    else
                        Timer.Stop();
                }
            }
        }

        /// <summary>
        /// Gets the timeout on specific server calls.
        /// </summary>
        public virtual int TimeOut { get; private set; }

        /// <summary>
        /// Gets the Modules list. Keys of the dictionary are modules addresses.
        /// </summary>
        public virtual Dictionary<int, IModule> Modules { get; private set; }

        /// <summary>
        /// Gets the last result of a module call.
        /// </summary>
        public virtual bool LastResult { get; protected set; }

        /// <summary>
        /// Gets the last error message of a module call. Returns an empty string if last call was successful.
        /// </summary>
        public virtual string LastError { get; protected set; }

        /// <summary>
        /// Timer used for ConnectionKeepAlive.
        /// </summary>
        private System.Timers.Timer Timer { get; set; }

        /// <summary>
        /// Last request timestamp, used for ConnectionKeepAlive.
        /// </summary>
        protected virtual DateTime LastRequest { get; set; }

        /// <summary>
        /// Gets the log client.
        /// </summary>
        public virtual LogClient LogClient { get; private set; }

        /// <summary>
        /// Instantiates a new iChooChoo Client.
        /// </summary>
        public IccClient(string server, int cmdPort = ICCConstants.TCP_CMD_PORT, int logPort = ICCConstants.TCP_LOG_PORT, int timeout = ICCConstants.CLIENT_DEFAULT_TIMEOUT)
        {
            iChooChoo_Server = server;
            iChooChoo_CommandPort = cmdPort;
            iChooChoo_LogPort = logPort;
            Connected = false;
            TimeOut = timeout;
            Modules = new Dictionary<int, IModule>();
            LastRequest = new DateTime(2000, 1, 1);

            LogClient = new LogClient(iChooChoo_Server, iChooChoo_LogPort);

            RefreshModulesList();
            Timer = new System.Timers.Timer() { Interval = 1000, AutoReset = false };
            Timer.Elapsed += Timer_Elapsed;
            if (Connected)
                ConnectionKeepAlive = true;
        }

        /// <summary>
        /// Timer interval handler, will poll server to keep connection alive.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event Data.</param>
        protected virtual void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Subtract(LastRequest).TotalSeconds > ICCConstants.TCP_POLL)
                Request("NOOP\n");

            Timer.Start();
        }

        /// <summary>
        /// Ensures proper disposal of the iChooChoo Client instance.
        /// </summary>
        public void Dispose()
        {
            LogClient.Dispose();
            Disconnect();
        }

        /// <summary>
        /// Reference to the client socket.
        /// </summary>
        private Socket _clientSocket;

        /// <summary>
        /// Gets the active socket.
        /// </summary>
        protected Socket ClientSocket
        {
            get
            {
                try
                {
                    // If a socket exists and is not connected, free the variable to enable a new socket creation.
                    if (_clientSocket != null)
                        if (!_clientSocket.Connected)
                        {
                            _clientSocket.Dispose();
                            _clientSocket = null;
                        }

                    // If no active socket, create a new one.
                    if (_clientSocket == null)
                    {
                        var IPs = Dns.GetHostAddresses(iChooChoo_Server);
                        if (IPs.Length > 0)
                        {
                            IPEndPoint ipEnd = new IPEndPoint(IPs[0], iChooChoo_CommandPort);
                            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            _clientSocket.Connect(ipEnd);
                            if (!_clientSocket.Connected)
                            {
                                _clientSocket.Dispose();
                                _clientSocket = null;
                                Connected = false;
                            }
                            else
                                Connected = true;
                        }
                    }

                    return _clientSocket;
                }
                catch (SocketException ex)
                {
                    if (_clientSocket != null)
                    {
                        _clientSocket.Dispose();
                        _clientSocket = null;
                    }
                    Connected = false;
                    if (Timer != null)
                        Timer.Stop();
                    throw new Exception(string.Format("iChooChoo server '{0}' on port {1} not responding", iChooChoo_Server, iChooChoo_CommandPort), ex);
                }
                catch (Exception)
                {
                    if (_clientSocket != null)
                    {
                        _clientSocket.Dispose();
                        _clientSocket = null;
                    }
                    Connected = false;
                    if (Timer != null)
                        Timer.Stop();
                    throw;
                }
            }
        }

        /// <summary>
        /// Closes connection to server.
        /// </summary>
        protected void Disconnect()
        {
            Timer.Stop();
            if (_clientSocket != null)
            {
                Request("EXIT\n");
                _clientSocket.Disconnect(false);
                _clientSocket.Dispose();
                _clientSocket = null;
            }
        }

        /// <summary>
        /// Sends a request to the iChooChoo server.
        /// </summary>
        /// <param name="sData">Request to send.</param>
        /// <returns>Response from the server.</returns>
        protected virtual string Request(string sData)
        {
            string sRecv = string.Empty;

            if (ClientSocket != null)
            {
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(sData);
                int bytesSent = ClientSocket.Send(msg, msg.Length, SocketFlags.None);
                LastRequest = DateTime.Now;

                while (!sRecv.EndsWith("\n"))
                {
                    byte[] bRecv = new byte[1024];
                    int i = ClientSocket.Receive(bRecv);

                    sRecv += Encoding.UTF8.GetString(bRecv, 0, i);
                }

            }

            return sRecv.Trim();
        }

        /// <summary>
        /// Retrieves modules list freshly from server.
        /// </summary>
        public virtual void RefreshModulesList()
        {
            LastResult = false;
            LastError = "Error retrieving modules list.";

            Modules.Clear();

            try
            {
                string sData = Request("GET_MODULELIST\n");

                if (sData.Length > 0)
                {
                    var terms = sData.Split(' ');
                    if (terms[0] == "+OK")
                    {
                        if (terms.Length > 1)
                        {
                            int iNb = int.Parse(terms[1]);
                            if (terms.Length == (iNb * 4) + 2)
                            {
                                for (int i = 0; i < iNb; i++)
                                {
                                    int iFirstArgument = (i * 4) + 2;
                                    byte bAddress = byte.Parse(terms[iFirstArgument], System.Globalization.NumberStyles.HexNumber);
                                    var iVersion = terms[iFirstArgument + 1].Split('.').Select(x => int.Parse(x)).ToArray();
                                    byte bType = byte.Parse(terms[iFirstArgument + 2], System.Globalization.NumberStyles.HexNumber);

                                    var module = ModuleFactory.CreateInstance(bType) as Module;
                                    module.ID = bAddress;
                                    module.Major = iVersion[0];
                                    module.Minor = iVersion[1];
                                    module.Build = iVersion[2];
                                    module.Description = terms[iFirstArgument + 3];
                                    module.ActionRaised += Module_ActionRaised;
                                    Modules.Add(bAddress, module);
                                }
                                LastResult = true;
                                LastError = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = string.Format("Error retrieving modules list: {0}.", ex.Message);
            }
        }

        /// <summary>
        /// Handles new actions initiated by modules.
        /// </summary>
        /// <param name="sender">Module instance that initiated the action.</param>
        /// <param name="e">EventArgs, contains action.</param>
        protected virtual void Module_ActionRaised(object sender, ActionEventArgs e)
        {
            int iCallCount = 0;

            while (iCallCount < 2)
            {
                // If a call fails because of a SocketException, maybe the connection to the server has been dropped.
                // Try a second time to reconnect to the server. If it still fails, rethrow the exception.
                try
                {
                    iCallCount++;
                    string sData = Request(e.Action.ToICCP());

                    if (sData.StartsWith("+OK"))
                    {
                        LastResult = true;
                        LastError = string.Empty;
                    }
                    else
                    {
                        LastResult = false;
                        LastError = (sData.Length > 4 ? sData.Substring(4) : "Unknown error");
                    }
                    iCallCount = 2;
                }
                catch (SocketException ex)
                {
                    LastResult = false;
                    LastError = string.Format("Error executing action: {0}.", ex.Message);
                }
                catch (Exception ex)
                {
                    LastResult = false;
                    LastError = string.Format("Error executing action: {0}.", ex.Message);
                    iCallCount = 2;
                }
            }
        }

        #region iChooChoo Protocol commands

        /// <summary>
        /// Forces iChooChoo server to rescan the I2C bus, then refreshes the modules list.
        /// </summary>
        public virtual void RefreshBus()
        {
            try
            {
                string sData = Request("DO_RESCAN\n");

                if (sData.StartsWith("+OK"))
                {
                    LastResult = true;
                    LastError = string.Empty;

                    // RefreshModulesList. If the server call fails, wait one second and retry until TimeOut is reached.
                    // This lets time to server to complete bus scan.
                    int i = 0;
                    while (i < TimeOut)
                    {
                        RefreshModulesList();
                        if (LastResult)
                            break;
                        else
                            Thread.Sleep(1000);
                        i++;
                    }
                }
                else
                {
                    LastResult = false;
                    LastError = (sData.Length > 4 ? sData.Substring(4) : "Unknown error.");
                    Modules.Clear();
                }
            }
            catch(Exception ex)
            {
                LastResult = false;
                LastError = string.Format("Error refreshing bus: {0}.", ex.Message);
            }
        }

#warning Remove GET_STATUS
        public virtual string[] GET_STATUS(int address)
        {
            string sData = Request(string.Format("GET_STATUS {0:x2}\n", address));

            try
            {
                if (sData.Length > 5)
                {
                    if (sData.Substring(0, 3) == "+OK")
                    {
                        var myData = sData.Substring(4).Split(' ');
                        if (myData.Length == 16)
                            return myData;
                    }
                }
            }
            catch { }

            throw new Exception("Invalid data received from server.");
        }

        #endregion
    }
}
