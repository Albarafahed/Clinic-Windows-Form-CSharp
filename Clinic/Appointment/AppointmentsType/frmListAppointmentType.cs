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

namespace Clinic.Appointment.AppointmentsType
{
    public partial class frmListAppointmentType : Form
    {
        private DataTable _dtAppointmetType = clsAppointmentType.GetAllAppointmentType();
        public frmListAppointmentType()
        {
            InitializeComponent();
        }
        private void frmListAppointmentType_Load(object sender, EventArgs e)
        {
            dgvAppointmentType.DataSource = _dtAppointmetType;
            lblRecordsCount.Text=_dtAppointmetType.Rows.Count.ToString();
            if (_dtAppointmetType.Rows.Count == 0)
                return;

            dgvAppointmentType.Columns["AppointmentTypeID"].HeaderText="Appointment ID";
            dgvAppointmentType.Columns["AppointmentTypeID"].Width = 120;

            dgvAppointmentType.Columns["TypeName"].HeaderText = "Appointment Type Name";
            dgvAppointmentType.Columns["TypeName"].Width = 200;

            dgvAppointmentType.Columns["DefaultFees"].HeaderText = "Default Fees";
            dgvAppointmentType.Columns["DefaultFees"].Width = 120;
        }

        private void _RefreshAppintmetsType()
        {
            _dtAppointmetType=clsAppointmentType.GetAllAppointmentType();
            dgvAppointmentType.DataSource= _dtAppointmetType;
            lblRecordsCount.Text=_dtAppointmetType.Rows.Count.ToString();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdddUpdateAppointmentType frm = 
             new frmAdddUpdateAppointmentType((int)dgvAppointmentType.CurrentRow.Cells["AppointmentTypeID"].Value);
            frm.ShowDialog();
            _RefreshAppintmetsType();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
