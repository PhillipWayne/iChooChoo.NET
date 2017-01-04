using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Class handling communication with a client for log transmission.
    /// </summary>
    public class ServerLogManagerThread
    {
        /// <summary>
        /// Gets or sets the client socket.
        /// </summary>
        public virtual TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        public virtual ILogger Log { get; set; }

        /// <summary>
        /// Gets or sets the Server Log Manager that instantiated the current object.
        /// </summary>
        public virtual IServerLogManager Parent { get; set; }

        /// <summary>
        /// Gets or sets the Session ID.
        /// </summary>
        public virtual int SessionID { get; set; }

        /// <summary>
        /// Gets the Log Entries list. This list is managing thread synchronisation.
        /// </summary>
        protected virtual BlockingCollection<ILogEntry> Entries { get; set; }

        /// <summary>
        /// Instantiates a new Server Log Manager Thread.
        /// </summary>
        /// <param name="parent">Server Log Manager that instantiated the current object.</param>
        public ServerLogManagerThread(IServerLogManager parent)
        {
            Entries = new BlockingCollection<ILogEntry>();
            Parent = parent;
            Parent.EntryAdded += Parent_EntryAdded;
        }

        /// <summary>
        /// Event handler for new entries.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event Data.</param>
        private void Parent_EntryAdded(object sender, LogEventArgs e)
        {
            Entries.Add(e.Entry);
        }

        /// <summary>
        /// Disconnects cleanly from the client.
        /// </summary>
        protected virtual void Disconnect()
        {
            Parent.EntryAdded -= Parent_EntryAdded;
            if (Client != null)
                Client.Close();
        }

        /// <summary>
        /// Loop managing the client connection and communication.
        /// </summary>
        public virtual void Loop()
        {
            var networkStream = Client.GetStream();

            var sData = string.Empty;
            bool bExiting = false;

            // Loop while thread is alive and Existing property is false.
            while (Thread.CurrentThread.IsAlive && !bExiting)
            {
                try
                {
                    // Get data from the stream, we are waiting for a HELO command to identify the client.
                    while (!networkStream.DataAvailable) ;
                    byte[] bytesFrom = new byte[Client.Available];
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    sData += System.Text.Encoding.ASCII.GetString(bytesFrom);

                    if (sData[sData.Length - 1] == ICCConstants.EOL)
                    {
                        sData = sData.Trim();

                        if (sData.Length > 0)
                        {
#if DEBUG
                            Log.LogText(string.Format("LogS Recv ({0}): {1}", SessionID, sData), LogLevel.Debug);
#endif

                            string serverResponse = string.Empty;
                            var sArray = sData.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                            if (sArray.Length == 2)
                            {
                                if (sArray[0] == "HELO")
                                {
                                    // We received the HELO command, send +OK to acknowledge.
                                    SendMessage(networkStream, "+OK");
                                    // Now loop while the current thread is alive to send log entries and wait for acknowledgement from the client.
                                    while (Thread.CurrentThread.IsAlive)
                                    {
                                        try
                                        {
                                            // When we have a new log entry in the list, take it and sent it in JSon format.
                                            var m = Entries.Take();
                                            SendMessage(networkStream, fastJSON.JSON.ToJSON(m, new fastJSON.JSONParameters() { UseExtensions = false }));

                                            // Wait for "+OK" acknowledgement from client.
                                            var sData2 = string.Empty;
                                            while (Thread.CurrentThread.IsAlive)
                                            {
                                                while (!networkStream.DataAvailable) ;
                                                byte[] bytesFrom2 = new byte[Client.Available];
                                                networkStream.Read(bytesFrom2, 0, bytesFrom2.Length);
                                                sData2 += System.Text.Encoding.ASCII.GetString(bytesFrom2);

                                                if (sData2[sData2.Length - 1] == ICCConstants.EOL)
                                                {
                                                    sData2 = sData2.Trim();
                                                    if (sData2 == "+OK")
                                                        break;
                                                    else
                                                        throw new ApplicationException(string.Format("Error acknoledgement in Log TCP Session {0}, closing.", SessionID));
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Disconnect();
                                            bExiting = true;
                                            Log.LogText(ex.Message);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Log.LogText(string.Format("Wrong command received on Log TCP Session {0}, closing.", SessionID), LogLevel.Error);
                                    SendMessage(networkStream, "-KO");
                                    break;
                                }
                            }
                            else
                            {
                                Log.LogText(string.Format("Wrong command received on Log TCP Session {0}, closing.", SessionID), LogLevel.Error);
                                SendMessage(networkStream, "-KO");
                                break;
                            }

                            sData = string.Empty;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.LogText(ex.ToString());
                    break;
                }

            }

            Disconnect();
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
            // Don't log sent data to avoid "mirror" effect and endless logging :)
            //Log.LogText(string.Format("LogS Sent ({0}): {1}", SessionID, message), LogLevel.Debug);
        }
    }
}
