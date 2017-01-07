using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library.Actions;
using ZM.iChooChoo.Library.Config;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Represents a Module. This abstract class cannot be instantiated. This class is the base class for all types of Modules.
    /// </summary>
    public abstract class Module : ConfElement, IModule
    {
        /// <summary>
        /// Instantiates a new Module.
        /// </summary>
        public Module()
        {
#if VERBOSEDEBUG
   Console.Writeline("Module()");         
#endif
            Type = ICCConstants.BICCP_GRP_UNKNOWN;
            Description = string.Empty;
            TypeDescription = "(unknown)";
        }

        /// <summary>
        /// Gets or sets the Major version number.
        /// </summary>
        public virtual int Major { get; set; }

        /// <summary>
        /// Gets or sets the Minor version number.
        /// </summary>
        public virtual int Minor { get; set; }

        /// <summary>
        /// Gets or sets the Build version number.
        /// </summary>
        public virtual int Build { get; set; }

        /// <summary>
        /// Gets or sts the Module type.
        /// </summary>
        public virtual int Type { get; set; }

        /// <summary>
        /// Gets the complete version number.
        /// </summary>
        public virtual string Version
        {
            get { return string.Format("{0}.{1}.{2}", Major, Minor, Build); }
        }

        /// <summary>
        /// Gets the icon of the module.
        /// </summary>
        public virtual Image Icon { get { return (Resources.ResourceManager.GetObject(string.Format("ModuleType{0:X2}", this.Type)) as System.Drawing.Icon).ToBitmap(); } }

        /// <summary>
        /// Gets the Module type description.
        /// </summary>
        public virtual string TypeDescription { get; protected set; }

        /// <summary>
        /// Gets the Module type complete description.
        /// </summary>
        public virtual string TypeFullDescription
        {
            get { return string.Format("0x{0:X2} - {1}", Type, TypeDescription); }
        }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        public virtual ILogger Log { get; set; }

        /// <summary>
        /// Actual handler storing subscribers
        /// </summary>
        protected virtual event ActionEventHandler _actionRaised;
        /// <summary>
        /// Event fired when a new action is raised by a Module.
        /// </summary>
        public virtual event ActionEventHandler ActionRaised
        {
            // Prevents a delegate to subscribe to the same event twice
            add
            {
                bool exists = false;
                if (_actionRaised != null)
                {
                    foreach (Delegate existingHandler in _actionRaised.GetInvocationList())
                    {
                        if (Delegate.Equals(existingHandler, value))
                            exists = true;
                    }
                }
                if (!exists)
                    _actionRaised += value;
            }
            remove { _actionRaised -= value; }
        }

        /// <summary>
        /// Raises an action.
        /// </summary>
        /// <param name="a">Action.</param>
        protected virtual void RaiseAction(IICCAction a)
        {
            if (_actionRaised != null)
                _actionRaised(this, new ActionEventArgs() { Action = a });
        }

        /// <summary>
        /// Writes the status of the module.
        /// </summary>
        /// <returns>Status of the module.</returns>
        public virtual string writeStatus()
        {
            throw new NotImplementedException("Module.writeStatus");
        }

        /// <summary>
        /// Returns a string representing this object.
        /// </summary>
        /// <returns>String representing this object.</returns>
        public override string ToString()
        {
            return string.Format("0x{0:X2} - {1} (0x{2:X2} - {3} - v{4}.{5}.{6})", ID, Description, Type, TypeDescription, Major, Minor, Build);
        }

        /// <summary>
        /// Checks if an address is valid.
        /// </summary>
        /// <param name="address">Address to be checked.</param>
        /// <returns>True if the address is valid, false otherwise.</returns>
        public static bool IsAddressValid(byte address)
        {
            return (address >= ICCConstants.ADDR_MIN && address < ICCConstants.ADDR_MAX);
        }

        #region Commands

        /// <summary>
        /// Performs a Hard reset of the Module (will revert back to factory state, as new module with address 0x77 and no type).
        /// </summary>
        public void HardReset()
        {
            var a = new ICCActionConfHardReset() { Address = (byte)ID, Log = Log };
            RaiseAction(a);
        }

        /// <summary>
        /// Sets the Module's address. Will work only if the module is new with current address 0x77.
        /// </summary>
        /// <param name="address">New address.</param>
        public void SetAddress(byte address)
        {
            if (ID != 0x77)
                throw new ArgumentException("Module.SetAddress: can execute only on new module with address 0x77.");

            var a = new ICCActionConfSetAddress() { Address = (byte)ID, Log = Log, Data = new object[] { address } };
            RaiseAction(a);
        }

        /// <summary>
        /// Sets the module's description.
        /// </summary>
        /// <param name="description">New description.</param>
        public void SetDescription(string description)
        {
            if (description == null)
                throw new ArgumentException("SetDescription : argument description cannot be null.");

            var a = new ICCActionConfSetDescription() { Address = (byte)ID, Log = Log, Data = new object[] { description.Replace(' ', '-') } };
            RaiseAction(a);
        }

        /// <summary>
        /// Sets the Module's type. Will work only if the module is new with current address 0x77.
        /// </summary>
        /// <param name="type">New type (see ICCConstants).</param>
        public void SetType(byte type)
        {
            if (ID != 0x77)
                throw new ArgumentException("Module.SetType: can execute only on new module with address 0x77.");

            var a = new ICCActionConfSetType() { Address = (byte)ID, Log = Log, Data = new object[] { type } };
            RaiseAction(a);
        }

        /// <summary>
        /// Performs a Soft reset of the Module. The module will reload its settings. Mandatory after setting new address and type.
        /// </summary>
        public void SoftReset()
        {
            var a = new ICCActionConfSoftReset() { Address = (byte)ID, Log = Log };
            RaiseAction(a);
        }

        #endregion
    }
}
