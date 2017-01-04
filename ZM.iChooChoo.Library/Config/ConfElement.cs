using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Config
{
    public class ConfElement : IConfElement
    {
        public ConfElement()
        {
        }

        public virtual string Description { get; set; }

        public virtual int ID { get; set; }
    }
}
