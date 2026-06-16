using Clinic.Doctor;
using Clinic.Medical_Services.Appointment;
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

        private void btnVisitsDashboard_Click(object sender, EventArgs e)
        {
        }

        private void btnMRecords_Click(object sender, EventArgs e)
        {
          
        }

        private void btnFindPatient_Click(object sender, EventArgs e)
        {
           
        }

        private void btnDiagonsisNotes_Click(object sender, EventArgs e)
        {

        }

        private void btnListAppoinment_Click(object sender, EventArgs e)
        {
            frmListAppointmets frm = new frmListAppointmets();
            frm.Show();
        }
    }
}
