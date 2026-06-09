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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Clinic.Medical_Services.Vital
{
    public partial class frmNurse : Form
    {

        private DataTable _dtAllVitals;

        private clsVital _Vital = new clsVital();
        public frmNurse()
        {
            InitializeComponent();
        }

        private void _RefreashData()
        {
            _dtAllVitals = clsVital.GetPatientsWaitingForVitals();
            dgvNurseQueue.DataSource = _dtAllVitals;
            lblRecordsCount.Text = _dtAllVitals.DefaultView.Count.ToString();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            txtBloodPressure.Focus();
        }

        private void frmNurse_Load(object sender, EventArgs e)
        {
            _RefreashData();

            if (_dtAllVitals.Rows.Count == 0)
                return;
            timer1.Start();
            timer2.Start();

            dgvNurseQueue.Columns["AppointmentID"].HeaderText = "AppointmentID";
            dgvNurseQueue.Columns["AppointmentID"].Width = 100;

            dgvNurseQueue.Columns["PatientName"].HeaderText = "Patient Name";
            dgvNurseQueue.Columns["PatientName"].Width = 280;

            dgvNurseQueue.Columns["CheckInTime"].HeaderText = "CheckInTime";
            dgvNurseQueue.Columns["CheckInTime"].Width = 150;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _RefreashData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _RefreashData();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lbDate.Text = DateTime.Now.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();

            timer2.Stop();
            timer2.Dispose();

            this.Close();
        }

        private void txtBloodPressure_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '/';

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvNurseQueue.CurrentRow == null)
            {
                MessageBox.Show("Please select a patient from the queue first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please correct the errors in the input fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int currentAppID = (int)dgvNurseQueue.CurrentRow.Cells["AppointmentID"].Value;
            _Vital.AppointmentID = currentAppID;
            _Vital.BloodPressure = txtBloodPressure.Text.Trim();
            _Vital.Pulse = (short)nudPulse.Value;
            _Vital.Temperature = nudTemperature.Value;
            _Vital.Weight = nudWeight.Value;
            _Vital.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_Vital.Save())
            {
                clsAppointment.UpdateAppointmentStatus(currentAppID, clsAppointment.enAppointmentStatus.Ready_For_Doctor, clsGlobal.CurrentUser.UserID);

                _CallNextPatientInQueue();

                MessageBox.Show("Vital signs saved successfully, and the next patient has been called.", "Process Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _RefreashData();
            }
            else
            {
                MessageBox.Show("An error occurred while saving data. Please try again.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _CallNextPatientInQueue()
        {
            DataTable dtQueue = clsVital.GetPatientsWaitingForVitals();

            if (dtQueue.Rows.Count > 0)
            {
                int nextAppointmentID = (int)dtQueue.Rows[0]["AppointmentID"];

                clsAppointment.UpdatePatientCallStatus(nextAppointmentID, true, 1);
            }
        }

        private void txtBloodPressure_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = string.IsNullOrEmpty(txtBloodPressure.Text);

            if (e.Cancel)
                errorProvider1.SetError(txtBloodPressure, "The Faile is Recuerid");
            else
                errorProvider1.SetError(txtBloodPressure, null);
        }


        private void dgvNurseQueue_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dgvNurseQueue.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

           
        }

        private void frmNurse_Resize(object sender, EventArgs e)
        {
            int padding = 10;
            if (this.Width > 1300)
                padding = 40;
            this.Padding = new Padding(padding);
            


        }

        private void dgvNurseQueue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNurseQueue.Rows.Count == 0) return;

            if (dgvNurseQueue.CurrentRow != null && dgvNurseQueue.CurrentRow.Index == 0) return;

            dgvNurseQueue.SelectionChanged -= dgvNurseQueue_SelectionChanged;

            dgvNurseQueue.ClearSelection();
            dgvNurseQueue.Rows[0].Selected = true;
            dgvNurseQueue.CurrentCell = dgvNurseQueue.Rows[0].Cells[0];

            dgvNurseQueue.SelectionChanged += dgvNurseQueue_SelectionChanged;
        }
    }
}
