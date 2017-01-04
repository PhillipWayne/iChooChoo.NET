using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library.Actions;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Represents a General Purpose Module.
    /// </summary>
    public class GenPurpModule : Module, IGenPurpModule
    {
        /// <summary>
        /// Outputs values.
        /// </summary>
        public bool[] Outputs { get; private set; }

        /// <summary>
        /// Instantiates a new General Purpose Module.
        /// </summary>
        public GenPurpModule() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("GenPurpModule()");         
#endif
            Type = ICCConstants.BICCP_GRP_GENPURP;
            TypeDescription = ICCConstants.GetTypeDescription((byte)Type);
            Outputs = new bool[16];
        }

        /// <summary>
        /// Get the output value of an On/Off output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <returns>Value of the output.</returns>
        public virtual bool getOutput(int output)
        {
            if (output >= 0 && output < 16)
                return Outputs[output];
            else
                throw new ArgumentOutOfRangeException("GenPurpModule: Output can only be 0 to 15.");
        }

        /// <summary>
        /// Set the value of an On/Off output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="value">Value to be set.</param>
        public virtual void setOutput(byte output, bool value)
        {
            if (output >= 0 && output < 16)
            {
                Outputs[output] = value;
                var a = new ICCActionGenPurpSetOutput() { Address = (byte)ID, Log = Log };
                a.Data = new object[] { output, value };
                RaiseAction(a);
            }
            else
                throw new ArgumentOutOfRangeException("GenPurpModule: Output can only be 0 to 15.");
        }

        /// <summary>
        /// Writes the status of the module.
        /// </summary>
        /// <returns>Status of the module.</returns>
        public override string writeStatus()
        {
            string s = string.Empty;

            for (int i = 0; i < 16; i++)
            {
                s += string.Format(" {0}", Outputs[i] ? 1 : 0);
            }

            return s;
        }
    }
}
