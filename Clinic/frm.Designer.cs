using System.Drawing;
using System.Windows.Forms;

namespace Clinic
{
    partial class frm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlMainContainer = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblTitle.Location = new System.Drawing.Point(28, 21);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(233, 45);
            this.lblTitle.TabIndex = 239;
            this.lblTitle.Text = "Clinic Station:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.label24.Location = new System.Drawing.Point(269, 25);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(201, 41);
            this.label24.TabIndex = 240;
            this.label24.Text = "Main Staition";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnExit.Image = global::Clinic.Properties.Resources.sign_out_32__2;
            this.btnExit.Location = new System.Drawing.Point(1424, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(59, 42);
            this.btnExit.TabIndex = 255;
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1495, 88);
            this.panel1.TabIndex = 10;
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.BackColor = System.Drawing.Color.Black;
            this.pnlMainContainer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlMainContainer.Cursor = System.Windows.Forms.Cursors.No;
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.Image = global::Clinic.Properties.Resources.ClinicWallpaper;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 88);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1495, 764);
            this.pnlMainContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pnlMainContainer.TabIndex = 11;
            this.pnlMainContainer.TabStop = false;
            // 
            // frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1495, 852);
            this.Controls.Add(this.pnlMainContainer);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "frm";
            this.Text = "frmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.ResumeLayout(false);

        }




        private void _CreateStatsCards()
        {
            // مصفوفة بأسماء الكروت وقيم تجريبية
            string[] cardNames = { "Today's Appointments", "Waiting Room", "Today's Income" };
            Color[] cardColors = { Color.FromArgb(150, 0, 122, 204), Color.FromArgb(150, 255, 140, 0), Color.FromArgb(150, 46, 139, 87) };

            int xPos = 50; // مكان البداية من اليسار
            for (int i = 0; i < 3; i++)
            {
                Panel pnlCard = new Panel();
                pnlCard.Size = new Size(250, 100);
                pnlCard.Location = new Point(xPos, 100); // تحت المنيو بمسافة
                pnlCard.BackColor = cardColors[i]; // لون نصف شفاف
                pnlCard.BorderStyle = BorderStyle.None;

                Label lblTitle = new Label();
                lblTitle.Text = cardNames[i];
                lblTitle.ForeColor = Color.White;
                lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblTitle.Location = new Point(10, 10);
                lblTitle.AutoSize = true;

                Label lblValue = new Label();
                lblValue.Text = (i == 2) ? "$0.00" : "0"; // قيم افتراضية
                lblValue.ForeColor = Color.White;
                lblValue.Font = new Font("Segoe UI", 20, FontStyle.Bold);
                lblValue.Location = new Point(10, 40);
                lblValue.AutoSize = true;

                pnlCard.Controls.Add(lblTitle);
                pnlCard.Controls.Add(lblValue);

                // ربط الـ Panel بالـ PictureBox لضمان الشفافية الصحيحة
                pnlMainContainer.Controls.Add(pnlCard);
                xPos += 270; // إزاحة الكارت القادم
            }
        }
        #endregion

        private Label lblTitle;
        private Label label24;
        private Button btnExit;
        private Panel panel1;
        private PictureBox pnlMainContainer;
    }
}