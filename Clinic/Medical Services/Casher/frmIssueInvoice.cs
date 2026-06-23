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
    public partial class frmIssueInvoice : Form
    {
        public frmIssueInvoice()
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
            txtSearchByPatientName.Focus();
        }

        private void txtSearchByPatientName_TextChanged(object sender, EventArgs e)
        {
            lbPlaceHoleder.Visible=string.IsNullOrEmpty(txtSearchByPatientName.Text);
        }
    }
}
