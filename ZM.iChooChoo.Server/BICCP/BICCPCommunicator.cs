using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Actions;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Server.BICCP
{
    /// <summary>
    /// Class handling communications with Modules. Handling is done in a separate thread.
    /// </summary>
    public class BICCPCommunicator : IBICCPCommunicator
    {
        /// <summary>
        /// Gets or sets the BICCP Manager.
        /// </summary>
        protected virtual IBICCPManager Biccp { get; set; }

        /// <summary>
        /// Gets or sets the Configuration Manager.
        /// </summary>
        protected virtual IConfManager Conf { get; set; }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        protected virtual ILogger Log { get; set; }

        /// <summary>
        /// Gets the Actions list. This list is managing thread synchronisation.
        /// </summary>
        protected virtual BlockingCollection<IICCAction> Actions { get; private set; }

        /// <summary>
        /// Instantiates a new BICCPCommunicator.
        /// </summary>
        /// <param name="biccp">BICCP Manager.</param>
        /// <param name="conf">Configuration Manager.</param>
        /// <param name="log">Logger.</param>
        public BICCPCommunicator(IBICCPManager biccp, IConfManager conf, ILogger log)
        {
            if (biccp == null)
                throw new ArgumentException("ConfManager : argument biccp cannot be null.");
            else
                Biccp = biccp;

            if (conf == null)
                throw new ArgumentException("ConfManager : argument conf cannot be null.");
            else
                Conf = conf;

            if (log == null)
                throw new ArgumentException("ConfManager : argument log cannot be null.");
            else
                Log = log;

            Actions = new BlockingCollection<IICCAction>();
        }

        /// <summary>
        /// Add an action to the list.
        /// </summary>
        /// <param name="action">Action to be added.</param>
        public virtual void AddMessage(IICCAction action)
        {
            Actions.Add(action);
        }

        /// <summary>
        /// Start the Communicator. Creates a new thread for BICCP communication handling.
        /// </summary>
        public virtual void Start()
        {
            var t = new Thread(new ThreadStart(Worker));
            t.Start();
        }

        /// <summary>
        /// Actual method called by the thread. Contains an endless loop to process actions detected in the list.
        /// </summary>
        private void Worker()
        {
            while (true)
            {
                var m = Actions.Take();

                lock (Conf)
                {
                    var answer = new BICCPData();
                    if (m.ToBICCP(Biccp, answer))
                        Log.LogMessage(true, string.Format("Executing action '{0}'", m.ToString()), m.Address);
                    else
                        Log.LogMessage(false, string.Format("Executing action '{0}'", m.ToString()), m.Address);
                }
            }
        }
    }
}
