using System.Windows.Forms;

namespace Clinic.Medical_Services.Pharmaciy
{
    partial class frmAllPrescriptions
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbCurrentUser = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbBlaceholder = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dgPrescriptionDetails = new System.Windows.Forms.DataGridView();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrescriptionDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.panTitle.Controls.Add(this.label5);
            this.panTitle.Controls.Add(this.pictureBox4);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lbCurrentUser);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(20, 20);
            this.panTitle.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1398, 69);
            this.panTitle.TabIndex = 253;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label5.Location = new System.Drawing.Point(1030, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 31);
            this.label5.TabIndex = 240;
            this.label5.Text = "Current User:";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = global::Clinic.Properties.Resources.Person_32;
            this.pictureBox4.Location = new System.Drawing.Point(410, 23);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(41, 31);
            this.pictureBox4.TabIndex = 255;
            this.pictureBox4.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnExit.Image = global::Clinic.Properties.Resources.sign_out_32__2;
            this.btnExit.Location = new System.Drawing.Point(1280, 12);
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
            this.lbCurrentUser.Location = new System.Drawing.Point(1092, 30);
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
            this.lblTitle.Location = new System.Drawing.Point(444, 12);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(386, 45);
            this.lblTitle.TabIndex = 236;
            this.lblTitle.Text = "Pharmacist Direct Entry";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbStatus);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbBlaceholder);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(20, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1398, 87);
            this.panel1.TabIndex = 254;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "All",
            "Pending",
            "Waiting For Payment",
            "Ready For Dispensing",
            "Dispensed",
            "Partially Dispensed",
            "Cancelled"});
            this.cmbStatus.Location = new System.Drawing.Point(1005, 23);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(181, 36);
            this.cmbStatus.TabIndex = 257;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(824, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 31);
            this.label1.TabIndex = 256;
            this.label1.Text = "Fillter by Status:";
            // 
            // lbBlaceholder
            // 
            this.lbBlaceholder.AutoSize = true;
            this.lbBlaceholder.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBlaceholder.ForeColor = System.Drawing.Color.Gray;
            this.lbBlaceholder.Location = new System.Drawing.Point(141, 22);
            this.lbBlaceholder.Name = "lbBlaceholder";
            this.lbBlaceholder.Size = new System.Drawing.Size(223, 25);
            this.lbBlaceholder.TabIndex = 255;
            this.lbBlaceholder.Text = "Search By Patient Name .....";
            this.lbBlaceholder.Click += new System.EventHandler(this.lbBlaceholder_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(127, 19);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(417, 38);
            this.txtSearch.TabIndex = 254;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(28, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 31);
            this.label6.TabIndex = 253;
            this.label6.Text = "Search :";
            // 
            // dgPrescriptionDetails
            // 
            this.dgPrescriptionDetails.AllowUserToAddRows = false;
            this.dgPrescriptionDetails.AllowUserToDeleteRows = false;
            this.dgPrescriptionDetails.AllowUserToResizeRows = false;
            this.dgPrescriptionDetails.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.dgPrescriptionDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgPrescriptionDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrescriptionDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgPrescriptionDetails.ColumnHeadersHeight = 80;
            this.dgPrescriptionDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgPrescriptionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPrescriptionDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgPrescriptionDetails.EnableHeadersVisualStyles = false;
            this.dgPrescriptionDetails.GridColor = System.Drawing.Color.MintCream;
            this.dgPrescriptionDetails.Location = new System.Drawing.Point(20, 176);
            this.dgPrescriptionDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgPrescriptionDetails.MultiSelect = false;
            this.dgPrescriptionDetails.Name = "dgPrescriptionDetails";
            this.dgPrescriptionDetails.ReadOnly = true;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(193)))), ((int)(((byte)(218)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            this.dgPrescriptionDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgPrescriptionDetails.RowHeadersVisible = false;
            this.dgPrescriptionDetails.RowHeadersWidth = 30;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrescriptionDetails.RowTemplate.Height = 70;
            this.dgPrescriptionDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPrescriptionDetails.Size = new System.Drawing.Size(1398, 633);
            this.dgPrescriptionDetails.TabIndex = 255;
            this.dgPrescriptionDetails.TabStop = false;
            // 
            // frmAllPrescriptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(1438, 829);
            this.Controls.Add(this.dgPrescriptionDetails);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Cornsilk;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmAllPrescriptions";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAllPrescriptions";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrescriptionDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lbCurrentUser;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgPrescriptionDetails;
        private ComboBox cmbStatus;
        private Label label1;
        private Label lbBlaceholder;
        private TextBox txtSearch;
        private Label label6;
    }
}