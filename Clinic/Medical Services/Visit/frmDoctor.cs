using Clinic.Controls;
using Clinic.Doctor;
using Clinic.Patient;
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

        public frmDoctor(int VisitID)
        {
            if (clsGlobal.CurrentUser.RoleID == 1
                ||
               clsGlobal.CurrentUser.RoleID == 2)
            {
                InitializeComponent();
                _VisitID = VisitID;
                _Mode = enMode.Update;
            }
            else
                return;
        }
        private void _RefreashData()
        {
            _dtAllVisit = clsVisit.GetPatientsWaitingForDoctors(_DoctorPersonID);
            lbVisitDate.Text=DateTime.Now.ToShortDateString();
            dgvDoctorQueue.DataSource = _dtAllVisit;
            lblRecordsCount.Text = _dtAllVisit.DefaultView.Count.ToString();


            if (dgvDoctorQueue.Columns["IsCalled"] != null)
            {
                // إخفاء العمود الأصلي
                dgvDoctorQueue.Columns["IsCalled"].Visible = false;

                // إنشاء عمود CheckBox جديد
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.Name = "IsCalledCheck";
                chk.HeaderText = "Is Called";
                chk.DataPropertyName = "IsCalled"; // سيربطه بالقيم (1 و 0)
                chk.TrueValue = 1;
                chk.FalseValue = 0;

                // إضافته للجدول
                dgvDoctorQueue.Columns.Add(chk);
            }

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

            lblVisitID.Text =_VisitID==-1? "[???]":_VisitID.ToString();
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

        DataTable _dtAllMedicines=new DataTable();
        private void InitializePrescriptionsTable()
        {
            _dtAllMedicines.Columns.Clear(); // تنظيف الجدول أولاً

            _dtAllMedicines.Columns.Add("MedicineID", typeof(int));
            _dtAllMedicines.Columns.Add("Quantity", typeof(int)).DefaultValue = 1;
            _dtAllMedicines.Columns.Add("Dosage", typeof(string));

            _dtAllMedicines.Columns.Add("Instructions", typeof(string));

            _dtAllMedicines.Columns.Add("SavedMedicineName", typeof(string)); 
            _dtAllMedicines.Columns.Add("SavedMedicinePrice", typeof(decimal)).DefaultValue = 0m;


            _dtAllMedicines.Columns.Add("Total", typeof(decimal), "(SavedMedicinePrice * Quantity)");
        }
        private void _SetupPrescriptionsColumnsGrid()
        {
            dgvMedicines.Columns.Clear();
            dgvMedicines.AutoGenerateColumns = false;

            // الأعمدة المخفية (للحفظ)
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicineID", DataPropertyName = "MedicineID", Visible = false });

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

            DataGridViewImageColumn imgUpdate = new DataGridViewImageColumn
            {
                Name = "Edit",
                HeaderText = "Action",
                Image = Properties.Resources.edit_32,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 50
            };
            dgvMedicines.Columns.Add(imgUpdate);

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
                MessageBox.Show("DontFound any Prescription");
                _Prescription = new clsPrescription();
                return;
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

        DataTable _dtAllServieces=new DataTable() ;

        private void InitializeServicesTable()
        {
            _dtAllServieces.Columns.Add("ServiceID", typeof(int));
            _dtAllServieces.Columns.Add("ServiceName", typeof(string));
            _dtAllServieces.Columns.Add("Quantity", typeof(int));
            _dtAllServieces.Columns.Add("SavedServicePrice", typeof(decimal));
            _dtAllServieces.Columns.Add("Discount", typeof(decimal));

            // هذا السطر يقوم بحساب "Total" تلقائياً عند إضافة أو تعديل البيانات
            _dtAllServieces.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");
        }

        private void _SetupServicesColumnsGrid()
        {
            dgvServices.Columns.Clear();
            dgvServices.AutoGenerateColumns = false;

            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "ServiceID", DataPropertyName = "ServiceID", Visible = false });
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
            dtpPrescriptionDate.Value= DateTime.Now;
            nudQuality.Value = 1;
            _FillServicesInCheckBox();
           

            if (_Mode == enMode.AddNew)
            {
                _Visit = new clsVisit();
                _Prescription = new clsPrescription();
                lblTitle.Text = "Add New Visit";
                timer1.Start();
                pnlWaittingList.Visible = true;
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
            lbVisitDate.Text = _Visit.VisitDate.ToShortDateString();
            txtDiagnosis.Text = _Visit.Diagnosis;
            txtNotes.Text = _Visit.VisitNotes;
            _SetupServicesColumnsGrid();
            _LoadVisitServices(_VisitID);
            _SetupPrescriptionsColumnsGrid();
            _LoadVisitPrescription(_VisitID);
            tpPrescriptionInfo.Enabled = true;
            tpServices.Enabled = true;
            pnlActions.Enabled = true;
            pnlWaittingList.Visible = false;
          
        }

        private void _SetupDataGridViewWaitingListColumns()
        {
            _RefreashData();
            if (_dtAllVisit.Rows.Count == 0)
                return;
            dgvDoctorQueue.Columns["AppointmentID"].HeaderText = "Ap.ID";
            dgvDoctorQueue.Columns["AppointmentID"].Width = 90;

            dgvDoctorQueue.Columns["PatientName"].HeaderText = "P.Name";
            dgvDoctorQueue.Columns["PatientName"].Width = 120;

            dgvDoctorQueue.Columns["CheckInTime"].HeaderText = "CheckInTime";
            dgvDoctorQueue.Columns["CheckInTime"].Width = 150;

            dgvDoctorQueue.Columns["StatusText"].HeaderText="Status";
            dgvDoctorQueue.Columns["StatusText"].Width = 150;

            dgvDoctorQueue.Columns["IsCalledCheck"].HeaderText = "Call";
            dgvDoctorQueue.Columns["IsCalledCheck"].Width = 70;
           
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
            timer1.Stop();
            timer1.Dispose();
            timer2.Stop();
            timer2.Dispose();
            this.Close();
        }

        private void btnNextPatient_Click(object sender, EventArgs e)
        {
            if (dgvDoctorQueue.SelectedRows.Count > 0)
            {
                // استخدم Convert.ToBoolean فهو يتعامل بذكاء مع الأرقام 0 و 1
                bool IsCalled = Convert.ToBoolean(dgvDoctorQueue.CurrentRow.Cells["IsCalledCheck"].Value);
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
            if(enMode.AddNew==_Mode)
            {
                if (dgvDoctorQueue.CurrentRow == null)
                {
                    MessageBox.Show("Please select a patient from the queue first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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
                if (_Mode == enMode.Update)
                    return;
                _VisitID = _Visit.VisitID;
                lblVisitID.Text = _VisitID.ToString();
                lbVisitID.Text = _VisitID.ToString();
                tpServices.Enabled = true;
                tpPrescriptionInfo.Enabled = true;

                _SetupPrescriptionsColumnsGrid();
                _SetupServicesColumnsGrid();
                tcVisitInfo.SelectedIndex = 1;
                
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
            {

                MessageBox.Show("SavedServices SuccessFully");
                tcVisitInfo.SelectedIndex = 2;
            }
            else
                MessageBox.Show("SavedServices Falid");


        }

        private void btnSavePrescription_Click(object sender, EventArgs e)
        {
            _Prescription.PrescriptionDate =dtpPrescriptionDate.Value;
            _Prescription.VisitID = _VisitID;
            _Prescription.PrescriptionNotes = txtPrescriptionNotes.Text;
            _Prescription.dtMedicines = _dtAllMedicines;

            if (_Prescription.Save())
            {
                MessageBox.Show("SavedServices SuccessFully");
                _PrescriptionID=_Prescription.PrescriptionID;
                lbPrescriptionID.Text = _PrescriptionID.ToString();
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

        private void btnAddMedicen_Click(object sender, EventArgs e)
        {
            frmAddUpdateMedicineToPrescription frm = new frmAddUpdateMedicineToPrescription(ref _dtAllMedicines);
            frm.DataBack += (sendform) => dgvMedicines.Refresh();
            frm.Show();
        }


        #endregion

        private void llPatientInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPatientInfo frm=new frmPatientInfo(int.Parse(lblPatientID.Text));
            frm.ShowDialog();
        }

        private void llDoctorInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDoctorInfo frm = new frmDoctorInfo(int.Parse(lblDoctorID.Text));
            frm.ShowDialog();
        }

        private void dataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGrid =sender as DataGridView;
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGrid.Columns[e.ColumnIndex].Name == "Delete")
            {
                dataGrid.Cursor = Cursors.Hand; // يتحول الماوس ليد
            }
            else
            {
                dataGrid.Cursor = Cursors.Default; // يعود لشكل السهم
            }
        }

        private void dgvMedicines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dgvMedicines.Columns[e.ColumnIndex].Name == "Delete")
            {
                // تأكيد الحذف
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Medicine?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _dtAllMedicines.Rows.RemoveAt(e.RowIndex);
                }
            }

            if (e.ColumnIndex >= 0 && dgvMedicines.Columns[e.ColumnIndex].Name == "Edit")
            {
              
                DataRow selectedRow = ((DataRowView)dgvMedicines.Rows[e.RowIndex].DataBoundItem).Row;

                // 2. إرسال الصف إلى فورم التعديل مع تمرير الـ DataTable بالمرجع (ref)
                frmAddUpdateMedicineToPrescription frm = new frmAddUpdateMedicineToPrescription(ref _dtAllMedicines, selectedRow);

             
                frm.DataBack += (senderForm) => {
                    dgvMedicines.Refresh(); 
                };

                frm.ShowDialog();
            }
        }

        private void dgvServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dgvServices.Columns[e.ColumnIndex].Name == "Delete")
            {
                // تأكيد الحذف
                DialogResult result = MessageBox.Show("Are you sure you want to delete this shift?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _dtAllServieces.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus newStatus, string actionName)
        {
            if (dgvDoctorQueue.CurrentRow == null) return;

            int currentAppointmentID = (int)dgvDoctorQueue.CurrentRow.Cells["AppointmentID"].Value;

            string message = $"Are you sure you want to {actionName}?";
            string title = "Confirm Action";

            if (MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsAppointment.UpdateAppointmentStatus(currentAppointmentID, newStatus, clsGlobal.CurrentUser.UserID))
                {
                    if (newStatus == clsAppointment.enAppointmentStatus.InQueue)
                    {
                        if (dgvDoctorQueue.CurrentRow == null) return;
                        int appointmentID = (int)dgvDoctorQueue.CurrentRow.Cells["AppointmentID"].Value;
                    }
                    MessageBox.Show($"✅ The operation was completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreashData();
                }

                else
                {
                    MessageBox.Show($"❌ An error occurred while updating the status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnInProgress_Click(object sender, EventArgs e) => _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus.Progress, "Progress this appointment");

        private void btnPostpone_Click(object sender, EventArgs e) => _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus.Postponed, "postpone this appointment");

        private void btnCalnceVisit_Click(object sender, EventArgs e) => _ProcessAppointmentStatus(clsAppointment.enAppointmentStatus.Cancelled, "cancel this appointment");


        private void frmDoctor_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer2.Stop();
            timer2.Dispose();

            timer1.Stop();
            timer1.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lbPluse.Text = "[???]  bpm";
            lblTemp.Text = "[???]  °C";
            lbBP.Text = "[???]  mmHg";
            lbWeight.Text = "[???]  kg";

            lblVisitID.Text = "[???]";
            lblAppointmentID.Text = "[???]";
            lblDoctorID.Text = "[???]";
            lblPatientID.Text = "[???]";
            TotalAmount = 0.0m;

            lbVisitID.Text = "[???]";
            lbVisitDate.Text = DateTime.Now.ToShortDateString();
            txtDiagnosis.Text = "";
            txtNotes.Text = "";

            dgvServices.DataSource = null;

            dgvMedicines.DataSource = null;
            lbPrescriptionID.Text = "[???]";
            txtPrescriptionNotes.Text = "";

            _dtAllMedicines = null;
            _dtAllServieces = null;
            _Visit = null;
            _Prescription = null;

            tcVisitInfo.SelectedIndex=0;

            _ResetDefaultValues();

            _CallNextPatientInQueue();
        }

        private void dgvDoctorQueue_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dgvDoctorQueue.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void dgvDoctorQueue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDoctorQueue.Rows.Count == 0) return;

            if (dgvDoctorQueue.CurrentRow != null && dgvDoctorQueue.CurrentRow.Index == 0) return;

            dgvDoctorQueue.SelectionChanged -= dgvDoctorQueue_SelectionChanged;

            dgvDoctorQueue.ClearSelection();
            dgvDoctorQueue.Rows[0].Selected = true;
            dgvDoctorQueue.CurrentCell = dgvDoctorQueue.Rows[0].Cells[0];

            dgvDoctorQueue.SelectionChanged += dgvDoctorQueue_SelectionChanged;
        }
    }
}
