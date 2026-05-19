using Clinic.Person;
using Clinic.Person.Controls;
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
    public partial class frmTrashPeople : Form
    {
        // سحب البيانات عند الطلب لمنع الـ Null وإمكانية إعادة التحميل الآمن
        private DataTable _dtAllPeople;

        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;

        public frmTrashPeople()
        {
            InitializeComponent();

            // ربط حدث الجنس برمجياً لضمان الفلترة الفورية
            cbGendor.SelectedIndexChanged += cbFilterGender_SelectedIndexChanged;
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _LoadTrashData();
        }

        private void _LoadTrashData()
        {

            _dtAllPeople = clsPerson.GetAllPersonsToTrash();
            cbFilterBy.SelectedIndex = 0;

            if (_dtAllPeople != null)
            {
                // تعيين المفتاح الأساسي لتسريع البحث والحذف المحلي في الـ DataTable
                _dtAllPeople.PrimaryKey = new DataColumn[] { _dtAllPeople.Columns["PersonID"] };
                dgvPeople.DataSource = _dtAllPeople;

                if (_dtAllPeople.Rows.Count > 0)
                {
                    _SetupDataGridViewColumns();
                }
                else
                {
                    lbresore.Enabled = false;
                    lbdelete.Enabled = false;
                    btnresore.Enabled = false;
                    btndelete.Enabled = false;
                }
            }

            _RefreshUIState();
        }

        private void _SetupDataGridViewColumns()
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

        private void _RefreshUIState()
        {
            int RowsCount = dgvPeople.Rows.Count;
            lblRecordsCount.Text = RowsCount.ToString();

            // إظهار نص العنوان "No Anything" فقط إذا كانت السلة فارغة تماماً
            lblTitle.Visible = (RowsCount == 0);

            // تحكم مركزي بجميع أزرار الواجهة والـ ContextMenu بناءً على وجود بيانات
            bool HasRows = RowsCount > 0;
            btnResoreAll.Enabled = HasRows;
            btnDeleteAll.Enabled = HasRows;
            lbResoreAll.Enabled = HasRows;
            lbDeleteAll.Enabled = HasRows;
            cmsPeople.Enabled = HasRows;
           
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsNone = cbFilterBy.Text == "None";
            bool IsGender = cbFilterBy.Text == "Gender";

            txtFilterValue.Visible = !IsNone && !IsGender;
            cbGendor.Visible = IsGender;

            if (_dtAllPeople != null)
                _dtAllPeople.DefaultView.RowFilter = string.Empty;

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();
            }
            else if (cbGendor.Visible)
            {
                cbGendor.SelectedIndex = 0;
            }

            _RefreshUIState();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // منع إدخال أي شيء عدا الأرقام لحقل المعرف
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (_dtAllPeople == null) return;

            string FilterColumn = _GetFilterColumnName(cbFilterBy.Text);

            if (string.IsNullOrEmpty(txtFilterValue.Text) || FilterColumn == "None")
            {
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
            }
            else if (FilterColumn == "PersonID")
            {
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn} = {txtFilterValue.Text}";
            }
            else
            {
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn} LIKE '%{txtFilterValue.Text}%'";
            }

            _RefreshUIState();
        }

        private string _GetFilterColumnName(string SelectedItem)
        {
            switch (SelectedItem)
            {
                case "Person ID": return "PersonID";
                case "Full Name": return "FullName";
                case "Phone": return "Phone";
                case "Country": return "Country";
                case "Address": return "Address";
                case "Email": return "Email";
                default: return "None";
            }
        }

        private void cbFilterGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtAllPeople == null) return;

            if (cbGendor.Text == "All")
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
            else
                _dtAllPeople.DefaultView.RowFilter = $"Gender = '{cbGendor.Text}'";

            _RefreshUIState();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.IsNewRow) return;

            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;

            if (MessageBox.Show("Are you sure you want to permanently delete this person from the system?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (clsPerson.Delete(PersonID))
                {
                    _RemoveRowFromLocalDataTable(PersonID);
                }
                else
                {
                    MessageBox.Show("Error: Could not delete this person due to dependencies.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.IsNewRow) return;

            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;

            if (clsPerson.Restore(PersonID))
            {
                _RemoveRowFromLocalDataTable(PersonID);
                DataBack?.Invoke(this, PersonID); // إرسال المعرف للشاشة السابقة لتحديث نفسها فوراً
            }
        }

        private void _RemoveRowFromLocalDataTable(int PersonID)
        {
            DataRow row = _dtAllPeople.Rows.Find(PersonID);
            if (row != null)
            {
                _dtAllPeople.Rows.Remove(row);
                _dtAllPeople.AcceptChanges();
                _RefreshUIState();
            }
        }

        private void toolStripResoreAll_Click(object sender, EventArgs e) // Restore All
        {
            if (_dtAllPeople == null || _dtAllPeople.Rows.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to restore all persons?", "Confirm Restore All", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            // هندسة برمجية حاسمة: نقوم بإنشاء قائمة مؤقتة بالمعرفات 
            // لأن المسح المباشر داخل الـ Rows أثناء الدوران يفجر التطبيق
            List<int> PersonIDsToRestore = new List<int>();
            foreach (DataRow row in _dtAllPeople.Rows)
            {
                PersonIDsToRestore.Add((int)row["PersonID"]);
            }

            foreach (int PersonID in PersonIDsToRestore)
            {
                if (clsPerson.Restore(PersonID))
                {
                    DataRow row = _dtAllPeople.Rows.Find(PersonID);
                    if (row != null) _dtAllPeople.Rows.Remove(row);

                    DataBack?.Invoke(this, PersonID);
                }
            }

            _dtAllPeople.AcceptChanges();
            _RefreshUIState();
        }

        private void toolStripDeleteAll_Click(object sender, EventArgs e) // Empty Trash (Delete All)
        {
            if (_dtAllPeople == null || _dtAllPeople.Rows.Count == 0) return;

            if (MessageBox.Show("CRITICAL WARNING!\nAre you sure you want to PERMANENTLY wipe out all records in the trash? This cannot be undone!", "Confirm Wipe Out", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
            {
                if (clsPerson.DeleteAllForTrash())
                {
                    _dtAllPeople.Rows.Clear();
                    _dtAllPeople.AcceptChanges();
                    _RefreshUIState();
                }
            }
        }

        private void dgvPeople_SelectionChanged(object sender, EventArgs e)
        {
            // حماية دفاعية (Defensive Coding) لمنع الـ NullReferenceException عند خلو الجدول
            bool IsRowSelected = dgvPeople.CurrentRow != null && !dgvPeople.CurrentRow.IsNewRow;

            lbdelete.Enabled = IsRowSelected;
            btndelete.Enabled = IsRowSelected;
            lbresore.Enabled = IsRowSelected;
            btnresore.Enabled = IsRowSelected;

            // مزامنة تفعيل خيارات الكليك يمين أيضاً
            deletToolStripMenuItem.Enabled = IsRowSelected;
            restoreToolStripMenuItem.Enabled = IsRowSelected;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}