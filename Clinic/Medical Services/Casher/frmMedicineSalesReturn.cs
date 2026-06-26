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
    public partial class frmMedicineSalesReturn : Form
    {
        public frmMedicineSalesReturn(int BillID)
        {
            InitializeComponent();
            txtBillIDSearch.Enter += txtBillIDSearch_Enter;
            txtBillIDSearch.Leave += txtBillIDSearch_Leave;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void txtBillIDSearch_Enter(object sender, EventArgs e)
        {
            // عند الكتابة: توهج سيان مشع مطابق للتصميم
            pnlSearchGlow.BackColor = Color.Cyan;
        }

        private void txtBillIDSearch_Leave(object sender, EventArgs e)
        {
            // عند الخروج: العودة للون الهادئ المتناسق مع الخلفية
            pnlSearchGlow.BackColor = Color.FromArgb(23, 73, 74);
        }

      
    }
}
