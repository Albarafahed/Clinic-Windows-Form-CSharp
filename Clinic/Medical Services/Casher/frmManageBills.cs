using Clinic_Business;
using System;
using System.Data;
using System.Data.SqlClient; // أو اسم الكلاس الخاص بطبقة البيانات لديك (Business Layer)
using System.Drawing;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Casher
{
    public partial class frmManageBills : Form
    {
        private DataView _dvBills = new DataView();
        private DataTable _dtBills = new DataTable();

        public frmManageBills()
        {
            InitializeComponent();

            txtSearchByPatientName.TextChanged += txtSearchByPatientName_TextChanged;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;

            chePending.CheckedChanged += FilterStatus_CheckedChanged;
            chePartial.CheckedChanged += FilterStatus_CheckedChanged;
            chePaid.CheckedChanged += FilterStatus_CheckedChanged;

            dgMasterBillList.CellFormatting += dgMasterBillList_CellFormatting;
            dgMasterBillList.CellContentClick += dgMasterBillList_CellContentClick;
        }

        private void frmManageBills_Load(object sender, EventArgs e)
        {
            lbCurrentUser.Text = clsGlobal.PersonName;

            chePending.Checked = true;
            chePartial.Checked = true;
            chePaid.Checked = true;

            _RefreshBillsList();
        }

        private void _RefreshBillsList()
        {
             _dtBills = clsBillingService.GetAllBills();
            _dvBills = _dtBills.DefaultView;
            dgMasterBillList.DataSource = _dvBills;

            // تطبيق الفلترة الافتراضية وتحديث العدادات
            _ApplyAdvancedFilter();
            _UpdateKPICards(_dtBills);
            _AddActionButtonsOnce();
        }


        private void _UpdateKPICards(DataTable dt)
        {
            // 1. التحقق من أن جدول البيانات يحتوي على أسطر فعلياً لتجنب الأخطاء
            if (dt == null || dt.Rows.Count == 0)
            {
                lblCountPending.Text = "0";
                lblCountPartial.Text = "0";
                lblCountPaid.Text = "0";
                return;
            }

            // 2. جلب التاريخ المختار من الـ DateTimePicker
            string targetDateStr = dateTimePicker1.Value.ToString("dd/MM/yyyy");

            // 3. فحص هل الكاشير اختار "عرض كل التواريخ" chkAllDates
            CheckBox chkAllDates = (CheckBox)this.Controls["chkAllDates"];

            int pendingCount, partialCount, paidCount;

            if (chkAllDates != null && chkAllDates.Checked)
            {
                // 🔓 إذا كان يعرض كل التواريخ: نحسب الإجمالي الشامل لجميع الفواتير في النظام
                pendingCount = dt.Select("Status = 'Pending'").Length;
                partialCount = dt.Select("Status = 'Partial'").Length;
                paidCount = dt.Select("Status = 'Paid'").Length;
            }
            else
            {
                // 🔒 إذا كان يفلتر بتاريخ معين: نحسب الإحصائيات لهذا التاريخ المختار فقط (تعديلك الممتاز)
                pendingCount = dt.Select($"Status = 'Pending' AND BillDateString = '{targetDateStr}'").Length;
                partialCount = dt.Select($"Status = 'Partial' AND BillDateString = '{targetDateStr}'").Length;
                paidCount = dt.Select($"Status = 'Paid' AND BillDateString = '{targetDateStr}'").Length;
            }

            // 4. تحديث الواجهة الرقمية المضيئة
            lblCountPending.Text = pendingCount.ToString();
            lblCountPartial.Text = partialCount.ToString();
            lblCountPaid.Text = paidCount.ToString();
        }

        private void _ApplyAdvancedFilter()
        {
            string nameTarget = txtSearchByPatientName.Text.Replace("'", "''").Trim();

            // 1. بناء فلتر الحالات
            string statusFilter = "";
            if (chePending.Checked) statusFilter += "'Pending',";
            if (chePartial.Checked) statusFilter += "'Partial',";
            if (chePaid.Checked) statusFilter += "'Paid',";

            if (statusFilter.EndsWith(","))
                statusFilter = statusFilter.TrimEnd(',');

            // 2. فحص خيار منع فلترة التاريخ (عرض كل التواريخ)
            string finalFilter = "";

            // ابحث عن الشيك بوكس الذي أضفناه
            CheckBox chkAllDates = (CheckBox)this.Controls["chkAllDates"];

            if (chkAllDates != null && chkAllDates.Checked)
            {
                // 🔓 إذا كان مؤشراً عليه: نضع شرطاً وهمياً دائماً صحيح ليتم تجاهل التاريخ وعرض الكل
                finalFilter = "1=1";
            }
            else
            {
                // 🔒 إذا لم يكن مؤشراً عليه: نفلتر بالتاريخ المختار كالمعتاد
                string dateTarget = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                finalFilter = $"BillDateString = '{dateTarget}'";
            }

            // 3. دمج فلتر الحالات
            if (!string.IsNullOrEmpty(statusFilter))
            {
                finalFilter += $" AND Status IN ({statusFilter})";
            }
            else
            {
                finalFilter += " AND 1=1"; ;
            }

            // 4. دمج فلتر اسم المريض
            if (!string.IsNullOrEmpty(nameTarget))
            {
                finalFilter += $" AND PatientName LIKE '%{nameTarget}%'";
            }

            // 5. تطبيق التصفية النهائية
            _dvBills.RowFilter = finalFilter;
            _UpdateKPICards(_dtBills);
        }

        private void txtSearchByPatientName_TextChanged(object sender, EventArgs e) => _ApplyAdvancedFilter();
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) => _ApplyAdvancedFilter();
        private void FilterStatus_CheckedChanged(object sender, EventArgs e) => _ApplyAdvancedFilter();

        private void _AddActionButtonsOnce()
        {
            if (dgMasterBillList.Columns.Contains("BillID")) dgMasterBillList.Columns["BillID"].Visible=false;

            if (dgMasterBillList.Columns.Contains("btnPayAction")) return;

            // الزر الأول: الدفع الكامل والإنهاء
            DataGridViewButtonColumn payColumn = new DataGridViewButtonColumn();
            payColumn.Name = "btnPayAction";
            payColumn.HeaderText = "Full Pay";
            payColumn.Text = "💳 Pay All";
            payColumn.UseColumnTextForButtonValue = true;
            payColumn.FlatStyle = FlatStyle.Flat;
            dgMasterBillList.Columns.Add(payColumn);
            
            // الزر الثاني: زر ديناميكي (تعديل ودفع جزئي في حالة المعلق / أو ارتجاع في حالة المدفوع)
            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
            editColumn.Name = "btnEditAction";
            editColumn.HeaderText = "Adjustment";
            editColumn.Text = "📝 Edit";
            editColumn.UseColumnTextForButtonValue = true;
            editColumn.FlatStyle = FlatStyle.Flat;
            dgMasterBillList.Columns.Add(editColumn);

            // الزر الثالث: زر الإلغاء النهائي (يظهر فقط للفواتير المعلقة)
            DataGridViewButtonColumn cancelColumn = new DataGridViewButtonColumn();
            cancelColumn.Name = "btnCancelAction";
            cancelColumn.HeaderText = "Cancel Bill";
            cancelColumn.Text = "❌ Cancel";
            cancelColumn.UseColumnTextForButtonValue = true;
            cancelColumn.FlatStyle = FlatStyle.Flat;
            dgMasterBillList.Columns.Add(cancelColumn);

            // ترتيب الأعمدة لتظهر أقصى اليمين بشكل منظم
            payColumn.DisplayIndex = dgMasterBillList.Columns.Count - 3;
            editColumn.DisplayIndex = dgMasterBillList.Columns.Count - 2;
            cancelColumn.DisplayIndex = dgMasterBillList.Columns.Count - 1;
        }

        private void dgMasterBillList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgMasterBillList.Rows.Count) return;

            string status = dgMasterBillList.Rows[e.RowIndex].Cells["Status"].Value?.ToString();
            if (string.IsNullOrEmpty(status)) return;

            // 🎨 [تلوين حقل الحالة بجميع الحالات الـ 5 المتاحة]
            if (dgMasterBillList.Columns[e.ColumnIndex].Name == "Status")
            {
                if (status == "Paid") { e.CellStyle.ForeColor = Color.SpringGreen; e.CellStyle.Font = new Font(dgMasterBillList.Font, FontStyle.Bold); }
                else if (status == "Partial") { e.CellStyle.ForeColor = Color.Orange; e.CellStyle.Font = new Font(dgMasterBillList.Font, FontStyle.Bold); }
                else if (status == "Pending") { e.CellStyle.ForeColor = Color.Tomato; e.CellStyle.Font = new Font(dgMasterBillList.Font, FontStyle.Bold); }
                else if (status == "Cancelled") { e.CellStyle.ForeColor = Color.Crimson; e.CellStyle.Font = new Font(dgMasterBillList.Font, FontStyle.Bold); } // أحمر صريح للملغية
                else if (status == "Refunded") { e.CellStyle.ForeColor = Color.DodgerBlue; e.CellStyle.Font = new Font(dgMasterBillList.Font, FontStyle.Bold); } // أزرق هادئ للمرتجعة
            }

            // [تنسيق الزر الأول: الدفع الكامل / الطباعة]
            if (dgMasterBillList.Columns[e.ColumnIndex].Name == "btnPayAction")
            {
                if (status == "Pending" || status == "Partial")
                {
                    e.Value = status == "Pending" ? "💳 Pay All" : "🔸 Complete";
                    e.CellStyle.BackColor = Color.FromArgb(14, 114, 95);
                    e.CellStyle.ForeColor = Color.White;
                }
                // إذا كانت الفاتورة مدفوعة أو مرتجعة، يتحول الزر إلى طباعة فوراً
                else if (status == "Paid" || status == "Refunded")
                {
                    e.Value = "🖨️ Print";
                    e.CellStyle.BackColor = Color.FromArgb(28, 58, 68);
                    e.CellStyle.ForeColor = Color.FromArgb(0, 210, 190);
                }
                else // في حالة Cancelled
                {
                    e.Value = "🔒 Locked";
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
                    e.CellStyle.ForeColor = Color.DarkGray;
                }
            }

            // [تنسيق الزر الثاني: زر التعديل والدفع الجزئي / الارتجاع]
            if (dgMasterBillList.Columns[e.ColumnIndex].Name == "btnEditAction")
            {
                if (status == "Pending")
                {
                    e.Value = "📝 Edit";
                    e.CellStyle.BackColor = Color.FromArgb(0, 80, 90);
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (status == "Paid" || status == "Partial")
                {
                    e.Value = "🔄 Refund";
                    e.CellStyle.BackColor = Color.FromArgb(28, 58, 68);
                    e.CellStyle.ForeColor = Color.FromArgb(0, 210, 190);
                }
                else // في حالة Cancelled أو Refunded بالكامل
                {
                    e.Value = "👁️ View";
                    e.CellStyle.BackColor = Color.FromArgb(30, 30, 30);
                    e.CellStyle.ForeColor = Color.DarkGray;
                }
            }

            // [تنسيق الزر الثالث: زر الإلغاء الفوري]
            if (dgMasterBillList.Columns[e.ColumnIndex].Name == "btnCancelAction")
            {
                if (status == "Pending")
                {
                    e.Value = "❌ Cancel";
                    e.CellStyle.BackColor = Color.FromArgb(120, 40, 40);
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.Value = "🔒 Disabled";
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(43)))), ((int)(((byte)(50)))));
                    e.CellStyle.ForeColor = Color.DarkGray;
                }
            }
        }

        private void dgMasterBillList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int billID = Convert.ToInt32(dgMasterBillList.Rows[e.RowIndex].Cells["BillID"].Value);
            string currentStatus = dgMasterBillList.Rows[e.RowIndex].Cells["Status"].Value.ToString();

            // [أ] الضغط على زر الدفع الكامل أو الطباعة (Full Pay / Print)
            if (dgMasterBillList.Columns[e.ColumnIndex].Name == "btnPayAction")
            {
                // 🖨️ إذا كانت مدفوعة أو مرتجعة نفتح شاشة الفاتورة للطباعة والمراجعة فقط
                if (currentStatus == "Paid" || currentStatus == "Refunded")
                {
                    clsFormHelper.ShowForm(() => new frmIssueInvoice(billID));

                    return;
                }

                // 🔒 حظر العمليات في حال كانت الفاتورة ملغية تماماً
                if (currentStatus == "Cancelled")
                {
                    MessageBox.Show("لا يمكن إجراء أي عملية دفع على فاتورة ملغية.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 💳 إذا كانت الفاتورة Pending أو Partial يفتح شاشة معالجة الدفع
                clsFormHelper.ShowForm( ()=>new frmProcessPayments(billID), _RefreshBillsList);
               
            }

            // 📝 [ب] الضغط على زر التعديل والدفع الجزئي أو الارتجاع
            if (dgMasterBillList.Columns[e.ColumnIndex].Name == "btnEditAction")
            {
                // حظر التعديل أو الارتجاع للفواتير الملغية أو التي تم ارتجاعها بالكامل سابقاً
                if (currentStatus == "Cancelled" || currentStatus == "Refunded")
                {
                    MessageBox.Show($"هذه الفاتورة مقفلة تماماً لأنها: {currentStatus}", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // الحالة الحرجة: الفاتورة معلقة والمريض ليس معه مال كافٍ
                if (currentStatus == "Pending")
                {
                    frmEditPendingBill frm = new frmEditPendingBill(billID);
                    frm.ShowDialog();
                    _RefreshBillsList();
                }
                // إذا كانت الفاتورة مدفوعة مسبقاً (Paid أو Partial)، يفتح شاشة الارتجاع
                else if (currentStatus == "Paid" || currentStatus == "Partial")
                {
                    frmRefundInvoice frm = new frmRefundInvoice(billID);
                    frm.ShowDialog();
                    _RefreshBillsList();
                }
            }

            // ❌ [ج] الضغط على زر الإلغاء الفوري للفاتورة المعلقة
            if (dgMasterBillList.Columns[e.ColumnIndex].Name == "btnCancelAction")
            {
                // حماية: منع الإلغاء لأي حالة أخرى غير Pending
                if (currentStatus != "Pending")
                {
                    MessageBox.Show("لا يمكن إلغاء الفواتير المدفوعة أو المرتجعة أو الملغية مسبقاً من هنا.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"هل أنت متأكد من إلغاء الفاتورة رقم {billID} نهائياً؟", "تأكيد الإلغاء",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //_CancelPendingBillInDatabase(billID);
                    _RefreshBillsList();
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkedBox_CheckedChanged(object sender, EventArgs e)=>_ApplyAdvancedFilter();

        private void chkAllDates_CheckedChanged(object sender, EventArgs e) => _ApplyAdvancedFilter();



    }
}