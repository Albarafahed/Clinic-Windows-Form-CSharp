using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Medical_Services.Pharmaciy
{
    public partial class frmManageDrugs : Form
    {
        public frmManageDrugs()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgMedicineDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbBlaceholder_Click(object sender, EventArgs e)
            => txtSearch.Focus();

        private void txtSearch_TextChanged(object sender, EventArgs e)
            => lbBlaceholder.Visible = string.IsNullOrEmpty(txtSearch.Text);
       
    }
}
