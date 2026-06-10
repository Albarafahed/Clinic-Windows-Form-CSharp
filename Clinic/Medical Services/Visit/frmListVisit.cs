using Clinic.Appointment;
using Clinic.Doctor;
using Clinic.Medical_Services.Appointment;
using Clinic.Patient;
using Clinic_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Visit
{
    public partial class frmListVisits : Form
    {
       

        static DataTable _dtAllVisits;
        DataTable _dtVists;

        public frmListVisits()
        {
            InitializeComponent();
        }

        private void _RefreshVisitList()
        {
            _dtAllVisits = clsVisit.GetAllVisits();
            if(_dtAllVisits.Rows.Count>0)
            _dtVists = _dtAllVisits.DefaultView.ToTable(false, "VisitID", "PatientID", "DoctorID", "AppointmentID", "PatientName", "DoctorName", "VisitDate", "Diagnosis","CreatedByUserID");

            dgvVisit.DataSource = _dtVists;
            lblRecordsCount.Text = dgvVisit.Rows.Count.ToString();
        }

        private void frmListVisits_Load(object sender, EventArgs e)
        {
            if (clsGlobal.CurrentUser.RoleID== 3 )
            {
                editToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
            }
            _RefreshVisitList();
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvVisit.Rows.Count.ToString();
            if (dgvVisit.Rows.Count > 0)
            {
                dgvVisit.Columns["VisitID"].HeaderText = "Visit ID";
                dgvVisit.Columns["VisitID"].Width = 110;

                dgvVisit.Columns["PatientID"].Visible = false; //PatientID
                dgvVisit.Columns["DoctorID"].Visible = false; //DoctorID

                dgvVisit.Columns["AppointmentID"].HeaderText = "App.ID";
                dgvVisit.Columns["AppointmentID"].Width = 90;

                dgvVisit.Columns["PatientName"].HeaderText = "Patient Name";
                dgvVisit.Columns["PatientName"].Width = 260;

                dgvVisit.Columns["DoctorName"].HeaderText = "Doctor Name";
                dgvVisit.Columns["DoctorName"].Width = 260;

                dgvVisit.Columns["VisitDate"].HeaderText = "Visit Date";
                dgvVisit.Columns["VisitDate"].Width = 200;

                dgvVisit.Columns["Diagnosis"].HeaderText = "Diagnosis";
                dgvVisit.Columns["Diagnosis"].Width = 260;

                dgvVisit.Columns["CreatedByUserID"].HeaderText = "C.UserID";
                dgvVisit.Columns["CreatedByUserID"].Width = 90;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Visit ID":
                    FilterColumn = "VisitID";
                    break;

                case "Appointment ID":
                    FilterColumn = "AppointmentID";
                    break;

                case "Patient Name":
                    FilterColumn = "PatientName"; 
                    break;

                case "Doctor Name":
                    FilterColumn = "DoctorName"; 
                    break;

                case "Diagnosis":
                    
                    FilterColumn = "Diagnosis";
                    break;
                case "CreatedByUserID":
                    FilterColumn = "CreatedByUserID";
                    break;

            }

            //Reset the filters in case nothing selected or filter value contains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtVists.DefaultView.RowFilter = "";
                lblRecordsCount.Text = _dtVists.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "AppointmentID" || FilterColumn == "VisitID" || FilterColumn == "CreatedByUserID")
                //in this case we deal with integer not string.
                _dtVists.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtVists.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = _dtVists.Rows.Count.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Appointment ID" || cbFilterBy.Text == "Visit ID" || cbFilterBy.Text == "CreatedByUserID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void ShowAppointmentListtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListAppointmets frm = new frmListAppointmets();
            frm.ShowDialog();
            frmListVisits_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int VisitID = (int)dgvVisit.CurrentRow.Cells["VisitID"].Value;

            frmDoctor frm = new frmDoctor(VisitID);
            frm.ShowDialog();
            frmListVisits_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int VisitID = (int)dgvVisit.CurrentRow.Cells["VisitID"].Value;
            int AppointmentID = (int)dgvVisit.CurrentRow.Cells["AppointmentID"].Value;
            if (MessageBox.Show("Are you sure you want delete this Visit!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsVisit.DeleteVisit(VisitID, AppointmentID))
                {
                    MessageBox.Show("Visit has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListVisits_Load(null, null);
                }
                else
                    MessageBox.Show("Visit is not deleted due to data connected to it.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showPatientDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int patientID = (int)dgvVisit.CurrentRow.Cells["PatientID"].Value;
            frmPatientInfo frm = new frmPatientInfo(patientID);
            frm.ShowDialog();
        }

        private void showDoctorDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DoctorID = (int)dgvVisit.CurrentRow.Cells["DoctorID"].Value;
            frmDoctorInfo frm = new frmDoctorInfo(DoctorID);
            frm.ShowDialog();
        }

        private void ShowVisitDetailstoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //frmShowVisitInfo frm = new frmShowVisitInfo((int)dgvVisit.CurrentRow.Cells[0].Value);
            //frm.ShowDialog();
        }

        private void rescheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
