using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.BICCP;

namespace ZM.iChooChoo.Server.BICCP
{
    /// <summary>
    /// Class containing BICCP data.
    /// </summary>
    public class BICCPData : IBICCPData
    {
        /// <summary>
        /// Instantiates a new BICCPData.
        /// </summary>
        public BICCPData()
        {
            Data = new byte[ICCConstants.DATASIZE];
        }

        /// <summary>
        /// I2C Data.
        /// </summary>
        public virtual byte[] Data { get; set; }
    }
}
