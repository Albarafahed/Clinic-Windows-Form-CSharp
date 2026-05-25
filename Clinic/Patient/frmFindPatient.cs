using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Patient.Controls
{
    public partial class frmFindPatient : Form
    {
        public frmFindPatient()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrDoctorCardInfoWithFilter1_OnPatientCreated(object sender, ctrDoctorCardInfoWithFilter1.PatientEventArgs e)
        {
            MessageBox.Show($"Patient ID {e.PatientID} PersonID {e.PersonID}");
        }

        private void frmFindPatient_Activated(object sender, EventArgs e)
        {
            ctrDoctorCardInfoWithFilter1.FocuseFilter();
        }
    }
}
