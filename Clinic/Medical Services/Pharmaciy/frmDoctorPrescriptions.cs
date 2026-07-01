
using Clinic_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        decimal TotalMedicinesAmount = 0.0m, TaxRate = 0.0m;
        private NumericUpDown _qtyNumericUpDown = new NumericUpDown();
        public frmPrescriptionDispnsing()
        {
            InitializeComponent();
            dgPrescriptionDetails.DataError += dgPrescriptionDetails_DataError;
            dgPrescriptionDetails.CellValueChanged += dgPrescriptionDetails_CellValueChanged;
            dgPrescriptionDetails.CellClick += dgPrescriptionDetails_CellClick;
            dgPrescriptionDetails.CellBeginEdit += dgPrescriptionDetails_CellBeginEdit;

            _qtyNumericUpDown.Minimum = 0;
            _qtyNumericUpDown.Visible = false; // مخفي في البداية

            // ربط الأحداث الخاصة بالكشاف لتحديث الخلايا عند التغيير وعند الخروج منها
            _qtyNumericUpDown.ValueChanged += QtyNumericUpDown_ValueChanged;
            _qtyNumericUpDown.Leave += QtyNumericUpDown_Leave;

            // إضافة الكشاف إلى عناصر جدول الروشتة
            dgPrescriptionDetails.Controls.Add(_qtyNumericUpDown);

            // ربط حدث الـ Scroll للجدول لإخفاء الكشاف إذا قام المستخدم بالتحريك
            dgPrescriptionDetails.Scroll += (s, ev) => { _qtyNumericUpDown.Visible = false; };
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
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "PatientName", HeaderText = "Patient", DataPropertyName = "PatientName", ReadOnly = true, FillWeight = 20 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "DoctorName", HeaderText = "Doctor", DataPropertyName = "DoctorName", ReadOnly = true, FillWeight = 20 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrescriptionTime", HeaderText = "Time", DataPropertyName = "PrescriptionTime", ReadOnly = true, FillWeight = 10 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", DataPropertyName = "Status", ReadOnly = true, FillWeight = 15 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", DataPropertyName = "Status", ReadOnly = true, FillWeight = 15 });
            dgvPrescription.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrescriptionType", HeaderText = "PrescriptionType", DataPropertyName = "PrescriptionType", ReadOnly = true, FillWeight = 15 });

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
            dtAllDetails = clsPrescription.GetAllPrescriptionDetails();
            dgPrescriptionDetails.DataSource = dtAllDetails;

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

        }

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

        private void btnSendToAccounting_Click(object sender, EventArgs e)
        {
            dgPrescriptionDetails.EndEdit();
            this.BindingContext[dgPrescriptionDetails.DataSource].EndCurrentEdit();

            // نستخدم الـ DataView المفلتر حالياً
            DataView dv = (DataView)dgPrescriptionDetails.DataSource;

            bool hasDispensedItems = false;
            bool isDispensed = false;
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

            //_CalculatePrescriptionTotals(dv, out TotalMedicinesAmount, out TaxRate);

            if (TotalMedicinesAmount <= 0)
            {
                MessageBox.Show("Please dispense at least one item.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // إرسال الجدول المفلتر والمحدث
            if (clsPrescription.SendToCashier(_currentPrescriptionId, dv.ToTable(), _VisitID, clsGlobal.CurrentUser.UserID))
            {
                //dtAllDetails.AcceptChanges(); // تحديث الذاكرة بعد النجاح
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
                lbPatientName.Text = dgvPrescription.SelectedRows[0].Cells["PatientName"].Value.ToString() + ")";
                lbPatientNameDetalis.Text = $"[{lbPatientName.Text}]";
                _currentPrescriptionId = Convert.ToInt32(dgvPrescription.SelectedRows[0].Cells["PrescriptionID"].Value);
                lbOrderID.Text = $"(Order [{_currentPrescriptionId.ToString()}] )";
                _AppointmentID = Convert.ToInt32(dgvPrescription.SelectedRows[0].Cells["AppointmentID"].Value);
                _VisitID = Convert.ToInt32(dgvPrescription.SelectedRows[0].Cells["VisitID"].Value);
                string Status = dgvPrescription.SelectedRows[0].Cells["Status"].Value.ToString();
                _ResetFormat(Status);
                // فلترة البيانات من الذاكرة
                dtAllDetails.DefaultView.RowFilter = $"PrescriptionID = {_currentPrescriptionId}";
                dtAllDetails.Columns["IsDispensed"].ReadOnly = false;
                dgPrescriptionDetails.DataSource = dtAllDetails.DefaultView;

                DataView dv = (DataView)dgPrescriptionDetails.DataSource;

                _CalculatePrescriptionTotals(dv, out TotalMedicinesAmount, out TaxRate);

                lbTotalMedicen.Text = "$ " + (TotalMedicinesAmount + TaxRate).ToString("N2");

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

                _CalculatePrescriptionTotals(dv, out TotalMedicinesAmount, out TaxRate);

                lbTotalMedicen.Text = "$ " + (TotalMedicinesAmount + TaxRate).ToString("N2");

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

        private void dgPrescriptionDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // 🎯 التحقق من الضغط على عمود كمية الصرف تحديداً
            if (dgPrescriptionDetails.Columns[e.ColumnIndex].Name == "DispensedQuantity")
            {
                DataGridViewRow row = dgPrescriptionDetails.Rows[e.RowIndex];

                // جلب الحد الأقصى المتاح لهذا الدواء باستخدام دالتك الحالية _GetMaxAvailable
                int maxAvailable = _GetMaxAvailable(row);

                // إذا كان الصنف خارج المخزن تماماً لا تظهر الكشاف
                if (maxAvailable <= 0)
                {
                    _qtyNumericUpDown.Visible = false;
                    return;
                }

                // ضبط الحد الأقصى للكشاف ليتطابق مع متاح المخزن
                _qtyNumericUpDown.Maximum = maxAvailable;

                // جلب مقاسات وموقع الخلية لتغطيتها تماماً
                Rectangle rect = dgPrescriptionDetails.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _qtyNumericUpDown.Location = rect.Location;
                _qtyNumericUpDown.Size = rect.Size;

                // جلب القيمة الحالية من الخلية ووضعها في الكشاف
                if (row.Cells[e.ColumnIndex].Value != DBNull.Value && row.Cells[e.ColumnIndex].Value != null)
                {
                    _qtyNumericUpDown.Value = Convert.ToDecimal(row.Cells[e.ColumnIndex].Value);
                }
                else
                {
                    _qtyNumericUpDown.Value = 0;
                }

                // إظهار الكشاف وتركيز الماوس عليه
                _qtyNumericUpDown.Visible = true;
                _qtyNumericUpDown.Focus();
            }
            else
            {
                // إذا ضغط على أي عمود آخر (مثل شيك بوكس الصرف) نخفي الكشاف
                _qtyNumericUpDown.Visible = false;
            }
        }
        private void dgPrescriptionDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // إلغاء ظهور رسالة الخطأ الافتراضية للنظام
            e.ThrowException = false;

            // اختياري: يمكنك هنا تسجيل الخطأ في Logger أو تنبيه المستخدم
            // MessageBox.Show("حدث خطأ في البيانات: " + e.Exception.Message);
        }

        // عند تغيير قيمة العداد برمجياً أو بالأسهم
        private void QtyNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (dgPrescriptionDetails.CurrentCell != null && dgPrescriptionDetails.CurrentCell.OwningColumn.Name == "DispensedQuantity")
            {
                // نقل القيمة للجدول فوراً (وهذا بدوره سيطلق دالتك الأصلية CellValueChanged تلقائياً)
                dgPrescriptionDetails.CurrentCell.Value = (int)_qtyNumericUpDown.Value;
            }
        }

        // عند انتقال التركيز أو خروج الكاشير من الخلية
        private void QtyNumericUpDown_Leave(object sender, EventArgs e)
        {
            _qtyNumericUpDown.Visible = false;
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
            // 1. التحقق من وجود بيانات في الـ DataGridView
            if (dgPrescriptionDetails.Rows.Count == 0)
            {
                MessageBox.Show("No data available to report shortage.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. إنشاء كائن الطباعة وربطه بحدث الرسم
            System.Drawing.Printing.PrintDocument printDoc = new System.Drawing.Printing.PrintDocument();
            printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintShortageReport_PrintPage);

            // 3. إظهار نافذة معاينة الطباعة (Print Preview) بشكل احترافي
            PrintPreviewDialog previewDlg = new PrintPreviewDialog();
            previewDlg.Document = printDoc;
            previewDlg.WindowState = FormWindowState.Maximized; // فتح المعاينة بكامل الشاشة
            previewDlg.ShowDialog();
        
        }

        private void PrintShortageReport_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // إعدادات الخطوط والألوان للتقرير
            Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 10, FontStyle.Regular);
            Font totalFont = new Font("Segoe UI", 11, FontStyle.Bold);

            Brush tealBrush = new SolidBrush(Color.FromArgb(0, 105, 92)); // لون التيل المعتمد للهيدر
            Brush textBrush = Brushes.Black;
            Pen linePen = new Pen(Color.DarkGray, 1);

            int startX = 50;
            int startY = 40;
            int offsetY = 0;

            // --- 1. ترويسة التتقرير (Header) ---
            e.Graphics.DrawString("Clinic Management System", headerFont, tealBrush, startX, startY);
            offsetY += 25;
            e.Graphics.DrawString("Medicine Shortage Report", titleFont, textBrush, startX, startY + offsetY);
            offsetY += 35;
            e.Graphics.DrawString($"Date: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}", bodyFont, textBrush, startX, startY + offsetY);
            offsetY += 25;

            // رسم خط فاصل
            e.Graphics.DrawLine(linePen, startX, startY + offsetY, e.PageBounds.Width - 50, startY + offsetY);
            offsetY += 15;

            // --- 2. ترويسة الجدول (Table Headers) ---
            // تحديد أماكن الأعمدة (X Positions)
            int colMedX = startX;       // اسم العلاج
            int colPriceX = startX + 250; // السعر
            int colReqX = startX + 370;   // المطلوبة
            int colDispX = startX + 470;  // المصروفة
            int colShortX = startX + 580; // النواقص

            e.Graphics.DrawString("Medicine Name", headerFont, tealBrush, colMedX, startY + offsetY);
            e.Graphics.DrawString("Price", headerFont, tealBrush, colPriceX, startY + offsetY);
            e.Graphics.DrawString("Req.Qty", headerFont, tealBrush, colReqX, startY + offsetY);
            e.Graphics.DrawString("Disp.Qty", headerFont, tealBrush, colDispX, startY + offsetY);
            e.Graphics.DrawString("Shortage", headerFont, tealBrush, colShortX, startY + offsetY);

            offsetY += 25;
            e.Graphics.DrawLine(linePen, startX, startY + offsetY, e.PageBounds.Width - 50, startY + offsetY);
            offsetY += 10;

            bool hasShortage = false;
            decimal totalShortageValue = 0;

            // --- 3. قراءة البيانات وفحص النواقص ---
            foreach (DataGridViewRow row in dgPrescriptionDetails.Rows)
            {
                // تجنب السطور الفارغة إن وجدت
                if (row.Cells["MedicineName"].Value == null) continue;

                // جلب القيم بأمان مع التحويل الرقمي
                string medicineName = row.Cells["MedicineName"].Value.ToString();
                decimal price = Convert.ToDecimal(row.Cells["SavedMedicinePrice"].Value ?? 0);
                int reqQty = Convert.ToInt32(row.Cells["RequiredQuantity"].Value ?? 0);
                int dispQty = Convert.ToInt32(row.Cells["DispensedQuantity"].Value ?? 0);

                // الشرط: إذا كانت الكمية المصروفة أصغر من الكمية المطلوبة
                if (dispQty < reqQty)
                {
                    hasShortage = true;
                    int shortageQty = reqQty - dispQty; // حساب كمية النواقص
                    decimal shortagePrice = price * shortageQty; // تكلفة النواقص لهذا العلاج
                    totalShortageValue += shortagePrice;

                    // رسم السطر الخاص بالعلاج الناقص
                    e.Graphics.DrawString(medicineName, bodyFont, textBrush, colMedX, startY + offsetY);
                    e.Graphics.DrawString(price.ToString("C2"), bodyFont, textBrush, colPriceX, startY + offsetY);
                    e.Graphics.DrawString(reqQty.ToString(), bodyFont, textBrush, colReqX, startY + offsetY);
                    e.Graphics.DrawString(dispQty.ToString(), bodyFont, textBrush, colDispX, startY + offsetY);

                    // تمييز كمية النواقص بلون أحمر داكن متناسق للفت الانتباه
                    e.Graphics.DrawString(shortageQty.ToString(), headerFont, new SolidBrush(Color.FromArgb(211, 47, 47)), colShortX, startY + offsetY);

                    offsetY += 25;

                    // تحقق من عدم تجاوز حدود الصفحة (أمان إضافي للطباعة)
                    if (startY + offsetY > e.PageBounds.Height - 100)
                    {
                        e.HasMorePages = true;
                        return;
                    }
                }
            }

            // --- 4. التذييل والمجموع (Footer) ---
            if (!hasShortage)
            {
                e.Graphics.DrawString("No shortages found. All medicines are fully dispensed!", bodyFont, Brushes.Green, startX, startY + offsetY + 20);
            }
            else
            {
                offsetY += 15;
                e.Graphics.DrawLine(linePen, startX, startY + offsetY, e.PageBounds.Width - 50, startY + offsetY);
                offsetY += 10;
                e.Graphics.DrawString($"Estimated Shortage Value: {totalShortageValue.ToString("C2")}", totalFont, tealBrush, colPriceX, startY + offsetY);
            }

            e.HasMorePages = false; // لا توجد صفحات أخرى
        }
        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure ...", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsPrescription.UpdatePrescriptionStatus(_currentPrescriptionId, clsPrescription.enPrescriptionStatus.Cancelled))
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

        private void btnDispense_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are You Sure ...", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsPrescription.UpdatePrescriptionStatus(_currentPrescriptionId, clsPrescription.enPrescriptionStatus.Dispensed))
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