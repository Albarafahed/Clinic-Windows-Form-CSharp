using Clinic.Doctor;
using Clinic.Medical_Services.Appointment;
using Clinic.Medical_Services.Casher;
using Clinic.Medical_Services.Visit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.ControlsMain
{
    public partial class ucCasherStation : UserControl
    {
        public ucCasherStation()
        {
            InitializeComponent();
        }
        private void btnListAppoinment_Click(object sender, EventArgs e)
        {
            frmListAppointmets frm = new frmListAppointmets();
            frm.Show();
        }

        private void btnMangeBills_Click(object sender, EventArgs e)
        {
            new frmMangeBils().ShowDialog();
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            new frmProcessPayments().ShowDialog();
        }

        private void btnIssueInvoice_Click(object sender, EventArgs e)
        {
            new frmIssueInvoice(-1).ShowDialog();
        }
    }
}
