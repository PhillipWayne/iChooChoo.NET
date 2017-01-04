using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.BICCP
{
    /// <summary>
    /// Interface for BICCP Data.
    /// </summary>
    public interface IBICCPData
    {
        /// <summary>
        /// I2C Data.
        /// </summary>
        byte[] Data { get; set; }
    }
}
