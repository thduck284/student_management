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

namespace QLSV
{
    public partial class ManageCouresesForm : Form
    {
        COURSE course = new COURSE();
        MY_DB mydb = new MY_DB();
        int pos;
        bool x = true;

        public ManageCouresesForm()
        {
            InitializeComponent();
        }

        private void ManageCouresesForm_Load(object sender, EventArgs e)
        {           
            loadSemester();
            reloadListBoxData();
        }

        void reloadListBoxData()
        {
            ListBoxCourses.Items.Clear();
            DataTable dt = new DataTable();
            dt = course.getAllCourses();
            foreach(DataRow item in dt.Rows)
            {
                ListBoxCourses.Items.Add(item["Name"].ToString());
            }

            LabelTotalCourses.Text = ("Total Courses: " + course.totalCourses());
        }

        void showData(int index)
        {
            DataTable dt = course.getDataCourses();

            if (dt.Rows.Count > 0 && index >= 0 && index < dt.Rows.Count)
            {
                DataRow dr = dt.Rows[index];
                TextBoxID.Text = dr["ID"].ToString(); // Thay vì sử dụng index của ItemArray, bạn có thể truy cập trực tiếp theo tên cột.
                TextBoxName.Text = dr["Name"].ToString();
                NumericUpDownPeriod.Value = Convert.ToInt32(dr["Period"]); // Sử dụng Convert.ToInt32() thay vì int.Parse() để tránh lỗi nếu giá trị không phải số nguyên.
                ComboBoxSemester.SelectedItem = dr["Semester"].ToString();
                TextBoxDescription.Text = dr["Description"].ToString();
            }
        }

        private void ListBoxCourses_Click(object sender, EventArgs e)
        {
            int index = ListBoxCourses.SelectedIndex;
            pos = index;
            showData(index);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxID.Text, out _) || course.courseIDIsExist(Convert.ToInt32(TextBoxID.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)) || InvalidCharacters(TextBoxName.Text) || !course.checkCourseName(TextBoxName.Text, Convert.ToInt32(ComboBoxSemester.SelectedItem)))
            {
                ValidateInput1();
                return;
            }

            int id = Convert.ToInt32(TextBoxID.Text);
            string cname = TextBoxName.Text;
            int period = (int)NumericUpDownPeriod.Value;
            int semester = Convert.ToInt32(ComboBoxSemester.SelectedItem);
            string cdescription = TextBoxDescription.Text;

            if (period < 10)
            {
                MessageBox.Show("The period is too short", "Invalid Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                if (course.insertCourse(id, cname, period, semester, cdescription))
                {
                    MessageBox.Show("New Course Added", "Course Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    reloadListBoxData();
                    loadSemester();
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

            pos = 0;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxID.Text, out _) || NumericUpDownPeriod.Value <= 0 || InvalidCharacters(TextBoxName.Text) || courseIDIsExist(Convert.ToInt32(TextBoxID.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)) == 0)
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
                    if (countCourse(name, id, semester) == 1)
                    {
                        MessageBox.Show("New Course Edited", "Course Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                        reloadListBoxData();
                        loadSemester();
                    }
                    else
                    {
                        MessageBox.Show("This Course Name already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Edit Course", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            pos = 0;
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int courseID = Convert.ToInt32(TextBoxID.Text);

                if (MessageBox.Show("Are you sure you want to delete this course", "Remove Course", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (course.deleteCourse(courseID))
                    {
                        TextBoxID.Text = "";
                        TextBoxName.Text = "";
                        NumericUpDownPeriod.Value = 10;
                        TextBoxDescription.Text = "";

                        reloadListBoxData();
                    }
                    else
                    {
                        MessageBox.Show("Course Not Deleted", "Remove Course", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Enter A Valid Numeric ID", "Remove Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            pos = 0;
        }

        private void ButtonFirst_Click(object sender, EventArgs e)
        {
            pos = 0;
            showData(pos);
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if(pos < (course.getAllCourses().Rows.Count - 1))
            {
                pos = pos + 1;
                showData(pos);
            }
        }

        private void ButtonPrevious_Click(object sender, EventArgs e)
        {
            if(pos > 0)
            {
                pos = pos - 1;
                showData(pos);
            }
        }

        private void ButtonLast_Click(object sender, EventArgs e)
        {
            pos = course.getAllCourses().Rows.Count - 1;
            showData(pos);
        }

        private void ValidateInput1()
        {
            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid Course ID. Please enter a valid numeric Course ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (course.courseIDIsExist(Convert.ToInt32(TextBoxID.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)))
            {
                MessageBox.Show("This Course ID already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!course.checkCourseName(TextBoxName.Text, Convert.ToInt32(ComboBoxSemester.SelectedItem)))
            {
                MessageBox.Show("This Course Name already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (InvalidCharacters(TextBoxName.Text))
            {
                MessageBox.Show("Invalid characters in Course Name. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateInput()
        {
            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid Course ID. Please enter a valid numeric Course ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (courseIDIsExist(Convert.ToInt32(TextBoxID.Text), Convert.ToInt32(ComboBoxSemester.SelectedItem)) == 0)
            {
                MessageBox.Show("This Course does not exist in the database. Please choose a different one.", "Ivalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (InvalidCharacters(TextBoxName.Text))
            {
                MessageBox.Show("Invalid characters in Course Name. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void loadSemester()
        {
            ComboBoxSemester.Items.Add("1");
            ComboBoxSemester.Items.Add("2");
            ComboBoxSemester.Items.Add("3");
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

        public void ClearFields()
        {
            TextBoxID.Clear();
            TextBoxName.Clear();
            NumericUpDownPeriod.Value = 0;
            ComboBoxSemester.Items.Clear();
            TextBoxDescription.Clear();
        }

        private void ListBoxCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!x)
            {
                CourseStudentListForm courseStudentListForm = new CourseStudentListForm();
                courseStudentListForm.ShowDialog();
            }
            x = false;
        }
    }
}
