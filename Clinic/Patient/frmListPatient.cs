using Clinic.global_classes;
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

namespace Clinic.Patient
{
    public partial class frmListPatient : Form
    {
        // سحب البيانات من طبقة الـ Business مرة واحدة عند الإقلاع
        private DataTable _dtAllPatients = clsPatient.GetAllPatients();

        public frmListPatient()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _ResetDefault()
        {
            _dtAllPatients = clsPatient.GetAllPatients();
            dgvPatient.DataSource = _dtAllPatients;
            lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();
        }

        private void frmListPatient_Load(object sender, EventArgs e)
        {
            dgvPatient.DataSource = _dtAllPatients;

            // تعيين المفتاح الأساسي للـ DataTable لتمكين خاصية Rows.Find من العمل بنجاح
            if (_dtAllPatients.Columns.Contains("PatientID"))
            {
                _dtAllPatients.PrimaryKey = new DataColumn[] { _dtAllPatients.Columns["PatientID"] };
            }

            cbFilterBy.SelectedIndex = 0; // سيقوم تلقائياً باستدعاء حدث SelectedIndexChanged لتنسيق العناصر
            lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();

            // تنسيق أعمدة الـ DataGridView بناءً على أسماء الحقول العائدة من دالة GetAllPatients الفعالية
            if (dgvPatient.Rows.Count > 0)
            {
                dgvPatient.Columns["PatientID"].HeaderText = "Patient ID";
                dgvPatient.Columns["PatientID"].Width = 100;

                dgvPatient.Columns["PersonID"].HeaderText = "Person ID";
                dgvPatient.Columns["PersonID"].Width = 100;

                dgvPatient.Columns["FullName"].HeaderText = "Full Name";
                dgvPatient.Columns["FullName"].Width = 220;

                dgvPatient.Columns["MedicalHistory"].HeaderText = "Medical History";
                dgvPatient.Columns["MedicalHistory"].Width = 180;

                dgvPatient.Columns["CountryName"].HeaderText = "Country";
                dgvPatient.Columns["CountryName"].Width = 120;

                dgvPatient.Columns["BloodTypeName"].HeaderText = "Blood Type";
                dgvPatient.Columns["BloodTypeName"].Width = 100;

                dgvPatient.Columns["CreatedDate"].HeaderText = "Created Date";
                dgvPatient.Columns["CreatedDate"].Width = 150;

                dgvPatient.Columns["EmergencyContact"].HeaderText = "Emergency Contact";
                dgvPatient.Columns["EmergencyContact"].Width = 150;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsNone = cbFilterBy.Text == "None";
            bool IsBloodType = cbFilterBy.Text == "Blood Type";

            txtFilterValue.Visible = !IsBloodType && !IsNone;
            cbBloodType.Visible = IsBloodType && !IsNone;

            // إعادة تعيين الفلتر عند تغيير العمود المراد البحث به
            _dtAllPatients.DefaultView.RowFilter = "";
            lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();
            }
            else if (cbBloodType.Visible)
            {
                cbBloodType.SelectedIndex = 0; // حقل "All" واجهة افتراضية
                cbBloodType.Focus();
            }
            else
            {
                cbFilterBy.Focus();
            }
        }

        private void cbBloodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtAllPatients == null) return;

            string FilterValue = cbBloodType.Text;

            if (FilterValue == "All")
                _dtAllPatients.DefaultView.RowFilter = "";
            else
                _dtAllPatients.DefaultView.RowFilter = string.Format("[BloodTypeName] = '{0}'", FilterValue);

            lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            // ربط الاختيارات بأسماء حقول الـ SQL الدقيقة بما فيها حقل الـ Country الجديد
            switch (cbFilterBy.Text)
            {
                case "Patient ID":
                    FilterColumn = "PatientID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Medical History":
                    FilterColumn = "MedicalHistory";
                    break;
                case "Country":  // الحقل الجديد الذي قمت بإضافته في الـ ComboBox
                    FilterColumn = "CountryName";
                    break;
                case "Emergency Contact":
                    FilterColumn = "EmergencyContact";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllPatients.DefaultView.RowFilter = "";
                lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();
                return;
            }

            if (FilterColumn == "PatientID" || FilterColumn == "PersonID")
            {
                if (int.TryParse(txtFilterValue.Text.Trim(), out _))
                {
                    _dtAllPatients.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
                }
                else
                {
                    _dtAllPatients.DefaultView.RowFilter = "1=0"; // إخفاء الصفوف عند إدخال نص في حقل رقمي
                }
            }
            else
            {
                // حماية وتطهير النص المدخل باستخدام Replace لمنع مشاكل الـ (') وتأمين البحث في الحقول النصية ومنها الدول
                _dtAllPatients.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim().Replace("'", "''"));
            }

            lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Patient ID" || cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            frmAddUpdatePatient frm = new frmAddUpdatePatient();
            frm.DataBack += _DataBackToAdd;
            frm.ShowDialog();
        }


        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPatient.CurrentRow == null) return;

            int PatientID = (int)dgvPatient.CurrentRow.Cells[0].Value;
            frmAddUpdatePatient frm = new frmAddUpdatePatient(PatientID);
            frm.DataBack += _DataBackToUpdate;
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPatient.CurrentRow == null) return;

            int PatientID = (int)dgvPatient.CurrentRow.Cells["PatientID"].Value;
            frmPatientInfo frm = new frmPatientInfo(PatientID);
            frm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPatient.CurrentRow == null) return;

            int PatientID = (int)dgvPatient.CurrentRow.Cells["PatientID"].Value;

            if (MessageBox.Show("Are you sure you want to delete Patient [" + PatientID + "]?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                clsPatient patient = clsPatient.Find(PatientID);
                if (patient == null)
                    return;
                if (patient.DeletePatient())
                {
                    MessageBox.Show("Patient Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _DataBackToDelete(PatientID);
                }
                else
                {
                    MessageBox.Show("Patient was not deleted because it has data linked to it in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ================== الـ DataBack Mechanism لسرعة وكفاءة التحديث بالذاكرة محلياً ==================

        private void _DataBackToAdd(object sender, int PatientID)
        {
            DataRow NewPatientRow = clsPatient.GetPatientByID(PatientID);

            if (NewPatientRow != null)
            {
               _dtAllPatients.UpsertRow(NewPatientRow,PatientID);
            }

            lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();
        }

        private void _DataBackToUpdate(object sender, int PatientID)
        {
            DataRow UpdatePatienRow = clsPatient.GetPatientByID(PatientID);
            if (UpdatePatienRow != null)
            {
                _dtAllPatients.UpsertRow(UpdatePatienRow, PatientID);
            }
            lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();
        }

        private void _DataBackToDelete(int PatientID)
        {
            DataRow row = _dtAllPatients.Rows.Find(PatientID);
            if (row != null)
            {
                _dtAllPatients.Rows.Remove(row);
                _dtAllPatients.AcceptChanges();
                lblRecordsCount.Text = _dtAllPatients.DefaultView.Count.ToString();
            }
        }

       
    }
}