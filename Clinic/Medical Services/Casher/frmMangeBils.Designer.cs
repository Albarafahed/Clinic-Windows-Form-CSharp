namespace Clinic.Medical_Services.Casher
{
    partial class frmMangeBils
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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbCurrentUser = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgPrescriptionDetails = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chePaid = new System.Windows.Forms.CheckBox();
            this.chePatial = new System.Windows.Forms.CheckBox();
            this.chePending = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSearchByPatientName = new System.Windows.Forms.TextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrescriptionDetails)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(114)))), ((int)(((byte)(95)))));
            this.panTitle.Controls.Add(this.label5);
            this.panTitle.Controls.Add(this.pictureBox4);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lbCurrentUser);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(20, 20);
            this.panTitle.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1398, 89);
            this.panTitle.TabIndex = 253;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label5.Location = new System.Drawing.Point(1029, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 31);
            this.label5.TabIndex = 240;
            this.label5.Text = "Current User:";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnExit.Image = global::Clinic.Properties.Resources.sign_out_32__2;
            this.btnExit.Location = new System.Drawing.Point(1281, 22);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(97, 42);
            this.btnExit.TabIndex = 254;
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lbCurrentUser
            // 
            this.lbCurrentUser.AutoSize = true;
            this.lbCurrentUser.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCurrentUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lbCurrentUser.Location = new System.Drawing.Point(1091, 45);
            this.lbCurrentUser.Name = "lbCurrentUser";
            this.lbCurrentUser.Size = new System.Drawing.Size(60, 31);
            this.lbCurrentUser.TabIndex = 241;
            this.lbCurrentUser.Text = "[???]";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblTitle.Location = new System.Drawing.Point(424, 22);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(485, 45);
            this.lblTitle.TabIndex = 236;
            this.lblTitle.Text = "MANAGE BILLS - System View";
            // 
            // dgPrescriptionDetails
            // 
            this.dgPrescriptionDetails.AllowUserToAddRows = false;
            this.dgPrescriptionDetails.AllowUserToDeleteRows = false;
            this.dgPrescriptionDetails.AllowUserToResizeRows = false;
            this.dgPrescriptionDetails.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(42)))), ((int)(((byte)(45)))));
            this.dgPrescriptionDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgPrescriptionDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(42)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrescriptionDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgPrescriptionDetails.ColumnHeadersHeight = 80;
            this.dgPrescriptionDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgPrescriptionDetails.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgPrescriptionDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgPrescriptionDetails.EnableHeadersVisualStyles = false;
            this.dgPrescriptionDetails.GridColor = System.Drawing.Color.MintCream;
            this.dgPrescriptionDetails.Location = new System.Drawing.Point(421, 129);
            this.dgPrescriptionDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgPrescriptionDetails.MultiSelect = false;
            this.dgPrescriptionDetails.Name = "dgPrescriptionDetails";
            this.dgPrescriptionDetails.ReadOnly = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(193)))), ((int)(((byte)(218)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dgPrescriptionDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgPrescriptionDetails.RowHeadersVisible = false;
            this.dgPrescriptionDetails.RowHeadersWidth = 30;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrescriptionDetails.RowTemplate.Height = 70;
            this.dgPrescriptionDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPrescriptionDetails.Size = new System.Drawing.Size(997, 680);
            this.dgPrescriptionDetails.TabIndex = 255;
            this.dgPrescriptionDetails.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(416, 129);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 680);
            this.panel2.TabIndex = 256;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(114)))), ((int)(((byte)(95)))));
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(23, 129);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(377, 60);
            this.flowLayoutPanel1.TabIndex = 257;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(464, 0);
            this.flowLayoutPanel2.TabIndex = 258;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(292, 38);
            this.label2.TabIndex = 262;
            this.label2.Text = "Bill Search And Filter";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(20, 109);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1398, 20);
            this.panel1.TabIndex = 254;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(30, 260);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(328, 38);
            this.dateTimePicker1.TabIndex = 259;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chePaid);
            this.panel3.Controls.Add(this.chePatial);
            this.panel3.Controls.Add(this.chePending);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(30, 321);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(328, 218);
            this.panel3.TabIndex = 260;
            // 
            // chePaid
            // 
            this.chePaid.AutoSize = true;
            this.chePaid.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.chePaid.ForeColor = System.Drawing.Color.White;
            this.chePaid.Location = new System.Drawing.Point(16, 146);
            this.chePaid.Name = "chePaid";
            this.chePaid.Size = new System.Drawing.Size(80, 35);
            this.chePaid.TabIndex = 263;
            this.chePaid.Text = "Paid";
            this.chePaid.UseVisualStyleBackColor = true;
            this.chePaid.Paint += new System.Windows.Forms.PaintEventHandler(this.checkBox_Paint);
            // 
            // chePatial
            // 
            this.chePatial.AutoSize = true;
            this.chePatial.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold);
            this.chePatial.ForeColor = System.Drawing.Color.White;
            this.chePatial.Location = new System.Drawing.Point(16, 107);
            this.chePatial.Name = "chePatial";
            this.chePatial.Size = new System.Drawing.Size(102, 35);
            this.chePatial.TabIndex = 262;
            this.chePatial.Text = "Partial";
            this.chePatial.UseVisualStyleBackColor = true;
            this.chePatial.Paint += new System.Windows.Forms.PaintEventHandler(this.checkBox_Paint);
            // 
            // chePending
            // 
            this.chePending.AutoSize = true;
            this.chePending.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chePending.ForeColor = System.Drawing.Color.White;
            this.chePending.Location = new System.Drawing.Point(16, 66);
            this.chePending.Name = "chePending";
            this.chePending.Size = new System.Drawing.Size(120, 35);
            this.chePending.TabIndex = 261;
            this.chePending.Text = "Pending";
            this.chePending.UseVisualStyleBackColor = true;
            this.chePending.Paint += new System.Windows.Forms.PaintEventHandler(this.checkBox_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 38);
            this.label1.TabIndex = 261;
            this.label1.Text = "Payment Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(13, 595);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 38);
            this.label4.TabIndex = 264;
            this.label4.Text = "Patient Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(27, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 38);
            this.label6.TabIndex = 264;
            this.label6.Text = "Date Range";
            // 
            // txtSearchByPatientName
            // 
            this.txtSearchByPatientName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(72)))), ((int)(((byte)(75)))));
            this.txtSearchByPatientName.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchByPatientName.ForeColor = System.Drawing.Color.White;
            this.txtSearchByPatientName.Location = new System.Drawing.Point(8, 645);
            this.txtSearchByPatientName.Name = "txtSearchByPatientName";
            this.txtSearchByPatientName.Size = new System.Drawing.Size(368, 38);
            this.txtSearchByPatientName.TabIndex = 305;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = global::Clinic.Properties.Resources.Bill_64;
            this.pictureBox4.Location = new System.Drawing.Point(331, 17);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(70, 59);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 255;
            this.pictureBox4.TabStop = false;
            // 
            // frmMangeBils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(42)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(1438, 829);
            this.Controls.Add(this.txtSearchByPatientName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgPrescriptionDetails);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Cornsilk;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmMangeBils";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAllPrescriptions";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrescriptionDetails)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lbCurrentUser;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgPrescriptionDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chePending;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chePaid;
        private System.Windows.Forms.CheckBox chePatial;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSearchByPatientName;
        private System.Windows.Forms.PictureBox pictureBox4;
    }
}