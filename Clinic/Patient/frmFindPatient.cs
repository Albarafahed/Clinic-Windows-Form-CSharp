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

       

        private void frmFindPatient_Activated(object sender, EventArgs e)
        {
            ctrDoctorCardInfoWithFilter1.FocuseFilter();
        }

        private void ctrDoctorCardInfoWithFilter1_PatientCreated(object sender, global_classes.clsEventArgs e)
        {
            MessageBox.Show($"Patient ID {e.ID} PersonID {e.PersonID}");

        }
    }
}
