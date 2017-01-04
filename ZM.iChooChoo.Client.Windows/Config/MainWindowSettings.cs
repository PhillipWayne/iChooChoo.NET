using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZM.iChooChoo.Client.Windows.Config
{
    /// <summary>
    /// Class containing the Main Window Settings.
    /// </summary>
    public class MainWindowSettings
    {
        /// <summary>
        /// Instantiates a new Main Window Settings.
        /// </summary>
        public MainWindowSettings()
        {
            WindowState = FormWindowState.Maximized;
            WindowTop = -8;
            WindowLeft = -8;
            WindowHeight = 400;
            WindowWidth = 700;
        }

        /// <summary>
        /// Gets or sets startup state of the main window.
        /// </summary>
        public virtual FormWindowState WindowState { get; set; }

        /// <summary>
        /// Gets or sets the top position of the main window.
        /// </summary>
        public virtual int WindowTop { get; set; }

        /// <summary>
        /// Gets or sets the left position of the main window.
        /// </summary>
        public virtual int WindowLeft { get; set; }

        /// <summary>
        /// Gets or sets the width of the main window.
        /// </summary>
        public virtual int WindowWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the main window.
        /// </summary>
        public virtual int WindowHeight { get; set; }
    }
}
