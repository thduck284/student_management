﻿using System;
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
    public partial class StaticsForm : Form
    {
        Color panTotalColor;
        Color panMaleColor;
        Color panFemaleColor;

        public StaticsForm()
        {
            InitializeComponent();
        }

        private void StaticsForm_Load(object sender, EventArgs e)
        {
            panTotalColor = PanelTotal.BackColor;
            panMaleColor = PanelMale.BackColor;
            panFemaleColor = PanelFemale.BackColor;

            STUDENT student = new STUDENT();
            double total = student.TotalStudentCount();
            double totalMale = student.TotalMaleStudentCount();
            double totalFemale = student.TotalFemaleStudentCount();

            double maleStudentPercentage = (totalMale * (100 / total));
            double femaleStudentPercentage = (totalFemale * (100 / total));
            LabelTotal.Text = ("Total Students: " + total.ToString());
            LabelMale.Text = ("Male: " + (maleStudentPercentage.ToString("0.00") + "%"));
            LabelFemale.Text = ("Male: " + (femaleStudentPercentage.ToString("0.00")));
        }
        

        private void LabelTotal_MouseEnter(object sender, EventArgs e)
        {
            LabelTotal.ForeColor = panTotalColor;
            PanelTotal.BackColor = Color.White;
        }

        private void LabelTotal_MouseLeave(object sender, EventArgs e)
        {
            LabelTotal.ForeColor = Color.White;
            PanelTotal.BackColor = panTotalColor;
        }

        private void LabelMale_MouseEnter(object sender, EventArgs e)
        {
            LabelMale.ForeColor = panMaleColor;
            PanelMale.BackColor = Color.White;
        }

        private void LabelMale_MouseLeave(object sender, EventArgs e)
        {
            LabelMale.ForeColor = Color.White;
            PanelMale.BackColor = panMaleColor;
        }

        private void LabelFemale_MouseEnter(object sender, EventArgs e)
        {
            LabelFemale.ForeColor = panFemaleColor;
            PanelFemale.BackColor = Color.White;
        }

        private void LabelFemale_MouseLeave(object sender, EventArgs e)
        {
            LabelFemale.ForeColor = Color.White;
            PanelFemale.BackColor = panFemaleColor;
        }
    }
}
