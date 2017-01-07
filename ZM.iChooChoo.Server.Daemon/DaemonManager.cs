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
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Log;
using ZM.iChooChoo.Server.BICCP;

namespace ZM.iChooChoo.Server.Daemon
{
    /// <summary>
    /// Class handling the Daemon mode of the iChooChoo server. It will handle the network TCP server and listen to new incoming connections.
    /// </summary>
    public class DaemonManager : IDaemonManager
    {
        /// <summary>
        /// BICCP Manager reference.
        /// </summary>
        protected virtual IBICCPManager Biccp { get; set; }

        /// <summary>
        /// Configuration Manager reference.
        /// </summary>
        protected virtual IConfManager Conf { get; set; }

        /// <summary>
        /// LogManager reference.
        /// </summary>
        protected virtual ILogger Log { get; set; }

        /// <summary>
        /// Communicator reference.
        /// </summary>
        protected virtual IBICCPCommunicator Communicator { get; set; }

        /// <summary>
        /// Instantiates a new instance of the Daemon Manager.
        /// </summary>
        /// <param name="biccp">BICCP Manager reference.</param>
        /// <param name="conf">Configuration Manager reference.</param>
        /// <param name="log">Log Manager reference.</param>
        public DaemonManager(IBICCPManager biccp, IConfManager conf, ILogger log)
        {
            if (biccp == null)
                throw new ArgumentException("DaemonManager : argument biccp cannot be null.");
            else
                Biccp = biccp;

            if (conf == null)
                throw new ArgumentException("DaemonManager : argument conf cannot be null.");
            else
                Conf = conf;

            if (log == null)
                throw new ArgumentException("DaemonManager : argument log cannot be null.");
            else
                Log = log;

            // Instantiates a new communicator to handle I2C communications
            Communicator = new BICCPCommunicator(Biccp, Conf, Log);
            Communicator.Start();

            // Register to the Configuration Manager's BusRescanned event to get notification of every new bus scan,
            // so we can take appropriate actions on new modules instances
            Conf.BusRescanned += Conf_BusRescanned;
            // Call to RegisterModules() to get notification of each change to a module, so the corresponding
            // action can be stacked to the actions list
            RegisterModules();
        }

        /// <summary>
        /// Handles the Bus Scan notification from the Configuration Manager.
        /// </summary>
        /// <param name="sender">Configuration Manager instance.</param>
        /// <param name="e">EventArgs.</param>
        protected virtual void Conf_BusRescanned(object sender, EventArgs e)
        {
            // When the bus is re-scanned, all modules instances are destroyed and new instances are created
            // from what is scanned. Need to register to notifications again.
            RegisterModules();
        }

        /// <summary>
        /// Handles the new action notification from modules.
        /// </summary>
        /// <param name="sender">Module instance sending the action.</param>
        /// <param name="e">EventArgs. Will contain the new action.</param>
        protected virtual void Modules_ActionRaised(object sender, ActionEventArgs e)
        {
            // Each time a module raises an action, stack it to the actions list
            Communicator.AddMessage(e.Action);
        }

        /// <summary>
        /// Register to new actions notifications on every module instance.
        /// </summary>
        protected virtual void RegisterModules()
        {
            foreach (var m in Conf.Modules.Values)
                m.ActionRaised += Modules_ActionRaised;
        }

        /// <summary>
        /// Initialisation steps.
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// Lauches the network TCP listener.
        /// </summary>
        /// <returns>true when terminated.</returns>
        public bool Start()
        {
            Init();

            // Initializes a TCP server and starts listening
            var serverSocket = new TcpListener(IPAddress.Any, ICCConstants.TCP_CMD_PORT);
            var clientSocket = default(TcpClient);
            serverSocket.Start();

            Log.LogText(string.Format("TCP Server low listening on port {0}", ICCConstants.TCP_CMD_PORT));

            int iSessionID = 0;

            // Endless loop waiting for new incoming connections
            while (true)
            {
                try
                {
                    // Blocking call to AcceptTcpClient will block until a new incoming connection is detected, and returns the socket reference
                    clientSocket = serverSocket.AcceptTcpClient();
                    iSessionID++;
                    Log.LogText(string.Format("Accepted connection from {0} to session {1}", (clientSocket.Client.RemoteEndPoint as IPEndPoint).Address, iSessionID));
                    var s = new ServerManager();
                    s.Client = clientSocket;
                    s.Biccp = Biccp;
                    s.Conf = Conf;
                    s.Log = Log;
                    s.Communicator = Communicator;
                    s.SessionID = iSessionID;

                    // Starts a new thread for the server manager that will handle the new client connection
                    var t = new Thread(new ThreadStart(s.Loop));
                    t.Start();
                }
                catch (Exception ex)
                {
                    Log.LogText(ex.ToString());
                    break;
                }
            }

            // Close cleanly the TCP listener
            serverSocket.Stop();
            Log.LogText("TCP Server closed");

            return true;
        }
    }
}
