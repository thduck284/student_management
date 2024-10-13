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
    public partial class StaticAdminForm : Form
    {
        bool isGender;

        public StaticAdminForm()
        {
            InitializeComponent();
        }

        private void radioButtonGender_CheckedChanged(object sender, EventArgs e)
        {
            isGender = true;
        }

        private void radioButtonResult_CheckedChanged(object sender, EventArgs e)
        {
            isGender = false;
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            if(isGender)
            {
                StaticsForm staticsForm = new StaticsForm();
                staticsForm.ShowDialog();
            }
            else
            {
                StaticResultForm staticResultForm = new StaticResultForm();
                staticResultForm.ShowDialog();
            }
        }
    }
}
