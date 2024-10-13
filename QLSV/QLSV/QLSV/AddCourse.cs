using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;

namespace QLSV
{
    public partial class AddCourse : Form
    {

        COURSE course = new COURSE();
        bool x;

        public AddCourse()
        {
            InitializeComponent();
        }

        private void AddCourse_Load(object sender, EventArgs e)
        {
            //loadSemester();
        }

        private void ButtonAddCourse_Click(object sender, EventArgs e)
        {
            x = false;

            if(!CheckBoxHK1.Checked && !CheckBoxHK2.Checked  && !CheckBoxHK3.Checked)
            {
                MessageBox.Show("Please select semester", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(CheckBoxHK1.Checked)
            {
                AddCourseBySemester(1);
            }

            if (CheckBoxHK2.Checked)
            {
                AddCourseBySemester(2);
            }

            if (CheckBoxHK3.Checked)
            {
                AddCourseBySemester(3);
            }

            if(x)
            {
                MessageBox.Show("New Course Added", "Course Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
        }

        private void AddCourseBySemester(int semester)
        {
            if (!int.TryParse(TextBoxID.Text, out _) || course.courseIDIsExist(Convert.ToInt32(TextBoxID.Text), semester) || !int.TryParse(TextBoxPeriod.Text, out _) || InvalidCharacters(TextBoxName.Text) || !course.checkCourseName(TextBoxName.Text, semester))
            {
                ValidateInput(semester);
                return;
            }

            int id = Convert.ToInt32(TextBoxID.Text);
            string cname = TextBoxName.Text;
            int period = Convert.ToInt32(TextBoxPeriod.Text);
            string cdescription = TextBoxDescription.Text;

            if (period < 10)
            {
                MessageBox.Show("The period is too short", "Invalid Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                if (course.insertCourse(id, cname, period, semester, cdescription))
                {                   
                    x = true;
                }
                else
                {
                    MessageBox.Show("Error", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Add Course", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void ClearFields()
        {
            TextBoxID.Clear();
            TextBoxName.Clear();
            TextBoxPeriod.Clear();
            //ComboBoxSemester.Items.Clear();
            TextBoxDescription.Clear();
        }

        bool verif()
        {
            if ((TextBoxID.Text.Trim() == "")
                        || (TextBoxName.Text.Trim() == "")
                        || (TextBoxDescription.Text.Trim() == ""))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void ValidateInput(int semester)
        {
            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid Course ID. Please enter a valid numeric Course ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (course.courseIDIsExist(Convert.ToInt32(TextBoxID.Text), semester))
            {
                MessageBox.Show("This Course ID already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(!course.checkCourseName(TextBoxName.Text, semester))
            {
                MessageBox.Show("This Course Name already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!int.TryParse(TextBoxPeriod.Text, out _))
            {
                MessageBox.Show("Invalid Period. Please enter a valid numeric Period.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (InvalidCharacters(TextBoxName.Text))
            {
                MessageBox.Show("Invalid characters in Course Name. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool InvalidCharacters(string input)
        {
            string pattern = @"\d";

            // Kiểm tra xem chuỗi có chứa ký tự số không
            return Regex.IsMatch(input, pattern);
        }
    }
}
