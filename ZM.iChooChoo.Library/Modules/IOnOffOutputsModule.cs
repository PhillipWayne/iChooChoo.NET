using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Interface to On/Off Outputs capable modules.
    /// </summary>
    public interface IOnOffOutputsModule
    {
        /// <summary>
        /// Get the output value of an On/Off output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <returns>Value of the output.</returns>
        bool getOutput(int output);

        /// <summary>
        /// Set the value of an On/Off output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="value">Value to be set.</param>
        void setOutput(byte output, bool value);
    }
}
