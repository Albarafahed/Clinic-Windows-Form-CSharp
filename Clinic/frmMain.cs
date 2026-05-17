using Clinic.Login;
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
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmLogin.ShowDialog();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            timer1.Interval = 1000;
            lblDateTime.Text = DateTime.Now.ToString();
          
        }
    }
}
