using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Pharmaciy
{
    public partial class frmAllPrescriptions : Form
    {
        public class OptimizedDataGridView : DataGridView
        {
            public OptimizedDataGridView()
            {
                this.DoubleBuffered = true;
            }
        }
        private DataSet _dsPrescriptions;
        private int _expandedRowIndex = -1;
        //private OptimizedDataGridView dgvDetailsPopup;
        //private Panel pnlDetails;

        public frmAllPrescriptions()
        {
            InitializeComponent();

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgPrescriptionDetails, new object[] { true });

            //InitializeChildGrid();
            dgPrescriptionDetails.CellContentClick += dgPrescriptionDetails_CellContentClick;
            dgPrescriptionDetails.Scroll += (s, e) => UpdateChildGridPosition();
            dgPrescriptionDetails.Resize += (s, e) => UpdateChildGridPosition();
        }

        private void SetupGridDesign()
        {
            dgPrescriptionDetails.AutoGenerateColumns = false;
            dgPrescriptionDetails.Columns.Clear();

            // 1. زر التوسيع الملون بالأكوا
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExpandButton",
                HeaderText = "",
                Width = 45,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 210, 190),
                    SelectionForeColor = Color.FromArgb(0, 210, 190)
                }
            });

            // 2. ربط الأعمدة بأسماء الحقول (DataPropertyName) لكي تظهر النصوص داخل الخلايا
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrescriptionID", HeaderText = "Prescription ID", DataPropertyName = "PrescriptionID", ReadOnly = true,Width= 180 });
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Date", DataPropertyName = "Date", ReadOnly = true, Width = 200 });
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "PatientName", HeaderText = "Patient Name", DataPropertyName = "PatientName", ReadOnly = true, Width = 300 });
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "VisitID", HeaderText = "Visit ID", DataPropertyName = "VisitID", ReadOnly = true, Width = 150 });

            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", DataPropertyName = "Status", ReadOnly = true, Width = 150 });
            dgPrescriptionDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "Notes", HeaderText = "Notes", DataPropertyName = "Notes", ReadOnly = true, Width = 200 });
            DataGridViewLinkColumn actionColumn = new DataGridViewLinkColumn
            {
                Name = "Actions",
                HeaderText = "Actions",
                Text = "Edit  |  Delete",
                UseColumnTextForLinkValue = true, // لعرض النص الثابت على كل الأسطر
                ActiveLinkColor = Color.Blue,
                LinkColor = Color.Brown, // لون مميز ومناسب للهوية البصرية للعيادة
                TrackVisitedState = false,
                Width = 148 // مساحة ثابتة وممتازة للكلمتين معاً
            };
            // جعل النص يتوسط الخلية تماماً بشكل أنيق
            actionColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgPrescriptionDetails.Columns.Add(actionColumn);

        }

        private void SetupChildGridDesign()
        {
            dgvDetailsPopup.AutoGenerateColumns = false;

            dgvDetailsPopup.Columns.Clear();
            dgvDetailsPopup.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedicineName", HeaderText = "Medicine Name", DataPropertyName = "MedicineName" });
            dgvDetailsPopup.Columns.Add(new DataGridViewTextBoxColumn { Name = "Dosage", HeaderText = "Dosage", DataPropertyName = "Dosage" });
            dgvDetailsPopup.Columns.Add(new DataGridViewTextBoxColumn { Name = "Frequency", HeaderText = "Frequency", DataPropertyName = "Frequency" });
            dgvDetailsPopup.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Quantity", DataPropertyName = "Quantity" });
            dgvDetailsPopup.Columns.Add(new DataGridViewTextBoxColumn { Name = "Instructions", HeaderText = "Instructions", DataPropertyName = "Instructions" });
            dgvDetailsPopup.Columns.Add(new DataGridViewTextBoxColumn { Name = "Discount", HeaderText = "Discount", DataPropertyName = "Discount" });






        }

        private void dgPrescriptionDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgPrescriptionDetails.Columns[e.ColumnIndex].Name != "ExpandButton") return;

            var currentRow = dgPrescriptionDetails.Rows[e.RowIndex];

            DataGridViewRow containerRow = null;
            if (e.RowIndex + 1 < dgPrescriptionDetails.Rows.Count &&
                dgPrescriptionDetails.Rows[e.RowIndex + 1].Tag?.ToString() == "Container")
            {
                containerRow = dgPrescriptionDetails.Rows[e.RowIndex + 1];
            }

            if (containerRow == null) return;

            if (currentRow.Cells["ExpandButton"].Value.ToString() == "➕")
            {
                // 1. إغلاق أي صفوف أخرى مفتوحة أولاً لمنع التداخل
                CloseAllExpandedRows();

                // 2. تحويل الأيقونة وإظهار الصف الحاوي مبدئياً
                currentRow.Cells["ExpandButton"].Value = "➖";
                containerRow.Visible = true;
                _expandedRowIndex = e.RowIndex;

                // 3. جلب بيانات الأدوية وربطها بالجدول الداخلي
                BindDetails(currentRow.Cells["PrescriptionID"].Value.ToString());

                // 4. استدعاء دالة تحديد موقع وحجم الـ Panel (هنا يتم حساب الارتفاع المطلوب للـ Panel)
                UpdateChildGridPosition();

                // 5. 🔥 الربط الديناميكي السحري:
                // نجعل ارتفاع الصف الحاوي يطابق الارتفاع الفعلي للـ Panel تماماً دون أي تدخل منك
                containerRow.Height = pnlDetails.Height+15;
            }
            else
            {
                CloseAllExpandedRows();
            }
        }

      
        public void CloseAllExpandedRows()
        {
            foreach (DataGridViewRow row in dgPrescriptionDetails.Rows)
            {
                if (row.Cells["ExpandButton"].Value?.ToString() == "➖")
                {
                    row.Cells["ExpandButton"].Value = "➕";
                }
                if (row.Tag?.ToString() == "Container")
                {
                    row.Visible = false;
                }
            }
            pnlDetails.Visible = false;
            _expandedRowIndex = -1;
        }

        private void UpdateChildGridPosition()
        {
            if (_expandedRowIndex < 0 || _expandedRowIndex + 1 >= dgPrescriptionDetails.Rows.Count) return;

            Rectangle rect = dgPrescriptionDetails.GetCellDisplayRectangle(0, _expandedRowIndex + 1, false);

            if (rect.Y == 0 && rect.Height == 0)
            {
                pnlDetails.Visible = false;
                return;
            }

            // rect.X + 45 تبدأ بعد زر التوسيع تماماً
            pnlDetails.Location = new Point(rect.X + 45, rect.Y+10);

            // 💡 هنا نتحكم في الحجم:
            // العرض: جعلناه يمتد بشكل متناسق مع الحواف
            // الارتفاع: قمنا بزيادته إلى 250 ليتحمل حجم الخط الكبير والخانات المفرودة
            pnlDetails.Size = new Size(dgPrescriptionDetails.ClientRectangle.Width - rect.X - 65, 250);

            pnlDetails.Visible = true;
            pnlDetails.BringToFront();
        }

        private void BindDetails(string prescriptionID)
        {
            DataView dv = new DataView(_dsPrescriptions.Tables["PrescriptionDetails"]);
            dv.RowFilter = $"PrescriptionID = '{prescriptionID}'";
            dgvDetailsPopup.DataSource = dv;
        }

        private void frmAllPrescriptions_Load(object sender, EventArgs e)
        {
            SetupGridDesign();
            SetupChildGridDesign();
            LoadHierarchicalPrescriptionGrid();
            this.dgPrescriptionDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            this.dgPrescriptionDetails.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDetailsPopup.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            // لون الخط الأفقي الفاصل بين الأسطر (درجة بترولية متناسقة مع الخلفية)
        }

        private void LoadHierarchicalPrescriptionGrid()
        {
            DataTable dtMaster = new DataTable("Prescriptions");
            dtMaster.Columns.Add("PrescriptionID", typeof(string));
            dtMaster.Columns.Add("Date", typeof(DateTime));
            dtMaster.Columns.Add("PatientName", typeof(string));
            dtMaster.Columns.Add("Status", typeof(string));

            DataTable dtDetails = new DataTable("PrescriptionDetails");
            dtDetails.Columns.Add("PrescriptionID", typeof(string));
            dtDetails.Columns.Add("MedicineName", typeof(string));
            dtDetails.Columns.Add("Dosage", typeof(string));
            dtDetails.Columns.Add("Quantity", typeof(int));

            dtMaster.Rows.Add("Rx-100", DateTime.Now, "أحمد علي خالد", "Pending");
            dtMaster.Rows.Add("Rx-101", DateTime.Now.AddHours(-2), "Ahmad Ali", "Dispensed");

            dtDetails.Rows.Add("Rx-100", "Panadol 500mg", "1 tab / day", 10);
            dtDetails.Rows.Add("Rx-100", "Amoxicillin 500mg", "3 times / day", 20);
            dtDetails.Rows.Add("Rx-101", "Cough Syrup", "as needed", 1);

            _dsPrescriptions = new DataSet();
            _dsPrescriptions.Tables.Add(dtMaster);
            _dsPrescriptions.Tables.Add(dtDetails);

            DataRelation relation = new DataRelation("MedicineDetailsRelation",
                _dsPrescriptions.Tables["Prescriptions"].Columns["PrescriptionID"],
                _dsPrescriptions.Tables["PrescriptionDetails"].Columns["PrescriptionID"]);
            _dsPrescriptions.Relations.Add(relation);

            // تفعيل تعبئة خلايا الجدول من الـ DataSet بشكل تلقائي وسلس
            dgPrescriptionDetails.Rows.Clear();
            foreach (DataRow masterRow in _dsPrescriptions.Tables["Prescriptions"].Rows)
            {
                dgPrescriptionDetails.Rows.Add("➕", masterRow["PrescriptionID"], Convert.ToDateTime(masterRow["Date"]).ToString("yyyy-MM-dd"), masterRow["PatientName"], masterRow["Status"]);
                int containerIndex = dgPrescriptionDetails.Rows.Count;
                dgPrescriptionDetails.Rows.Add("", "", "", "", "");

                dgPrescriptionDetails.Rows[containerIndex].Tag = "Container";
                dgPrescriptionDetails.Rows[containerIndex].Visible = false;
                dgPrescriptionDetails.Rows[containerIndex].Height = 130;

                // جعل خلفية سطر الحاوية تطابق لون الجدول تماماً حتى لا تصبح بيضاء وتحجب الـ Panel
                dgPrescriptionDetails.Rows[containerIndex].DefaultCellStyle.BackColor = Color.FromArgb(20, 43, 50);
                dgPrescriptionDetails.Rows[containerIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 43, 50);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}