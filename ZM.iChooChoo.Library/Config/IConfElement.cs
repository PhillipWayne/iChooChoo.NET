using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public interface IConfElement
    {
        int ID { get; set; }
        string Description { get; set; }
    }
}
