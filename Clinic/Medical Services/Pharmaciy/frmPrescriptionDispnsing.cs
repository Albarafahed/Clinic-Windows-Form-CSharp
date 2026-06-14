using Clinic_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Pharmaciy
{
    public partial class frmPrescriptionDispnsing : Form
    {
        public frmPrescriptionDispnsing()
        {
            InitializeComponent();
        }

        private void SetupDataGridView()
        {
            // 1. الألوان الأساسية بناءً على طلبك (0, 150, 150)
            Color mainColor = Color.FromArgb(0, 150, 150);
            Color lightBackground = Color.FromArgb(240, 245, 245); // لون فاتح جداً مائل للأخضر
            Color alternatingRowColor = Color.FromArgb(225, 235, 235); // رمادي مخضر فاتح للتمييز
            Color textColor = Color.FromArgb(20, 40, 40); // لون الخط (داكن جداً للقراءة)

            // 2. إعدادات الجدول العامة
            dgvPrescriptions.BackgroundColor = lightBackground;
            dgvPrescriptions.DefaultCellStyle.BackColor = lightBackground;
            dgvPrescriptions.DefaultCellStyle.ForeColor = textColor;
            dgvPrescriptions.AlternatingRowsDefaultCellStyle.BackColor = alternatingRowColor;
            dgvPrescriptions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrescriptions.AllowUserToAddRows = false;
            dgvPrescriptions.ReadOnly = true;
            dgvPrescriptions.RowTemplate.Height = 45; // زيادة الارتفاع ليتناسب مع الخط الكبير
            dgPrescriptionDetails.SelectionMode=DataGridViewSelectionMode.FullRowSelect;
            // اجعل لون الصف المختار "أنيقاً" وغير مزعج
            dgvPrescriptions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 120); // لون أغمق قليلاً من اللون الرئيسي
            dgvPrescriptions.DefaultCellStyle.SelectionForeColor = mainColor;
            // 3. تنسيق الرؤوس (Header) - بتركيز على اللون الرئيسي
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.BackColor = mainColor;
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // خط أبيض لتباين عالٍ
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold); // خط أكبر
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPrescriptions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPrescriptions.EnableHeadersVisualStyles = false; // مهم جداً لتطبيق الألوان على الرؤوس

            // 4. تنسيق الخلايا (Cells)
            dgvPrescriptions.DefaultCellStyle.Font = new Font("Segoe UI", 14); // تكبير الخط للبيانات
            dgvPrescriptions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPrescriptions.GridColor = Color.FromArgb(200, 200, 200); // لون الخطوط الفاصلة

            // إخفاء رؤوس الصفوف لتبدو الواجهة أنظف
            dgvPrescriptions.RowHeadersVisible = false;
        }
        private void btnSendToAccounting_Click(object sender, EventArgs e)
        {

        }

        private void btnPrintShotageSlip_Click(object sender, EventArgs e)
        {

        }

        private void btnDispense_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lblPlaceholderSer.Visible=string.IsNullOrEmpty(txtSearch.Text);
        }

        private void frmPrescriptionDispnsing_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            dgvPrescriptions.DataSource = clsVital.GetPatientsWaitingForVitals();
        }
    }
}
