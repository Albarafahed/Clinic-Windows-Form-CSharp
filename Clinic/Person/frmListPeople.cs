using Clinic.Person;
using Clinic.Person.Controls;
using Clinic_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic
{
    public partial class frmListPeople : Form
    {
        private DataTable _dtAllPeople = clsPerson.GetAllPersons();




        public frmListPeople()
        {
            InitializeComponent();

        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            if (_dtAllPeople != null)
            {
                _dtAllPeople.PrimaryKey = new DataColumn[] { _dtAllPeople.Columns["PersonID"] };
                _dtAllPeople.Columns["GenderCaption"].ReadOnly = false;
                dgvPeople.DataSource = _dtAllPeople;
                lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
                cbFilterBy.SelectedIndex = 0;
                if (dgvPeople.Rows.Count > 0)
                {
                    dgvPeople.Columns["PersonID"].HeaderText = "PersonID";
                    dgvPeople.Columns["PersonID"].Width = 100;

                    dgvPeople.Columns["FullName"].HeaderText = "FullName";
                    dgvPeople.Columns["FullName"].Width = 200;

                    dgvPeople.Columns["DateOfBirth"].HeaderText = "DateOfBirth";
                    dgvPeople.Columns["DateOfBirth"].Width = 150;

                    dgvPeople.Columns["GenderCaption"].HeaderText = "Gender";
                    dgvPeople.Columns["GenderCaption"].Width = 100;

                    dgvPeople.Columns["PhoneNumber"].HeaderText = "Phone";
                    dgvPeople.Columns["PhoneNumber"].Width = 170;

                    dgvPeople.Columns["CountryName"].HeaderText = "Country";
                    dgvPeople.Columns["CountryName"].Width = 100;

                    dgvPeople.Columns["Address"].HeaderText = "Address";
                    dgvPeople.Columns["Address"].Width = 170;

                    dgvPeople.Columns["Email"].HeaderText = "Email";
                    dgvPeople.Columns["Email"].Width = 170;


                }
            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dtAllPeople.DefaultView.RowFilter = string.Empty;
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();

            bool None = cbFilterBy.Text == "None";
            bool Gender = cbFilterBy.Text == "Gender";

            txtFilterValue.Visible = !None && !Gender;
            cbFilterGender.Visible = Gender && !None;

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();

            }
            else if (cbFilterGender.Visible)
            {
                cbFilterGender.SelectedIndex = 0;
                cbFilterBy.Focus();

            }
            else
            {
                cbFilterBy.Focus();
                txtFilterValue.Text = "";

            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (_dtAllPeople == null) return;
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Phone":
                    FilterColumn = "PhoneNumber";
                    break;
                case "Country":
                    FilterColumn = "CountryName";
                    break;
                case "Address":
                    FilterColumn = "Address";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilterValue.Text == "" || FilterColumn == "None")
            {
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID")
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn}={txtFilterValue.Text}";
            else
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn} LIKE '%{txtFilterValue.Text}%'";
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
        }

        private void cbFilterGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtAllPeople == null)
                return;
            if (cbFilterGender.Text == "All")
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
            else
                _dtAllPeople.DefaultView.RowFilter = $"GenderCaption = '{cbFilterGender.Text}'";

            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += _DatatBackToAdd;
            frm.ShowDialog();
            frmListPeople_Load(null, null);

        }

        private void _DatatBackToAdd(object sender, int PersonID)
        {
            // 1. جلب بيانات الشخص الجديد كـ DataRow من الـ Business Layer
            DataRow CurrentRow = clsPerson.GetPersonByID(PersonID);

            if (CurrentRow != null)
            {
                // 2. إنشاء سطر جديد فارغ يمتلك نفس هيكلية وأسماء أعمدة الجدول المحلي (_dtAllPeople)
                DataRow NewRow = _dtAllPeople.NewRow();

                // 3. نمر على الأعمدة بالاسم لنسخ البيانات بأمان تام دون الاعتماد على الترتيب الرقمي
                foreach (DataColumn column in _dtAllPeople.Columns)
                {

                    NewRow[column.ColumnName] = CurrentRow[column.ColumnName];
                }

                // 4. إضافة السطر الجديد بعد تعبئته إلى مجموعة أسطر الـ DataTable في الذاكرة
                _dtAllPeople.Rows.Add(NewRow);

                // ستلاحظ أن الـ DataGridView ستعرض السطر الجديد فوراً وبسلاسة لأنها مرتبطة بالـ DataTable
            }

            // 5. تحديث عداد السجلات في الشاشة ليعكس العدد الحقيقي الحالي
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
        }
        private void _DataBackToUpdate(object sender, int PersonID)
        {
            DataRow CurrentRow = clsPerson.GetPersonByID(PersonID);
            if (CurrentRow != null)
            {
                DataRow OldRow = _dtAllPeople.Rows.Find(PersonID);
                if (OldRow != null)
                {
                    // نمر على الأعمدة بالاسم لضمان عدم حدوث أي تداخل في البيانات مستقبلاً
                    foreach (DataColumn column in _dtAllPeople.Columns)
                    {
                        // نستثني الأعمدة التي لا يمكن أو لا يجب تعديلها يدوياً
                        if (column.ColumnName == "PersonID")
                            continue;

                        // التحديث الآمن المبني على أسماء الأعمدة
                        OldRow[column.ColumnName] = CurrentRow[column.ColumnName];
                    }

                    _dtAllPeople.AcceptChanges();
                }
            }
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.DataBack += _DataBackToUpdate;
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            _DataBackToUpdate(null, PersonID);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {

                int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
                //Perform Delele and refresh
                if (clsPerson.SoftDelete(PersonID))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _DataBackToDelete(PersonID);
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void _DataBackToDelete(int PersonID)
        {
            DataRow row = _dtAllPeople.Rows.Find(PersonID);
            if (row != null)
            {
                _dtAllPeople.Rows.Remove(row);
                _dtAllPeople.AcceptChanges();
                lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

     

        private void btnTrash_Click(object sender, EventArgs e)
        {
            frmTrashPeople frm = new frmTrashPeople();
            frm.DataBack += _DatatBackToAdd;
            frm.ShowDialog();
        }
    }
}
