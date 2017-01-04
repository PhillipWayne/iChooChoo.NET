using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Interface to Dimmable outputs capable Modules.
    /// </summary>
    public interface IDimmableOutputsModule
    {
        /// <summary>
        /// Get the output value of a dimmable output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <returns>Value of the dimmable output.</returns>
        byte getDimmableOutput(int output);

        /// <summary>
        /// Set the value of a dimmable output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="value">Value to be set.</param>
        void setDimmableOutput(byte output, byte value);
    }
}
