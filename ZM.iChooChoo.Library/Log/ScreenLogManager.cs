using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Log Manager for on screen rendering.
    /// </summary>
    public class ScreenLogManager : LogManager, IScreenLogManager
    {
        /// <summary>
        /// Instantiates a new ScreenLogManager.
        /// </summary>
        public ScreenLogManager() : base()
        {

        }

        /// <summary>
        /// Writes a log entry to the output.
        /// </summary>
        /// <param name="entry">Log entry to be written.</param>
        /// <param name="NewLineAtEnd">If set to true, ends the current line and eventually write a newline char if supported by the underlying support.</param>
        protected override void Write(ILogEntry entry, bool NewLineAtEnd)
        {
            if (NewLineAtEnd)
                Console.WriteLine(entry.ToString());
            else
                Console.Write(entry.ToString());
        }
    }
}
