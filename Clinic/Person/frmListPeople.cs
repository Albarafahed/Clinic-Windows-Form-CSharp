using Clinic.Person;
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

namespace Clinic
{
    public partial class frmListPeople : Form
    {
        private DataTable _dtAllPeople=clsPerson.GetAllPersons();
      



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
                    dgvPeople.Columns[0].HeaderText = "PersonID";
                    dgvPeople.Columns[0].Width = 110;

                    dgvPeople.Columns[1].HeaderText = "FullName";
                    dgvPeople.Columns[1].Width = 300;

                    dgvPeople.Columns[2].HeaderText = "DateOfBirth";
                    dgvPeople.Columns[2].Width = 150;

                    dgvPeople.Columns[3].HeaderText = "Gender";
                    dgvPeople.Columns[3].Width = 120;

                    dgvPeople.Columns[4].HeaderText = "Phone";
                    dgvPeople.Columns[4].Width = 170;

                    dgvPeople.Columns[5].HeaderText = "Country";
                    dgvPeople.Columns[5].Width = 130;

                    dgvPeople.Columns[6].HeaderText = "Address";
                    dgvPeople.Columns[6].Width = 170;

                    dgvPeople.Columns[7].HeaderText = "Email";
                    dgvPeople.Columns[7].Width = 170;


                }
            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            if(cbFilterBy.Text== "Person ID")
                e.Handled= !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if(_dtAllPeople==null) return;
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
                    FilterColumn = "Phone";
                    break;
                case "Country":
                    FilterColumn = "Country";
                    break;
                case "Address":
                    FilterColumn = "Address";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                default:
                    FilterColumn= "None";
                    break;
            }
            if (txtFilterValue.Text == "" || FilterColumn == "None")
            {
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
                return;
            }

            if(FilterColumn == "PersonID")
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn}={txtFilterValue.Text}";
            else
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn} LIKE '%{txtFilterValue.Text}%'";
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
        }

        private void cbFilterGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtAllPeople == null)
                return;
            if(cbFilterGender.Text == "All")
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
            else
                _dtAllPeople.DefaultView.RowFilter = $"Gender = '{cbFilterGender.Text}'";
               
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm=new frmAddUpdatePerson();
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
    }
}
