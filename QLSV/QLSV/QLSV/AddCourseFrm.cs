using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;

namespace QLSV
{
    public partial class AddCourseFrm : Form
    {
        STUDENT student = new STUDENT();
        MY_DB mydb = new MY_DB();
        COURSE course = new COURSE();
        string s;
        bool x = false;

        public AddCourseFrm()
        {
            InitializeComponent();
        }

        private void AddCourseFrm_Load(object sender, EventArgs e)
        {
            loadSemester();
            ButtonSave.Enabled = false;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        { 
            if (!student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
            {
                MessageBox.Show("Invalid Student ID. The Student ID does not exist.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (ComboBoxSemester.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a semester.", "Select Semester", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ListBoxCourse.SelectedItem == null)
            {
                MessageBox.Show("Please select a course by clicking on the course name in the list.", "Select Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ListBoxSelectedCourse.Items.Add(ListBoxCourse.SelectedItem);
                int index;
                index = ListBoxCourse.SelectedIndex;
                ListBoxCourse.Items.RemoveAt(index);
            }
            ButtonSave.Enabled = true;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            List<object> itemsToRemove = new List<object>();
            foreach (var item in ListBoxSelectedCourse.Items)
            {
                insertCourse(item.ToString());
                ListBoxRegisted.Items.Add(item);
                itemsToRemove.Add(item);
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                ListBoxSelectedCourse.Items.Remove(itemToRemove);
            }

            /*this.Controls.Clear(); // Xóa tất cả các điều khiển trên form
            InitializeComponent(); // Khởi tạo lại tất cả các điều khiển và sự kiện của form
            AddCourseFrm_Load(this, EventArgs.Empty);*/
        }

        private void loadSemester()
        {
            ComboBoxSemester.Items.Add("1");
            ComboBoxSemester.Items.Add("2");
            ComboBoxSemester.Items.Add("3");
        }

        private void ComboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(x)
            {
                removeCourse();
                if (ComboBoxSemester.SelectedIndex == -1)
                {
                    return;
                }

                DataTable dt = new DataTable();
                dt = getAllCourses();
                foreach (DataRow item in dt.Rows)
                {
                    ListBoxCourse.Items.Add(item["Name"].ToString());
                }

                DataTable dt1 = new DataTable();
                dt1 = loadCourseRegistered();
                foreach (DataRow item in dt1.Rows)
                {
                    ListBoxRegisted.Items.Add(item["Name"].ToString());
                    ListBoxCourse.Items.Remove(item["Name"].ToString());
                }
            }
        }

        private DataTable loadCourseRegistered()
        {
            SqlCommand command = new SqlCommand("SELECT Name FROM coursestd WHERE ID = @id AND Semester = @semester", mydb.getConnection());
            command.Parameters.AddWithValue("@id", Convert.ToInt32(TextBoxID.Text));
            command.Parameters.AddWithValue("@semester", Convert.ToInt32(ComboBoxSemester.SelectedItem));
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        private DataTable getAllCourses()
        {
            SqlCommand command = new SqlCommand(@"
                                        SELECT c.Name
                                        FROM course c
                                        LEFT JOIN coursestd cs ON c.Name = cs.Name AND cs.Semester = c.Semester AND cs.ID = @ID
                                        WHERE cs.Name IS NULL AND c.Semester = @semester", mydb.getConnection());
            command.Parameters.AddWithValue("@ID", Convert.ToInt32(TextBoxID.Text));
            command.Parameters.AddWithValue("@semester", Convert.ToInt32(ComboBoxSemester.SelectedItem));
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        private bool insertCourse(string name)
        {
            using (SqlCommand command = new SqlCommand("insert into coursestd (ID, Name, Semester) VALUES (@id, @name, @semester)", mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@id", Convert.ToInt32(TextBoxID.Text));
                command.Parameters.AddWithValue("@semester", Convert.ToInt32(ComboBoxSemester.SelectedItem));
                command.Parameters.AddWithValue("@name", name);

                mydb.openConnection();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void removeCourse()
        {
            List<object> itemsToRemove = new List<object>();
            List<object> itemsToRemove1 = new List<object>();

            foreach (var item in ListBoxRegisted.Items)
            {
                itemsToRemove.Add(item);
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                ListBoxRegisted.Items.Remove(itemToRemove);
            }

            foreach (var item in ListBoxCourse.Items)
            {
                itemsToRemove1.Add(item);
            }

            foreach (var itemToRemove in itemsToRemove1)
            {
                ListBoxCourse.Items.Remove(itemToRemove);
            }
        }

        private void ComboBoxSemester_Click(object sender, EventArgs e)
        {
            x = false;

            if (TextBoxID.Text == "")
            {
                MessageBox.Show("Please enter id student before select semester");
                return;
            }

            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid Student ID. Please enter a valid numeric Student ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
            {
                MessageBox.Show("ID student is not exist. Please select another valid id student");
                return;
            }

            x = true;
        }


    }
}
