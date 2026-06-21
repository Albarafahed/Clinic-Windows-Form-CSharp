namespace Clinic.ControlsMain
{
    partial class ucPrescriptionItems
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDetailsPopup = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailsPopup)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDetailsPopup
            // 
            this.dgvDetailsPopup.AllowUserToAddRows = false;
            this.dgvDetailsPopup.AllowUserToDeleteRows = false;
            this.dgvDetailsPopup.AllowUserToResizeRows = false;
            this.dgvDetailsPopup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetailsPopup.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.dgvDetailsPopup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDetailsPopup.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetailsPopup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetailsPopup.ColumnHeadersHeight = 60;
            this.dgvDetailsPopup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDetailsPopup.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDetailsPopup.EnableHeadersVisualStyles = false;
            this.dgvDetailsPopup.GridColor = System.Drawing.Color.MintCream;
            this.dgvDetailsPopup.Location = new System.Drawing.Point(174, 27);
            this.dgvDetailsPopup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvDetailsPopup.MultiSelect = false;
            this.dgvDetailsPopup.Name = "dgvDetailsPopup";
            this.dgvDetailsPopup.ReadOnly = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(193)))), ((int)(((byte)(218)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDetailsPopup.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDetailsPopup.RowHeadersVisible = false;
            this.dgvDetailsPopup.RowHeadersWidth = 30;
            this.dgvDetailsPopup.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvDetailsPopup.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.dgvDetailsPopup.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.dgvDetailsPopup.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDetailsPopup.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.dgvDetailsPopup.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDetailsPopup.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetailsPopup.RowTemplate.Height = 50;
            this.dgvDetailsPopup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetailsPopup.Size = new System.Drawing.Size(1003, 253);
            this.dgvDetailsPopup.TabIndex = 256;
            this.dgvDetailsPopup.TabStop = false;
            // 
            // ucPrescriptionItems
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.dgvDetailsPopup);
            this.Name = "ucPrescriptionItems";
            this.Size = new System.Drawing.Size(1376, 312);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailsPopup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDetailsPopup;
    }
}
