using Clinic_Business;
using System;
using System.Data;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Appointment
{
    public partial class frmQueueDisplay : Form
    {
        private int _CurrentDoctorID = -1;

        public event Action<object, int> DataBack;
        public frmQueueDisplay(int DoctorID)
        {
            InitializeComponent();
            _CurrentDoctorID = DoctorID;
        }

        private void frmQueueDisplay_Load(object sender, EventArgs e)
        {
            _RefreshQueueList();
        }

        private void _RefreshQueueList()
        {
            // استدعاء الدالة التي تجلب المرضى المنتظرين فقط لهذا الطبيب
            DataTable dtQueue = clsAppointment.GetWaitingPatients(_CurrentDoctorID);

            dgvQueue.DataSource = dtQueue;

            if (dtQueue.Rows.Count > 0)
            {
                _SetupDataGridViewColumns();
            }
        }

        private void _SetupDataGridViewColumns()
        {
            // تنسيق الأعمدة لتناسب استعلام المرضى (PatientName, CheckInTime)
            if (dgvQueue.Columns["PatientName"] != null)
            {
                dgvQueue.Columns["PatientName"].HeaderText = "اسم المريض";
                dgvQueue.Columns["PatientName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvQueue.Columns["CheckInTime"] != null)
            {
                dgvQueue.Columns["CheckInTime"].HeaderText = "وقت الحضور";
                dgvQueue.Columns["CheckInTime"].DefaultCellStyle.Format = "hh:mm tt";
                dgvQueue.Columns["CheckInTime"].Width = 150;
            }

            // إخفاء أي أعمدة تقنية غير ضرورية للطبيب
            if (dgvQueue.Columns["AppointmentID"] != null)
                dgvQueue.Columns["AppointmentID"].Visible = false;
        }

        private void dgvQueue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblHeader_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus newStatus, string actionName)
        {
            if (dgvQueue.CurrentRow == null) return;

            int currentAppointmentID = (int)dgvQueue.CurrentRow.Cells[0].Value;

            string message = $"Are you sure you want to {actionName}?";
            string title = "Confirm Action";

            if (MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsAppointment.UpdateAppointmentStatus(currentAppointmentID, newStatus, clsGlobal.CurrentUser.UserID))
                {
                    MessageBox.Show($"✅ The operation was completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshQueueList();
                    DataBack?.Invoke(this, currentAppointmentID);
                }
                else
                {
                    MessageBox.Show($"❌ An error occurred while updating the status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnInProgress_Click(object sender, EventArgs e) => _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus.Progress, "start the examination");

        private void btnCompleted_Click(object sender, EventArgs e) => _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus.Completed, "mark this appointment as completed");
        private void btnPostponed_Click(object sender, EventArgs e) => _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus.Postponed, "postpone this appointment");

        private void dgvQueue_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dgvQueue.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void dgvQueue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvQueue.CurrentRow == null) return;

            string currentStatus = dgvQueue.CurrentRow.Cells["StatusText"].Value.ToString();
            btnInProgress.Enabled = (currentStatus == "In-Queue" || currentStatus== "Postponed");

            btnCompleted.Enabled = (currentStatus == "In-Progress");

            btnPostponed.Enabled = (currentStatus == "In-Queue" || currentStatus == "In-Progress");

        }
    }
}