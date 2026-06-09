using Clinic.Controls;
using Clinic_Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Clinic_Business.clsDoctor;

namespace Clinic.Medical_Services.Visit
{
    
    public partial class frmDoctor : Form
    {
        private DataTable _dtAllVisit;
        private DataTable _dtAllMedicines;
        private int _DoctorPersonID = clsGlobal.CurrentUser.PersonID;

        public enum enMode { AddNew=1, Update=2,ShowVisit=3}
        private enMode _Mode=enMode.AddNew;
        private clsVisit _Visit;
        private int _VisitID = -1;
        public frmDoctor()
        {
            if (clsGlobal.CurrentUser.RoleID == 1
                ||
               clsGlobal.CurrentUser.RoleID == 2)
                InitializeComponent();
            else
                return;
        }

        private void _RefreashData()
        {
            _dtAllVisit = clsVital.GetPatientsWaitingForVitals();
            dgvDoctorQueue.DataSource = _dtAllVisit;
            lblRecordsCount.Text = _dtAllVisit.DefaultView.Count.ToString();
          
        }

        private void _FillVitalsInfo(int AppointmentID=-1)
        {
            clsVital vital;
            if (_Mode == enMode.AddNew)
            {
                AppointmentID = (int)dgvDoctorQueue.CurrentRow.Cells["AppointmentID"].Value;

               vital = clsVital.FindByAppointmentID(AppointmentID);
            }
            else
            {
                vital= clsVital.FindByVisitID(AppointmentID);
            }

            
            if (vital == null)
            {
                MessageBox.Show("Vital Not Found");
                return;
            }

            lbPluse.Text = vital.Pulse.ToString() + "  bpm";
            lblTemp.Text = vital.Temperature.ToString() + " °C";
            lbBP.Text = vital.BloodPressure.ToString() + " mmHg";
            lbWeight.Text = vital.Weight.ToString() + " kg";
        }

        private void _FillPatientInfo(int AppointmentID = -1)
        {
            if (_Mode == enMode.AddNew)
                AppointmentID = (int)dgvDoctorQueue.CurrentRow.Cells["AppointmentID"].Value;

            clsAppointment Appointment = clsAppointment.Find(AppointmentID);
            if (Appointment == null)
            {
                MessageBox.Show("Appointment Not Found");
                return;
            }

            lblVisitID.Text = Appointment.PatientID.ToString();
            lblAppointmentID.Text = Appointment.AppointmentID.ToString();
            lblDoctorID.Text = Appointment.DoctorID.ToString();
            lblPatientID.Text = Appointment.PatientID.ToString();
        }

        private void _FillServicesInCheckBox()
        {
            DataTable dtServices = clsServicesType.GetServiceList();
            if (dtServices != null)
            {
                cmbServices.DataSource = dtServices;
                cmbServices.DisplayMember = "ServiceName";
                cmbServices.ValueMember = "ServiceID";
            }
        }

        private void _SetupPrescriptionsColumnsGrid()
        {
            dgvMedicines.Columns.Clear();
            dgvMedicines.AutoGenerateColumns = false;

            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicineID", DataPropertyName = "MedicineID", Visible = false });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicineName", HeaderText = "Medicine Name", DataPropertyName = "MedicineName", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Dosage", HeaderText = "Dosage", DataPropertyName = "Dosage", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicinePrice", HeaderText = "Unit Price", DataPropertyName = "MedicinePrice", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Qty", DataPropertyName = "Quantity", ReadOnly = true });

            // الإجمالي للسطر الواحد
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", ReadOnly = true });

            DataGridViewImageColumn imgDelete = new DataGridViewImageColumn
            {
                Name = "Delete",
                HeaderText = "Action",
                Image = Properties.Resources.Delete_32,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 50
            };
            dgvMedicines.Columns.Add(imgDelete); 

            dgvMedicines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMedicines.DataSource = _dtAllMedicines; 
        }

        DataTable _dtAllServieces = new DataTable();

        private void InitializeServicesTable()
        {
            _dtAllServieces.Columns.Add("ServiceID", typeof(int));
            _dtAllServieces.Columns.Add("VisitID", typeof(int));
            _dtAllServieces.Columns.Add("ServiceName", typeof(string));
            _dtAllServieces.Columns.Add("SavedServicePrice", typeof(decimal));
            _dtAllServieces.Columns.Add("Quantity", typeof(int));
            _dtAllServieces.Columns.Add("Discount", typeof(decimal));

            _dtAllServieces.Columns["VisitID"].Expression = _VisitID.ToString();
            // هذا السطر يقوم بحساب "Total" تلقائياً عند إضافة أو تعديل البيانات
            _dtAllServieces.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");
        }

        private void _SetupServicesColumnsGrid()
        {
            dgvServices.Columns.Clear();
            dgvServices.AutoGenerateColumns = false;

            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServiceID", DataPropertyName = "ServiceID", Visible = false });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "VisitID", DataPropertyName = "VisitID", Visible = false });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServiceName", HeaderText = "Service Name", DataPropertyName = "ServiceName", ReadOnly = true });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "SavedServicePrice", HeaderText = "Unit Price", DataPropertyName = "SavedServicePrice", ReadOnly = true });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Qty", DataPropertyName = "Quantity", ReadOnly = true });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "Discount", HeaderText = "Discount", DataPropertyName = "Discount", ReadOnly = false });

            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", ReadOnly = true });

            DataGridViewImageColumn imgDelete = new DataGridViewImageColumn
            {
                Name = "Delete",
                HeaderText = "Action",
                Image = Properties.Resources.Delete_32,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 50
            };
            dgvServices.Columns.Add(imgDelete);

            dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServices.DataSource = _dtAllServieces;
        }

        private void _LoadVisitServices(int VisitID)
        {
            // 1. جلب البيانات
            _dtAllServieces = clsVisitServices.GetVisitServices(VisitID);

            // 2. إعادة تعريف عمود الحساب إذا لم يكن موجوداً في البيانات القادمة
            if (!_dtAllServieces.Columns.Contains("Total"))
            {
                _dtAllServieces.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");
            }

            // 3. إعادة ربط الشبكة
            dgvServices.DataSource = _dtAllServieces;
        }

        private void _ResetDefaultValues()
        {
            _FillServicesInCheckBox();
           _SetupPrescriptionsColumnsGrid();
            _SetupServicesColumnsGrid();

            tpServices.Enabled = false;
            tpPrescriptionInfo.Enabled = false;

            btnSave.Enabled = false;

            if (_Mode == enMode.AddNew)
            {
                _Visit = new clsVisit();
                lblTitle.Text = "Add New Visit";
                timer1.Start();
               
            }
            else if (_Mode == enMode.Update)
            {
                lblTitle.Text = "Update Visit Info";
              
            }
            else
            {
                lblTitle.Text = "Visit Info";
            }

            this.Text = lblTitle.Text;
        }

        private void _LoadData()
        {
            _Visit = clsVisit.Find(_VisitID);
            if (_Visit == null)
            {
                MessageBox.Show("No Doctor with ID = " + _VisitID, "Doctor Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
           
            lbVisitID.Text= _VisitID.ToString();
            _FillPatientInfo(_Visit.AppointmentID);
            _FillVitalsInfo(_Visit.VisitID);
            lbVisitDate.Text = _Visit.VisitDate.ToString();
            txtDiagnosis.Text = _Visit.Diagnosis;
            txtNotes.Text = _Visit.VisitNotes;
            _LoadVisitServices(_VisitID);
            tpPrescriptionInfo.Enabled = true;
            tpServices.Enabled = true;
            btnSave.Enabled = true;
        }

         private void _SetupDataGridViewWaitingListColumns()
          {
            dgvDoctorQueue.Columns["AppointmentID"].HeaderText = "AppointmentID";
            dgvDoctorQueue.Columns["AppointmentID"].Width = 120;

            dgvDoctorQueue.Columns["PatientName"].HeaderText = "Patient Name";
            dgvDoctorQueue.Columns["PatientName"].Width = 250;

            dgvDoctorQueue.Columns["CheckInTime"].HeaderText = "CheckInTime";
            dgvDoctorQueue.Columns["CheckInTime"].Width = 150;

            dgvDoctorQueue.Columns["StatusText"].HeaderText = "Status";
            dgvDoctorQueue.Columns["StatusText"].Width = 100;

            dgvDoctorQueue.Columns["IsCalled"].HeaderText = "Call";
            dgvDoctorQueue.Columns["IsCalled"].Width = 70;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _RefreashData();
            if (_dtAllVisit.Rows.Count == 0)
                return;
            timer2.Start();
            _FillVitalsInfo();

          

        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        private void btnNextPatient_Click(object sender, EventArgs e)
        {
            if (dgvDoctorQueue.SelectedRows.Count > 0)
            {
                bool IsCalled = (bool)dgvDoctorQueue.CurrentRow.Cells["IsCalled"].Value;

                if (IsCalled)
                {
                    int AppointmentID = (int)dgvDoctorQueue.CurrentRow.Cells["AppointmentID"].Value;

                    if (MessageBox.Show("The Patient IS Called Are youe Update Status To Postpone and Call Next Patient") == DialogResult.OK)
                    {
                        clsAppointment.UpdateAppointmentStatus(AppointmentID, clsAppointment.enAppointmentStatus.Postponed, clsGlobal.CurrentUser.UserID);
                        _CallNextPatientInQueue();
                    }

                }
                else
                    _CallNextPatientInQueue();



            }
        }

        private void _CallNextPatientInQueue()
        {
            DataTable dtQueue =new DataTable();

            if (dtQueue.Rows.Count > 0)
            {
                
                int nextAppointmentID = (int)dtQueue.Rows[0]["AppointmentID"];

                clsAppointment.UpdatePatientCallStatus(nextAppointmentID, true, 2);

                _FillVitalsInfo();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _RefreashData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _RefreashData();
        }

        private void txtDiagnosis_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDiagnosis.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDiagnosis, "This Falid Is Requaired");
            }
            else
                errorProvider1.SetError(txtDiagnosis, null);
        }

        private void btnSaveandNext_Click(object sender, EventArgs e)
        {
            if (dgvDoctorQueue.CurrentRow == null)
            {
                MessageBox.Show("Please select a patient from the queue first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please correct the errors in the input fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            

            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lbVisitDate.Text = DateTime.Now.ToString();
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            if (cmbServices.SelectedValue == null) return;

            int serviceID = (int)cmbServices.SelectedValue;

            // 1. منع التكرار
            DataRow[] existingRows = _dtAllServieces.Select($"ServiceID = {serviceID}");
            if (existingRows.Length > 0)
            {
                MessageBox.Show("هذه الخدمة مضافة بالفعل للزيارة!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. الحصول على سعر الخدمة من الـ DataSource الخاص بالـ ComboBox (لا حاجة لقاعدة البيانات)
            DataTable dtSource = (DataTable)cmbServices.DataSource;
            DataRow[] foundRows = dtSource.Select($"ServiceID = {serviceID}");

            if (foundRows.Length > 0)
            {
                DataRow serviceRow = foundRows[0];

                // 3. إضافة الصف الجديد
                DataRow newRow = _dtAllServieces.NewRow();
                newRow["ServiceID"] = serviceID;
                newRow["ServiceName"] = serviceRow["ServiceName"];
                newRow["SavedServicePrice"] = serviceRow["ServiceFees"];
                newRow["Quantity"] = nudQuality.Value;
                newRow["Discount"] = 0;

                _dtAllServieces.Rows.Add(newRow);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
