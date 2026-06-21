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

namespace Clinic.Medical_Services.Pharmaciy
{
    public partial class frmPrescriptionDispnsing : Form
    {
        DataTable dtAllDetails = clsPrescription.GetAllPrescriptionDetails();
        DataTable dtAllActivePrescriptions = clsPrescription.GetAllActivePrescriptions();
        private bool _isUpdating = false; // هذا هو مفتاح الحماية
        private int _currentPrescriptionId = -1;
        private int _AppointmentID = -1;
        private int _VisitID = -1;
        public frmPrescriptionDispnsing()
        {
            InitializeComponent();
            dgPrescriptionDetails.DataError += dgPrescriptionDetails_DataError;
            dgPrescriptionDetails.CellValueChanged += dgPrescriptionDetails_CellValueChanged;
           
            dgPrescriptionDetails.CellBeginEdit+= dgPrescriptionDetails_CellBeginEdit;
        }

        private void _AddColumn(string header, string dataProp, bool visible, int fillWeight = 10)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = header;
            col.DataPropertyName = dataProp;
            col.Name = dataProp; // تأكد أن الاسم يطابق الـ DataPropertyName
            col.Visible = visible;
            col.FillWeight = fillWeight;
            col.ReadOnly = true; // كل الأعمدة للقراءة فقط إلا إذا استثنيناها
            dgPrescriptionDetails.Columns.Add(col);
        }

        private void _ResetFormat(string Status)
        {
            lbNotReady.Text = "(Not Ready for Dispensing)";
            lbX.Text = "✕";
            lbStatus.Text = Status;
            panbtnDispense.Enabled = false;
            panbtnSendToAccounting.Enabled = false;
            panbtnCancelOrder.Enabled = false;
            dgPrescriptionDetails.EditMode = DataGridViewEditMode.EditProgrammatically;

            if (Status == "Pending")
            {
                lbStatus.ForeColor = Color.Teal;
                lbNotReady.ForeColor = Color.Teal;
                lbX.ForeColor = Color.Teal;
                lbContainer.BackColor = Color.Teal;

                panbtnSendToAccounting.Enabled = true;
                panbtnCancelOrder.Enabled = true;

                dgPrescriptionDetails.ReadOnly = false;
                dgPrescriptionDetails.EditMode = DataGridViewEditMode.EditOnEnter;



            }
            else if (Status == "Waiting For Payment")
            {
                lbStatus.ForeColor = Color.Brown;
                lbNotReady.ForeColor = Color.Brown;
                lbX.ForeColor = Color.Brown;
                lbContainer.BackColor = Color.Brown;
                panbtnSendToAccounting.Enabled = false;

            }
            else if (Status == "Ready For Dispensing")
            {
                lbStatus.ForeColor = Color.Green;
                lbNotReady.ForeColor = Color.Green;
                lbX.ForeColor = Color.Green;
                lbContainer.BackColor = Color.Green;
                lbNotReady.Text = "(Ready for Dispensing)";
                lbX.Text = "✓";
                panbtnDispense.Enabled = true;

            }

            else if (Status == "PartiallyDispensed")
            {
                lbStatus.ForeColor = Color.GreenYellow;
                lbNotReady.ForeColor = Color.GreenYellow;
                lbX.ForeColor = Color.GreenYellow;
                lbContainer.ForeColor = Color.GreenYellow;
                btnDispense.Enabled = true;
                panbtnSendToAccounting.Enabled = true;
            }

        }
        private void _SetupPrescriptionGrid()
        {
            dgvPrescription.AutoGenerateColumns = false;
            dgvPrescription.Columns.Clear();
            dgvPrescription.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrescription.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPrescription.MultiSelect = false;

            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "VisitID", DataPropertyName = "VisitID", Visible = false });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "AppointmentID", DataPropertyName = "AppointmentID", Visible = false });

            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrescriptionID", DataPropertyName = "PrescriptionID", FillWeight = 15 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "PatientName", HeaderText = "Patient", DataPropertyName = "PatientName", ReadOnly = true, FillWeight = 30 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "DoctorName", HeaderText = "Doctor", DataPropertyName = "DoctorName", ReadOnly = true, FillWeight = 20 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrescriptionDate", HeaderText = "Date", DataPropertyName = "PrescriptionDate", ReadOnly = true, FillWeight = 20 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", DataPropertyName = "Status", ReadOnly = true, FillWeight = 15 });

            dgvPrescription.DataSource = dtAllActivePrescriptions;
        }
        private void _SetupPrescriptionDetailsGrid()
        {
            dgPrescriptionDetails.AutoGenerateColumns = false;
            dgPrescriptionDetails.Columns.Clear();
            dgPrescriptionDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgPrescriptionDetails.RowTemplate.Height = 30;

            // تأكد أن الجدول نفسه يسمح بالتعديل
            dgPrescriptionDetails.ReadOnly = false;

            _AddColumn("Status", "AvailableStatus", true, 30);
            _AddColumn("ID", "PrescriptionDetailsID", false);
            _AddColumn("PID", "PrescriptionID", false);
            _AddColumn("Medicine", "MedicineName", true, 20);
            _AddColumn("Price", "SavedMedicinePrice", true, 20);
            _AddColumn("Discount", "DiscountAmount", true, 10);
            _AddColumn("Dosage", "Dosage", true, 15);
            _AddColumn("Freq", "Frequency", true, 10);
            _AddColumn("Req Qty", "RequiredQuantity", true, 10);

            // هذا العمود يجب أن يكون ReadOnly = false
            DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn();
            colQty.HeaderText = "Disp Qty";
            colQty.DataPropertyName = "DispensedQuantity";
            colQty.Name = "DispensedQuantity"; // هام جداً للوصول للخلية
            colQty.ReadOnly = false; // هنا نفتح التعديل
            colQty.FillWeight = 10;
            dgPrescriptionDetails.Columns.Add(colQty);

            // إضافة أعمدة خاصة (Check Box) - تأكد أنها أيضاً غير مغلقة
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn
            {
                Name = "IsDispensed",
                HeaderText = "Dispensed",
                DataPropertyName = "IsDispensed",
                ReadOnly = false, // تأكد من هذه
                FillWeight = 10
            };
            dgPrescriptionDetails.Columns.Add(chk);

            _AddColumn("Instr.", "Instructions", false, 15);
        }

        private int _GetMaxAvailable(DataGridViewRow row)
        {
            string status = row.Cells["AvailableStatus"].Value.ToString();
            if (status == "Fully Available")
                return Convert.ToInt32(row.Cells["RequiredQuantity"].Value);

            if (status.StartsWith("Partially Available"))
            {
                string numStr = status.Substring(status.IndexOf("(") + 1, status.IndexOf(")") - status.IndexOf("(") - 1);
                return int.Parse(numStr);
            }
            return 0;
        }

        private void _CalculatePrescriptionTotals(DataView dv, out decimal totalNet, out decimal totalTax)
        {
            totalNet = 0;
            totalTax = 0;

            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                if (row.RowState == DataRowState.Deleted) continue; // حماية

                if (Convert.ToBoolean(row["IsDispensed"] ?? false))
                {
                    decimal price = Convert.ToDecimal(row["SavedMedicinePrice"] ?? 0);
                    int qty = Convert.ToInt32(row["DispensedQuantity"] ?? 0);
                    decimal discount = Convert.ToDecimal(row["DiscountAmount"] ?? 0);
                    decimal taxRate = Convert.ToDecimal(row["TaxRate"] ?? 0) / 100;

                    decimal itemNet = (price * qty) - discount;

                    // 2. حساب الضريبة على المبلغ الصافي
                    decimal itemTax = itemNet * taxRate;

                    totalNet += itemNet;
                    totalTax += itemTax;

                }
            }

           
        }

        private void _RefrashData()
        {
            //dtAllDetails = clsPrescription.GetAllPrescriptionDetails();
            //dgPrescriptionDetails.DataSource = dtAllDetails;

            dtAllActivePrescriptions = clsPrescription.GetAllActivePrescriptions();
            dgvPrescription.DataSource = dtAllActivePrescriptions;
        }
        private void frmPrescriptionDispnsing_Load(object sender, EventArgs e)
        {
            dtAllDetails = clsPrescription.GetAllPrescriptionDetails();
            dtAllDetails.DefaultView.AllowEdit = true;
            dtAllDetails.Columns["DispensedQuantity"].ReadOnly = false;
            dtAllDetails.Columns["IsDispensed"].ReadOnly = false;
            lbCurrentUser.Text = clsGlobal.CurrentUser.PersonInfo.Name;
            _SetupPrescriptionGrid();
            _SetupPrescriptionDetailsGrid();
           
            dgPrescriptionDetails.DataBindingComplete += DgPrescriptionDetails_DataBindingComplete;
        }

        // 1. تحسين التحقق في CellBeginEdit
        private void dgPrescriptionDetails_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string columnName = dgPrescriptionDetails.Columns[e.ColumnIndex].Name;

            // الشروط تنطبق على الكمية أو الـ CheckBox
            if (columnName == "DispensedQuantity" || columnName == "IsDispensed")
            {
                string status = dgPrescriptionDetails.Rows[e.RowIndex].Cells["AvailableStatus"].Value.ToString();

                // 1. إذا كان الدواء غير متوفر (Out of Stock)، امنع التعديل تماماً
                if (status == "Out of Stock")
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        // 2. تحسين دالة الإرسال للكاشير (استخدام GetChanges لتقليل البيانات المنقولة)
        private void btnSendToAccounting_Click(object sender, EventArgs e)
        {
            dgPrescriptionDetails.EndEdit();
            this.BindingContext[dgPrescriptionDetails.DataSource].EndCurrentEdit();

            // نستخدم الـ DataView المفلتر حالياً
            DataView dv = (DataView)dgPrescriptionDetails.DataSource;

            bool hasDispensedItems = false;
            bool isDispensed=false;
            foreach (DataRowView row in dv)
            {

                isDispensed = (row["IsDispensed"] != DBNull.Value) && Convert.ToBoolean(row["IsDispensed"]);
                if (isDispensed)
                {
                    hasDispensedItems = true;
                    break; // وجدنا دواء واحداً، لا داعي لإكمال البحث
                }
            }

            if (!hasDispensedItems)
            {
                MessageBox.Show("لا يمكن الإرسال: يجب تحديد دواء واحد على الأقل للصرف (IsDispensed).",
                                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal TotalMedicinesAmount, TaxRate;
            _CalculatePrescriptionTotals(dv, out TotalMedicinesAmount, out TaxRate);

            if (TotalMedicinesAmount <= 0)
            {
                MessageBox.Show("Please dispense at least one item.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // إرسال الجدول المفلتر والمحدث
            if (clsPrescription.SendToCashier(_currentPrescriptionId, dv.ToTable(), _VisitID, _AppointmentID, TotalMedicinesAmount, TaxRate, clsGlobal.CurrentUser.UserID))
            {
                dtAllDetails.AcceptChanges(); // تحديث الذاكرة بعد النجاح
                MessageBox.Show("Prescription successfully sent!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefrashData();
            }
            else
            {
                MessageBox.Show("Prescription Faild sent!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void dgvPrescription_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrescription.SelectedRows.Count > 0)
            {
                lbPatientName.Text= dgvPrescription.SelectedRows[0].Cells["PatientName"].Value.ToString()+")";
                lbPatientNameDetalis.Text = $"[{lbPatientName.Text}]";
                _currentPrescriptionId = Convert.ToInt32(dgvPrescription.SelectedRows[0].Cells["PrescriptionID"].Value);
                lbOrderID.Text= $"(Order [{_currentPrescriptionId.ToString()}] )";
                _AppointmentID = Convert.ToInt32(dgvPrescription.SelectedRows[0].Cells["AppointmentID"].Value);
                _VisitID = Convert.ToInt32(dgvPrescription.SelectedRows[0].Cells["VisitID"].Value);
                string Status = dgvPrescription.SelectedRows[0].Cells["Status"].Value.ToString();
                _ResetFormat(Status);
                // فلترة البيانات من الذاكرة
                dtAllDetails.DefaultView.RowFilter = $"PrescriptionID = {_currentPrescriptionId}";
                dtAllDetails.Columns["IsDispensed"].ReadOnly = false;
                dgPrescriptionDetails.DataSource = dtAllDetails.DefaultView;

                DataView dv = (DataView)dgPrescriptionDetails.DataSource;

                decimal TotalMedicinesAmount, TaxRate;
                _CalculatePrescriptionTotals(dv, out TotalMedicinesAmount, out TaxRate);

                lbTotalMedicen.Text = (TotalMedicinesAmount + TaxRate).ToString();

            }
        }


        private void dgPrescriptionDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 1. منع تكرار الحدث (مفتاح الحماية)
            if (_isUpdating || e.RowIndex < 0) return;

            _isUpdating = true; // تفعيل الحماية

            try
            {
                string colName = dgPrescriptionDetails.Columns[e.ColumnIndex].Name;
                DataGridViewRow row = dgPrescriptionDetails.Rows[e.RowIndex];
                DataRowView rowView = (DataRowView)row.DataBoundItem;

                string status = row.Cells["AvailableStatus"].Value?.ToString() ?? "";
                int maxAvailable = _GetMaxAvailable(row);

                // 2. تحديث المنطق بناءً على العمود المعدل
                if (colName == "IsDispensed")
                {
                    object val = row.Cells["IsDispensed"].Value;
                    bool isChecked = (val != null && val != DBNull.Value) && Convert.ToBoolean(val);

                    if (isChecked && status == "Out of Stock")
                    {
                        rowView["IsDispensed"] = false;
                    }
                    else if (isChecked)
                    {
                        rowView["DispensedQuantity"] = maxAvailable;
                    }
                    else
                    {
                        rowView["DispensedQuantity"] = 0;
                    }
                }
                else if (colName == "DispensedQuantity")
                {
                    int newQty = Convert.ToInt32(row.Cells["DispensedQuantity"].Value ?? 0);

                    if (newQty > maxAvailable)
                    {
                        MessageBox.Show($"عذراً، المتاح للصرف لهذا الصنف هو {maxAvailable} فقط.");
                        rowView["DispensedQuantity"] = maxAvailable;
                    }
                    else
                    {
                        rowView["IsDispensed"] = (newQty > 0);
                    }
                }

                // 3. إنهاء التعديل للتأكد من انتقال القيمة للـ DataTable
                rowView.EndEdit();

                // 4. تحديث السعر الإجمالي
                DataView dv = (DataView)dgPrescriptionDetails.DataSource;
                //DataTable dtDispensed = dv.ToTable();

                decimal TotalMedicinesAmount, TaxRate;
                _CalculatePrescriptionTotals(dv, out TotalMedicinesAmount, out TaxRate);

                lbTotalMedicen.Text = (TotalMedicinesAmount + TaxRate).ToString("N2");

            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث البيانات: " + ex.Message);
            }
            finally
            {
                _isUpdating = false; // إيقاف الحماية دائماً
            }
        }
        private void DgPrescriptionDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }
      
        private void dgPrescriptionDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // إلغاء ظهور رسالة الخطأ الافتراضية للنظام
            e.ThrowException = false;

            // اختياري: يمكنك هنا تسجيل الخطأ في Logger أو تنبيه المستخدم
            // MessageBox.Show("حدث خطأ في البيانات: " + e.Exception.Message);
        }


        private void btnExit_Click(object sender, EventArgs e) => this.Close();
   
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lblPlaceholderSer.Visible = string.IsNullOrEmpty(txtSearch.Text);
            string filter = string.IsNullOrEmpty(txtSearch.Text) ? "" : $"PatientName LIKE '%{txtSearch.Text}%'";
            dtAllActivePrescriptions.DefaultView.RowFilter = filter;

        }

        private void btnPrintShotageSlip_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure ...", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if(clsPrescription.UpdatePrescriptionStatus(_currentPrescriptionId,clsPrescription.enPrescriptionStatus.Cancelled))
                {
                    MessageBox.Show("Succes FullY ...");
                    _RefrashData();
                }
                else
                {
                    MessageBox.Show("Succes FullY ...");


                }
            }
        }

       

        private void lblPlaceholderSer_Click(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }
    }
}
