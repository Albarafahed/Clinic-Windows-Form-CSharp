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
    public partial class frmListDoctors : Form
    {
        public frmListDoctors()
        {
            InitializeComponent();
        }

        private void btnAddDoctor_Click(object sender, EventArgs e)
        {
            frmAddUpdateDoctor frm = new frmAddUpdateDoctor();
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            frmDoctorInfo frm = new frmDoctorInfo(DoctorID);
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            frmAddUpdateDoctor frm = new frmAddUpdateDoctor(DoctorID);
            frm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dgvDoctors.CurrentRow == null) return;

            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;

            if (MessageBox.Show("Are you sure you want to delete Doctor [" + DoctorID + "]?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                clsDoctor doctor = clsDoctor.Find(DoctorID);
                if (doctor == null)
                    return;
                if (doctor.DeleteDoctor())
                {
                    MessageBox.Show("Doctor Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //_DataBackToDelete(DoctorID);
                }
                else
                {
                    MessageBox.Show("Doctor was not deleted because it has data linked to it in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
