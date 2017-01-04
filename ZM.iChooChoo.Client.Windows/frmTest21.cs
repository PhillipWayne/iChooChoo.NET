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
    public partial class frmTest21 : Form
    {
        public virtual IccClient Client { get; set; }

        public virtual LightingModule Module { get; set; }

        public frmTest21(IccClient client, LightingModule module)
        {
            Client = client;
            Module = module;

            InitializeComponent();

            PopulateOutputLists(this);

#if DEBUG
            // Init default outputs
            lstStartLinear1sOutput1.SelectedIndex = 0xF;
            lstStopLinear2sOutput1.SelectedIndex = 0xF;
            lstStartTungOutput1.SelectedIndex = 0xF;
            lstStopTungOutput1.SelectedIndex = 0xF;
            lstStartMono1Output1.SelectedIndex = 0x1;
            lstStartMono2Output1.SelectedIndex = 0x2;
            lstStartDualOutput1.SelectedIndex = 0x1;
            lstStartDualOutput2.SelectedIndex = 0x2;
            lstStartOldMonoOutput1.SelectedIndex = 0xF;
            lstStartOldDualOutput1.SelectedIndex = 0xE;
            lstStartOldDualOutput2.SelectedIndex = 0xF;
            lstTrafficLightFraMonoOutput1.SelectedIndex = 0x8;
            lstTrafficLightFraMonoOutput2.SelectedIndex = 0x7;
            lstTrafficLightFraMonoOutput3.SelectedIndex = 0xF;
            lstTrafficLightFraDualOutput1.SelectedIndex = 0x8;
            lstTrafficLightFraDualOutput2.SelectedIndex = 0x7;
            lstTrafficLightFraDualOutput3.SelectedIndex = 0xF;
            lstTrafficLightFraDualOutput4.SelectedIndex = 0x6;
            lstTrafficLightFraDualOutput5.SelectedIndex = 0x5;
            lstTrafficLightFraDualOutput6.SelectedIndex = 0xE;
            lstTrafficLightDeuMonoOutput1.SelectedIndex = 0x8;
            lstTrafficLightDeuMonoOutput2.SelectedIndex = 0x7;
            lstTrafficLightDeuMonoOutput3.SelectedIndex = 0xF;
            lstTrafficLightDeuDualOutput1.SelectedIndex = 0x8;
            lstTrafficLightDeuDualOutput2.SelectedIndex = 0x7;
            lstTrafficLightDeuDualOutput3.SelectedIndex = 0xF;
            lstTrafficLightDeuDualOutput4.SelectedIndex = 0x6;
            lstTrafficLightDeuDualOutput5.SelectedIndex = 0x5;
            lstTrafficLightDeuDualOutput6.SelectedIndex = 0xE;
            lstChaserOutput1.SelectedIndex = 0x8;
            lstChaserOutput2.SelectedIndex = 0x7;
            lstChaserOutput3.SelectedIndex = 0xF;
            lstChaserOutput4.SelectedIndex = 0x6;
            lstChaserOutput5.SelectedIndex = 0x5;
            lstChaserOutput6.SelectedIndex = 0xE;
            lstChaserOutput7.SelectedIndex = 0xD;
            lstChaserOutput8.SelectedIndex = 0x0;
            lstArcWeldingOutput1.SelectedIndex = 0x1;
            lstCameraFlashOutput1.SelectedIndex = 0x2;
            lstFireOutput1.SelectedIndex = 0xD;
            lstFireOutput2.SelectedIndex = 0xE;
#endif
        }

        private void PopulateOutputLists(Control c)
        {
            foreach(Control ctl in c.Controls)
            {
                if (ctl.Name.Contains("Output") && ctl is ComboBox)
                {
                    var lst = ctl as ComboBox;
                    lst.Items.Clear();
                    for (int i = 0; i < 16; i++)
                        lst.Items.Add(i.ToString("X"));
                }
                PopulateOutputLists(ctl);
            }
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
            if (b < 0xA)
            {
                var mod = Module as IOnOffOutputsModule;
                mod.setOutput(b, true);
                if (!Client.LastResult)
                    MessageBox.Show(string.Format("An error occured:\n\n{0}", Client.LastError), "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                var bar = grpDimmable.Controls["bar" + b.ToString("X")] as TrackBar;
                bar.Value = 0xFF;
                bar_KeyUp(bar, new KeyEventArgs(Keys.Up));
            }
            GetStatus();
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            byte b = byte.Parse(btn.Name.Substring(btn.Name.Length - 1), NumberStyles.HexNumber);
            if (b < 0xA)
            {
                var mod = Module as IOnOffOutputsModule;
                mod.setOutput(b, false);
                if (!Client.LastResult)
                    MessageBox.Show(string.Format("An error occured:\n\n{0}", Client.LastError), "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                var bar = grpDimmable.Controls["bar" + b.ToString("X")] as TrackBar;
                bar.Value = 0x00;
                bar_KeyUp(bar, new KeyEventArgs(Keys.Up));
            }
            GetStatus();
        }

        private void btnStopScen_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            byte b = byte.Parse(btn.Name.Substring(btn.Name.Length - 1), NumberStyles.HexNumber);
            var mod = Module as IScenariosModule;
            mod.StopScenario(b);
            if (!Client.LastResult)
                MessageBox.Show(string.Format("An error occured:\n\n{0}", Client.LastError), "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            GetStatus();
        }

        private void btnStartLinear1s_Click(object sender, EventArgs e)
        {
            if (lstStartLinear1sOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStartLinear1sOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisStep = int.Parse(txtLinear1sMillisStep.Text);
                byte bStartValue = byte.Parse(txtLinear1sStartValue.Text);
                byte bTargetValue = byte.Parse(txtLinear1sTargetValue.Text);
                int iStep = int.Parse(txtLinear1sStep.Text);
                mod.StartProgressiveChange(bOutput1, iMillisStep, bStartValue, bTargetValue, iStep, (byte)(chkLinear1sLinear.Checked ? 1 : 0));
            }
        }

        private void btnStopLinear2s_Click(object sender, EventArgs e)
        {
            if (lstStopLinear2sOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStopLinear2sOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisStep = int.Parse(txtLinear2sMillisStep.Text);
                byte bStartValue = byte.Parse(txtLinear2sStartValue.Text);
                byte bTargetValue = byte.Parse(txtLinear2sTargetValue.Text);
                int iStep = int.Parse(txtLinear2sStep.Text);
                mod.StartProgressiveChange(bOutput1, iMillisStep, bStartValue, bTargetValue, iStep, (byte)(chkLinear1sLinear.Checked ? 1 : 0));
            }
        }

        private void btnStartTung_Click(object sender, EventArgs e)
        {
            if (lstStartTungOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStartTungOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                mod.StartTungstenOn(bOutput1);
            }
        }

        private void btnStopTung_Click(object sender, EventArgs e)
        {
            if (lstStopTungOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStopTungOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                mod.StartTungstenOff(bOutput1);
            }
        }

        private void btnStartMono1_Click(object sender, EventArgs e)
        {
            if (lstStartMono1Output1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStartMono1Output1.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisOn1 = int.Parse(txtBlinkerMono1MillisOn1.Text);
                int iMillisOn2 = int.Parse(txtBlinkerMono1MillisOn2.Text);
                mod.StartBlinkerLed(bOutput1, 0, iMillisOn1, iMillisOn2);
            }
        }

        private void btnStartMono2_Click(object sender, EventArgs e)
        {
            if (lstStartMono2Output1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStartMono2Output1.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisOn1 = int.Parse(txtBlinkerMono2MillisOn1.Text);
                int iMillisOn2 = int.Parse(txtBlinkerMono2MillisOn2.Text);
                mod.StartBlinkerLed(bOutput1, 0, iMillisOn1, iMillisOn2);
            }
        }

        private void btnStartDual_Click(object sender, EventArgs e)
        {
            if (lstStartDualOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstStartDualOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStartDualOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstStartDualOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisOn1 = int.Parse(txtBlinkerDualMillisOn1.Text);
                int iMillisOn2 = int.Parse(txtBlinkerDualMillisOn2.Text);
                mod.StartBlinkerLed(bOutput1, bOutput2, iMillisOn1, iMillisOn2);
            }
        }

        private void btnStartOldMono_Click(object sender, EventArgs e)
        {
            if (lstStartOldMonoOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStartOldMonoOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisOn1 = int.Parse(txtBlinkerOldMonoMillisOn1.Text);
                int iMillisOn2 = int.Parse(txtBlinkerOldMonoMillisOn2.Text);
                mod.StartBlinkerOld(bOutput1, 0, iMillisOn1, iMillisOn2);
            }
        }

        private void btnStartOldDual_Click(object sender, EventArgs e)
        {
            if (lstStartOldDualOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstStartOldDualOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstStartOldDualOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstStartOldDualOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisOn1 = int.Parse(txtBlinkerOldDualMillisOn1.Text);
                int iMillisOn2 = int.Parse(txtBlinkerOldDualMillisOn2.Text);
                mod.StartBlinkerOld(bOutput1, bOutput2, iMillisOn1, iMillisOn2);
            }
        }

        private void btnTrafficLightFraMono_Click(object sender, EventArgs e)
        {
            if (lstTrafficLightFraMonoOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightFraMonoOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightFraMonoOutput3.SelectedIndex == -1)
                MessageBox.Show("Please select output 3 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstTrafficLightFraMonoOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstTrafficLightFraMonoOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput3 = byte.Parse(lstTrafficLightFraMonoOutput3.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisGreen = int.Parse(txtTrafficFraMonoMillisGreen.Text);
                int iMillisYellow = int.Parse(txtTrafficFraMonoMillisYellow.Text);
                int iMillisRed = int.Parse(txtTrafficFraMonoMillisRed.Text);
                mod.StartTrafficLightsFrench(bOutput1, bOutput2, bOutput3, 0, 0, 0, iMillisGreen, iMillisYellow, iMillisRed);
            }
        }

        private void btnTrafficLightFraDual_Click(object sender, EventArgs e)
        {
            if (lstTrafficLightFraDualOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightFraDualOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightFraDualOutput3.SelectedIndex == -1)
                MessageBox.Show("Please select output 3 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightFraDualOutput4.SelectedIndex == -1)
                MessageBox.Show("Please select output 4 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightFraDualOutput5.SelectedIndex == -1)
                MessageBox.Show("Please select output 5 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightFraDualOutput6.SelectedIndex == -1)
                MessageBox.Show("Please select output 6 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstTrafficLightFraDualOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstTrafficLightFraDualOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput3 = byte.Parse(lstTrafficLightFraDualOutput3.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput4 = byte.Parse(lstTrafficLightFraDualOutput4.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput5 = byte.Parse(lstTrafficLightFraDualOutput5.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput6 = byte.Parse(lstTrafficLightFraDualOutput6.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisGreen = int.Parse(txtTrafficFraDualMillisGreen.Text);
                int iMillisYellow = int.Parse(txtTrafficFraDualMillisYellow.Text);
                int iMillisRed = int.Parse(txtTrafficFraDualMillisRed.Text);
                mod.StartTrafficLightsFrench(bOutput1, bOutput2, bOutput3, bOutput4, bOutput5, bOutput6, iMillisGreen, iMillisYellow, iMillisRed);
            }
        }

        private void btnTrafficLightDeuMono_Click(object sender, EventArgs e)
        {
            if (lstTrafficLightDeuMonoOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightDeuMonoOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightDeuMonoOutput3.SelectedIndex == -1)
                MessageBox.Show("Please select output 3 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstTrafficLightDeuMonoOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstTrafficLightDeuMonoOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput3 = byte.Parse(lstTrafficLightDeuMonoOutput3.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisGreen = int.Parse(txtTrafficDeuMonoMillisGreen.Text);
                int iMillisYellow = int.Parse(txtTrafficDeuMonoMillisYellow.Text);
                int iMillisRed = int.Parse(txtTrafficDeuMonoMillisRed.Text);
                mod.StartTrafficLightsGerman(bOutput1, bOutput2, bOutput3, 0, 0, 0, iMillisGreen, iMillisYellow, iMillisRed);
            }
        }

        private void btnTrafficLightDeuDual_Click(object sender, EventArgs e)
        {
            if (lstTrafficLightDeuDualOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightDeuDualOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightDeuDualOutput3.SelectedIndex == -1)
                MessageBox.Show("Please select output 3 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightDeuDualOutput4.SelectedIndex == -1)
                MessageBox.Show("Please select output 4 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightDeuDualOutput5.SelectedIndex == -1)
                MessageBox.Show("Please select output 5 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstTrafficLightDeuDualOutput6.SelectedIndex == -1)
                MessageBox.Show("Please select output 6 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstTrafficLightDeuDualOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstTrafficLightDeuDualOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput3 = byte.Parse(lstTrafficLightDeuDualOutput3.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput4 = byte.Parse(lstTrafficLightDeuDualOutput4.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput5 = byte.Parse(lstTrafficLightDeuDualOutput5.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput6 = byte.Parse(lstTrafficLightDeuDualOutput6.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisGreen = int.Parse(txtTrafficDeuDualMillisGreen.Text);
                int iMillisYellow = int.Parse(txtTrafficDeuDualMillisYellow.Text);
                int iMillisRed = int.Parse(txtTrafficDeuDualMillisRed.Text);
                mod.StartTrafficLightsGerman(bOutput1, bOutput2, bOutput3, bOutput4, bOutput5, bOutput6, iMillisGreen, iMillisYellow, iMillisRed);
            }
        }

        private void btnChaser_Click(object sender, EventArgs e)
        {
            if (lstChaserOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstChaserOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstChaserOutput3.SelectedIndex == -1)
                MessageBox.Show("Please select output 3 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstChaserOutput4.SelectedIndex == -1)
                MessageBox.Show("Please select output 4 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstChaserOutput5.SelectedIndex == -1)
                MessageBox.Show("Please select output 5 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstChaserOutput6.SelectedIndex == -1)
                MessageBox.Show("Please select output 6 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstChaserOutput7.SelectedIndex == -1)
                MessageBox.Show("Please select output 7 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstChaserOutput8.SelectedIndex == -1)
                MessageBox.Show("Please select output 8 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstChaserOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstChaserOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput3 = byte.Parse(lstChaserOutput3.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput4 = byte.Parse(lstChaserOutput4.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput5 = byte.Parse(lstChaserOutput5.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput6 = byte.Parse(lstChaserOutput6.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput7 = byte.Parse(lstChaserOutput7.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput8 = byte.Parse(lstChaserOutput8.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisStep = int.Parse(txtChaserMillisStep.Text);
                int iMillisOn = int.Parse(txtChaserMillisOn.Text);
                int iMillisPause = int.Parse(txtChaserMillisPause.Text);
                int iCoexistence = int.Parse(txtChaserModeCoexistence.Text, NumberStyles.HexNumber);
                mod.StartChaser(bOutput1, bOutput2, bOutput3, bOutput4, bOutput5, bOutput6, bOutput7, bOutput8, iMillisStep, iMillisOn, iMillisPause, iCoexistence);
            }
        }

        private void btnArcWelding_Click(object sender, EventArgs e)
        {
            if (lstArcWeldingOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstArcWeldingOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisOn = int.Parse(txtArcMillisOn.Text);
                int iMillisOff = int.Parse(txtArcMillisOff.Text);
                int iPauseMin = int.Parse(txtArcPauseMin.Text);
                int iPauseMax = int.Parse(txtArcPauseMax.Text);
                int iFlashMin = int.Parse(txtArcFlashMin.Text);
                int iFlashMax = int.Parse(txtArcFlashMax.Text);
                mod.StartArcWelding(bOutput1, iMillisOn, iMillisOff, iPauseMin, iPauseMax, iFlashMin, iFlashMax);
            }
        }

        private void btnCameraFlash_Click(object sender, EventArgs e)
        {
            if (lstCameraFlashOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstCameraFlashOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iMillisOn = int.Parse(txtCameraMillisOn.Text);
                int iPauseMin = int.Parse(txtCameraPauseMin.Text);
                int iPauseMax = int.Parse(txtCameraPauseMax.Text);
                mod.StartCameraFlash(bOutput1, iMillisOn, iPauseMin, iPauseMax);
            }
        }

        private void btnFire_Click(object sender, EventArgs e)
        {
            if (lstFireOutput1.SelectedIndex == -1)
                MessageBox.Show("Please select output 1 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (lstFireOutput2.SelectedIndex == -1)
                MessageBox.Show("Please select output 2 first!", "iChooChoo Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                var mod = Module as LightingModule;
                byte bOutput1 = byte.Parse(lstFireOutput1.SelectedItem.ToString(), NumberStyles.HexNumber);
                byte bOutput2 = byte.Parse(lstFireOutput2.SelectedItem.ToString(), NumberStyles.HexNumber);
                int iFireSpeed = int.Parse(txtFireSpeed.Text);
                mod.StartFire(bOutput1, bOutput2, iFireSpeed);
            }
        }

        private void bar_ValueChanged(object sender, EventArgs e)
        {
            var bar = sender as TrackBar;
            string sLetter = bar.Name.Substring(3);
            var lbl = bar.Parent.Controls["lblOUtputDim" + sLetter] as Label;
            lbl.Text = string.Format("Output {0} : {1}", sLetter, bar.Value);
        }

        private void bar_MouseUp(object sender, MouseEventArgs e)
        {
            var bar = sender as TrackBar;
            string sLetter = bar.Name.Substring(3);
            byte bOutput = byte.Parse(sLetter, NumberStyles.HexNumber);

            var mod = Module as IDimmableOutputsModule;
            mod.setDimmableOutput(bOutput, (byte)bar.Value);
        }

        private void bar_KeyUp(object sender, KeyEventArgs e)
        {
            var bar = sender as TrackBar;
            string sLetter = bar.Name.Substring(3);
            byte bOutput = byte.Parse(sLetter, NumberStyles.HexNumber);

            var mod = Module as IDimmableOutputsModule;
            mod.setDimmableOutput(bOutput, (byte)bar.Value);
        }
    }
}
