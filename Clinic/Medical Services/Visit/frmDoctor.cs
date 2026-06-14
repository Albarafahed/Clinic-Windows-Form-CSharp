using Clinic.Controls;
using Clinic.Doctor;
using Clinic.Patient;
using Clinic_Business;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Visit
{
    public partial class frmDoctor : Form
    {
        #region Private Members
        private enMode _Mode;
        private int _VisitID = -1;
        private clsVisit _Visit;
        private clsPrescription _Prescription;
        private DataTable _dtAllMedicines = new DataTable();
        private DataTable _dtAllServices = new DataTable();
        private int _DoctorPersonID = clsGlobal.CurrentUser.PersonID;
        private decimal TotalAmount = 0.0m;
        #endregion

        public enum enMode { AddNew = 1, Update = 2, ShowVisit = 3 }

        #region Constructor
        public frmDoctor(int visitID = -1, enMode mode = enMode.AddNew)
        {
            InitializeComponent();
            _VisitID = visitID;
            _Mode = mode;
        }
        #endregion

        #region UI Feedback (Professional Messaging)
        private void ShowSuccess(string message) => MessageBox.Show(message, "✅ Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowError(string message) => MessageBox.Show(message, "❌ Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private void ShowWarning(string message) => MessageBox.Show(message, "⚠️ Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private DialogResult ConfirmAction(string message) => MessageBox.Show(message, "❓ Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        #endregion

        #region Initialization
        private void frmDoctor_Load(object sender, EventArgs e)
        {
            if (!CheckAuth()) return;

            _SetupUI();
            _FillServicesInCheckBox();
            timer2.Start();

            if (_Mode == enMode.AddNew)
                _LoadAddNewMode();
            else if (_Mode == enMode.Update)
                _LoadUpdateMode();
            else
                _LoadShowMode();
        }

        private bool CheckAuth()
        {
            if (clsGlobal.CurrentUser.RoleID == 1 || clsGlobal.CurrentUser.RoleID == 2) return true;
            ShowError("Access Denied! You do not have permission to access this area.");
            this.Close();
            return false;
        }

        private void _SetupUI()
        {
            // Initialize Grid structures once
            _SetupServicesGrid();
            _SetupMedicinesGrid();
        }

        private void _LoadAddNewMode()
        {
            lblTitle.Text = "Add New Visit";
            _Visit = new clsVisit();
            _Prescription = new clsPrescription();
            pnlWaittingList.Visible = true;
            timer1.Start();
            tpServices.Enabled = false;
            tpPrescriptionInfo.Enabled = false;
            dtpPrescriptionDate.Value = DateTime.Now;
            lbVisitDate.Text = DateTime.Now.ToShortDateString();
            nudQuality.Value = 1;
            _SetupWaitingListGrid();
            _RefreshQueueData();




            if (dgvDoctorQueue.Rows.Count > 0)
            {
                dgvDoctorQueue.Rows[0].Selected = true;
                int AppoinmentID = (int)dgvDoctorQueue.CurrentRow.Cells["AppointmentID"].Value;
                _FillPatientInfo(AppoinmentID);
                _FillVitalsInfo(AppoinmentID);
            }
            else
            {
                llPatientInfo.Enabled = false;
                llDoctorInfo.Enabled = false;
            }

        }

        private void _LoadUpdateMode()
        {
            lblTitle.Text = "Update Visit Info";
            _LoadVisitData(_VisitID);
        }

        private void _LoadShowMode()
        {
            lblTitle.Text = "Visit Details";
            _LoadVisitData(_VisitID);

            pnlVisitInfo.Enabled = false;
            btnSaveVisit.Visible = false;

            pnlServicesInfo.Enabled = false;
            btnSaveVisitServices.Visible = false;
            btnAddService.Visible = false;
            lbServeces.Visible = false;
            cmbServices.Visible = false;
            lbQuality.Visible = false;
            nudQuality.Visible = false;

            pnlPrescriptionInfo.Enabled = false;
            btnSavePrescription.Visible = false;
            btnAddMedicen.Visible = false;
            lbPrescriptionDate.Visible = false;
            dtpPrescriptionDate.Visible = false;
            pbPrecriptionDate.Visible = false;

        }
        #endregion

        #region Data Loading
        private void _LoadVisitData(int visitID)
        {
            _Visit = clsVisit.Find(visitID);
            if (_Visit == null)
            {
                ShowError($"Visit with ID {visitID} could not be found.");
                this.Close();
                return;
            }

            // Fill basic info
            lbVisitID.Text = _Visit.VisitID.ToString();
            txtDiagnosis.Text = _Visit.Diagnosis;
            txtNotes.Text = _Visit.VisitNotes;
            lbVisitDate.Text = _Visit.VisitDate.ToShortDateString();
            pnlAction.Visible = false;

            _FillPatientInfo(_Visit.AppointmentID);
            _FillVitalsInfo(_Visit.VisitID);

            // Load detail tables
            _dtAllServices = clsVisitServices.GetVisitServices(visitID);

            if (_dtAllServices.Rows.Count > 0)
            {

                if (!_dtAllServices.Columns.Contains("Total"))
                {
                    
                    _dtAllServices.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");
                }
            }
            else
            {
                InitializeServicesTable();
            }

            dgvServices.DataSource = _dtAllServices;

            _Prescription = clsPrescription.Find(visitID);
            if (_Prescription != null)
            {
                _dtAllMedicines = _Prescription.dtMedicines;
                if (_dtAllMedicines.Rows.Count > 0)
                {
                    if (!_dtAllMedicines.Columns.Contains("Total"))
                    {
                        _dtAllMedicines.Columns.Add("Total", typeof(decimal), "(SavedMedicinePrice * Quantity)-DiscountAmount");

                    }
                }
                else
                    InitializePrescriptionsTable();

                lbPrescriptionID.Text = _Prescription.PrescriptionID.ToString();
                dtpPrescriptionDate.Value = _Prescription.PrescriptionDate;
                txtPrescriptionNotes.Text = _Prescription.PrescriptionNotes;
                dgvMedicines.DataSource = _dtAllMedicines;

            }



            tpPrescriptionInfo.Enabled = true;
            tpServices.Enabled = true;
            pnlActions.Enabled = true;
            pnlWaittingList.Visible = false;

        }

        private void _FillVitalsInfo(int AppointmentID)
        {
            if (AppointmentID == -1)
                return;
            clsVital vital;
            if (_Mode == enMode.AddNew)
            {

                vital = clsVital.FindByAppointmentID(AppointmentID);
            }
            else
            {
                vital = clsVital.FindByVisitID(AppointmentID);
            }


            if (vital == null)
            {
                clsAppointment.UpdateAppointmentStatus(AppointmentID, clsAppointment.enAppointmentStatus.Waiting_For_Vitals, clsGlobal.CurrentUser.UserID);
                _RefreshQueueData();
                return;
            }

            lbPluse.Text = vital.Pulse.ToString() + "  bpm";
            lblTemp.Text = vital.Temperature.ToString() + " °C";
            lbBP.Text = vital.BloodPressure.ToString() + " mmHg";
            lbWeight.Text = vital.Weight.ToString() + " kg";
        }

        private void _FillPatientInfo(int AppointmentID)
        {

            clsAppointment Appointment = clsAppointment.Find(AppointmentID);
            if (Appointment == null)
            {
                ShowError("Appointment Not Found");
                return;
            }

            lblVisitID.Text = _VisitID == -1 ? "[???]" : _VisitID.ToString();
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

        #endregion

        #region Saving Logic

        private void btnSaveVisit_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                ShowWarning("Please correct the input errors before saving.");
                return;
            }



            _Visit.PatientID = int.Parse(lblPatientID.Text);
            _Visit.DoctorID = int.Parse(lblDoctorID.Text);
            _Visit.Diagnosis = txtDiagnosis.Text;
            _Visit.VisitNotes = txtNotes.Text;
            _Visit.VisitDate = DateTime.Now;
            _Visit.AppointmentID = int.Parse(lblAppointmentID.Text);
            _Visit.DoctorID = int.Parse(lblDoctorID.Text);
            _Visit.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _Visit.TotalAmount = TotalAmount;

            if (_Visit.Save())
            {
                ShowSuccess("Visit details saved successfully.");
                _VisitID = _Visit.VisitID;
                _Mode = enMode.Update;
                tpServices.Enabled = true;
                tpPrescriptionInfo.Enabled = true;
                lblVisitID.Text = _VisitID.ToString();
                lbVisitID.Text = _VisitID.ToString();
                tcVisitInfo.SelectedIndex = 1;
            }
            else
            {
                ShowError("An error occurred while saving the visit data.");
            }
        }

        private void btnSaveVisitServices_Click(object sender, EventArgs e)
        {
            //if (_dtAllServices.Rows.Count == 0)
            //{
            //    ShowWarning("The service list is empty. Please add services first.");
            //    return;
            //}

            if (clsVisitServices.SaveVisitServices(_VisitID, _dtAllServices))
            {
                ShowSuccess("Services saved successfully.");
                tcVisitInfo.SelectedIndex = 2;
            }
            else
                ShowError("Failed to save services.");
        }

        private void btnSavePrescription_Click(object sender, EventArgs e)
        {
            _Prescription.PrescriptionDate = dtpPrescriptionDate.Value;
            _Prescription.VisitID = _VisitID;
            _Prescription.PrescriptionNotes = txtPrescriptionNotes.Text;
            _Prescription.dtMedicines = _dtAllMedicines;

            if (_Prescription.Save())
            {
                ShowSuccess("Prescription saved successfully.");
                lbPrescriptionID.Text = _Prescription.PrescriptionID.ToString();
            }
            else
                ShowError("Failed to save Prescription.");
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            if (cmbServices.SelectedValue == null) return;

            int serviceID = (int)cmbServices.SelectedValue;

            decimal discount = 0;
            decimal.TryParse(txtDiscount.Text, out discount);

            // التحقق من الصلاحية: (استخدم ! لعكس النتيجة)
            if (!clsDiscount.ValidateDiscount(clsGlobal.CurrentUser.RoleID, clsDiscount.enTargetType.MedicalService, discount))
            {
                MessageBox.Show("Discount exceeds your allowed limit or is invalid for this role.", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // يجب الخروج وعدم إكمال الحفظ
            }

            // 1. منع التكرار
            DataRow[] existingRows = _dtAllServices.Select($"ServiceID = {serviceID}");
            if (existingRows.Length > 0)
            {
                ShowWarning("This service has already been added to the visit.");
                return;
            }
            // 2. الحصول على سعر الخدمة من الـ DataSource الخاص بالـ ComboBox (لا حاجة لقاعدة البيانات)
            DataTable dtSource = (DataTable)cmbServices.DataSource;
            DataRow[] foundRows = dtSource.Select($"ServiceID = {serviceID}");

            if (foundRows.Length > 0)
            {
                DataRow serviceRow = foundRows[0];

                // 3. إضافة الصف الجديد
                DataRow newRow = _dtAllServices.NewRow();
                newRow["ServiceID"] = serviceID;
                newRow["ServiceName"] = serviceRow["ServiceName"];
                newRow["SavedServicePrice"] = serviceRow["ServiceFees"];
                newRow["Quantity"] = nudQuality.Value;
                newRow["Discount"] = discount;

                _dtAllServices.Rows.Add(newRow);
            }
        }

        private void btnAddMedicen_Click(object sender, EventArgs e)
        {
            frmAddUpdateMedicineToPrescription frm = new frmAddUpdateMedicineToPrescription(ref _dtAllMedicines);
            frm.DataBack += (sendform) => dgvMedicines.DataSource = _dtAllMedicines;
            frm.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _ResetForm();
            _CallNextPatientInQueue();
        }

        #endregion

        #region Grid Definitions
        private void _SetupWaitingListGrid()
        {
            dgvDoctorQueue.AutoGenerateColumns = false;

            if (dgvDoctorQueue.Columns.Count == 0)
            {
                dgvDoctorQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "AppointmentID", DataPropertyName = "AppointmentID", HeaderText = "Ap.ID", Width = 90 });
                dgvDoctorQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "PatientName", DataPropertyName = "PatientName", HeaderText = "P.Name", Width = 120 });
                dgvDoctorQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "CheckInTime", DataPropertyName = "CheckInTime", HeaderText = "CheckIn", Width = 170 });
                dgvDoctorQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusText", DataPropertyName = "StatusText", HeaderText = "Status", Width = 150 });

                // عمود الـ CheckBox
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn
                {
                    Name = "IsCalledCheck",
                    HeaderText = "Call",
                    DataPropertyName = "IsCalled",
                    TrueValue = 1,
                    FalseValue = 0,
                    Width = 70
                };
                dgvDoctorQueue.Columns.Add(chk);
            }
        }
        private void _SetupServicesGrid()
        {

            dgvServices.AutoGenerateColumns = false;
            if (dgvServices.Rows.Count > 0)
                return;
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

            dgvServices.DataSource = _dtAllServices;
        }

        private void _SetupMedicinesGrid()
        {


            dgvMedicines.AutoGenerateColumns = false;

            if (dgvMedicines.Columns.Count > 0)
                return;
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicineID", DataPropertyName = "MedicineID", Visible = false });

            // الأعمدة المرئية (نستخدم الأسماء الجديدة لتطابق الجدول)
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "SavedMedicineName", HeaderText = "M.Name", DataPropertyName = "SavedMedicineName", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Dosage", HeaderText = "Dosage", DataPropertyName = "Dosage", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Instructions", HeaderText = "Instructions", DataPropertyName = "Instructions", ReadOnly = true });

            // استخدام السعر المحفوظ للعرض
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "SavedMedicinePrice", HeaderText = "Unit Price", DataPropertyName = "SavedMedicinePrice", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Qty", DataPropertyName = "Quantity", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "Frequency", HeaderText = "FRY", DataPropertyName = "Frequency", ReadOnly = true });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiscountAmount", HeaderText = "Discount", DataPropertyName = "DiscountAmount", ReadOnly = true });

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
            InitializePrescriptionsTable();
            dgvMedicines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMedicines.DataSource = _dtAllMedicines;
        }
        #endregion

        #region Table Definitions
        private void InitializePrescriptionsTable()
        {
            _dtAllMedicines.Columns.Clear(); // تنظيف الجدول أولاً

            _dtAllMedicines.Columns.Add("MedicineID", typeof(int));
            _dtAllMedicines.Columns.Add("Quantity", typeof(int)).DefaultValue = 1;
            _dtAllMedicines.Columns.Add("Dosage", typeof(string));
            _dtAllMedicines.Columns.Add("Frequency", typeof(int));
            _dtAllMedicines.Columns.Add("Instructions", typeof(string));
            _dtAllMedicines.Columns.Add("DiscountAmount", typeof(decimal));
            _dtAllMedicines.Columns.Add("SavedMedicineName", typeof(string));
            _dtAllMedicines.Columns.Add("SavedMedicinePrice", typeof(decimal)).DefaultValue = 0m;


            _dtAllMedicines.Columns.Add("Total", typeof(decimal), "(SavedMedicinePrice * Quantity)-DiscountAmount");
        }

        private void InitializeServicesTable()
        {
            _dtAllServices.Columns.Clear(); // تنظيف الجدول أولاً

            _dtAllServices.Columns.Add("ServiceID", typeof(int));
            _dtAllServices.Columns.Add("ServiceName", typeof(string));
            _dtAllServices.Columns.Add("Quantity", typeof(int));
            _dtAllServices.Columns.Add("SavedServicePrice", typeof(decimal));
            _dtAllServices.Columns.Add("Discount", typeof(decimal));
            _dtAllServices.Columns.Add("Total", typeof(decimal), "(SavedServicePrice * Quantity) - Discount");


        }

        #endregion

        #region Utilities
        private void _RefreshQueueData()
        {
            DataTable dtQueue = clsVisit.GetPatientsWaitingForDoctors(_DoctorPersonID);
            dgvDoctorQueue.DataSource = dtQueue;
            lblRecordsCount.Text = dtQueue.Rows.Count.ToString();
        }

        private void _ResetForm()
        {

            // 1. إعادة ضبط المتغيرات الأساسية
            _Mode = enMode.AddNew;
            _VisitID = -1;
            _Visit = new clsVisit();
            _Prescription = new clsPrescription();
            TotalAmount = 0.0m;

            lbPluse.Text = "[???]  bpm";
            lblTemp.Text = "[???]  °C";
            lbBP.Text = "[???]  mmHg";
            lbWeight.Text = "[???]  kg";
            // 2. تنظيف الجداول وإعادة ربطها (للتأكد من مسح البيانات من الواجهة)
            _dtAllMedicines.Clear();
            _dtAllServices.Clear();

            // 3. مسح نصوص وعناصر الواجهة
            txtDiagnosis.Clear();
            txtNotes.Clear();
            txtPrescriptionNotes.Clear();
            lbVisitDate.Text = DateTime.Now.ToShortDateString();
            lbVisitID.Text = "[???]";
            lblPatientID.Text = "[???]";
            lblAppointmentID.Text = "[???]";
            lblDoctorID.Text = "[???]";
            lblVisitID.Text = "[???]";
            llDoctorInfo.Enabled = false;
            llPatientInfo.Enabled = false;

            // 4. إعادة ضبط خيارات افتراضية
            nudQuality.Value = 1;
            dtpPrescriptionDate.Value = DateTime.Now;
            lbPrescriptionID.Text = "[???]";

            // 5. تحديث الحالة البصرية
            tpServices.Enabled = false;
            tpPrescriptionInfo.Enabled = false;

            // 6. تحديث قائمة الانتظار
            _RefreshQueueData();
            errorProvider1.Clear();
        }
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

                    if (ConfirmAction("The Patient IS Called Are youe Update Status To Postpone and Call Next Patient") == DialogResult.Yes)
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
                _RefreshQueueData();
                _FillVitalsInfo(nextAppointmentID);
                _FillPatientInfo(nextAppointmentID);
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _RefreshQueueData();
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
        private void llPatientInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (lblPatientID.Text == "[???]")
                return;
            frmPatientInfo frm = new frmPatientInfo(int.Parse(lblPatientID.Text));
            frm.ShowDialog();
        }

        private void llDoctorInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (lblDoctorID.Text == "[???]")
                return;
            frmDoctorInfo frm = new frmDoctorInfo(int.Parse(lblDoctorID.Text));
            frm.ShowDialog();
        }

        private void dataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGrid = sender as DataGridView;
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && (dataGrid.Columns[e.ColumnIndex].Name == "Delete" || dataGrid.Columns[e.ColumnIndex].Name == "Edit"))
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
                DialogResult result = ConfirmAction("Are you sure you want to delete this Medicine?");
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


                frm.DataBack += (senderForm) =>
                {
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
                DialogResult result = ConfirmAction("Are you sure you want to delete this Services?");
                if (result == DialogResult.Yes)
                {
                    _dtAllServices.Rows.RemoveAt(e.RowIndex);
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
                    _RefreshQueueData();
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

        private void frmDoctor_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer2.Stop();
            timer2.Dispose();

            timer1.Stop();
            timer1.Dispose();
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح فقط بالأرقام، أزرار التحكم (BackSpace)، والفاصلة
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // منع تكرار الفاصلة العشرية
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            decimal value;
            if (decimal.TryParse(txtDiscount.Text, out value))
            {
                txtDiscount.Text = value.ToString("F2");
            }
            else
            {
                // إذا كان الحقل فارغاً أو خطأ، اجعله صفر
                txtDiscount.Text = "0.00";
            }
        }
        #endregion

        #region Code Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            _RefreshQueueData();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            lbDate.Text = DateTime.Now.ToString();
        }




        #endregion



    }
}