using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library.Config;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Event handler for Actions.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event Data.</param>
    public delegate void ActionEventHandler(object sender, ActionEventArgs e);
    
    /// <summary>
    /// Interface to Modules.
    /// </summary>
    public interface IModule : IConfElement
    {
        /// <summary>
        /// Event fired when a new action is raised by a Module.
        /// </summary>
        event ActionEventHandler ActionRaised;

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        ILogger Log { get; set; }

        /// <summary>
        /// Gets or sts the Module type.
        /// </summary>
        int Type { get; set; }

        /// <summary>
        /// Gets the Module type description.
        /// </summary>
        string TypeDescription { get; }

        /// <summary>
        /// Gets the Module type complete description.
        /// </summary>
        string TypeFullDescription { get; }

        /// <summary>
        /// Gets or sets the Major version number.
        /// </summary>
        int Major { get; set; }

        /// <summary>
        /// Gets or sets the Minor version number.
        /// </summary>
        int Minor { get; set; }

        /// <summary>
        /// Gets or sets the Build version number.
        /// </summary>
        int Build { get; set; }

        /// <summary>
        /// Gets the complete version number.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Writes the status of the module.
        /// </summary>
        /// <returns>Status of the module.</returns>
        string writeStatus();

        /// <summary>
        /// Performs a Hard reset of the Module (will revert back to factory state, as new module with address 0x77 and no type).
        /// </summary>
        void HardReset();

        /// <summary>
        /// Sets the Module's address. Will work only if the module is new with current address 0x77.
        /// </summary>
        /// <param name="address">New address.</param>
        void SetAddress(byte address);

        /// <summary>
        /// Sets the module's description.
        /// </summary>
        /// <param name="description">New description.</param>
        void SetDescription(string description);

        /// <summary>
        /// Sets the Module's type. Will work only if the module is new with current address 0x77.
        /// </summary>
        /// <param name="type">New type (see ICCConstants).</param>
        void SetType(byte type);

        /// <summary>
        /// Performs a Soft reset of the Module. The module will reload its settings. Mandatory after setting new address and type.
        /// </summary>
        void SoftReset();
    }
}
