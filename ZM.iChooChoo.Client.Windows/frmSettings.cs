using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZM.iChooChoo.Client.Windows.Config;

namespace ZM.iChooChoo.Client.Windows
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();

            txtServerSettingsName.Text = Configuration.Config.ServerSettings.ServerName;
            txtServerSettingsCmdPort.Text = Configuration.Config.ServerSettings.ServerCommandPort.ToString();
            txtServerSettingsLogPort.Text = Configuration.Config.ServerSettings.ServerLogPort.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Configuration.Config.ServerSettings.ServerName = txtServerSettingsName.Text;
            Configuration.Config.ServerSettings.ServerCommandPort = int.Parse(txtServerSettingsCmdPort.Text);
            Configuration.Config.ServerSettings.ServerLogPort = int.Parse(txtServerSettingsLogPort.Text);

            Configuration.SaveToFile();
        }

        private void txt_Enter(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                var txt = sender as TextBox;
                txt.SelectAll();
            }
            else if (sender is MaskedTextBox)
            {
                BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll(sender as MaskedTextBox); });
            }
        }

        private void SetMaskedTextBoxSelectAll(MaskedTextBox sender)
        {
            var txt = sender as MaskedTextBox;
            txt.SelectionStart = 0;
            txt.SelectionLength = txt.Text.Length;
        }
    }
}
