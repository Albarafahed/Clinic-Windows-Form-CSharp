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

namespace Clinic.User
{
    public partial class frmlistUser : Form
    {
        // سحب البيانات من طبقة الـ Business
        private DataTable _dtAllUsers = clsUser.GetAllUsers();

        

        public frmlistUser()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmlistUser_Load(object sender, EventArgs e)
        {
            dgvUsers.DataSource = _dtAllUsers;
            cbFilterBy.SelectedIndex = 0; // سيقوم تلقائياً باستدعاء حدث SelectedIndexChanged لتنسيق العناصر

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();

            // تنسيق أعمدة الـ DataGridView حتى لو لم يكن هناك صفوف
            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 110;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 120;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 350;

                dgvUsers.Columns[3].HeaderText = "UserName";
                dgvUsers.Columns[3].Width = 120;

                dgvUsers.Columns[4].HeaderText = "Role Name";
                dgvUsers.Columns[4].Width = 140;

                dgvUsers.Columns[5].HeaderText = "Is Active";
                dgvUsers.Columns[5].Width = 120;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsNone = cbFilterBy.Text == "None";
            bool IsActive = cbFilterBy.Text == "IsActive" || cbFilterBy.Text == "Is Active"; // تحسباً للمسافة

            txtFilterValue.Visible = !IsActive && !IsNone;
            cbIsActive.Visible = IsActive && !IsNone;

            // إعادة تعيين الفلتر عند تغيير العمود المراد البحث به
            _dtAllUsers.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();
            }
            else if (cbIsActive.Visible)
            {
                cbIsActive.SelectedIndex = 0;
                cbIsActive.Focus();
            }
            else
            {
                cbFilterBy.Focus();
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch (FilterValue)
            {
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
                default:
                    FilterValue = "All";
                    break;
            }

            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            // جلب العدد من الـ DataView المفلتر بدقة
            lblRecordsCount.Text = _dtAllUsers.DefaultView.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "User Name":
                    FilterColumn = "UserName";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Role Name":
                    FilterColumn = "RoleName";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            // إذا كان النص فارغاً أو الفلتر None، نقوم بإعادة تصفير الفلترة
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
                return;
            }

            // 💡 الحماية الحديدية: التأكد من أن القيمة رقمية للأعمدة الرقمية لمنع الكراش
            if (FilterColumn == "UserID" || FilterColumn == "PersonID")
            {
                if (int.TryParse(txtFilterValue.Text.Trim(), out _))
                {
                    _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
                }
                else
                {
                    _dtAllUsers.DefaultView.RowFilter = "1=0"; // إخفاء الصفوف في حال إدخال قيمة خاطئة مؤقتاً
                }
            }
            else
            {
                // الفلترة النصية المباشرة باستخدام LIKE
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim().Replace("'", "''")); // تأمين ضد الـ Single Quotes
            }

            lblRecordsCount.Text = _dtAllUsers.DefaultView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 💡 تصحيح الفحص ليعتمد على قيمة الـ ComboBox لتحديد منع الحروف
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
            {
                // السماح بالأرقام وأزرار التحكم كـ Backspace فقط
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }
    }
}