using Clinic_Business;
using System;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Appointment
{
    public partial class frmCheckInDialog : Form
    {
        // متغيرات لتخزين بيانات الموعد القادمة من الشاشة الرئيسية
        private int _AppointmentID;
        private decimal _Fees;

        // خاصية عامة (Property) لجلب طريقة الدفع التي اختارها الموظف
        public string SelectedPaymentMethod => cbPaymentMethod.SelectedItem?.ToString();

        private decimal _Descount = 0;
        public decimal Descount =>_Descount;
        // المشيد (Constructor) المعدل لاستقبال البيانات
        public frmCheckInDialog(int appointmentID, decimal fees, string patientName)
        {
            InitializeComponent();
            _AppointmentID = appointmentID;
            _Fees = fees;

            // عرض البيانات في الواجهة
            lblPatientInfo.Text = $"Patient Name: {patientName}";
            lblFees.Text = $"Consultation Fees: {_Fees:C}"; // التنسيق كعملة
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // التحقق من أن الموظف اختار طريقة دفع
            if (cbPaymentMethod.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a payment method before confirming.",
                                "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None; // تمنع النافذة من الإغلاق
                return;
            }

            decimal.TryParse(txtDiscount.Text, out _Descount);

            // التحقق من الصلاحية: (استخدم ! لعكس النتيجة)
            if (!clsDiscount.ValidateDiscount(clsGlobal.CurrentUser.RoleID, clsDiscount.enTargetType.Medicine, _Descount))
            {
                MessageBox.Show("Discount exceeds your allowed limit or is invalid for this role.", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return; // يجب الخروج وعدم إكمال الحفظ
            }


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmCheckInDialog_Load(object sender, EventArgs e)
        {
            // إعدادات أولية عند تحميل النافذة
            cbPaymentMethod.SelectedIndex = 0; // اختيار "Cash" كافتراضي

            txtDiscount.Focus();
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح فقط بالأرقام، أزرار التحكم (BackSpace)، والفاصلة
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // منع تكرار الفاصلة العشرية
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            decimal value;
            // محاولة التحويل (F3 لثلاث خانات)
            if (decimal.TryParse(txtDiscount.Text, out value))
            {
                txtDiscount.Text = value.ToString("F2");
            }
            else
            {
                // إذا كان الحقل فارغاً أو خطأ، اجعله صفر
                txtDiscount.Text = "0.00";
            }
        }
    }
}