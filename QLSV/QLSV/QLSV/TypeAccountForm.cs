using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class TypeAccountForm : Form
    {
        string typeaccount;

        public TypeAccountForm()
        {
            InitializeComponent();
        }

        private void RadioButtonStudent_CheckedChanged(object sender, EventArgs e)
        {
            typeaccount = "student";
        }

        private void RadioButtonHumanResource_CheckedChanged(object sender, EventArgs e)
        {
            typeaccount = "humanresource";
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (typeaccount == "student")
            {
                StudentAccountForm studentAccountForm = new StudentAccountForm();
                studentAccountForm.ShowDialog();
            }
            else
            {               
                HRAccountForm addUserForm = new HRAccountForm();
                addUserForm.ShowDialog();               
            }
            this.Close();
        }
    }
}
