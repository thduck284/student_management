using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class EditCourseForm : Form
    {
        COURSE course = new COURSE();
        MY_DB mydb = new MY_DB();

        public EditCourseForm()
        {
            InitializeComponent();
        }

        private void EditCourseForm_Load(object sender, EventArgs e)
        {
            loadSemester();
        }

        private void ButtonEditCourse_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxID.Text, out _)  || NumericUpDownPeriod.Value <= 0 || InvalidCharacters(TextBoxName.Text) || courseIDIsExist(Convert.ToInt32(TextBoxID.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)) == 0)
            {
                ValidateInput();
                return;
            }

            int id = Convert.ToInt32(TextBoxID.Text);
            string name = TextBoxName.Text;
            int period = (int)NumericUpDownPeriod.Value;
            int semester = Convert.ToInt32(ComboBoxSemester.SelectedItem);
            string description = TextBoxDescription.Text;

            if (period < 10)
            {
                MessageBox.Show("The period is too short", "Invalid Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                if (course.editCourse(id, name, period, semester, description))
                {
                    if(countCourse(name, id, semester) == 1)
                    {
                        MessageBox.Show("New Course Edited", "Course Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }else
                    {
                        MessageBox.Show("This Course Name already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClearFields();
                    }
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Edit Course", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            loadSemester();
        }

        public void ClearFields()
        {
            TextBoxID.Clear();
            TextBoxName.Clear();
            NumericUpDownPeriod.Value = 0;
            ComboBoxSemester.Items.Clear();
            TextBoxDescription.Clear();
        }

        bool verif()
        {
            if ((TextBoxID.Text.Trim() == "")
                        || (TextBoxName.Text.Trim() == "")
                        || (NumericUpDownPeriod.Value == 0)
                        || (ComboBoxSemester.SelectedIndex == -1)
                        || (TextBoxDescription.Text.Trim() == ""))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void ValidateInput()
        {
            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid Course ID. Please enter a valid numeric Course ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(courseIDIsExist(Convert.ToInt32(TextBoxID.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)) == 0)
            {
                MessageBox.Show("This Course does not exist in the database. Please choose a different one.", "Ivalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (InvalidCharacters(TextBoxName.Text))
            {
                MessageBox.Show("Invalid characters in Course Name. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool InvalidCharacters(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c) && c != ' ')
                {
                    return true;
                }
            }
            return false;
        }

        public int courseIDIsExist(int courseID, int semester)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM course WHERE ID = @id and Semester = @semester";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", courseID);
            cmd.Parameters.AddWithValue("@semester", semester);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count;
        }

        private void loadSemester()
        {
            ComboBoxSemester.Items.Add("1");
            ComboBoxSemester.Items.Add("2");
            ComboBoxSemester.Items.Add("3");
        }

        public int checkCourseName(string coursename, int semester)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM course WHERE Name = @name and Semester = @semester";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@name", coursename);
            cmd.Parameters.AddWithValue("@semester", semester);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count;
        }

        public int countCourse(string courseName, int courseID, int semester)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(Name) FROM course WHERE (ID = @id OR Name = @name) AND Semester = @semester";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", courseID);
            cmd.Parameters.AddWithValue("@name", courseName);
            cmd.Parameters.AddWithValue("@semester", semester);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count;
        }

    }
}
