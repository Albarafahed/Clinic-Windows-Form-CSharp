using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Controls
{
    public partial class ctrDoctorCardInfoWithFilter : UserControl
    {
        public ctrDoctorCardInfoWithFilter()
        {
            InitializeComponent();
        }

        private bool _FilterEnabled = false;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        private bool _btnAddNewEnabled = false;
        public bool btnAddNewEnabled
        {
            get
            {
                return _btnAddNewEnabled;
            }
            set
            {
                _btnAddNewEnabled = value;
                btnAddNewDoctor.Enabled = _btnAddNewEnabled;
            }
        }

        public void FocusOnFilter()
        {
            txtFilterValue.Focus();
        }
    }
}
