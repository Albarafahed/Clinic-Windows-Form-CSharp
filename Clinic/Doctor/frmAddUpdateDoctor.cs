using Clinic.global_classes;
using Clinic_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Clinic_Business.clsDoctor;

namespace Clinic.Doctor
{
    public partial class frmAddUpdateDoctor : Form
    {
        private int _DoctorID = -1; // -1 indicates a new doctor, otherwise it's an update
        private clsDoctor _Doctor;
        public enum enMode { AddNew = 1, Update = 2 };
        private enMode _Mode = enMode.AddNew;

        public delegate void frmAddUpdateDoctorEventHandler(object sender, int DoctorID);

        public event frmAddUpdateDoctorEventHandler DataBack;

        public frmAddUpdateDoctor()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateDoctor(int DoctorID)
        {
            InitializeComponent();
            _DoctorID = DoctorID;
            _Mode = enMode.Update;
        }

        private void _FillSpecializationsInCheckBox()
        {
            DataTable dt=clsSpecialization.GetAllSpecializations(); 
            if(dt!=null && dt.Rows.Count>0)
            {
                clbSpesalizations.DataSource = dt;
                clbSpesalizations.DisplayMember = "SpecializationName";
                clbSpesalizations.ValueMember = "SpecializationID";
                clbSpesalizations.SetItemChecked(clbSpesalizations.FindString("General Practice"), true);
                clbSpesalizations.SetSelected(clbSpesalizations.FindString("General Practice"), true);
            }
        }

        private void _FillWorkingDays()
        {
            // 1. التأكد من عدم التكرار
            if (clbWorkingDays.Items.Count > 0) return;

            // 2. تجميد الرسم (Suspension of Painting)
            clbWorkingDays.BeginUpdate();

            try
            {
                // 3. إضافة العناصر
                foreach (string day in Enum.GetNames(typeof(enDayOfWeek)))
                {
                    clbWorkingDays.Items.Add(day);
                }
            }
            finally
            {
                // 4. إعادة تفعيل الرسم مرة واحدة فقط في النهاية
                clbWorkingDays.EndUpdate();
            }

            clbWorkingDays.SetItemChecked((int)enDayOfWeek.Sunday -1, true);
            clbWorkingDays.SetSelected((int)enDayOfWeek.Sunday - 1, true);
        }

        private void _ResetDefaultValues()
        {
            _FillSpecializationsInCheckBox();
            _FillWorkingDays();

            tpDoctorInfo.Enabled = false;
            btnSave.Enabled = false;

            if (_Mode == enMode.AddNew)
            {
                _Doctor = new clsDoctor();
                ctrlPersonCardWithFilter1.FilterEnabled = true;
                ctrlPersonCardWithFilter1.FocuseTextBox(); // تأكد أن الدالة موجودة بكارت الفلتر لديك
                lblTitle.Text = "Add New Doctor";
            }
            else
            {
                lblTitle.Text = "Update Doctor Info";
            }

            this.Text = lblTitle.Text;
        }

        private void _LoadData()
        {
            _Doctor = clsDoctor.FindDoctorByID(_DoctorID);
            if (_Doctor == null)
            {
                MessageBox.Show("No Doctor with ID = " + _DoctorID, "Doctor Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            ctrlPersonCardWithFilter1.LoadPersonInfo(_Doctor.PersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false; 
            lbDoctorID.Text = _Doctor.DoctorID.ToString();
            txtConsultationFees.Text = _Doctor.ConsultationFees.ToString();
            txtLicenseNo.Text = _Doctor.LicenseNumber;
            chkIsActive.Checked = _Doctor.IsActive;

            if(_Doctor.SelectedSpecialtyIDs.Count > 0)
            {
                int lastCheckedIndex = 0;

                for (int i = 0; i < clbSpesalizations.Items.Count; i++)
                {
                    DataRowView row = (DataRowView)clbSpesalizations.Items[i];
                    int specializationID = Convert.ToInt32(row["SpecializationID"]);
                    if (_Doctor.SelectedSpecialtyIDs.Contains(specializationID))
                    {
                        clbSpesalizations.SetItemChecked(i, true);
                        lastCheckedIndex = i;
                    
                    }
                }
                if (clbSpesalizations.Items.Count > 0)
                {
                    clbSpesalizations.SetSelected(lastCheckedIndex, true);
                }
            }

            if (_Doctor.SelectedDayIDs.Count > 0)
            {
                int lastCheckedIndex = 0;

                for (int i = 0; i < clbWorkingDays.Items.Count; i++)
                {
                    // القيمة الرقمية للأيام في قاعدة البيانات تبدأ من 1، 2، 3...
                    // بما أن الـ Enum مرتب من 1 إلى 7، فإن (i + 1) تطابق الـ ID دائماً
                    if (_Doctor.SelectedDayIDs.Contains(i + 1))
                    {
                        clbWorkingDays.SetItemChecked(i, true);
                        lastCheckedIndex = i;
                    }
                }

                clbWorkingDays.SetSelected(lastCheckedIndex, true);
            }

            tpDoctorInfo.Enabled = true;
            btnSave.Enabled = true;
        }
        private void frmAddUpdateDoctor_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
            {
                _LoadData();
            }

        }

        private void tcDoctor_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage.Name == "tpDoctorInfo" && _Mode == enMode.AddNew)
            {
                if (ctrlPersonCardWithFilter1.PersonID == -1 || clsDoctor.IsDoctorExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    e.Cancel = true; // إلغاء الانتقال القسري
                    MessageBox.Show("Please select a valid person first before moving to doctor info.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid! Please correct the errors and try again.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Doctor.PersonID = ctrlPersonCardWithFilter1.PersonID;

            _Doctor.ConsultationFees = Convert.ToSingle(txtConsultationFees.Text.Trim());

            _Doctor.LicenseNumber = txtLicenseNo.Text.Trim();

            _Doctor.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            _Doctor.IsActive = chkIsActive.Checked;

            _Doctor.SelectedDayIDs = clbWorkingDays.CheckedIndices.Cast<int>().Select(index=>index+1).ToList();

            _Doctor.SelectedSpecialtyIDs = clbSpesalizations.GetCheckedIDs("SpecializationID");

            if (_Doctor.SaveDoctor())
            {
                lbDoctorID.Text = _Doctor.DoctorID.ToString();

                _Mode = enMode.Update;
                ctrlPersonCardWithFilter1.FilterEnabled = false;
               
                lblTitle.Text = "Update Doctor Info";
                this.Text = lblTitle.Text;
                DataBack?.Invoke(this, _Doctor.DoctorID);
                MessageBox.Show("Doctor information saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

               

            }
            else
            {

                MessageBox.Show("Failed to save doctor information. Please try again.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             


            }
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                tcDoctor.SelectedTab = tpDoctorInfo;
                tpDoctorInfo.Enabled = true;
                btnSave.Enabled = true;
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a valid person first before moving to doctor info.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            if (clsDoctor.IsDoctorExistForPersonID(ctrlPersonCardWithFilter1.PersonID) && _Mode==enMode.AddNew)
            {
                MessageBox.Show("The selected person is already assigned as a doctor. Please select a different person.", "Duplicate Doctor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tcDoctor.SelectedTab = tpDoctorInfo;
            tpDoctorInfo.Enabled = true;
            btnSave.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtConsultationFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled=!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) ;
        }

        
        private void TextBoxValidating(object sender, CancelEventArgs e)
        {
            TextBox txt = (TextBox) sender ;
            if(string.IsNullOrEmpty(txt.Text)) {
                e.Cancel = true;
                errorProvider1.SetError(txt, "This field is required.");
            } 
            else {
                errorProvider1.SetError(txt, null);
            }
        }

        private void CheckedListBoxValidating(object sender, CancelEventArgs e)
        {
            CheckedListBox checkedListBox = (CheckedListBox)sender;

            if (checkedListBox.CheckedItems.Count == 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(checkedListBox, "Please select at least one specialization.");
            }
            else
            {
                errorProvider1.SetError(checkedListBox, null);
            }
        }

        private void frmAddUpdateDoctor_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FocuseTextBox();
        }
    }
}
