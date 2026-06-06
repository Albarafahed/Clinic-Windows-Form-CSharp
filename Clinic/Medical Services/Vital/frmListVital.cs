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

namespace Clinic.Medical_Services.Vital
{
    public partial class frmListVital : Form
    {
        private DataTable _dtAllVitals;

        private clsVital _Vital=new clsVital();
        public frmListVital()
        {
            InitializeComponent();
        }
        private void _ResetDefault()
        {
            _dtAllVitals = clsVital.GetPatientsWaitingForVitals();
            dgvNurseQueue.DataSource = _dtAllVitals;
            lblRecordsCount.Text = _dtAllVitals.DefaultView.Count.ToString();
        }
        private void frmListVital_Load(object sender, EventArgs e)
        {
            _ResetDefault();

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _ResetDefault();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _ResetDefault();


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

    

        private void txtKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvNurseQueue.CurrentRow == null) return;

            int currentAppID = (int)dgvNurseQueue.CurrentRow.Cells["AppointmentID"].Value;

            _Vital.AppointmentID = currentAppID;
            _Vital.BloodPressure=txtBloodPressure.Text;
            _Vital.Pulse=int.Parse(txtPulse.Text);
            _Vital.Temperature=Convert.ToDecimal(txtTemperature.Text);
            _Vital.Weight=Convert.ToDecimal(txtWeight.Text);

            if (_Vital.Save())
            {
                // 2. تغيير حالة المريض الحالي إلى 8 (جاهز للطبيب)
                clsAppointment.UpdateAppointmentStatus(currentAppID, clsAppointment.enAppointmentStatus.Ready_For_Doctor, clsGlobal.CurrentUser.UserID);

                // 3. نداء المريض "التالي" تلقائياً
                _CallNextPatientInQueue();

                MessageBox.Show("تم حفظ المريض الحالي، وتم نداء المريض التالي في الطابور.");
                _ResetDefault();
            }
        }

        private void _CallNextPatientInQueue()
        {
            // جلب أول مريض في الطابور (حالة 7)
            DataTable dtQueue = clsVital.GetPatientsWaitingForVitals();

            if (dtQueue.Rows.Count > 0)
            {
                // المريض في الصف الأول (Index 0) هو التالي
                int nextAppointmentID = (int)dtQueue.Rows[0]["AppointmentID"];

                // نداء المريض التالي
                clsAppointment.UpdatePatientCallStatus(nextAppointmentID, true, 1);
            }
        }

        private void txtValidating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            e.Cancel = string.IsNullOrEmpty(textBox.Text);

            if (e.Cancel)
                errorProvider1.SetError(textBox, "The Faile is Recuerid");
            else
                errorProvider1.SetError(textBox, null);
        }
       
    }
}
