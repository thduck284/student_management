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

namespace QLSV
{
    public partial class ManageScoreForm : Form
    {
        SCORE Score = new SCORE();
        STUDENT student = new STUDENT();
        MY_DB mydb = new MY_DB();
        bool x,y = true;

        public ManageScoreForm()
        {
            InitializeComponent();
        }

        private void ManageScoreForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getInfor_Score();
            loadSemester();
        }

        private void ButtonAddScore_Click(object sender, EventArgs e)
        {
            if (!verif())
            {
                MessageBox.Show("Empty Fields", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            if (!int.TryParse(TextBoxID.Text, out _) || !double.TryParse(TextBoxScore.Text, out _) || !student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
            {
                ValidateInput();
                return;
            }           

            int id = Convert.ToInt32(TextBoxID.Text);
            int semester = Convert.ToInt32(ComboBoxSemester.SelectedItem);
            string name = ComboBoxCourse.SelectedItem.ToString();
            float score = Convert.ToSingle(TextBoxScore.Text);
            string description = TextBoxDescription.Text;

            if (score < 0 || score > 10)
            {
                MessageBox.Show("The Score must be between 0 and 10", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                if (Score.insertScore(id, semester, name, score, description))
                {
                    MessageBox.Show("Score Added", "Course Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    loadSemester();
                    dataGridView1.DataSource = getInfor_Score();
                }
                else
                {
                    MessageBox.Show("Error", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if(!verif())
            {
                MessageBox.Show("Empty Fields", "Remove Score", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!int.TryParse(TextBoxID.Text, out _) || !double.TryParse(TextBoxScore.Text, out _) || !student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
            {
                ValidateInput();
                return;
            }

            if(!checkColumnScore())
            {
                MessageBox.Show("Column Score does not exist in database. Please select another column", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            int id = Convert.ToInt32(TextBoxID.Text);
            int semester = Convert.ToInt32(ComboBoxSemester.SelectedItem);
            string name = ComboBoxCourse.SelectedItem.ToString();
            float score = Convert.ToSingle(TextBoxScore.Text);
            string description = TextBoxDescription.Text;

            SqlCommand command = new SqlCommand("DELETE FROM score WHERE ID = @id AND Semester = @semester AND Name = @name", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@semester", SqlDbType.Int).Value = semester;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;

            mydb.openConnection();
            int rowsAffected = command.ExecuteNonQuery();
            dataGridView1.DataSource = getInfor_Score();
            mydb.closeConnection();          
        }

        private void ButtonAverage_Click(object sender, EventArgs e)
        {
            /*if ((TextBoxID.Text.Trim() == "")
                        || (ComboBoxCourse.SelectedIndex == -1)
                        || (ComboBoxSemester.SelectedIndex == -1))
            {
                MessageBox.Show("Empty Fields", "Remove Score", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int id = Convert.ToInt32(TextBoxID.Text);
            int semester = Convert.ToInt32(ComboBoxSemester.SelectedItem);
            string name = ComboBoxCourse.SelectedItem.ToString();

            // Lấy DataTable từ phương thức getAvgScoreByCourse
            DataTable dt = Score.getAvgScoreByCourse(id, semester, name);

            // Kiểm tra xem DataTable có dòng nào không
            if (dt.Rows.Count > 0)
            {
                // Lấy dữ liệu từ dòng đầu tiên (giả sử chỉ có một dòng kết quả)
                DataRow row = dt.Rows[0];

                // Hiển thị dữ liệu trong các điều khiển
                TextBoxID.Text = row["ID"].ToString();
                ComboBoxSemester.SelectedItem = row["Semester"].ToString();
                ComboBoxCourse.SelectedItem = row["Name"].ToString();
                TextBoxScore.Text = row["AvgScore"].ToString();
                if(Convert.ToSingle(TextBoxScore.Text) < 5)
                {
                    TextBoxDescription.Text = "FAIL";
                }
                else
                {
                    TextBoxDescription.Text = "PASS";
                }
            }
            else
            {
                // Nếu không có dữ liệu, bạn có thể xử lý một cách thích hợp ở đây, ví dụ: hiển thị thông báo cho người dùng
                MessageBox.Show("No data found for ID: " + id + ", Semester: " + semester + ", and Course: " + name);
            }*/
            AvgScoreByCourse avgScoreByCourse = new AvgScoreByCourse();
            avgScoreByCourse.ShowDialog();
        }

        private void ButtonShowStudent_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT ID, FName, LName, Birthday FROM std", mydb.getConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            y = false;
        }

        private void ButtonShowScore_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getInfor_Score();
            y = true;
        }

        private void ValidateInput()
        {
            if (!int.TryParse(TextBoxID.Text, out _) || !int.TryParse(TextBoxScore.Text, out _))
            {
                MessageBox.Show("Invalid Student ID. Please enter a valid numeric Student ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
            {
                MessageBox.Show("Invalid Student ID. Please enter a valid numeric Student ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        bool verif()
        {
            if ((TextBoxID.Text.Trim() == "")
                        || (TextBoxScore.Text.Trim() == "")
                        || (ComboBoxCourse.SelectedIndex == -1)
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

        private void loadSemester()
        {
            ComboBoxSemester.Items.Add("1");
            ComboBoxSemester.Items.Add("2");
            ComboBoxSemester.Items.Add("3");
        }

        public void ClearFields()
        {
            TextBoxID.Clear();
            TextBoxScore.Clear();
            ComboBoxCourse.Items.Remove(ComboBoxCourse.SelectedItem);
            ComboBoxCourse.Items.Clear();
            ComboBoxSemester.Items.Remove(ComboBoxSemester.SelectedItem);
            ComboBoxSemester.Items.Clear();
            TextBoxDescription.Clear();
        }

        private void ComboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(x)
            {
                ComboBoxCourse.Items.Clear();
                if (ComboBoxSemester.SelectedIndex != -1)
                {
                    selectCourse();
                }
            }            
        }

        private void selectCourse()
        {
            DataTable dt = new DataTable();
            dt = getAllCourses();
            foreach (DataRow row in dt.Rows)
            {
                ComboBoxCourse.Items.Add(row["Name"].ToString());
            }
        }

        private DataTable getAllCourses()
        {
            SqlCommand command = new SqlCommand("SELECT Name FROM coursestd WHERE Semester = @semester AND ID = @id", mydb.getConnection());
            command.Parameters.AddWithValue("@id", Convert.ToInt32(TextBoxID.Text));
            command.Parameters.AddWithValue("@semester", Convert.ToInt32(ComboBoxSemester.SelectedItem));
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        private DataTable getInfor_Score()
        {
            SqlCommand command = new SqlCommand("SELECT std.ID as ID_Student, std.FName, std.LName, course.ID as Course_ID, score.Semester, score.Name as Name_Subject, score.Score, score.Description as Result FROM score LEFT JOIN std ON score.ID = std.ID LEFT JOIN course ON score.Name = course.Name AND score.Semester = course.Semester", mydb.getConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(y)
            {
                ClearFields();
                loadSemester();
                ClearFields();
                loadSemester();
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count - 1)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    TextBoxID.Text = row.Cells["ID_Student"].Value.ToString();
                    ComboBoxSemester.SelectedItem = row.Cells["Semester"].Value.ToString();
                    selectCourse();
                    ComboBoxCourse.SelectedItem = row.Cells["Name_Subject"].Value.ToString();
                    TextBoxScore.Text = row.Cells["Score"].Value.ToString();
                    TextBoxDescription.Text = row.Cells["Result"].Value.ToString();
                }
            }
        }

        private void ComboBoxCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBoxCourse.Items.Clear();
            //selectCourse();
        }

        private bool checkColumnScore()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM course WHERE ID = @id OR Name = @name AND Semester = @semester";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", TextBoxID.Text);
            cmd.Parameters.AddWithValue("@name", ComboBoxCourse.SelectedItem);
            cmd.Parameters.AddWithValue("@semester", ComboBoxSemester.SelectedItem);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
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
            x = true;
        }

        
    }
}
