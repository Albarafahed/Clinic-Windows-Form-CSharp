using System.Drawing;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Pharmaciy
{
    partial class frmPrescriptionDispnsing
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbCurrentUser = new System.Windows.Forms.Label();
            this.lbDate = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblPlaceholderSer = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvPrescriptions = new System.Windows.Forms.DataGridView();
            this.panel11 = new System.Windows.Forms.Panel();
            this.lbPatientName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lbOrderID = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPrintShotageSlip = new System.Windows.Forms.Button();
            this.btnSendToAccounting = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.dgPrescriptionDetails = new System.Windows.Forms.DataGridView();
            this.panel16 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnCancelOrder = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.btnDispense = new System.Windows.Forms.Button();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lbX = new System.Windows.Forms.Label();
            this.lbNotReady = new System.Windows.Forms.Label();
            this.lbWaitingforPayment = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbOrderNumber = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnNextExpected = new System.Windows.Forms.Button();
            this.btnReprtShortage = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrescriptions)).BeginInit();
            this.panel11.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrescriptionDetails)).BeginInit();
            this.panel16.SuspendLayout();
            this.panel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.panel15.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.panTitle.Controls.Add(this.label5);
            this.panTitle.Controls.Add(this.pictureBox4);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lbCurrentUser);
            this.panTitle.Controls.Add(this.lbDate);
            this.panTitle.Controls.Add(this.label24);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(10, 10);
            this.panTitle.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1534, 69);
            this.panTitle.TabIndex = 238;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label5.Location = new System.Drawing.Point(1202, 9);
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
            this.pictureBox4.Location = new System.Drawing.Point(318, 19);
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
            this.btnExit.Location = new System.Drawing.Point(1365, 18);
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
            this.lbCurrentUser.Location = new System.Drawing.Point(1228, 32);
            this.lbCurrentUser.Name = "lbCurrentUser";
            this.lbCurrentUser.Size = new System.Drawing.Size(60, 31);
            this.lbCurrentUser.TabIndex = 241;
            this.lbCurrentUser.Text = "[???]";
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lbDate.Location = new System.Drawing.Point(7, 9);
            this.lbDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(178, 45);
            this.lbDate.TabIndex = 239;
            this.lbDate.Text = "Date Time";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label24.Location = new System.Drawing.Point(616, 20);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(342, 41);
            this.label24.TabIndex = 238;
            this.label24.Text = "Prescription Dispensing";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblTitle.Location = new System.Drawing.Point(350, 16);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(321, 45);
            this.lblTitle.TabIndex = 236;
            this.lblTitle.Text = "Pharmacist Station:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(210)))), ((int)(((byte)(222)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1534, 10);
            this.panel1.TabIndex = 239;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 89);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1534, 383);
            this.panel2.TabIndex = 240;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(237)))), ((int)(((byte)(239)))));
            this.panel7.Controls.Add(this.lblPlaceholderSer);
            this.panel7.Controls.Add(this.txtSearch);
            this.panel7.Controls.Add(this.dgvPrescriptions);
            this.panel7.Controls.Add(this.panel11);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1209, 383);
            this.panel7.TabIndex = 2;
            // 
            // lblPlaceholderSer
            // 
            this.lblPlaceholderSer.AutoSize = true;
            this.lblPlaceholderSer.BackColor = System.Drawing.Color.White;
            this.lblPlaceholderSer.ForeColor = System.Drawing.Color.Gray;
            this.lblPlaceholderSer.Location = new System.Drawing.Point(27, 342);
            this.lblPlaceholderSer.Name = "lblPlaceholderSer";
            this.lblPlaceholderSer.Size = new System.Drawing.Size(291, 28);
            this.lblPlaceholderSer.TabIndex = 245;
            this.lblPlaceholderSer.Text = "Search by Patient Name..................";
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(3, 342);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(626, 41);
            this.txtSearch.TabIndex = 243;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dgvPrescriptions
            // 
            this.dgvPrescriptions.AllowUserToAddRows = false;
            this.dgvPrescriptions.AllowUserToDeleteRows = false;
            this.dgvPrescriptions.AllowUserToResizeRows = false;
            this.dgvPrescriptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrescriptions.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(210)))), ((int)(((byte)(222)))));
            this.dgvPrescriptions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrescriptions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPrescriptions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrescriptions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPrescriptions.ColumnHeadersHeight = 40;
            this.dgvPrescriptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPrescriptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvPrescriptions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPrescriptions.EnableHeadersVisualStyles = false;
            this.dgvPrescriptions.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvPrescriptions.Location = new System.Drawing.Point(0, 70);
            this.dgvPrescriptions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvPrescriptions.MultiSelect = false;
            this.dgvPrescriptions.Name = "dgvPrescriptions";
            this.dgvPrescriptions.ReadOnly = true;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(193)))), ((int)(((byte)(218)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPrescriptions.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPrescriptions.RowHeadersWidth = 30;
            this.dgvPrescriptions.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvPrescriptions.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.dgvPrescriptions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.dgvPrescriptions.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dgvPrescriptions.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.dgvPrescriptions.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPrescriptions.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrescriptions.RowTemplate.Height = 50;
            this.dgvPrescriptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrescriptions.Size = new System.Drawing.Size(1209, 308);
            this.dgvPrescriptions.TabIndex = 248;
            this.dgvPrescriptions.TabStop = false;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(121)))), ((int)(((byte)(162)))));
            this.panel11.Controls.Add(this.lbPatientName);
            this.panel11.Controls.Add(this.label1);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(1209, 70);
            this.panel11.TabIndex = 241;
            // 
            // lbPatientName
            // 
            this.lbPatientName.AutoSize = true;
            this.lbPatientName.BackColor = System.Drawing.Color.Transparent;
            this.lbPatientName.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold);
            this.lbPatientName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lbPatientName.Location = new System.Drawing.Point(506, 9);
            this.lbPatientName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPatientName.Name = "lbPatientName";
            this.lbPatientName.Size = new System.Drawing.Size(134, 45);
            this.lbPatientName.TabIndex = 243;
            this.lbPatientName.Text = " [???]   )";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(785, 45);
            this.label1.TabIndex = 240;
            this.label1.Text = "Tab 1: Pending Prescriptions ( Patient:                    ";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(237)))), ((int)(((byte)(239)))));
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(1209, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(14, 383);
            this.panel6.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(234)))), ((int)(((byte)(236)))));
            this.panel5.Controls.Add(this.lbOrderID);
            this.panel5.Controls.Add(this.pictureBox3);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.pictureBox2);
            this.panel5.Controls.Add(this.pictureBox1);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.btnPrintShotageSlip);
            this.panel5.Controls.Add(this.btnSendToAccounting);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1223, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(311, 383);
            this.panel5.TabIndex = 0;
            // 
            // lbOrderID
            // 
            this.lbOrderID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOrderID.AutoSize = true;
            this.lbOrderID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.lbOrderID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbOrderID.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOrderID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lbOrderID.Location = new System.Drawing.Point(173, 146);
            this.lbOrderID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbOrderID.Name = "lbOrderID";
            this.lbOrderID.Size = new System.Drawing.Size(52, 23);
            this.lbOrderID.TabIndex = 255;
            this.lbOrderID.Text = "[???])";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(135)))), ((int)(((byte)(39)))));
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Image = global::Clinic.Properties.Resources.Prescription_64;
            this.pictureBox3.Location = new System.Drawing.Point(193, 254);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(72, 74);
            this.pictureBox3.TabIndex = 254;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.btnPrintShotageSlip_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(135)))), ((int)(((byte)(39)))));
            this.label10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label10.Location = new System.Drawing.Point(63, 316);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 38);
            this.label10.TabIndex = 253;
            this.label10.Text = "Slip";
            this.label10.Click += new System.EventHandler(this.btnPrintShotageSlip_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(135)))), ((int)(((byte)(39)))));
            this.label9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label9.Location = new System.Drawing.Point(29, 275);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 38);
            this.label9.TabIndex = 252;
            this.label9.Text = "Shortage";
            this.label9.Click += new System.EventHandler(this.btnPrintShotageSlip_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(135)))), ((int)(((byte)(39)))));
            this.label8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label8.Location = new System.Drawing.Point(63, 234);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 38);
            this.label8.TabIndex = 251;
            this.label8.Text = "Print";
            this.label8.Click += new System.EventHandler(this.btnPrintShotageSlip_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.label7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label7.Location = new System.Drawing.Point(24, 146);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(184, 23);
            this.label7.TabIndex = 250;
            this.label7.Text = "Prepare Invoice (Order";
            this.label7.Click += new System.EventHandler(this.btnSendToAccounting_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label6.Location = new System.Drawing.Point(21, 105);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 38);
            this.label6.TabIndex = 249;
            this.label6.Text = "Accounting";
            this.label6.Click += new System.EventHandler(this.btnSendToAccounting_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::Clinic.Properties.Resources.Payment_64;
            this.pictureBox2.Location = new System.Drawing.Point(194, 72);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(87, 71);
            this.pictureBox2.TabIndex = 248;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.btnSendToAccounting_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::Clinic.Properties.Resources.Medicine_32;
            this.pictureBox1.Location = new System.Drawing.Point(139, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(37, 34);
            this.pictureBox1.TabIndex = 247;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.btnSendToAccounting_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label3.Location = new System.Drawing.Point(47, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 38);
            this.label3.TabIndex = 242;
            this.label3.Text = "Send To";
            this.label3.Click += new System.EventHandler(this.btnSendToAccounting_Click);
            // 
            // btnPrintShotageSlip
            // 
            this.btnPrintShotageSlip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(135)))), ((int)(((byte)(39)))));
            this.btnPrintShotageSlip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrintShotageSlip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintShotageSlip.Location = new System.Drawing.Point(19, 206);
            this.btnPrintShotageSlip.Name = "btnPrintShotageSlip";
            this.btnPrintShotageSlip.Size = new System.Drawing.Size(276, 171);
            this.btnPrintShotageSlip.TabIndex = 246;
            this.btnPrintShotageSlip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintShotageSlip.UseVisualStyleBackColor = false;
            this.btnPrintShotageSlip.Click += new System.EventHandler(this.btnPrintShotageSlip_Click);
            // 
            // btnSendToAccounting
            // 
            this.btnSendToAccounting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.btnSendToAccounting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendToAccounting.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSendToAccounting.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold);
            this.btnSendToAccounting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnSendToAccounting.Location = new System.Drawing.Point(19, 9);
            this.btnSendToAccounting.Name = "btnSendToAccounting";
            this.btnSendToAccounting.Size = new System.Drawing.Size(276, 171);
            this.btnSendToAccounting.TabIndex = 245;
            this.btnSendToAccounting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSendToAccounting.UseVisualStyleBackColor = false;
            this.btnSendToAccounting.Click += new System.EventHandler(this.btnSendToAccounting_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(210)))), ((int)(((byte)(222)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 472);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1534, 10);
            this.panel3.TabIndex = 241;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Controls.Add(this.panel10);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(10, 482);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1534, 318);
            this.panel4.TabIndex = 242;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel12);
            this.panel8.Controls.Add(this.panel14);
            this.panel8.Controls.Add(this.panel13);
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1197, 318);
            this.panel8.TabIndex = 5;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.dgPrescriptionDetails);
            this.panel12.Controls.Add(this.panel16);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(859, 318);
            this.panel12.TabIndex = 0;
            // 
            // dgPrescriptionDetails
            // 
            this.dgPrescriptionDetails.AllowUserToAddRows = false;
            this.dgPrescriptionDetails.AllowUserToDeleteRows = false;
            this.dgPrescriptionDetails.AllowUserToResizeRows = false;
            this.dgPrescriptionDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgPrescriptionDetails.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(210)))), ((int)(((byte)(222)))));
            this.dgPrescriptionDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgPrescriptionDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgPrescriptionDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrescriptionDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgPrescriptionDetails.ColumnHeadersHeight = 40;
            this.dgPrescriptionDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgPrescriptionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPrescriptionDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgPrescriptionDetails.EnableHeadersVisualStyles = false;
            this.dgPrescriptionDetails.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgPrescriptionDetails.Location = new System.Drawing.Point(0, 72);
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
            this.dgPrescriptionDetails.RowHeadersWidth = 30;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(98)))), ((int)(((byte)(149)))));
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgPrescriptionDetails.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrescriptionDetails.RowTemplate.Height = 50;
            this.dgPrescriptionDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPrescriptionDetails.Size = new System.Drawing.Size(859, 246);
            this.dgPrescriptionDetails.TabIndex = 247;
            this.dgPrescriptionDetails.TabStop = false;
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(121)))), ((int)(((byte)(162)))));
            this.panel16.Controls.Add(this.label4);
            this.panel16.Controls.Add(this.label2);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel16.Location = new System.Drawing.Point(0, 0);
            this.panel16.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(859, 72);
            this.panel16.TabIndex = 244;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label4.Location = new System.Drawing.Point(376, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 41);
            this.label4.TabIndex = 236;
            this.label4.Text = "[Patient Name]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label2.Location = new System.Drawing.Point(24, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(430, 45);
            this.label2.TabIndex = 239;
            this.label2.Text = "Tab 2: Prescription Details:";
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(234)))), ((int)(((byte)(236)))));
            this.panel14.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel14.Location = new System.Drawing.Point(859, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(20, 318);
            this.panel14.TabIndex = 245;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.label20);
            this.panel13.Controls.Add(this.label19);
            this.panel13.Controls.Add(this.label16);
            this.panel13.Controls.Add(this.btnCancelOrder);
            this.panel13.Controls.Add(this.label17);
            this.panel13.Controls.Add(this.pictureBox6);
            this.panel13.Controls.Add(this.label18);
            this.panel13.Controls.Add(this.btnDispense);
            this.panel13.Controls.Add(this.panel15);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel13.Location = new System.Drawing.Point(879, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(318, 318);
            this.panel13.TabIndex = 244;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label20.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label20.Location = new System.Drawing.Point(252, 163);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 41);
            this.label20.TabIndex = 259;
            this.label20.Text = "✓";
            this.label20.Click += new System.EventHandler(this.btnDispense_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Brown;
            this.label19.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label19.Location = new System.Drawing.Point(242, 252);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 41);
            this.label19.TabIndex = 258;
            this.label19.Text = "×";
            this.label19.Click += new System.EventHandler(this.btnCancelOrder_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Brown;
            this.label16.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label16.Location = new System.Drawing.Point(43, 255);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(191, 31);
            this.label16.TabIndex = 257;
            this.label16.Text = "**Cancel Order**";
            this.label16.Click += new System.EventHandler(this.btnCancelOrder_Click);
            // 
            // btnCancelOrder
            // 
            this.btnCancelOrder.BackColor = System.Drawing.Color.Brown;
            this.btnCancelOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelOrder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancelOrder.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold);
            this.btnCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnCancelOrder.Location = new System.Drawing.Point(20, 241);
            this.btnCancelOrder.Name = "btnCancelOrder";
            this.btnCancelOrder.Size = new System.Drawing.Size(276, 67);
            this.btnCancelOrder.TabIndex = 256;
            this.btnCancelOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelOrder.UseVisualStyleBackColor = false;
            this.btnCancelOrder.Click += new System.EventHandler(this.btnCancelOrder_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label17.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label17.Location = new System.Drawing.Point(48, 166);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(206, 41);
            this.label17.TabIndex = 255;
            this.label17.Text = "Medication**";
            this.label17.Click += new System.EventHandler(this.btnDispense_Click);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pictureBox6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox6.Image = global::Clinic.Properties.Resources.Medicine_32;
            this.pictureBox6.Location = new System.Drawing.Point(31, 129);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(37, 34);
            this.pictureBox6.TabIndex = 253;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.btnDispense_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label18.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label18.Location = new System.Drawing.Point(96, 122);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(172, 41);
            this.label18.TabIndex = 251;
            this.label18.Text = "**Dispense";
            this.label18.Click += new System.EventHandler(this.btnDispense_Click);
            // 
            // btnDispense
            // 
            this.btnDispense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnDispense.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDispense.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDispense.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold);
            this.btnDispense.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnDispense.Location = new System.Drawing.Point(21, 74);
            this.btnDispense.Name = "btnDispense";
            this.btnDispense.Size = new System.Drawing.Size(276, 160);
            this.btnDispense.TabIndex = 252;
            this.btnDispense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDispense.UseVisualStyleBackColor = false;
            this.btnDispense.Click += new System.EventHandler(this.btnDispense_Click);
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(121)))), ((int)(((byte)(162)))));
            this.panel15.Controls.Add(this.label12);
            this.panel15.Controls.Add(this.label15);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel15.Location = new System.Drawing.Point(0, 0);
            this.panel15.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(318, 61);
            this.panel15.TabIndex = 241;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label12.Location = new System.Drawing.Point(494, 18);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(231, 41);
            this.label12.TabIndex = 236;
            this.label12.Text = "[Patient Name]";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label15.Location = new System.Drawing.Point(24, 9);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(186, 41);
            this.label15.TabIndex = 239;
            this.label15.Text = "Final Action";
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(234)))), ((int)(((byte)(236)))));
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(1197, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(20, 318);
            this.panel9.TabIndex = 4;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(234)))), ((int)(((byte)(236)))));
            this.panel10.Controls.Add(this.lbX);
            this.panel10.Controls.Add(this.lbNotReady);
            this.panel10.Controls.Add(this.lbWaitingforPayment);
            this.panel10.Controls.Add(this.label14);
            this.panel10.Controls.Add(this.label13);
            this.panel10.Controls.Add(this.lbOrderNumber);
            this.panel10.Controls.Add(this.label11);
            this.panel10.Controls.Add(this.btnNextExpected);
            this.panel10.Controls.Add(this.btnReprtShortage);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel10.Location = new System.Drawing.Point(1217, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(317, 318);
            this.panel10.TabIndex = 3;
            // 
            // lbX
            // 
            this.lbX.AutoSize = true;
            this.lbX.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX.ForeColor = System.Drawing.Color.Brown;
            this.lbX.Location = new System.Drawing.Point(253, 262);
            this.lbX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(34, 31);
            this.lbX.TabIndex = 253;
            this.lbX.Text = "✕";
            // 
            // lbNotReady
            // 
            this.lbNotReady.AutoSize = true;
            this.lbNotReady.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNotReady.ForeColor = System.Drawing.Color.Brown;
            this.lbNotReady.Location = new System.Drawing.Point(30, 262);
            this.lbNotReady.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNotReady.Name = "lbNotReady";
            this.lbNotReady.Size = new System.Drawing.Size(228, 23);
            this.lbNotReady.TabIndex = 252;
            this.lbNotReady.Text = "(Not Ready for Dispensing)";
            // 
            // lbWaitingforPayment
            // 
            this.lbWaitingforPayment.AutoSize = true;
            this.lbWaitingforPayment.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWaitingforPayment.ForeColor = System.Drawing.Color.Brown;
            this.lbWaitingforPayment.Location = new System.Drawing.Point(30, 231);
            this.lbWaitingforPayment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbWaitingforPayment.Name = "lbWaitingforPayment";
            this.lbWaitingforPayment.Size = new System.Drawing.Size(226, 31);
            this.lbWaitingforPayment.TabIndex = 251;
            this.lbWaitingforPayment.Text = "Waiting for Payment";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(25, 223);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(266, 85);
            this.label14.TabIndex = 250;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Brown;
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(21, 219);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(274, 94);
            this.label13.TabIndex = 249;
            // 
            // lbOrderNumber
            // 
            this.lbOrderNumber.AutoSize = true;
            this.lbOrderNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOrderNumber.ForeColor = System.Drawing.Color.Black;
            this.lbOrderNumber.Location = new System.Drawing.Point(21, 163);
            this.lbOrderNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbOrderNumber.Name = "lbOrderNumber";
            this.lbOrderNumber.Size = new System.Drawing.Size(192, 41);
            this.lbOrderNumber.TabIndex = 248;
            this.lbOrderNumber.Text = "(Order [???] )";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(10, 122);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(188, 41);
            this.label11.TabIndex = 242;
            this.label11.Text = "Biling Status";
            // 
            // btnNextExpected
            // 
            this.btnNextExpected.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnNextExpected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextExpected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextExpected.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextExpected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnNextExpected.Location = new System.Drawing.Point(19, 67);
            this.btnNextExpected.Name = "btnNextExpected";
            this.btnNextExpected.Size = new System.Drawing.Size(264, 52);
            this.btnNextExpected.TabIndex = 247;
            this.btnNextExpected.Text = "Next Expected Patient";
            this.btnNextExpected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNextExpected.UseVisualStyleBackColor = false;
            // 
            // btnReprtShortage
            // 
            this.btnReprtShortage.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnReprtShortage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReprtShortage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReprtShortage.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReprtShortage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnReprtShortage.Location = new System.Drawing.Point(17, 9);
            this.btnReprtShortage.Name = "btnReprtShortage";
            this.btnReprtShortage.Size = new System.Drawing.Size(264, 52);
            this.btnReprtShortage.TabIndex = 246;
            this.btnReprtShortage.Text = "Report Shortage";
            this.btnReprtShortage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReprtShortage.UseVisualStyleBackColor = false;
            // 
            // frmPrescriptionDispnsing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(237)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(1554, 827);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmPrescriptionDispnsing";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "frmPrescriptionDispnsing";
            this.Load += new System.EventHandler(this.frmPrescriptionDispnsing_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrescriptions)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgPrescriptionDetails)).EndInit();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnPrintShotageSlip;
        private System.Windows.Forms.Button btnSendToAccounting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPlaceholderSer;
        private System.Windows.Forms.Label lbCurrentUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnNextExpected;
        private System.Windows.Forms.Button btnReprtShortage;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbOrderNumber;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbWaitingforPayment;
        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.Label lbNotReady;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnDispense;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnCancelOrder;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbPatientName;
        private System.Windows.Forms.Label lbOrderID;
        private DataGridView dgPrescriptionDetails;
        private DataGridView dgvPrescriptions;
    }
}