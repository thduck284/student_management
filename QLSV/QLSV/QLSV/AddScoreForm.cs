using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class AddScoreForm : Form
    {
        SCORE Score = new SCORE();
        COURSE course = new COURSE();
        MY_DB mydb = new MY_DB();
        STUDENT student = new STUDENT();

        public AddScoreForm()
        {
            InitializeComponent();
        }

        private void AddScoreForm_Load(object sender, EventArgs e)
        {
            loadDataStudent();
            loadSemester();
        }

        private void loadDataStudent()
        {
            SqlCommand command = new SqlCommand("SELECT ID, FName, LName FROM std");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            dataGridView1.AllowUserToAddRows = false;
        }

        private DataTable getAllCourses()
        {
            SqlCommand command = new SqlCommand("SELECT name FROM coursestd WHERE Semester = @semester AND ID = @id", mydb.getConnection());
            command.Parameters.AddWithValue("@id", Convert.ToInt32(TextBoxID.Text));
            command.Parameters.AddWithValue("@semester", Convert.ToInt32(ComboBoxSemester.SelectedItem));
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }


        private void selectCourse()
        {
            DataTable dt = new DataTable();
            dt = getAllCourses();
            foreach (DataRow row in dt.Rows)
            {
                ComboBoxCourse.Items.Add(row["name"].ToString());
            }
        }

        private void ButtonAddScore_Click(object sender, EventArgs e)
        {
            if (!float.TryParse(TextBoxID.Text, out _) || !double.TryParse(TextBoxScore.Text, out _) || !student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
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
            else if (verif())
            {
                if (Score.insertScore(id, semester, name, score, description))
                {
                    MessageBox.Show("Score Added", "Score Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    loadSemester();
                }
                else
                {
                    MessageBox.Show("Error", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Add Score", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearFields();
            ComboBoxSemester.Items.Clear();
            loadSemester();
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            TextBoxID.Text = id.ToString();
        }

        private void loadSemester()
        {
            ComboBoxSemester.Items.Add("1"); 
            ComboBoxSemester.Items.Add("2");
            ComboBoxSemester.Items.Add("3");
        }

        private void ComboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxCourse.Items.Clear();
            selectCourse();
        }

        private void ValidateInput()
        {
            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid Student ID. Please enter a valid numeric Student ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(!int.TryParse(TextBoxScore.Text, out _))
            {
                MessageBox.Show("Invalid Score. Please enter a valid numeric Score.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(!student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
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

        public DataTable getSemester()
        {
            SqlCommand command = new SqlCommand("SELECT DISTINT Semester FROM score WHERE ID = @id", mydb.getConnection());
            command.Parameters.AddWithValue("@id", Convert.ToInt32(TextBoxID.Text));
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
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
    }
}
