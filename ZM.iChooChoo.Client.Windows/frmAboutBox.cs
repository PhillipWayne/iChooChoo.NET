using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZM.iChooChoo.Client.Windows
{
    public partial class frmAboutBox : Form
    {
        private string _progTitre;
        private string _progVersion;
        private Image _icone;

        public static void Show(string ProgName, string Version)
        {
            frmAboutBox f = new frmAboutBox();
            f.ProgName = ProgName;
            f.ProgVersion = Version;
            f.ShowDialog();
        }

        public static void Show(string ProgName, string Version, Image Icone)
        {
            frmAboutBox f = new frmAboutBox();
            f.ProgName = ProgName;
            f.ProgVersion = Version;
            f.Icone = Icone;
            f.ShowDialog();
        }

        public frmAboutBox()
        {
            InitializeComponent();
        }

        public Image Icone
        {
            get { return _icone; }
            set
            {
                _icone = value;
                //this.Icon = (Icon)_icone;
                picIcone.Image = _icone;
            }
        }

        public string ProgName
        {
            get { return _progTitre; }
            set
            {
                _progTitre = value;
                lblProgTitre.Text = _progTitre;
            }
        }

        public string ProgVersion
        {
            get { return _progVersion; }
            set
            {
                _progVersion = value;
                lblProgVersion.Text = "Version " + _progVersion;
            }
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lkbLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start((sender as LinkLabel).Text);
        }

        private void lnkZTBMedia_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.ztb-media.fr");
        }
    }
}