namespace ZM.iChooChoo.Client.Windows
{
    partial class frmAboutBox
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAboutBox));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.picIcone = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblProgTitre = new System.Windows.Forms.Label();
            this.btnFermer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProgVersion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lkbLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkZTBMedia = new System.Windows.Forms.LinkLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcone)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(80, 267);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.picIcone);
            this.panel2.Location = new System.Drawing.Point(4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(56, 56);
            this.panel2.TabIndex = 1;
            // 
            // picIcone
            // 
            this.picIcone.BackColor = System.Drawing.Color.White;
            this.picIcone.Image = global::ZM.iChooChoo.Client.Windows.Properties.Resources.iChooChoo_icon_48_Transparent;
            this.picIcone.Location = new System.Drawing.Point(4, 4);
            this.picIcone.Name = "picIcone";
            this.picIcone.Size = new System.Drawing.Size(48, 48);
            this.picIcone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picIcone.TabIndex = 0;
            this.picIcone.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Location = new System.Drawing.Point(48, 88);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(64, 64);
            this.panel3.TabIndex = 2;
            // 
            // lblProgTitre
            // 
            this.lblProgTitre.AutoSize = true;
            this.lblProgTitre.Font = new System.Drawing.Font("Segoe UI", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgTitre.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblProgTitre.Location = new System.Drawing.Point(118, 92);
            this.lblProgTitre.Name = "lblProgTitre";
            this.lblProgTitre.Size = new System.Drawing.Size(241, 37);
            this.lblProgTitre.TabIndex = 3;
            this.lblProgTitre.Text = "iChooChoo Client";
            // 
            // btnFermer
            // 
            this.btnFermer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFermer.Location = new System.Drawing.Point(413, 296);
            this.btnFermer.Name = "btnFermer";
            this.btnFermer.Size = new System.Drawing.Size(75, 23);
            this.btnFermer.TabIndex = 4;
            this.btnFermer.Text = "Fermer";
            this.btnFermer.UseVisualStyleBackColor = true;
            this.btnFermer.Click += new System.EventHandler(this.btnFermer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Achevé en 2017 par Stéphane Moitry";
            // 
            // lblProgVersion
            // 
            this.lblProgVersion.AutoSize = true;
            this.lblProgVersion.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblProgVersion.Location = new System.Drawing.Point(120, 129);
            this.lblProgVersion.Name = "lblProgVersion";
            this.lblProgVersion.Size = new System.Drawing.Size(114, 25);
            this.lblProgVersion.TabIndex = 6;
            this.lblProgVersion.Text = "Version 0.37";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Website :";
            // 
            // lkbLink
            // 
            this.lkbLink.AutoSize = true;
            this.lkbLink.Location = new System.Drawing.Point(191, 190);
            this.lkbLink.Name = "lkbLink";
            this.lkbLink.Size = new System.Drawing.Size(114, 13);
            this.lkbLink.TabIndex = 9;
            this.lkbLink.TabStop = true;
            this.lkbLink.Text = "http://ichoochoo.ztb.fr";
            this.lkbLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkbLink_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(301, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "iChooChoo is open source software maintained by ZTB Media.";
            // 
            // lnkZTBMedia
            // 
            this.lnkZTBMedia.BackColor = System.Drawing.Color.Transparent;
            this.lnkZTBMedia.LinkColor = System.Drawing.Color.Transparent;
            this.lnkZTBMedia.Location = new System.Drawing.Point(243, 3);
            this.lnkZTBMedia.Name = "lnkZTBMedia";
            this.lnkZTBMedia.Size = new System.Drawing.Size(257, 19);
            this.lnkZTBMedia.TabIndex = 12;
            this.lnkZTBMedia.TabStop = true;
            this.lnkZTBMedia.Text = "                                                                                 " +
    "  ";
            this.lnkZTBMedia.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkZTBMedia_LinkClicked);
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::ZM.iChooChoo.Client.Windows.Properties.Resources.ZTBMedia_AboutBox500;
            this.panel4.Controls.Add(this.lnkZTBMedia);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(500, 64);
            this.panel4.TabIndex = 13;
            // 
            // frmAboutBox
            // 
            this.AcceptButton = this.btnFermer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 331);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lkbLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblProgVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFermer);
            this.Controls.Add(this.lblProgTitre);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAboutBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "A propos de ...";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcone)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picIcone;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblProgTitre;
        private System.Windows.Forms.Button btnFermer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProgVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lkbLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkZTBMedia;
        private System.Windows.Forms.Panel panel4;
    }
}