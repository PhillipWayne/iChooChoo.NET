using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Interface to General Purpose Modules.
    /// </summary>
    public interface IGenPurpModule : IOnOffOutputsModule
    {
        /// <summary>
        /// Outputs values.
        /// </summary>
        bool[] Outputs { get; }
    }
}
