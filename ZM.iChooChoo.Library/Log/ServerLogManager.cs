using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Log Manager for TCP Server, to send log entries to clients.
    /// </summary>
    public class ServerLogManager : LogManager, IServerLogManager
    {
        /// <summary>
        /// Reference to the Logger.
        /// </summary>
        protected virtual ILogger Log { get; set; }

        /// <summary>
        /// Reference to the listener thread.
        /// </summary>
        protected virtual Thread Listener { get; set; }

        /// <summary>
        /// Instantiates a new Log Server Manager.
        /// </summary>
        /// <param name="log">Reference to the active Logger.</param>
        public ServerLogManager(ILogger log) : base()
        {
            Log = log;

            Listener = new Thread(Start);
            Listener.Start();
        }

        /// <summary>
        /// Event fired when a new Log Entry is added.
        /// </summary>
        public event LogEventHandler EntryAdded;

        /// <summary>
        /// Writes a log entry to the output.
        /// </summary>
        /// <param name="entry">Log entry to be written.</param>
        /// <param name="NewLineAtEnd">If set to true, ends the current line and eventually write a newline char if supported by the underlying support.</param>
        protected override void Write(ILogEntry entry, bool NewLineAtEnd)
        {
            if (EntryAdded != null)
                EntryAdded(this, new LogEventArgs() { Entry = entry });
        }

        /// <summary>
        /// Lauches the network TCP listener.
        /// </summary>
        /// <returns>true when terminated.</returns>
        public virtual void Start()
        {
            // Initializes a TCP server and starts listening
            var serverSocket = new TcpListener(IPAddress.Any, ICCConstants.TCP_LOG_PORT);
            var clientSocket = default(TcpClient);
            serverSocket.Start();

            Log.LogText(string.Format("Log TCP Server low listening on port {0}", ICCConstants.TCP_LOG_PORT));

            int iSessionID = 0;

            // Endless loop waiting for new incoming connections
            while (true)
            {
                try
                {
                    // Blocking call to AcceptTcpClient will block until a new incoming connection is detected, and returns the socket reference
                    clientSocket = serverSocket.AcceptTcpClient();
                    iSessionID++;
                    Log.LogText(string.Format("Accepted log connection from {0} to session {1}", (clientSocket.Client.RemoteEndPoint as IPEndPoint).Address, iSessionID));
                    var l = new ServerLogManagerThread(this);
                    l.Client = clientSocket;
                    l.Log = Log;
                    l.SessionID = iSessionID;

                    // Starts a new thread for the server manager that will handle the new client connection
                    var t = new Thread(l.Loop);
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
        }
    }
}
