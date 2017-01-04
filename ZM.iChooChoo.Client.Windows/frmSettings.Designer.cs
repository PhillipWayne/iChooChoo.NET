namespace ZM.iChooChoo.Client.Windows
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpServerSettings = new System.Windows.Forms.GroupBox();
            this.txtServerSettingsLogPort = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServerSettingsCmdPort = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServerSettingsName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grpServerSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(574, 391);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(493, 391);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "iChooChoo Client Settings";
            // 
            // grpServerSettings
            // 
            this.grpServerSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpServerSettings.Controls.Add(this.txtServerSettingsLogPort);
            this.grpServerSettings.Controls.Add(this.label4);
            this.grpServerSettings.Controls.Add(this.txtServerSettingsCmdPort);
            this.grpServerSettings.Controls.Add(this.label3);
            this.grpServerSettings.Controls.Add(this.txtServerSettingsName);
            this.grpServerSettings.Controls.Add(this.label2);
            this.grpServerSettings.Location = new System.Drawing.Point(12, 64);
            this.grpServerSettings.Name = "grpServerSettings";
            this.grpServerSettings.Size = new System.Drawing.Size(637, 115);
            this.grpServerSettings.TabIndex = 3;
            this.grpServerSettings.TabStop = false;
            this.grpServerSettings.Text = "Server settings";
            // 
            // txtServerSettingsLogPort
            // 
            this.txtServerSettingsLogPort.Location = new System.Drawing.Point(123, 79);
            this.txtServerSettingsLogPort.Mask = "00000";
            this.txtServerSettingsLogPort.Name = "txtServerSettingsLogPort";
            this.txtServerSettingsLogPort.PromptChar = ' ';
            this.txtServerSettingsLogPort.Size = new System.Drawing.Size(100, 20);
            this.txtServerSettingsLogPort.TabIndex = 6;
            this.txtServerSettingsLogPort.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Log TCP Port:";
            // 
            // txtServerSettingsCmdPort
            // 
            this.txtServerSettingsCmdPort.Location = new System.Drawing.Point(123, 53);
            this.txtServerSettingsCmdPort.Mask = "00000";
            this.txtServerSettingsCmdPort.Name = "txtServerSettingsCmdPort";
            this.txtServerSettingsCmdPort.PromptChar = ' ';
            this.txtServerSettingsCmdPort.Size = new System.Drawing.Size(100, 20);
            this.txtServerSettingsCmdPort.TabIndex = 4;
            this.txtServerSettingsCmdPort.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Command TCP Port:";
            // 
            // txtServerSettingsName
            // 
            this.txtServerSettingsName.Location = new System.Drawing.Point(123, 27);
            this.txtServerSettingsName.MaxLength = 50;
            this.txtServerSettingsName.Name = "txtServerSettingsName";
            this.txtServerSettingsName.Size = new System.Drawing.Size(200, 20);
            this.txtServerSettingsName.TabIndex = 1;
            this.txtServerSettingsName.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Server name or IP:";
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(661, 426);
            this.Controls.Add(this.grpServerSettings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.grpServerSettings.ResumeLayout(false);
            this.grpServerSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpServerSettings;
        private System.Windows.Forms.MaskedTextBox txtServerSettingsLogPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox txtServerSettingsCmdPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServerSettingsName;
        private System.Windows.Forms.Label label2;
    }
}