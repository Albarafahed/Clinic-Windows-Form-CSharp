using Clinic.global_classes;
using Clinic_Business;
using System;
using System.Data;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Appointment
{
    public partial class frmListAppointmets : Form
    {
        private DataTable _dtAppointmets = clsAppointment.GetAllAppointments();
        public frmListAppointmets()
        {
            InitializeComponent();
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            frmAddUpdateAppointment frm = new frmAddUpdateAppointment();
            frm.DatatBack += _DatatBackToAdd;
            frm.ShowDialog();
        }

        private void frmListAppointmets_Load(object sender, EventArgs e)
        {
            dgvAppointments.DataSource = _dtAppointmets;
            lblRecordsCount.Text = _dtAppointmets.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;
            if (_dtAppointmets.Rows.Count == 0)
                return;
            _dtAppointmets.PrimaryKey = new DataColumn[] { _dtAppointmets.Columns["AppointmentID"] };
            dgvAppointments.Columns["AppointmentID"].HeaderText = "Appointment ID";
            dgvAppointments.Columns["AppointmentID"].Width = 150;

            dgvAppointments.Columns["PatientName"].HeaderText = "Patient Name";
            dgvAppointments.Columns["PatientName"].Width = 150;

            dgvAppointments.Columns["DoctorName"].HeaderText = "Doctor Name";
            dgvAppointments.Columns["DoctorName"].Width = 150;

            dgvAppointments.Columns["TypeName"].HeaderText = "AppointmentType";
            dgvAppointments.Columns["TypeName"].Width = 150;

            dgvAppointments.Columns["AppointmentFees"].HeaderText = "Fees";
            dgvAppointments.Columns["AppointmentFees"].Width = 100;

            dgvAppointments.Columns["AppointmentDate"].HeaderText = "Date";
            dgvAppointments.Columns["AppointmentDate"].Width = 150;

            dgvAppointments.Columns["AppointmentStatus"].HeaderText = "Status";
            dgvAppointments.Columns["AppointmentStatus"].Width = 100;

            dgvAppointments.Columns["CreatedByUserID"].HeaderText = "C.UserID";
            dgvAppointments.Columns["CreatedByUserID"].Width = 90;

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsNone = cbFilterBy.Text == "None";
            bool IsStatus = cbFilterBy.Text == "Status";

            txtFilterValue.Visible = !IsNone && !IsStatus;
            cbStatus.Visible = IsStatus;

            _dtAppointmets.DefaultView.RowFilter = string.Empty;
            lblRecordsCount.Text = _dtAppointmets.Rows.Count.ToString();
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();
            }
            else if (cbStatus.Visible)
            {
                cbStatus.SelectedIndex = 0;
                cbStatus.Focus();
            }
            else
            {
                cbFilterBy.Focus();
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "AppointmentID" || cbFilterBy.Text == "CreatedByUserID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatus.SelectedIndex == 0)
            {
                _dtAppointmets.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                _dtAppointmets.DefaultView.RowFilter = $"AppointmentStatus='{cbStatus.Text}'";
            }
            lblRecordsCount.Text = _dtAppointmets.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (_dtAppointmets == null) return;

            string FilterColumn = _GetFilterColumnName(cbFilterBy.Text);

            if (string.IsNullOrEmpty(txtFilterValue.Text) || FilterColumn == "None")
            {
                _dtAppointmets.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                // استخدام LIKE للنصوص و = للأرقام
                if (FilterColumn == "AppointmentID" || FilterColumn == "CreatedByUserID")
                    _dtAppointmets.DefaultView.RowFilter = $"{FilterColumn} = {txtFilterValue.Text}";

                else
                    _dtAppointmets.DefaultView.RowFilter = $"{FilterColumn} LIKE '{txtFilterValue.Text}%'";
            }
            lblRecordsCount.Text = _dtAppointmets.Rows.Count.ToString();
        }

        private string _GetFilterColumnName(string FilterBy)
        {
            switch (FilterBy)
            {
                case "Appointment ID": return "AppointmentID";
                case "Patient Name": return "PatientName";
                case "Doctor Name": return "DoctorName";
                case "AppointmentType": return "TypeName";
                case "C.UserID": return "CreatedByUserID";
                default: return "None";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmQueueDisplay frm = new frmQueueDisplay();
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = (int)dgvAppointments.CurrentRow.Cells["AppointmentID"].Value;

            frmAddUpdateAppointment frm = new frmAddUpdateAppointment(AppointmentID);
            frm.DatatBack += _DataBackToUpdate;
            frm.ShowDialog();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dgvAppointments.CurrentRow == null) return;

            int AppointmentID = (int)dgvAppointments.CurrentRow.Cells["AppointmentID"].Value;

            if (MessageBox.Show("Are you sure you want to delete Appoinment [" + AppointmentID + "]?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsAppointment.DeleteAppointment(AppointmentID))
                {
                    MessageBox.Show("Appoinment Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _DataBackToDelete(AppointmentID);
                }
                else
                {
                    MessageBox.Show("Appoinment was not deleted because it has data linked to it in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void _RefreshRecords()
        {
            lblRecordsCount.Text = _dtAppointmets.Rows.Count.ToString();
        }
        private void _DatatBackToAdd(object sender, int AppointmentID)
        {
            DataRow NewAppointmentRow = clsAppointment.GetAppointmentInfoByID(AppointmentID);

            if (NewAppointmentRow != null)
            {
                _dtAppointmets.UpsertRow(NewAppointmentRow, AppointmentID);
                _RefreshRecords();
            }


        }

        private void _DataBackToUpdate(object sender, int AppointmentID)
        {
            DataRow UpdateAppointmentRow = clsAppointment.GetAppointmentInfoByID(AppointmentID);

            if (UpdateAppointmentRow != null)
            {
                _dtAppointmets.UpsertRow(UpdateAppointmentRow, AppointmentID);
                _RefreshRecords();
            }
        }

        private void _DataBackToDelete(int AppointmentID)
        {
            DataRow row = _dtAppointmets.Rows.Find(AppointmentID);
            if (row != null)
            {
                _dtAppointmets.Rows.Remove(row);
                _dtAppointmets.AcceptChanges();
                _RefreshRecords();
            }
        }

      
    }
}
