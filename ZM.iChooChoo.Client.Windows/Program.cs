using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZM.iChooChoo.Client.Windows.Config;

namespace ZM.iChooChoo.Client.Windows
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Configuration.ReadFromFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occured while starting iChooChoo Client. Please use the iChooChoo forum to ask for some help and put the following error message to help other people to understand what's happening:\n\n{0}", ex.Message), "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }

            Application.Run(new frmMaster());
        }
    }
}
