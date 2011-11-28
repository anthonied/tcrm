namespace PastelCrmDataPump
{
    partial class DataPump
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
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCustomerDescriptionStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdClientStatus = new System.Windows.Forms.Button();
            this.txtCustCodeStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.cmdSync = new System.Windows.Forms.Button();
            this.cmdSyncCustomer = new System.Windows.Forms.Button();
            this.picCrmExistPastel = new System.Windows.Forms.PictureBox();
            this.picPastelExistStatus = new System.Windows.Forms.PictureBox();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.TabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCrmExistPastel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPastelExistStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // TabPage1
            // 
            this.TabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage1.Controls.Add(this.groupBox1);
            this.TabPage1.Controls.Add(this.cmdClientStatus);
            this.TabPage1.Controls.Add(this.txtCustCodeStatus);
            this.TabPage1.Controls.Add(this.label2);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(926, 392);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Navrae";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdSyncCustomer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.picCrmExistPastel);
            this.groupBox1.Controls.Add(this.txtCustomerDescriptionStatus);
            this.groupBox1.Controls.Add(this.picPastelExistStatus);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(439, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(469, 192);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kliënt Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Customer Description";
            // 
            // txtCustomerDescriptionStatus
            // 
            this.txtCustomerDescriptionStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomerDescriptionStatus.Location = new System.Drawing.Point(135, 27);
            this.txtCustomerDescriptionStatus.Name = "txtCustomerDescriptionStatus";
            this.txtCustomerDescriptionStatus.ReadOnly = true;
            this.txtCustomerDescriptionStatus.Size = new System.Drawing.Size(302, 20);
            this.txtCustomerDescriptionStatus.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Pastel";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(268, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "CRM";
            // 
            // cmdClientStatus
            // 
            this.cmdClientStatus.Location = new System.Drawing.Point(225, 18);
            this.cmdClientStatus.Name = "cmdClientStatus";
            this.cmdClientStatus.Size = new System.Drawing.Size(176, 23);
            this.cmdClientStatus.TabIndex = 2;
            this.cmdClientStatus.Text = "Kry status van kliënt";
            this.cmdClientStatus.UseVisualStyleBackColor = true;
            this.cmdClientStatus.Click += new System.EventHandler(this.cmdClientStatus_Click);
            // 
            // txtCustCodeStatus
            // 
            this.txtCustCodeStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustCodeStatus.Location = new System.Drawing.Point(91, 20);
            this.txtCustCodeStatus.Name = "txtCustCodeStatus";
            this.txtCustCodeStatus.Size = new System.Drawing.Size(128, 20);
            this.txtCustCodeStatus.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Kliënte Kode";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(339, 123);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(459, 33);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Bemarkingsprogram Data Pomp";
            // 
            // TabPage2
            // 
            this.TabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(926, 392);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Kliënte nie in web stelsel";
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Location = new System.Drawing.Point(12, 215);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(934, 418);
            this.TabControl1.TabIndex = 6;
            // 
            // cmdSync
            // 
            this.cmdSync.BackgroundImage = global::PastelCrmDataPump.Properties.Resources.synch;
            this.cmdSync.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cmdSync.Location = new System.Drawing.Point(861, 106);
            this.cmdSync.Name = "cmdSync";
            this.cmdSync.Size = new System.Drawing.Size(85, 85);
            this.cmdSync.TabIndex = 0;
            this.cmdSync.UseVisualStyleBackColor = true;
            this.cmdSync.Click += new System.EventHandler(this.cmdSync_Click);
            // 
            // cmdSyncCustomer
            // 
            this.cmdSyncCustomer.BackgroundImage = global::PastelCrmDataPump.Properties.Resources.synch;
            this.cmdSyncCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cmdSyncCustomer.Location = new System.Drawing.Point(386, 127);
            this.cmdSyncCustomer.Name = "cmdSyncCustomer";
            this.cmdSyncCustomer.Size = new System.Drawing.Size(51, 54);
            this.cmdSyncCustomer.TabIndex = 8;
            this.cmdSyncCustomer.UseVisualStyleBackColor = true;
            this.cmdSyncCustomer.Click += new System.EventHandler(this.cmdSyncCustomer_Click);
            // 
            // picCrmExistPastel
            // 
            this.picCrmExistPastel.Image = global::PastelCrmDataPump.Properties.Resources.question1;
            this.picCrmExistPastel.Location = new System.Drawing.Point(386, 53);
            this.picCrmExistPastel.Name = "picCrmExistPastel";
            this.picCrmExistPastel.Size = new System.Drawing.Size(51, 51);
            this.picCrmExistPastel.TabIndex = 8;
            this.picCrmExistPastel.TabStop = false;
            // 
            // picPastelExistStatus
            // 
            this.picPastelExistStatus.Image = global::PastelCrmDataPump.Properties.Resources.question1;
            this.picPastelExistStatus.Location = new System.Drawing.Point(135, 53);
            this.picPastelExistStatus.Name = "picPastelExistStatus";
            this.picPastelExistStatus.Size = new System.Drawing.Size(51, 51);
            this.picPastelExistStatus.TabIndex = 7;
            this.picPastelExistStatus.TabStop = false;
            // 
            // PictureBox2
            // 
            this.PictureBox2.Image = global::PastelCrmDataPump.Properties.Resources.top_tab;
            this.PictureBox2.Location = new System.Drawing.Point(331, 0);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(630, 98);
            this.PictureBox2.TabIndex = 5;
            this.PictureBox2.TabStop = false;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::PastelCrmDataPump.Properties.Resources.logo;
            this.PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(331, 166);
            this.PictureBox1.TabIndex = 4;
            this.PictureBox1.TabStop = false;
            // 
            // DataPump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 654);
            this.Controls.Add(this.cmdSync);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.PictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DataPump";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pastel - CRM Data Pomp";
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCrmExistPastel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPastelExistStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.Button cmdSync;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.PictureBox PictureBox2;
        internal System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.Button cmdClientStatus;
        private System.Windows.Forms.TextBox txtCustCodeStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picPastelExistStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCustomerDescriptionStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picCrmExistPastel;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button cmdSyncCustomer;
    }
}