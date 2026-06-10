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
        private int _DoctorPersonID = clsGlobal.CurrentUser.PersonID;

        public enum enMode { AddNew = 1, Update = 2, ShowVisit = 3 }
        private enMode _Mode = enMode.AddNew;
        private clsVisit _Visit;
        private int _VisitID = -1;
        private clsPrescription _Prescription;
        private int _PrescriptionID = -1;

        private decimal TotalAmount = 0.0m;
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
            _dtAllVisit = clsVisit.GetPatientsWaitingForDoctors(_DoctorPersonID);
            dgvDoctorQueue.DataSource = _dtAllVisit;
            lblRecordsCount.Text = _dtAllVisit.DefaultView.Count.ToString();

        }

        private void _FillVitalsInfo(int AppointmentID = -1)
        {
            clsVital vital;
            if (_Mode == enMode.AddNew)
            {
                AppointmentID = (int)dgvDoctorQueue.CurrentRow.Cells["AppointmentID"].Value;

                vital = clsVital.FindByAppointmentID(AppointmentID);
            }
            else
            {
                vital = clsVital.FindByVisitID(AppointmentID);
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
            TotalAmount = Appointment.AppointmentFees;
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


        #region Data Grid Setup,Initialization and Load of Prescriptions

        DataTable _dtAllMedicines = new DataTable();
        private void InitializePrescriptionsTable()
        {
            _dtAllMedicines.Columns.Clear(); // تنظيف الجدول أولاً

            // 1. الأعمدة الأساسية (تطابق قاعدة البيانات تماماً)
            _dtAllMedicines.Columns.Add("MedicineID", typeof(int));
            _dtAllMedicines.Columns.Add("VisitID", typeof(int));
            _dtAllMedicines.Columns.Add("Quantity", typeof(int)).DefaultValue = 1;
            _dtAllMedicines.Columns.Add("Dosage", typeof(string));

            // ملاحظة: Instructions لم تكن موجودة في تعريفك، أضفتها لتطابق الـ Mapping
            _dtAllMedicines.Columns.Add("Instructions", typeof(string));

            _dtAllMedicines.Columns.Add("SavedMedicineName", typeof(string)); // بديل للـ MedicineName
            _dtAllMedicines.Columns.Add("SavedMedicinePrice", typeof(decimal)).DefaultValue = 0m;

            // 2. قيم افتراضية
            _dtAllMedicines.Columns["VisitID"].DefaultValue = _VisitID;

            // 3. أعمدة العرض (فقط للـ UI)
            // نستخدم اسم MedicineName للعرض في الـ Grid

            // عمود Total للحسابات التلقائية في الـ UI
            _dtAllMedicines.Columns.Add("Total", typeof(decimal), "(SavedMedicinePrice * Quantity)");
        }
        private void _SetupPrescriptionsColumnsGrid()
        {
            dgvMedicines.Columns.Clear();
            dgvMedicines.AutoGenerateColumns = false;

            // الأعمدة المخفية (للحفظ)
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicineID", DataPropertyName = "MedicineID", Visible = false });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "VisitID", DataPropertyName = "VisitID", Visible = false });

            // الأعمدة المرئية (نستخدم الأسماء الجديدة لتطابق الجدول)
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "SavedMedicineName", HeaderText = "Medicine Name", DataPropertyName = "SavedMedicineName", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Dosage", HeaderText = "Dosage", DataPropertyName = "Dosage", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Instructions", HeaderText = "Instructions", DataPropertyName = "Instructions", ReadOnly = true });

            // استخدام السعر المحفوظ للعرض
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "SavedMedicinePrice", HeaderText = "Unit Price", DataPropertyName = "SavedMedicinePrice", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Qty", DataPropertyName = "Quantity", ReadOnly = true });

            // الإجمالي (يستخدم عمود Total المحسوب في الـ DataTable)
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", DataPropertyName = "Total", ReadOnly = true });

            // عمود الحذف
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
            InitializePrescriptionsTable();
            dgvMedicines.DataSource = _dtAllMedicines;
        }
        private void _LoadVisitPrescription(int VisitID)
        {
            // 1. جلب البيانات
            _Prescription = clsPrescription.Find(VisitID);
            if (_Prescription == null)
            {
                MessageBox.Show("DontFound any Prescription"); return;
            }
            lbPrescriptionID.Text = _Prescription.PrescriptionID.ToString();
            dtpPrescriptionDate.Value = _Prescription.PrescriptionDate;
            txtPrescriptionNotes.Text = _Prescription.PrescriptionNotes;
            _dtAllMedicines = _Prescription.dtMedicines;

            // 2. إعادة تعريف عمود الحساب إذا لم يكن موجوداً في البيانات القادمة
            if (!_dtAllServieces.Columns.Contains("Total"))
            {
                _dtAllServieces.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");
            }

            // 3. إعادة ربط الشبكة
            dgvMedicines.DataSource = _dtAllMedicines;
        }

        #endregion


        #region Data Grid Setup,Initialization and Load of Services

        DataTable _dtAllServieces = new DataTable();

        private void InitializeServicesTable()
        {
            _dtAllServieces.Columns.Add("ServiceID", typeof(int));
            _dtAllServieces.Columns.Add("VisitID", typeof(int));
            _dtAllServieces.Columns.Add("ServiceName", typeof(string));
            _dtAllServieces.Columns.Add("Quantity", typeof(int));
            _dtAllServieces.Columns.Add("SavedServicePrice", typeof(decimal));
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

            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", DataPropertyName = "Total", ReadOnly = true });

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
            InitializeServicesTable();
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
        #endregion


        #region Setup Screen
        private void _ResetDefaultValues()
        {
            tpServices.Enabled = false;
            tpPrescriptionInfo.Enabled = false;

            if (_Mode == enMode.AddNew)
            {
                _Visit = new clsVisit();
                _Prescription = new clsPrescription();
                lblTitle.Text = "Add New Visit";
                timer1.Start();
                panWaittingList.Visible = true;
                _SetupDataGridViewWaitingListColumns();
                if (_dtAllVisit.Rows.Count > 0)
                {
                    dgvDoctorQueue.Rows[0].Selected = true;
                    _FillPatientInfo();
                    _FillVitalsInfo();
                }

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
                MessageBox.Show("No Visit with ID = " + _VisitID, "Visit Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lbVisitID.Text = _VisitID.ToString();
            _FillPatientInfo(_Visit.AppointmentID);
            _FillVitalsInfo(_Visit.VisitID);
            lbVisitDate.Text = _Visit.VisitDate.ToString();
            txtDiagnosis.Text = _Visit.Diagnosis;
            txtNotes.Text = _Visit.VisitNotes;
            _LoadVisitServices(_VisitID);
            _LoadVisitPrescription(_VisitID);
            tpPrescriptionInfo.Enabled = true;
            tpServices.Enabled = true;
          
        }

        private void _SetupDataGridViewWaitingListColumns()
        {
            _RefreashData();
            if (_dtAllVisit.Rows.Count == 0)
                return;
            dgvDoctorQueue.Columns["AppointmentID"].HeaderText = "Ap.ID";
            dgvDoctorQueue.Columns["AppointmentID"].Width = 120;

            dgvDoctorQueue.Columns["PatientName"].HeaderText = "Patient Name";
            dgvDoctorQueue.Columns["PatientName"].Width = 250;

            dgvDoctorQueue.Columns["CheckInTime"].HeaderText = "CheckInTime";
            dgvDoctorQueue.Columns["CheckInTime"].Width = 150;

            dgvDoctorQueue.Columns["StatusText"].Visible=false;

            dgvDoctorQueue.Columns["IsCalled"].HeaderText = "Call";
            dgvDoctorQueue.Columns["IsCalled"].Width = 70;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            timer2.Start();
            if (_Mode != enMode.AddNew)
                _LoadData();

        }

        #endregion
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

                    if (MessageBox.Show("The Patient IS Called Are youe Update Status To Postpone and Call Next Patient","Qu",MessageBoxButtons.OKCancel) == DialogResult.OK)
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
            DataTable dtQueue = clsVisit.GetPatientsWaitingForDoctors(_DoctorPersonID);

            if (dtQueue.Rows.Count > 0)
            {

                int nextAppointmentID = (int)dtQueue.Rows[0]["AppointmentID"];

                clsAppointment.UpdatePatientCallStatus(nextAppointmentID, true, 2);
                _RefreashData();
                _FillVitalsInfo();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _RefreashData();
        }

        #region Code Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            _RefreashData();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            lbDate.Text = DateTime.Now.ToString();
        }
        #endregion

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


        #region SaveData To DataBase
        private void btnSaveVisit_Click(object sender, EventArgs e)
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


            _Visit.PatientID=int.Parse(lblPatientID.Text);
            _Visit.DoctorID = int.Parse(lblDoctorID.Text);
            _Visit.Diagnosis=txtDiagnosis.Text;
            _Visit.VisitNotes=txtNotes.Text;
            _Visit.VisitDate = DateTime.Now;
            _Visit.AppointmentID =int.Parse(lblAppointmentID.Text);
            _Visit.DoctorID =int.Parse(lblDoctorID.Text);
            _Visit.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _Visit.TotalAmount = TotalAmount;
            if (_Visit.Save())
            {
                MessageBox.Show("Saved Success.", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Question);
                _VisitID = _Visit.VisitID;
                lblVisitID.Text = _VisitID.ToString();
                lbVisitID.Text = _VisitID.ToString();
                tpServices.Enabled = true;
                tpPrescriptionInfo.Enabled = true;

                _FillServicesInCheckBox();
                _SetupPrescriptionsColumnsGrid();
                _SetupServicesColumnsGrid();
                
            }
            else
            {
                MessageBox.Show("Saved faild.", "faild", MessageBoxButtons.OK, MessageBoxIcon.Question);

            }

        }

        private void btnSaveVisitServices_Click(object sender, EventArgs e)
        {
            if (_dtAllServieces.Rows.Count == 0)
            {
                MessageBox.Show("Please Add Services");
                return;
            }

            if (clsVisitServices.SaveVisitServices(_VisitID, _dtAllServieces))
                MessageBox.Show("SavedServices SuccessFully");
            else
                MessageBox.Show("SavedServices Falid");


        }

        private void btnSavePrescription_Click(object sender, EventArgs e)
        {
            _Prescription.PrescriptionDate = dtpPrescriptionDate.Value;
            _Prescription.VisitID = _VisitID;
            _Prescription.PrescriptionNotes = txtPrescriptionNotes.Text;
            _Prescription.dtMedicines = _dtAllMedicines;

            if (_Prescription.Save())
            {
                MessageBox.Show("SavedServices SuccessFully");
                lbPrescriptionID.Text = _Prescription.PrescriptionID.ToString();
            }
            else
                MessageBox.Show("SavedServices Falid");
        }

        #endregion


        #region SaveData To DataGrid
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

        #endregion


    }
}
