
using Clinic.Doctor;
using Clinic.Patient;
using Clinic.Patient.Controls;
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
      
        //public frmMain(frmLogin frmLogin)
        //{
        //    InitializeComponent();
        //    _CreateStatsCards();
        //    _frmLogin = frmLogin;
        //}

        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_frmLogin.ShowDialog();
            this.Close();
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
    }
}
