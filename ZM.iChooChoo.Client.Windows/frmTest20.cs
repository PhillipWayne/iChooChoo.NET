using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Client.Windows
{
    public partial class frmTest20 : Form
    {
        public virtual IccClient Client { get; set; }

        public virtual GenPurpModule Module { get; set; }

        public frmTest20(IccClient client, GenPurpModule module)
        {
            Client = client;
            Module = module;

            InitializeComponent();

            this.Text = string.Format("General Purpose Module: {0}", Module.ToString());
        }

        private void GetStatus()
        {
            var l = Client.GET_STATUS(Module.ID);
            lblStatus.Text = string.Join("-", l);
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            byte b = byte.Parse(btn.Name.Substring(btn.Name.Length - 1), NumberStyles.HexNumber);
            var mod = Module as IOnOffOutputsModule;
            mod.setOutput(b, true);
            if (!Client.LastResult)
                MessageBox.Show(string.Format("An error occured:\n\n{0}", Client.LastError), "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            GetStatus();
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            byte b = byte.Parse(btn.Name.Substring(btn.Name.Length - 1), NumberStyles.HexNumber);
            var mod = Module as IOnOffOutputsModule;
            mod.setOutput(b, false);
            if (!Client.LastResult)
                MessageBox.Show(string.Format("An error occured:\n\n{0}", Client.LastError), "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            GetStatus();
        }
    }
}
