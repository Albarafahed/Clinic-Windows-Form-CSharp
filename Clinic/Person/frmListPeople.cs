using Clinic.Person;
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

namespace Clinic
{
    public partial class frmListPeople : Form
    {
        private DataTable _dtAllPeople=clsPerson.GetAllPersons();

        
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            if (_dtAllPeople != null)
            {

                dgvPeople.DataSource = _dtAllPeople;
                lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
                cbFilterBy.SelectedIndex = 0;
                if (dgvPeople.Rows.Count > 0)
                {
                    dgvPeople.Columns[0].HeaderText = "PersonID";
                    dgvPeople.Columns[0].Width = 110;

                    dgvPeople.Columns[1].HeaderText = "FullName";
                    dgvPeople.Columns[1].Width = 300;

                    dgvPeople.Columns[2].HeaderText = "DateOfBirth";
                    dgvPeople.Columns[2].Width = 150;

                    dgvPeople.Columns[3].HeaderText = "Gender";
                    dgvPeople.Columns[3].Width = 120;

                    dgvPeople.Columns[4].HeaderText = "Phone";
                    dgvPeople.Columns[4].Width = 170;

                    dgvPeople.Columns[5].HeaderText = "Country";
                    dgvPeople.Columns[5].Width = 130;

                    dgvPeople.Columns[6].HeaderText = "Address";
                    dgvPeople.Columns[6].Width = 170;

                    dgvPeople.Columns[7].HeaderText = "Email";
                    dgvPeople.Columns[7].Width = 170;


                }
            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool None = cbFilterBy.Text == "None";
            bool Gender = cbFilterBy.Text == "Gender";

            txtFilterValue.Visible = !None && !Gender;
            cbFilterGender.Visible = Gender && !None;

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();

            }
            else if (cbFilterGender.Visible)
            {
                cbFilterGender.SelectedIndex = 0;
                cbFilterBy.Focus();
               
            }
            else
            {
                cbFilterBy.Focus();
                txtFilterValue.Text = "";
               
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text== "Person ID")
                e.Handled= !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if(_dtAllPeople==null) return;
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "Country":
                    FilterColumn = "Country";
                    break;
                case "Address":
                    FilterColumn = "Address";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                default:
                    FilterColumn= "None";
                    break;
            }
            if (txtFilterValue.Text == "" || FilterColumn == "None")
            {
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
                return;
            }

            if(FilterColumn == "PersonID")
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn}={txtFilterValue.Text}";
            else
                _dtAllPeople.DefaultView.RowFilter = $"{FilterColumn} LIKE '%{txtFilterValue.Text}%'";
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
        }

        private void cbFilterGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtAllPeople == null)
                return;
            if(cbFilterGender.Text == "All")
                _dtAllPeople.DefaultView.RowFilter = string.Empty;
            else
                _dtAllPeople.DefaultView.RowFilter = $"Gender = '{cbFilterGender.Text}'";
               
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm=new frmAddUpdatePerson();
            frm.DataBack += _DatatBack;
            frm.ShowDialog();
            frmListPeople_Load(null, null);
            
        }

        private void _DatatBack(object sender, int PersonID)
        {
            _dtAllPeople = clsPerson.GetAllPersons();
            dgvPeople.DataSource = _dtAllPeople;
            lblRecordsCount.Text = _dtAllPeople.Rows.Count.ToString();

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.DataBack += _DatatBack;
            frm.ShowDialog();
        }
    }
}
