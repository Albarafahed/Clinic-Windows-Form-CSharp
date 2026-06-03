
using Clinic.Appointment.AppointmentsType;
using Clinic.Doctor;
using Clinic.Login;
using Clinic.Medical_Services.Appointment;
using Clinic.Patient;
using Clinic.Serveces.ServecesType;
using Clinic.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic
{
    public partial class frmMain : Form
    {

        private frmLogin _frmLogin;
        public frmMain(frmLogin frmLogin)
        {
            InitializeComponent();
            _CreateStatsCards();
            _frmLogin = frmLogin;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblUser.Text=$"User : {clsGlobal.CurrentUser.RoleInfo.RoleName}";
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            this.Close();
            _frmLogin.Show();
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            timer1.Interval = 1000;
            lblDateTime.Text = DateTime.Now.ToString();
          
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPeople frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void UsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmlistUser frm=new frmlistUser();
            frm.ShowDialog();
        }

        private void PatientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPatient frm=new frmListPatient();
            frm.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void Password32ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void btnSearchPat_Click(object sender, EventArgs e)
        {
            frmFindDoctor frm=new frmFindDoctor();
            frm.ShowDialog();
        }

        private void btnFindDoctor_Click(object sender, EventArgs e)
        {
            frmFindDoctor frm=new frmFindDoctor();
            frm.ShowDialog();
        }

        private void btnFindPatient_Click(object sender, EventArgs e)
        {
            frmFindPatient frm = new frmFindPatient();
            frm.ShowDialog();
        }

        private void DoctorsStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListDoctors frm=new frmListDoctors();
            frm.ShowDialog();
        }

        private void mangeAppointmentTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListAppointmentType frm = new frmListAppointmentType();
            frm.ShowDialog();
        }

        private void manageServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListServicesType frm = new frmListServicesType();
            frm.ShowDialog();
        }

        private void appointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListAppointmets frm = new frmListAppointmets();
            frm.ShowDialog();
        }

        private void btnAddApp_Click(object sender, EventArgs e)
        {
            frmAddUpdateAppointment frm = new frmAddUpdateAppointment();
            frm.ShowDialog();
        }
    }
}
