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

namespace Clinic.Medical_Services.Appointment
{
    public partial class frmQueueDisplay : Form
    {
       private DataTable _dtAllDoctors=clsDoctor.GetAllDoctors();
        public frmQueueDisplay()
        {
            InitializeComponent();
        }
        private void _SetupDataGridViewColumns()
        {
            _dtAllDoctors.PrimaryKey = new DataColumn[] { _dtAllDoctors.Columns["DoctorID"] };

            dgvQueue.Columns["DoctorID"].HeaderText = "Doctor ID";
            dgvQueue.Columns["DoctorID"].Width = 100;
            dgvQueue.Columns["PersonID"].HeaderText = "Person ID";
            dgvQueue.Columns["PersonID"].Width = 100;
            dgvQueue.Columns["FullName"].HeaderText = "Full Name";
            dgvQueue.Columns["FullName"].Width = 200;
            dgvQueue.Columns["LicenseNumber"].HeaderText = "License #";
            dgvQueue.Columns["LicenseNumber"].Width = 150;
            dgvQueue.Columns["ConsultationFees"].HeaderText = "Fees";
            dgvQueue.Columns["ConsultationFees"].Width = 120;
            dgvQueue.Columns["Specializations"].HeaderText = "Specializations";
            dgvQueue.Columns["Specializations"].Width = 200;
            dgvQueue.Columns["WorkingDays"].HeaderText = "Working Days";
            dgvQueue.Columns["WorkingDays"].Width = 200;
            dgvQueue.Columns["CountryName"].HeaderText = "Country";
            dgvQueue.Columns["CountryName"].Width = 120;
            dgvQueue.Columns["IsActive"].HeaderText = "Is Active";
            dgvQueue.Columns["IsActive"].Width = 90;
        }

        private void _RefreshDoctorsList()
        {
          DataTable  _dtAllDoctors = clsDoctor.GetAllDoctors();

            dgvQueue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (!_dtAllDoctors.Columns.Contains("FeesText"))
            {
                _dtAllDoctors.Columns.Add("FeesText", typeof(string), "Convert(ConsultationFees, 'System.String')");
            }

            dgvQueue.DataSource = _dtAllDoctors;
            if (dgvQueue.Columns.Contains("FeesText"))
            {
                dgvQueue.Columns["FeesText"].Visible = false;
            }
         

            if (_dtAllDoctors.Rows.Count > 0)
            {
                _SetupDataGridViewColumns();
            }
        }
        private void dgvQueue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
