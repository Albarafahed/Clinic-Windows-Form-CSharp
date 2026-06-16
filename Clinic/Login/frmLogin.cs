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

namespace Clinic.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                ShowError("Please fill all required fields.", "Validation Error");
                return;
            }

            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            clsUser user = clsUser.FindByUsernameAndPassword(username, password);

            // 1. التحقق من وجود المستخدم (Guard Clause)
            if (user == null)
            {
                ShowError("Invalid Username/Password.", "Wrong Credentials");
                return;
            }

            // 2. إدارة التذكر (Remember Me)
            clsGlobal.RememberUsernameAndPassword(chkRememberMe.Checked ? username : "", chkRememberMe.Checked ? password : "");

            // 3. التحقق من حالة التنشيط
            if (!user.IsActive)
            {
                ShowError("Your account is not Active, Contact Admin.", "In Active Account");
                return;
            }

            // 4. الدخول الناجح
            clsGlobal.CurrentUser = user;
            this.Hide();
            using (frm frm = new frm())
            {
                frm.ShowDialog();
                this.Show(); // إظهار شاشة الدخول مجدداً عند إغلاق الشاشة الرئيسية
            }
        }

        // دالة مساعدة لتقليل التكرار
        private void ShowError(string message, string title)
        {
            txtUserName.Focus();
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            
           string Username = string.Empty;
            string Password = string.Empty;
            if(clsGlobal.GetStoredCredential(ref Username, ref Password))
            {
                txtUserName.Text = Username;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
            {
                chkRememberMe.Checked = false;
                txtPassword.Text= "";
                txtUserName.Text = "";
                txtUserName.Focus();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBoxValiditin(object sender, CancelEventArgs e)
        {
            e.Cancel = string.IsNullOrWhiteSpace((sender as TextBox).Text);
            if(e.Cancel)
            {
                errorProvider1.SetError((sender as TextBox), "This field is required.");
            }
            else
            {
                errorProvider1.SetError((sender as TextBox), "");
            }
        }
    }
}
