using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class RemoveCourseForm : Form
    {
        COURSE course = new COURSE();
        MY_DB mydb = new MY_DB();

        public RemoveCourseForm()
        {
            InitializeComponent();
        }

        private void RemoveCourseForm_Load(object sender, EventArgs e)
        {
            loadSemester();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxIDCourseRemove.Text, out _) || TextBoxIDCourseRemove.Text == "" || !course.courseIDIsExist(Convert.ToInt32(TextBoxIDCourseRemove.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)) || ComboBoxSemester.SelectedIndex == -1)
            {
                ValidateInput();
                return;
            }
            int courseid = Convert.ToInt32(TextBoxIDCourseRemove.Text);

            if(!int.TryParse(TextBoxIDCourseRemove.Text, out _))
            {
                MessageBox.Show("Invalid Course ID. Please enter a valid numeric Course ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(course.courseIDIsExist(Convert.ToInt32(TextBoxIDCourseRemove.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)))
            {
                SqlCommand command = new SqlCommand("DELETE FROM course WHERE ID = @id AND Semester = @semester", mydb.getConnection());
                command.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(TextBoxIDCourseRemove.Text);
                command.Parameters.Add("@semester", SqlDbType.Int).Value = Convert.ToInt32(ComboBoxSemester.SelectedItem);

                mydb.openConnection();
                int rowsAffected = command.ExecuteNonQuery();
                mydb.closeConnection();
                loadSemester();
                MessageBox.Show("The Course Deleted", "Deleted Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }      
        }

        private void ValidateInput()
        {
            if (!int.TryParse(TextBoxIDCourseRemove.Text, out _) || TextBoxIDCourseRemove.Text == "")
            {
                MessageBox.Show("Invalid Course ID. Please enter a valid numeric Course ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(ComboBoxSemester.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Semester.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(!course.courseIDIsExist(Convert.ToInt32(TextBoxIDCourseRemove.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)))
            {
                MessageBox.Show("This Course ID does not exist in the database. Can not Remove and please choose a different one.", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadSemester()
        {
            ComboBoxSemester.Items.Add("1");
            ComboBoxSemester.Items.Add("2");
            ComboBoxSemester.Items.Add("3");
        }
    }
}
