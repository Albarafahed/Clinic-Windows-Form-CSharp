using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Vital
{
    partial class frmListVital
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
            this.dgvNurseQueue = new System.Windows.Forms.DataGridView();
            this.gbVitals = new System.Windows.Forms.GroupBox();
            this.lblBP = new System.Windows.Forms.Label();
            this.txtBloodPressure = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTemperature = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPulse = new System.Windows.Forms.TextBox();
            this.lblRecordsCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lbDate = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNurseQueue)).BeginInit();
            this.gbVitals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvNurseQueue
            // 
            this.dgvNurseQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNurseQueue.BackgroundColor = System.Drawing.Color.White;
            this.dgvNurseQueue.ColumnHeadersHeight = 29;
            this.dgvNurseQueue.Location = new System.Drawing.Point(12, 56);
            this.dgvNurseQueue.Name = "dgvNurseQueue";
            this.dgvNurseQueue.ReadOnly = true;
            this.dgvNurseQueue.RowHeadersWidth = 51;
            this.dgvNurseQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNurseQueue.Size = new System.Drawing.Size(540, 526);
            this.dgvNurseQueue.TabIndex = 0;
            // 
            // gbVitals
            // 
            this.gbVitals.Controls.Add(this.label4);
            this.gbVitals.Controls.Add(this.txtPulse);
            this.gbVitals.Controls.Add(this.label3);
            this.gbVitals.Controls.Add(this.txtWeight);
            this.gbVitals.Controls.Add(this.label2);
            this.gbVitals.Controls.Add(this.txtTemperature);
            this.gbVitals.Controls.Add(this.label1);
            this.gbVitals.Controls.Add(this.lblBP);
            this.gbVitals.Controls.Add(this.txtBloodPressure);
            this.gbVitals.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.gbVitals.Location = new System.Drawing.Point(558, 56);
            this.gbVitals.Name = "gbVitals";
            this.gbVitals.Size = new System.Drawing.Size(530, 465);
            this.gbVitals.TabIndex = 1;
            this.gbVitals.TabStop = false;
            this.gbVitals.Text = "بيانات القياس الحيوية";
            // 
            // lblBP
            // 
            this.lblBP.Location = new System.Drawing.Point(188, 120);
            this.lblBP.Name = "lblBP";
            this.lblBP.Size = new System.Drawing.Size(100, 23);
            this.lblBP.TabIndex = 0;
            // 
            // txtBloodPressure
            // 
            this.txtBloodPressure.Location = new System.Drawing.Point(299, 85);
            this.txtBloodPressure.Name = "txtBloodPressure";
            this.txtBloodPressure.Size = new System.Drawing.Size(201, 32);
            this.txtBloodPressure.TabIndex = 1;
            this.txtBloodPressure.Validating += new System.ComponentModel.CancelEventHandler(this.txtValidating);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(558, 537);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(250, 60);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "حفظ وإرسال للطبيب";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Turquoise;
            this.label1.ForeColor = System.Drawing.Color.DarkGreen;
            this.label1.Location = new System.Drawing.Point(137, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "BloodPressure :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Turquoise;
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(137, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 25);
            this.label2.TabIndex = 12;
            this.label2.Text = "Temperature :";
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(299, 144);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(201, 32);
            this.txtTemperature.TabIndex = 11;
            this.txtTemperature.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeyPress);
            this.txtTemperature.Validating += new System.ComponentModel.CancelEventHandler(this.txtValidating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Turquoise;
            this.label3.ForeColor = System.Drawing.Color.DarkGreen;
            this.label3.Location = new System.Drawing.Point(188, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 25);
            this.label3.TabIndex = 14;
            this.label3.Text = "Weight:";
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(299, 203);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(201, 32);
            this.txtWeight.TabIndex = 13;
            this.txtWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeyPress);
            this.txtWeight.Validating += new System.ComponentModel.CancelEventHandler(this.txtValidating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Turquoise;
            this.label4.ForeColor = System.Drawing.Color.DarkGreen;
            this.label4.Location = new System.Drawing.Point(199, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 25);
            this.label4.TabIndex = 16;
            this.label4.Text = "Pulse :";
            // 
            // txtPulse
            // 
            this.txtPulse.Location = new System.Drawing.Point(299, 262);
            this.txtPulse.Name = "txtPulse";
            this.txtPulse.Size = new System.Drawing.Size(201, 32);
            this.txtPulse.TabIndex = 15;
            this.txtPulse.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeyPress);
            this.txtPulse.Validating += new System.ComponentModel.CancelEventHandler(this.txtValidating);
            // 
            // lblRecordsCount
            // 
            this.lblRecordsCount.AutoSize = true;
            this.lblRecordsCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblRecordsCount.Location = new System.Drawing.Point(117, 606);
            this.lblRecordsCount.Name = "lblRecordsCount";
            this.lblRecordsCount.Size = new System.Drawing.Size(28, 25);
            this.lblRecordsCount.TabIndex = 100;
            this.lblRecordsCount.Text = "??";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.label5.Location = new System.Drawing.Point(12, 606);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 25);
            this.label5.TabIndex = 99;
            this.label5.Text = "# Records:";
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(362, 2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(126, 48);
            this.btnUpdate.TabIndex = 101;
            this.btnUpdate.Text = "تحديث";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDate.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbDate.Location = new System.Drawing.Point(25, 2);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(120, 50);
            this.lbDate.TabIndex = 102;
            this.lbDate.Text = "label1";
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(962, 593);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 48);
            this.btnClose.TabIndex = 103;
            this.btnClose.Text = "اغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmListVital
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbDate);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lblRecordsCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvNurseQueue);
            this.Controls.Add(this.gbVitals);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmListVital";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "محطة الممرض - سجل القياسات الحيوية";
            this.Load += new System.EventHandler(this.frmListVital_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNurseQueue)).EndInit();
            this.gbVitals.ResumeLayout(false);
            this.gbVitals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

        #endregion

       private System.Windows.Forms.GroupBox gbVitals;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvNurseQueue;
        private System.Windows.Forms.TextBox txtBloodPressure;
        private Label lblBP;
        private Label label4;
        private TextBox txtPulse;
        private Label label3;
        private TextBox txtWeight;
        private Label label2;
        private TextBox txtTemperature;
        private Label label1;
        private Label lblRecordsCount;
        private Label label5;
        private Timer timer1;
        private Button btnUpdate;
        private Label lbDate;
        private Timer timer2;
        private Button btnClose;
        private ErrorProvider errorProvider1;
    }
}