using Clinic_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Doctor
{
    public partial class frmAddUpdateDoctor : Form
    {
        private int _DoctorID = -1; // -1 indicates a new doctor, otherwise it's an update
        private clsDoctor _Doctor;

        public frmAddUpdateDoctor()
        {
            InitializeComponent();
        }

        public frmAddUpdateDoctor(int DoctorID)
        {
            InitializeComponent();
            _DoctorID = DoctorID;
        }
    }
}
