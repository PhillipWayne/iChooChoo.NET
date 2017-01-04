using fastJSON;
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
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Log;
using ZM.iChooChoo.Library.Modules;

namespace ZM.iChooChoo.Client.Windows
{
    public enum ConnectionStatus
    {
        NotConnected,
        Connecting,
        Connected
    }

    public partial class frmMaster : Form
    {
        public IccClient Client { get; set; }

        public frmMaster()
        {
            InitializeComponent();

            this.Location = new Point(Configuration.Config.MainWindowSettings.WindowLeft, Configuration.Config.MainWindowSettings.WindowTop);
            this.Size = new Size(Configuration.Config.MainWindowSettings.WindowWidth, Configuration.Config.MainWindowSettings.WindowHeight);
            this.WindowState = Configuration.Config.MainWindowSettings.WindowState;
        }

        private void frmMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            Configuration.Config.MainWindowSettings.WindowState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Configuration.Config.MainWindowSettings.WindowTop = this.Location.Y;
                Configuration.Config.MainWindowSettings.WindowLeft = this.Location.X;
                Configuration.Config.MainWindowSettings.WindowWidth = this.Size.Width;
                Configuration.Config.MainWindowSettings.WindowHeight = this.Size.Height;
            }

            Configuration.SaveToFile();
            Client.LogClient.EntryReceived -= LogClient_EntryReceived;
            Client.Dispose();
        }

        private void frmMaster_Shown(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            PrintConnectStatus(ConnectionStatus.Connecting);
            PrintStatus(string.Format("Connecting to server '{0}' on command port...", Configuration.Config.ServerSettings.ServerName));

            Client = new IccClient(Configuration.Config.ServerSettings.ServerName, Configuration.Config.ServerSettings.ServerCommandPort, Configuration.Config.ServerSettings.ServerLogPort);
            if (!Client.Connected)
            {
                PrintStatus(Client.LastError);
                PrintConnectStatus(ConnectionStatus.NotConnected);
            }
            else
            {
                PrintConnectStatus(ConnectionStatus.Connected);
                PrintStatus("Connected");
                Client.LogClient.EntryReceived += LogClient_EntryReceived;
                PopulateModules();
            }
            this.Cursor = Cursors.Default;
        }

        private void LogClient_EntryReceived(object sender, Library.Log.LogEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new LogEventHandler(LogClient_EntryReceived), new object[] { sender, e });
                return;
            }

            if (lstServerLog.Items.Count > 100)
                lstServerLog.Items.RemoveAt(0);

            lstServerLog.Items.Add(e.Entry);
            lstServerLog.TopIndex = lstServerLog.Items.Count - 1;
        }

        private void PopulateModules()
        {
            mnuModules.DropDownItems.Clear();
            var l = Client.Modules.Values.ToList();
            foreach (var m in l)
            {
                var mnu = new ToolStripMenuItem(m.ToString());
                mnu.Click += mnuModulesItems_Click;
                mnu.Tag = m;
                mnuModules.DropDownItems.Add(mnu);
            }

            if (mnuModules.DropDownItems.Count > 0)
                mnuModules.DropDownItems.Add("-");

            var mnuRescan = new ToolStripMenuItem("Rescan bus");
            mnuRescan.Click += MnuRescan_Click;
            mnuModules.DropDownItems.Add(mnuRescan);
        }

        private void MnuRescan_Click(object sender, EventArgs e)
        {
            Client.RefreshBus();
            PopulateModules();
        }

        public virtual void PrintConnectStatus(ConnectionStatus s)
        {
            if (s == ConnectionStatus.NotConnected)
            {
                toolStripStatusLabel2.Text = "Not connected";
                toolStripStatusLabel2.BackColor = Color.Red;
            }
            else if (s == ConnectionStatus.Connecting)
            {
                toolStripStatusLabel2.Text = "Connecting";
                toolStripStatusLabel2.BackColor = Color.Orange;
            }
            else
            {
                toolStripStatusLabel2.Text = "Connected";
                toolStripStatusLabel2.BackColor = Color.Green;
            }
        }

        public virtual void PrintStatus(string s)
        {
            toolStripStatusLabel1.Text = s;
            Application.DoEvents();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuFileSettings_Click(object sender, EventArgs e)
        {
            var f = new frmSettings();
            f.ShowDialog();
        }

        private void mnuHelpAboutIChooChoo_Click(object sender, EventArgs e)
        {
            frmAboutBox.Show("iChooChoo Client", string.Format("{0}.{1}.{2}", ICCConstants.PROG_VERSION_MAJ, ICCConstants.PROG_VERSION_MIN, ICCConstants.PROG_VERSION_BLD));
        }

        private void mnuModulesItems_Click(object sender, EventArgs e)
        {
            var mod = (sender as ToolStripMenuItem).Tag as IModule;
            if (mod.Type == ICCConstants.BICCP_GRP_LIGHTING)
            {
                var f = new frmTest21(Client, mod as LightingModule);
                f.Show();
            }
        }
    }
}
