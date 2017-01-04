using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Interface to Scenario capable Modules.
    /// </summary>
    public interface IScenariosModule
    {
        /// <summary>
        /// Start a lighting scenario. Use preferably direct scenario methods that provide a clear explanation of parameters.
        /// </summary>
        /// <param name="action">Scenario ID (see ICCConstants).</param>
        /// <param name="args">Arguments to be passed to the scenario.</param>
        void StartScenario(byte action, params int[] args);

        /// <summary>
        /// Stop a lighting scenario.
        /// </summary>
        /// <param name="output">One of the output pins used by the scenario to be stopped.</param>
        void StopScenario(byte output);
    }
}
