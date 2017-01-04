using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Config;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Library
{
    /// <summary>
    /// Interface for Configuration Manager.
    /// </summary>
    public interface IConfManager
    {
        /// <summary>
        /// Event fired when the I2C bus is rescanned.
        /// </summary>
        event EventHandler BusRescanned;

        /// <summary>
        /// Reads the configuration file.
        /// </summary>
        /// <param name="path">File name and path.</param>
        /// <returns>True on success, false otherwise.</returns>
        bool ReadConf(string path);

        /// <summary>
        /// Display configuration on standard output.
        /// </summary>
        /// <param name="s">Stream on which output is sent.</param>
        void Display(StreamWriter s);

        /// <summary>
        /// Scans the I2C bus.
        /// </summary>
        /// <returns>QUantity of modules scanned.</returns>
        int ScanBus();

        /// <summary>
        /// Gets or sets if a bus rescan is pending.
        /// </summary>
        bool RescanPending { get; set; }

        /// <summary>
        /// List of modules referenced by their address.
        /// </summary>
        Dictionary<byte, IModule> Modules { get; }

        /// <summary>
        /// List of positions referenced by their ID.
        /// </summary>
        Dictionary<int, ConfPosition> Positions { get; }

        /// <summary>
        /// List of relays referenced by their ID.
        /// </summary>
        Dictionary<int, ConfRelay> Relays { get; }

        /// <summary>
        /// List of sections referenced by their ID.
        /// </summary>
        Dictionary<int, ConfSection> Sections { get; }

        /// <summary>
        /// List of sensors referenced by their ID.
        /// </summary>
        Dictionary<int, ConfSensor> Sensors { get; }

        /// <summary>
        /// List of switches referenced by their ID.
        /// </summary>
        Dictionary<int, ConfSwitch> Switchs { get; }
    }
}
