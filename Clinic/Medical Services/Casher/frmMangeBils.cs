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
    public partial class frmMangeBils : Form
    {
        public frmMangeBils()
        {
            InitializeComponent();
            lbCurrentUser.Text = clsGlobal.PersonName;

        }

        private void checkBox_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox=(CheckBox)sender;
            // إخفاء الرسم الافتراضي في هذه المنطقة
            e.Graphics.Clear(Color.FromArgb(20, 43, 50)); // لون خلفية الشاشة البترولية

            // 1. رسم نص الـ CheckBox باللون الأبيض
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString(checkBox.Text, checkBox.Font, textBrush, 20, 0);
            }

            // 2. تحديد أبعاد المربع الصغير
            Rectangle boxRect = new Rectangle(2, 4, 14, 14);

            // 3. رسم خلفية المربع باللون البترولي الداكن وحوافه
            using (Brush boxBrush = new SolidBrush(Color.FromArgb(28, 58, 68)))
            using (Pen boxPen = new Pen(Color.FromArgb(0, 210, 190), 1)) // حواف بترولية مضيئة
            {
                e.Graphics.FillRectangle(boxBrush, boxRect);
                e.Graphics.DrawRectangle(boxPen, boxRect);
            }

            // 4. إذا كان محدداً.. ارسم علامة الصح باللون الأبيض أو البترولي الفاتح
            if (checkBox.Checked)
            {
                using (Pen checkPen = new Pen(Color.White, 2))
                {
                    // رسم خطوط علامة الصح داخل المربع
                    e.Graphics.DrawLine(checkPen, 5, 11, 8, 14);
                    e.Graphics.DrawLine(checkPen, 8, 14, 13, 7);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
