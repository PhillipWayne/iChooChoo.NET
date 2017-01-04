using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Log
{
    /// <summary>
    /// Interface for Log Manager.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <param name="logger"></param>
        void Init(ILogger logger);
    }
}
