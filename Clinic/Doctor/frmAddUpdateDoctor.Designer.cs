namespace Clinic.Doctor
{
    partial class frmAddUpdateDoctor
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
            this.components = new System.ComponentModel.Container();
            this.tcDoctor = new System.Windows.Forms.TabControl();
            this.tpPersonalInfo = new System.Windows.Forms.TabPage();
            this.btnPersonInfoNext = new System.Windows.Forms.Button();
            this.tpDoctorInfo = new System.Windows.Forms.TabPage();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConsultationFees = new System.Windows.Forms.TextBox();
            this.txtLicenseNo = new System.Windows.Forms.TextBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.clbWorkingDays = new System.Windows.Forms.CheckedListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lbDoctorID = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.clbSpecialization = new System.Windows.Forms.CheckedListBox();
            this.ctrlPersonCardWithFilter1 = new Clinic.Controls.ctrlPersonCardWithFilter();
            this.tcDoctor.SuspendLayout();
            this.tpPersonalInfo.SuspendLayout();
            this.tpDoctorInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tcDoctor
            // 
            this.tcDoctor.Controls.Add(this.tpPersonalInfo);
            this.tcDoctor.Controls.Add(this.tpDoctorInfo);
            this.tcDoctor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tcDoctor.Location = new System.Drawing.Point(12, 97);
            this.tcDoctor.Name = "tcDoctor";
            this.tcDoctor.SelectedIndex = 0;
            this.tcDoctor.Size = new System.Drawing.Size(931, 474);
            this.tcDoctor.TabIndex = 117;
            // 
            // tpPersonalInfo
            // 
            this.tpPersonalInfo.BackColor = System.Drawing.Color.White;
            this.tpPersonalInfo.Controls.Add(this.ctrlPersonCardWithFilter1);
            this.tpPersonalInfo.Controls.Add(this.btnPersonInfoNext);
            this.tpPersonalInfo.Location = new System.Drawing.Point(4, 32);
            this.tpPersonalInfo.Name = "tpPersonalInfo";
            this.tpPersonalInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpPersonalInfo.Size = new System.Drawing.Size(923, 438);
            this.tpPersonalInfo.TabIndex = 0;
            this.tpPersonalInfo.Text = "Personal Info";
            // 
            // btnPersonInfoNext
            // 
            this.btnPersonInfoNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPersonInfoNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnPersonInfoNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnPersonInfoNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPersonInfoNext.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnPersonInfoNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnPersonInfoNext.Image = global::Clinic.Properties.Resources.Next_32;
            this.btnPersonInfoNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPersonInfoNext.Location = new System.Drawing.Point(764, 388);
            this.btnPersonInfoNext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPersonInfoNext.Name = "btnPersonInfoNext";
            this.btnPersonInfoNext.Size = new System.Drawing.Size(126, 37);
            this.btnPersonInfoNext.TabIndex = 119;
            this.btnPersonInfoNext.Text = "Next   ";
            this.btnPersonInfoNext.UseVisualStyleBackColor = true;
            // 
            // tpDoctorInfo
            // 
            this.tpDoctorInfo.BackColor = System.Drawing.Color.White;
            this.tpDoctorInfo.Controls.Add(this.clbSpecialization);
            this.tpDoctorInfo.Controls.Add(this.chkIsActive);
            this.tpDoctorInfo.Controls.Add(this.label1);
            this.tpDoctorInfo.Controls.Add(this.pictureBox1);
            this.tpDoctorInfo.Controls.Add(this.pictureBox3);
            this.tpDoctorInfo.Controls.Add(this.label6);
            this.tpDoctorInfo.Controls.Add(this.txtConsultationFees);
            this.tpDoctorInfo.Controls.Add(this.txtLicenseNo);
            this.tpDoctorInfo.Controls.Add(this.pictureBox6);
            this.tpDoctorInfo.Controls.Add(this.label15);
            this.tpDoctorInfo.Controls.Add(this.pictureBox4);
            this.tpDoctorInfo.Controls.Add(this.label5);
            this.tpDoctorInfo.Controls.Add(this.clbWorkingDays);
            this.tpDoctorInfo.Controls.Add(this.label8);
            this.tpDoctorInfo.Controls.Add(this.lbDoctorID);
            this.tpDoctorInfo.Controls.Add(this.pictureBox5);
            this.tpDoctorInfo.Location = new System.Drawing.Point(4, 32);
            this.tpDoctorInfo.Name = "tpDoctorInfo";
            this.tpDoctorInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpDoctorInfo.Size = new System.Drawing.Size(923, 438);
            this.tpDoctorInfo.TabIndex = 1;
            this.tpDoctorInfo.Text = "Doctor Info";
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.chkIsActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.chkIsActive.Location = new System.Drawing.Point(413, 361);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(107, 29);
            this.chkIsActive.TabIndex = 4;
            this.chkIsActive.Text = "Is Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.label1.Location = new System.Drawing.Point(15, 165);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 25);
            this.label1.TabIndex = 164;
            this.label1.Text = "License No:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Clinic.Properties.Resources.ID_32;
            this.pictureBox1.Location = new System.Drawing.Point(138, 164);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(31, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 165;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Clinic.Properties.Resources.WorkingDays_32;
            this.pictureBox3.Location = new System.Drawing.Point(673, 275);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(31, 26);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 162;
            this.pictureBox3.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.label6.Location = new System.Drawing.Point(521, 276);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 25);
            this.label6.TabIndex = 161;
            this.label6.Text = "Working Days:";
            // 
            // txtConsultationFees
            // 
            this.txtConsultationFees.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtConsultationFees.Location = new System.Drawing.Point(222, 269);
            this.txtConsultationFees.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsultationFees.MaxLength = 50;
            this.txtConsultationFees.Name = "txtConsultationFees";
            this.txtConsultationFees.Size = new System.Drawing.Size(223, 32);
            this.txtConsultationFees.TabIndex = 1;
            // 
            // txtLicenseNo
            // 
            this.txtLicenseNo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtLicenseNo.Location = new System.Drawing.Point(206, 160);
            this.txtLicenseNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLicenseNo.MaxLength = 50;
            this.txtLicenseNo.Name = "txtLicenseNo";
            this.txtLicenseNo.Size = new System.Drawing.Size(223, 32);
            this.txtLicenseNo.TabIndex = 0;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::Clinic.Properties.Resources.money_32;
            this.pictureBox6.Location = new System.Drawing.Point(179, 269);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(31, 26);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 158;
            this.pictureBox6.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.label15.Location = new System.Drawing.Point(9, 268);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(169, 25);
            this.label15.TabIndex = 157;
            this.label15.Text = "Consultation Fees";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Clinic.Properties.Resources.Specialization_32;
            this.pictureBox4.Location = new System.Drawing.Point(675, 59);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(31, 26);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 156;
            this.pictureBox4.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.label5.Location = new System.Drawing.Point(524, 58);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 25);
            this.label5.TabIndex = 155;
            this.label5.Text = "Specialization:";
            // 
            // clbWorkingDays
            // 
            this.clbWorkingDays.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.clbWorkingDays.Items.AddRange(new object[] {
            "Sat",
            "Sun",
            "Mon",
            "Tue",
            "Wed",
            "Thu",
            "Fri"});
            this.clbWorkingDays.Location = new System.Drawing.Point(717, 245);
            this.clbWorkingDays.Name = "clbWorkingDays";
            this.clbWorkingDays.Size = new System.Drawing.Size(200, 85);
            this.clbWorkingDays.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.label8.Location = new System.Drawing.Point(25, 58);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 25);
            this.label8.TabIndex = 148;
            this.label8.Text = "Doctor ID:";
            // 
            // lbDoctorID
            // 
            this.lbDoctorID.AutoSize = true;
            this.lbDoctorID.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lbDoctorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lbDoctorID.Location = new System.Drawing.Point(233, 58);
            this.lbDoctorID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDoctorID.Name = "lbDoctorID";
            this.lbDoctorID.Size = new System.Drawing.Size(36, 25);
            this.lbDoctorID.TabIndex = 129;
            this.lbDoctorID.Text = "???";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::Clinic.Properties.Resources.ID_32;
            this.pictureBox5.Location = new System.Drawing.Point(194, 59);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(31, 26);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 153;
            this.pictureBox5.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTitle.Location = new System.Drawing.Point(12, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(931, 45);
            this.lblTitle.TabIndex = 118;
            this.lblTitle.Text = "Add New Doctor";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Enabled = false;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnSave.Image = global::Clinic.Properties.Resources.Save_32;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(813, 590);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(126, 37);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "   Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnClose.Image = global::Clinic.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(679, 590);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 37);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "   Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // clbSpecialization
            // 
            this.clbSpecialization.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.clbSpecialization.Location = new System.Drawing.Point(721, 44);
            this.clbSpecialization.Name = "clbSpecialization";
            this.clbSpecialization.Size = new System.Drawing.Size(194, 112);
            this.clbSpecialization.TabIndex = 2;
            // 
            // ctrlPersonCardWithFilter1
            // 
            this.ctrlPersonCardWithFilter1.BackColor = System.Drawing.Color.White;
            this.ctrlPersonCardWithFilter1.btnAddNewEnabled = true;
            this.ctrlPersonCardWithFilter1.FilterEnabled = false;
            this.ctrlPersonCardWithFilter1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ctrlPersonCardWithFilter1.Location = new System.Drawing.Point(4, 4);
            this.ctrlPersonCardWithFilter1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlPersonCardWithFilter1.Name = "ctrlPersonCardWithFilter1";
            this.ctrlPersonCardWithFilter1.Size = new System.Drawing.Size(916, 375);
            this.ctrlPersonCardWithFilter1.TabIndex = 119;
            // 
            // frmAddUpdateDoctor
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(960, 650);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.tcDoctor);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmAddUpdateDoctor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add/Update Doctor";
            this.tcDoctor.ResumeLayout(false);
            this.tpPersonalInfo.ResumeLayout(false);
            this.tpDoctorInfo.ResumeLayout(false);
            this.tpDoctorInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tcDoctor;
        private System.Windows.Forms.TabPage tpPersonalInfo;
        private System.Windows.Forms.TabPage tpDoctorInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lbDoctorID;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnPersonInfoNext;
        private Clinic.Controls.ctrlPersonCardWithFilter ctrlPersonCardWithFilter1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.CheckedListBox clbWorkingDays;
        private System.Windows.Forms.TextBox txtConsultationFees;
        private System.Windows.Forms.TextBox txtLicenseNo;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.CheckedListBox clbSpecialization;
    }
}