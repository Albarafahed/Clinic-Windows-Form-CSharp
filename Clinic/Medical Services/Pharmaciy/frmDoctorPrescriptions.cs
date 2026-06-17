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

        private void btnExit_Click(object sender, EventArgs e) => this.Close();
        private void txtSearch_TextChanged(object sender, EventArgs e)
            => lblPlaceholderSer.Visible=string.IsNullOrEmpty(txtSearch.Text);
         
          

        private void frmPrescriptionDispnsing_Load(object sender, EventArgs e)
        {
            dgvPrescriptions.DataSource = clsVital.GetPatientsWaitingForVitals();
            this.dgvPrescriptions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            dgPrescriptionDetails.DataSource= clsVital.GetPatientsWaitingForVitals();
            this.dgPrescriptionDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);

        }

        private void dgvPrescriptions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // التحقق مما إذا كان الصف الحالي هو الصف المحدد
            //if (dgvPrescriptions.Rows[e.RowIndex].Selected)
            //{
            //    // جعل الخط أسمك (Bold) عند التحديد
            //    e.CellStyle.Font = new Font(dgvPrescriptions.Font, FontStyle.Bold);

            //    // يمكنك أيضاً تعيين ألوان التحديد هنا لضمان التطابق
            //    e.CellStyle.SelectionBackColor = Color.FromArgb(0, 120, 120);
            //    e.CellStyle.SelectionForeColor = Color.White;
            //}
            //else
            //{
            //    // إعادة الخط لطبيعته (Regular) عند إلغاء التحديد
            //    e.CellStyle.Font = new Font("Segoe UI", 14F, FontStyle.Regular);
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
