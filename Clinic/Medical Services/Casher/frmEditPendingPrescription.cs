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
    public partial class frmEditPendingPrescription : Form
    {
        public frmEditPendingPrescription(int BillID)
        {
            InitializeComponent();
        }

        private void frmEditPendingBill_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
