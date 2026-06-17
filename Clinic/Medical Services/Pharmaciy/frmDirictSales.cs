using Clinic.Medical_Services.Visit;
using Clinic_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Pharmaciy
{
    public partial class frmDirictSales : Form
    {
        private DataTable _dtAllMedicines = new DataTable();
        public frmDirictSales()
        {
            InitializeComponent();
        }

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

        private void _SetupMedicinesGrid()
        {
            // 1. منع التوليد التلقائي للأعمدة لضمان التحكم بالتنسيق يدوياً
            dgPrescriptionDetails.AutoGenerateColumns = false;

            // منع تكرار بناء الأعمدة إذا تم استدعاء الدالة أكثر من مرة
            if (dgPrescriptionDetails.Columns.Count > 0)
                return;

            // 2. إضافة الأعمدة المخفية (التي يحتاجها النظام في الخلفية)
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicineID", DataPropertyName = "MedicineID", Visible = false });
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "Instructions", HeaderText = "Instructions", DataPropertyName = "Instructions", Visible = false });

            // 3. إضافة وتنسيق عمود اسم العلاج (حجم ثابت وموزون مع منع القص)
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "SavedMedicineName",
                HeaderText = "Medicine Name",
                DataPropertyName = "SavedMedicineName",
                ReadOnly = true,
                Width = 220 // حجم متناسق يعطي مساحة لباقي الأعمدة ويمنع السيطرة على الشاشة
            };
            // تفعيل ميزة نزول النص لسطر جديد تلقائياً إذا كان اسم الدواء طويل جداً
            //nameColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgPrescriptionDetails.Columns.Add(nameColumn);

            // 4. إضافة باقي أعمدة البيانات بأسلوب الأحجام التلقائية المرنة (AllCells) لضمان عدم قص الأرقام
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Dosage",
                HeaderText = "Dosage",
                DataPropertyName = "Dosage",
                ReadOnly = true,
                Width = 220
            });

            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SavedMedicinePrice",
                HeaderText = "Unit Price",
                DataPropertyName = "SavedMedicinePrice",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Qty",
                DataPropertyName = "Quantity",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Frequency",
                HeaderText = "FRQ",
                DataPropertyName = "Frequency",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiscountAmount",
                HeaderText = "Discount",
                DataPropertyName = "DiscountAmount",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Total Price",
                DataPropertyName = "Total",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // 5. دمج أزرار التحكم (تعديل وحذف) في عمود روابط نصية واحد واحترافي
            DataGridViewLinkColumn actionColumn = new DataGridViewLinkColumn
            {
                Name = "Actions",
                HeaderText = "Actions",
                Text = "Edit  |  Delete",
                UseColumnTextForLinkValue = true, // لعرض النص الثابت على كل الأسطر
                ActiveLinkColor = Color.Blue,
                LinkColor = Color.Brown, // لون مميز ومناسب للهوية البصرية للعيادة
                TrackVisitedState = false,
                Width = 110 // مساحة ثابتة وممتازة للكلمتين معاً
            };
            // جعل النص يتوسط الخلية تماماً بشكل أنيق
            actionColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgPrescriptionDetails.Columns.Add(actionColumn);

            // 6. تهيئة مصفوفة البيانات وجدول الـ DataTable
            InitializePrescriptionsTable();

            // 7. اللمسات النهائية لضبط مظهر الجدول بالكامل
            dgPrescriptionDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // الاعتماد على مقاساتنا المحددة فوق
            //dgPrescriptionDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // ليتسع الارتفاع إذا نزل اسم العلاج لسطر جديد
            dgPrescriptionDetails.RowTemplate.Height = 50; // يمكنك تغيير 40 إلى الطول المناسب لك
            // ربط الجدول بمصدر البيانات المعتمد
            dgPrescriptionDetails.DataSource = _dtAllMedicines;

        }
        private void btnSendToAccounting_Click(object sender, EventArgs e)
        {

        }
        private void btnDispense_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddToPrescription_Click(object sender, EventArgs e)
        {
            using (frmAddUpdateMedicineToPrescription frm = new frmAddUpdateMedicineToPrescription(ref _dtAllMedicines))
            {
                frm.DataBack += _DataBack;
                frm.ShowDialog();
               
            }
            //this.Show();

        }

        private void frmDirictSales_Load(object sender, EventArgs e)
        {
            _SetupMedicinesGrid();
        }

        private void CalculateTotalFastestWay()
        {

            // أسرع سطر برميجي في الأداء: يجمع عمود الـ Total من قاعدة البيانات/الذاكرة مباشرة
            decimal totalSum = _dtAllMedicines.Rows.Count > 0 ? Convert.ToDecimal(_dtAllMedicines.Compute("Sum(Total)", "")) : 0;

            // عرض الناتج
            lblTotalAmount.Text = "YER " + totalSum.ToString();
        }
        private void _DataBack(object sender)
        {
            dgPrescriptionDetails.DataSource = _dtAllMedicines;
            CalculateTotalFastestWay();

        }

        private void dgPrescriptionDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. التأكد من أن النقر تم على سطر حقيقي وليس على رأس الجدول (Header)
            if (e.RowIndex < 0) return;

            // 2. التأكد من أن العمود المضغوط هو عمود العمليات "Actions"
            if (dgPrescriptionDetails.Columns[e.ColumnIndex].Name == "Actions")
            {
                // جلب معرف الدواء (MedicineID) للسطر الحالي المختار
                int medicineID = Convert.ToInt32(dgPrescriptionDetails.Rows[e.RowIndex].Cells["MedicineID"].Value);

                // جلب السطر بالكامل من الـ DataTable لاستخدامه في التعديل أو الحذف
                DataRow selectedRow = _dtAllMedicines.Rows[e.RowIndex];

                // 3. تحديد مكان النقرة لمعرفة الكلمة المضغوطة (Edit أم Delete)
                // نأخذ إحداثيات نقرة الماوس داخل الخلية الحالية
                var cellMousePos = dgPrescriptionDetails.PointToClient(Cursor.Position);
                var cellRect = dgPrescriptionDetails.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int clickXInsideCell = cellMousePos.X - cellRect.X; // المسافة الأفقية للنقرة من بداية الخلية

                // نقسم الخلية افتراضياً من المنتصف: 
                // إذا كانت النقرة في النصف الأول (اليمين أو اليسار حسب لغة الواجهة) تكون Edit، وإلا Delete
                // في التصميم العادي: النصف الأول (الأيسر) هو Edit والنصف الثاني (الأيمن) هو Delete
                if (clickXInsideCell < (cellRect.Width / 2))
                {
                    // ----------------------------------------------------
                    // 📝 حدث التعديل (EDIT)
                    // ----------------------------------------------------
                    // هنا تفتح شاشة التعديل وتمرر لها السطر الحالي
                    using (frmAddUpdateMedicineToPrescription frm = new frmAddUpdateMedicineToPrescription(ref _dtAllMedicines, selectedRow))
                    {
                        frm.DataBack += _DataBack;
                        frm.ShowDialog();
                    }
                }
                else
                {
                    // ----------------------------------------------------
                    // ❌ حدث الحذف (DELETE)
                    // ----------------------------------------------------
                    DialogResult result = MessageBox.Show("Are you sure you want to remove this medicine from prescription?",
                                                            "Confirm Delete",
                                                            MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // حذف السطر من مصدر البيانات مباشرة
                        _dtAllMedicines.Rows[e.RowIndex].Delete();
                        _dtAllMedicines.AcceptChanges();

                        // إعادة حساب الإجمالي فوراً بعد الحذف
                        CalculateTotalFastestWay();
                    }
                }
            }
        }

    }
}
