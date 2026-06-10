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

namespace Clinic.Medical_Services.Visit
{
    public partial class frmAddUpdateMedicineToPrescription : Form
    {
        public event Action<object> DataBack;
        public enum enMode { AddNew = 1, Update = 2, ShowVisit = 3 }
        private enMode _Mode = enMode.AddNew;
        private DataRow _Row;
        private DataTable _dtMedicines;
        public frmAddUpdateMedicineToPrescription(ref DataTable dtMedicines)
        {
            InitializeComponent();
            _dtMedicines = dtMedicines; 
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateMedicineToPrescription(ref DataTable dtMedicines, DataRow Row)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _dtMedicines = dtMedicines;
            _Row = Row;
        }

        private void _FillMedicensInCheckBox()
        {
            DataTable dtMedicen = clsPrescription.GetAllMedicines();
            if (dtMedicen != null)
            {
                cbMedicines.DataSource = dtMedicen;
                cbMedicines.DisplayMember = "MedicineName";
                cbMedicines.ValueMember = "MedicineID";
            }
        }

        private void _ResetDefaultValues()
        {
            _FillMedicensInCheckBox();
            cbMedicines.SelectedIndex = 1;
            NUDQuantity.Value = 1;
            txtDosage.Text = "";
            txtInstructions.Text = "";
            if (_Mode == enMode.AddNew)
            {
              
                lblTitle.Text = "Add New Medicine";
              

            }
            else if (_Mode == enMode.Update)
            {
                lblTitle.Text = "Update Medicine Info";

            }

            this.Text = lblTitle.Text;
        }

        private void _LoadData()
        {
            if (_Row == null)
            {
                this.Close();
                return;

            }
            cbMedicines.SelectedValue = _Row["MedicineID"];
            NUDQuantity.Value =(int) _Row["Quantity"];
            txtDosage.Text = _Row["Dosage"].ToString();
            txtInstructions.Text = _Row["Instructions"] == DBNull.Value || _Row["Instructions"] == null ? "" : _Row["Instructions"].ToString();

        }

        private void frmAddUpdateMedicineToPrescription_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void txtDosage_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtDosage.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDosage, "This Falid Is Required");

            }
            else
            
            errorProvider1.SetError(txtDosage, null);
        }

        private void cbMedicines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // البحث عن الصف الذي يحمل الـ ID المحدد
            if (cbMedicines.DataSource == null || cbMedicines.SelectedValue == null)
                return;

            // 2. استخدم int.TryParse أو تحقق من النوع لمنع الأخطاء
            if (int.TryParse(cbMedicines.SelectedValue.ToString(), out int selectedID))
            {
                // 3. البحث باستخدام الفلتر
                DataRow[] foundRows = ((DataTable)cbMedicines.DataSource).Select($"MedicineID = {selectedID}");

                if (foundRows.Length > 0)
                {
                    decimal price = Convert.ToDecimal(foundRows[0]["MedicinePrice"]);
                    lblMedicinePrice.Text = price.ToString("F2"); // تنسيق الرقم ليظهر بمنزلتين عشريتين
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please fill all required fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (cbMedicines.SelectedValue == null) return;




            if (_Mode == enMode.AddNew)
            {
                DataRow[] existingRows = _dtMedicines.Select($"MedicineID = {(int)cbMedicines.SelectedValue}");
                if (existingRows.Length > 0)
                {
                    MessageBox.Show("هذه الوصفة مضافة بالفعل للزيارة!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // إنشاء صف جديد في الجدول الممرر
                DataRow newRow = _dtMedicines.NewRow();
                newRow["MedicineID"] = cbMedicines.SelectedValue;
                newRow["SavedMedicineName"] = cbMedicines.Text;
                newRow["Quantity"] = NUDQuantity.Value;
                newRow["SavedMedicinePrice"] = lblMedicinePrice.Text;
                newRow["Dosage"]=txtDosage.Text;
                newRow["Instructions"] = txtInstructions.Text;

                _dtMedicines.Rows.Add(newRow);
                DataBack?.Invoke(this);
            }
            else if (_Mode == enMode.Update)
            {
                // تحديث الصف الحالي
                _Row["MedicineID"] = cbMedicines.SelectedValue;
                _Row["SavedMedicineName"] = cbMedicines.Text;
                _Row["Quantity"] = NUDQuantity.Value;
                _Row["SavedMedicinePrice"] = lblMedicinePrice.Text;
                _Row["Dosage"] = txtDosage.Text;
                _Row["Instructions"] = txtInstructions.Text;
                DataBack?.Invoke(this);
                this.Close();
            }

           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
