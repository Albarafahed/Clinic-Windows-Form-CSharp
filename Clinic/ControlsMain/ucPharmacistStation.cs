using Clinic.Doctor;
using Clinic.Medical_Services.Pharmaciy;
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
    public partial class ucPharmacistStation : UserControl
    {
        public ucPharmacistStation()
        {
            InitializeComponent();
        }

        private void btnVisitsDashboard_Click(object sender, EventArgs e)
        {
            frmListVisits frm = new frmListVisits();
            frm.Show();
        }

        private void btnMRecords_Click(object sender, EventArgs e)
        {
           
        }

        private void btnFindPatient_Click(object sender, EventArgs e)
        {
            frmPrescriptionDispnsing frm = new frmPrescriptionDispnsing();
            frm.Show();
        }

        private void btnDiagonsisNotes_Click(object sender, EventArgs e)
        {

        }
    }
}
