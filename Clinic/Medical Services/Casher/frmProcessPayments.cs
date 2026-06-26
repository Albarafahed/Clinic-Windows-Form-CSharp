using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Clinic.Medical_Services.Casher
{
    public partial class frmProcessPayments : Form
    {
        public frmProcessPayments(int BillID)
        {
            InitializeComponent();
            lbCurrentUser.Text = clsGlobal.PersonName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbPlaceHolederBillID_Click(object sender, EventArgs e)
        {
            txtSerachByBillID.Focus();
        }


        private void radioButton_Paint(object sender, PaintEventArgs e)
        {
            // تحويل الـ sender تلقائياً ليعبر عن أي RadioButton يتم رسمه حالياً
         RadioButton radioButton = (RadioButton)sender;

            // 1. تنظيف الخلفية بلون الشاشة البترولية لإخفاء الرسم الافتراضي للويندوز
            e.Graphics.Clear(Color.FromArgb(20, 43, 50));

            // تفعيل تنعيم الحواف (Anti-aliasing) لتظهر الدوائر بشكل ناعم واحترافي بدون تكسر
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 2. رسم نص الـ RadioButton المخصص باللون الأبيض
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                // تم تغييرها إلى radioButton.Text و radioButton.Font لتصبح ديناميكية
                e.Graphics.DrawString(radioButton.Text, radioButton.Font, textBrush, 20, 0);
            }

            // 3. تحديد أبعاد الدائرة الخارجية (القطر 14 بكسل)
            Rectangle outerCircle = new Rectangle(2, 4, 14, 14);

            // 4. رسم خلفية الدائرة باللون البترولي الداكن وحوافها المضيئة
            using (Brush bgBrush = new SolidBrush(Color.FromArgb(28, 58, 68)))
            using (Pen borderPen = new Pen(Color.FromArgb(0, 210, 190), 1.5f)) // حافة بترولية مضيئة
            {
                e.Graphics.FillEllipse(bgBrush, outerCircle);  // تعبئة الدائرة
                e.Graphics.DrawEllipse(borderPen, outerCircle); // رسم الإطار الخارجي
            }

            // 5. إذا كان الخيار محدداً (Checked).. ارسم نقطة التحديد الداخلية (Dot)
            if (radioButton.Checked) // تم تعديلها هنا أيضاً إلى radioButton
            {
                // تحديد أبعاد الدائرة الداخلية لتكون في المنتصف تماماً
                Rectangle innerDot = new Rectangle(6, 8, 6, 6);

                // رسم النقطة باللون الأبيض الناصع
                using (Brush dotBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillEllipse(dotBrush, innerDot);
                }
            }
        }

        private void txtSerachByBillID_TextChanged(object sender, EventArgs e)
        {
            lbPlaceHolederBillID.Visible = string.IsNullOrEmpty(txtSerachByBillID.Text);
        }
    }
}
