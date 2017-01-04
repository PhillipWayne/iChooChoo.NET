using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Client
{
    /// <summary>
    /// Class handling the reception of log from the server.
    /// </summary>
    public class LogClient : IDisposable
    {
        /// <summary>
        /// Gets the iChooChoo server name or IP address.
        /// </summary>
        public virtual string iChooChoo_Server { get; private set; }

        /// <summary>
        /// Gets the iChooChoo server Log TCP port.
        /// </summary>
        public virtual int iChooChoo_LogPort { get; private set; }

        /// <summary>
        /// Gets the connection state to the server.
        /// </summary>
        public virtual bool Connected { get; private set; }

        /// <summary>
        /// Reference to the thread managing the log server connection.
        /// </summary>
        private Thread t { get; set; }

        /// <summary>
        /// Instantiates a new iChooChoo Log Client.
        /// </summary>
        public LogClient(string server, int logPort = ICCConstants.TCP_LOG_PORT)
        {
            iChooChoo_Server = server;
            iChooChoo_LogPort = logPort;
            Connected = false;

            t = new Thread(Loop);
            t.Start();
        }

        /// <summary>
        /// Ensures proper disposal of the iChooChoo Client instance.
        /// </summary>
        public void Dispose()
        {
            t.Abort();
            Disconnect();
        }

        /// <summary>
        /// Reference to the socket.
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
                    if (_clientSocket != null)
                        if (!_clientSocket.Connected)
                        {
                            _clientSocket.Dispose();
                            _clientSocket = null;
                        }

                    if (_clientSocket == null)
                    {
                        var IPs = Dns.GetHostAddresses(iChooChoo_Server);
                        if (IPs.Length > 0)
                        {
                            IPEndPoint ipEnd = new IPEndPoint(IPs[0], iChooChoo_LogPort);
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
                    throw new Exception(string.Format("iChooChoo Log server '{0}' on port {1} not responding", iChooChoo_Server, iChooChoo_LogPort), ex);
                }
                catch (Exception ex)
                {
                    if (_clientSocket != null)
                    {
                        _clientSocket.Dispose();
                        _clientSocket = null;
                    }
                    Connected = false;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Closes connection to server.
        /// </summary>
        protected void Disconnect()
        {
            if (_clientSocket != null)
            {
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
        protected virtual void Loop()
        {
            if (ClientSocket != null)
            {
                Send("HELO 0\n");

                string sRecv = Receive();
                if (sRecv == "+OK")
                {
                    while (Thread.CurrentThread.IsAlive)
                    {
                        string s = Receive();
                        var entry = fastJSON.JSON.ToObject<LogEntry>(s);
                        ReceiveEntry(entry);
                        Send("+OK\n");
                    }
                }
                else
                    throw new ApplicationException("Log Server acknowledgement error.");
            }
        }

        /// <summary>
        /// Send data thru TCP connection.
        /// </summary>
        /// <param name="s">String to be sent.</param>
        protected virtual void Send(string s)
        {
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(s);
            int bytesSent = ClientSocket.Send(msg, msg.Length, SocketFlags.None);
        }

        /// <summary>
        /// Receive data thru TCP connection.
        /// </summary>
        /// <returns>Data received.</returns>
        protected virtual string Receive()
        {
            string sRecv = string.Empty;

            while (!sRecv.EndsWith("\n"))
            {
                byte[] bRecv = new byte[1024];
                int i = ClientSocket.Receive(bRecv);

                sRecv += Encoding.UTF8.GetString(bRecv, 0, i);
            }

            return sRecv.Trim();
        }

        /// <summary>
        /// Event fired when a new log entry is received from the server. Beware that this event is fired by another thread. Be sure to check for InvokeRequired and Invoke/BeginInvoke if necessary.
        /// </summary>
        public virtual event LogEventHandler EntryReceived;

        /// <summary>
        /// Fire the EntryReceived event and sent the Log Entry.
        /// </summary>
        /// <param name="entry">LogEntry received.</param>
        protected virtual void ReceiveEntry(ILogEntry entry)
        {
            if (EntryReceived != null)
                EntryReceived(this, new LogEventArgs() { Entry = entry });
        }
    }
}
