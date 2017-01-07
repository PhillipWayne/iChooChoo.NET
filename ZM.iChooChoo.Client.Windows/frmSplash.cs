using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZM.iChooChoo.Library;

namespace ZM.iChooChoo.Client.Windows
{
    public partial class frmSplash : Form
    {
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);

        public virtual int MinimumShowTime { get; set; }

        public virtual bool CanBeClosed { get; set; }

        private Timer Timer { get; set; }

        public frmSplash(int minimumShowTime)
        {
            //SetForegroundWindow(this.Handle);
            InitializeComponent();

            lblVersion.Text = string.Format("Version {0}.{1}.{2}", ICCConstants.PROG_VERSION_MAJ, ICCConstants.PROG_VERSION_MIN, ICCConstants.PROG_VERSION_BLD);

            MinimumShowTime = minimumShowTime;
            CanBeClosed = false;

            Timer = new Timer();
            Timer.Interval = MinimumShowTime * 1000;
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            if (CanBeClosed)
            {
                this.Close();
            }
            else
            {
                Timer.Interval = 1000;
                Timer.Start();
            }
        }
    }
}
