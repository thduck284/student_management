using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ReportingServices.Diagnostics.Internal;

namespace QLSV
{
    public partial class ManageStudentForm : Form
    {
        STUDENT student = new STUDENT();
        MY_DB mydb = new MY_DB();

        public ManageStudentForm()
        {
            InitializeComponent();
        }

        private void ManageStudentForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            LoadStudentDetails();
            string total = student.TotalStudentCount().ToString();
            LabelTotalStudent.Text = "Total Student: " + total;
            DataGridViewTextBoxColumn selectedCourseColumn = new DataGridViewTextBoxColumn();
            selectedCourseColumn.HeaderText = "Selected Course";
            selectedCourseColumn.Name = "SelectedCourseColumn";
            dataGridView1.Columns.Add(selectedCourseColumn);

            

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[i].Cells["ID"].Value);

                DataTable dataTable = new DataTable();

                mydb.openConnection();
                string query = "SELECT DISTINCT Name FROM coursestd WHERE ID = @id";
                using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    mydb.closeConnection();
                }

                string scourse = "";
                int x = 0;
                foreach (DataRow rowdt in dataTable.Rows)
                {
                    string s = "";
                    string z = rowdt["Name"].ToString();
                    for (int j = 0; j < z.Length; j++)
                    {
                        if(z[j] == ' ' && z[j + 1] == ' ')
                        {
                            break;
                        }
                        s += z[j];
                    }
                    scourse += (x == 0) ? s : ", " + s;
                    x++;
                }

                dataGridView1.Rows[i].Cells["SelectedCourseColumn"].Value = scourse;
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TextBoxID.Text);
            if (student.isStudentIdExists(id))
            {
                STUDENT student = new STUDENT();
                student.deleteStudent(Convert.ToInt32(TextBoxID.Text));
                MessageBox.Show("Success, deleted student", "Deleted Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string total = student.TotalStudentCount().ToString();
                LabelTotalStudent.Text = "Total Student: " + total;
            }
            else
            {
                MessageBox.Show("Error! Student with ID " + id.ToString() + " does not exist in the database.", "Student Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadStudentDetails();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            TextBoxID.Clear();
            TextBoxFname.Clear();
            TextBoxLname.Clear();
            TextBoxPhone.Clear();
            TextBoxAddress.Clear();
            PictureBoxStudentImage.Image = null;
            RadioButtonMale.Checked = false;
            RadioButtonFemale.Checked = false;
            DateTimePicker1.Value = DateTime.Now;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxID.Text, out _) || student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)) || InvalidCharacters(TextBoxFname.Text) || InvalidCharacters(TextBoxLname.Text) || !RadioButtonMale.Checked && !RadioButtonFemale.Checked || !long.TryParse(TextBoxPhone.Text, out _))
            {
                ValidateInput();
                return;
            }

            int id = Convert.ToInt32(TextBoxID.Text);
            string fname = TextBoxFname.Text;
            string lname = TextBoxLname.Text;
            DateTime bdate = DateTimePicker1.Value;
            string phone = TextBoxPhone.Text;
            string adrs = TextBoxAddress.Text;
            string gender = "Male";

            if (RadioButtonFemale.Checked)
            {
                gender = "Female";
            }

            MemoryStream pic = new MemoryStream();
            int born_year = DateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            //  sv tu 10-100,  co the thay doi
            if (((this_year - born_year) < 10) || ((this_year - born_year) > 100))
            {
                MessageBox.Show("The Student Age Must Be Between 10 and 100 year", "Invalid Birth Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);
                if (student.insertStudent(id, fname, lname, bdate, gender, phone, adrs, pic))
                {
                    MessageBox.Show("New Student Added", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudentDetails();
                    string total = student.TotalStudentCount().ToString();
                    LabelTotalStudent.Text = "Total Student: " + total;
                }
                else
                {
                    MessageBox.Show("Error", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //  chuc nang kiem tra du lieu input
        bool verif()
        {
            if ((TextBoxFname.Text.Trim() == "")
                        || (TextBoxLname.Text.Trim() == "")
                        || (TextBoxAddress.Text.Trim() == "")
                        || (TextBoxPhone.Text.Trim() == "")
                        || (PictureBoxStudentImage.Image == null))
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
                MessageBox.Show("Invalid ID. Please enter a valid numeric ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)))
            {
                MessageBox.Show("This ID already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (InvalidCharacters(TextBoxFname.Text) && InvalidCharacters(TextBoxLname.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME and LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (InvalidCharacters(TextBoxFname.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (InvalidCharacters(TextBoxLname.Text))
            {
                MessageBox.Show("Invalid characters in LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!RadioButtonMale.Checked && !RadioButtonFemale.Checked)
            {
                MessageBox.Show("Please select a gender.", "Gender not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!long.TryParse(TextBoxPhone.Text, out _))
            {
                MessageBox.Show("Invalid phone number. Please enter a valid numeric phone number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            string searchText = TextBoxSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                DataTable searchData = SearchStudent(searchText);
                if (searchData.Rows.Count > 0)
                {
                    dataGridView1.DataSource = searchData;
                }
                else
                {
                    MessageBox.Show("No matching students found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Display all students again if no results found
                    LoadStudentDetails();
                }
            }
            else
            {
                LoadStudentDetails(); // Display all students again if TextBoxSearch is empty
            }

            string total = student.TotalStudentCount().ToString();
            LabelTotalStudent.Text = "Total Student: " + total;
        }

        private DataTable SearchStudent(string searchText)
        {
            DataTable dataTable = new DataTable();
            MY_DB mydb = new MY_DB();

            using (SqlConnection connection = mydb.getConnection())
            {
                connection.Open();

                string query = "SELECT ID, FName, LName, Birthday, Gender, Phone, Address, Picture FROM std WHERE FName LIKE @searchText OR LName LIKE @searchText OR Address LIKE @searchText";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchText", "%" + searchText);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        private bool validateInput()
        {
            if (!int.TryParse(TextBoxID.Text, out _) || InvalidCharacters(TextBoxFname.Text) || InvalidCharacters(TextBoxLname.Text) || !RadioButtonMale.Checked && !RadioButtonFemale.Checked || !long.TryParse(TextBoxPhone.Text, out _))
            {
                return true;
            }
            return false;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (validateInput())
            {
                ValidateInput();
                return;
            }

            int id = Convert.ToInt32(TextBoxID.Text);
            string fname = TextBoxFname.Text;
            string lname = TextBoxLname.Text;
            DateTime bdate = DateTimePicker1.Value;
            string phone = TextBoxPhone.Text;
            string adrs = TextBoxAddress.Text;
            string gender = "Male";

            if (RadioButtonFemale.Checked)
            {
                gender = "Female";
            }

            MemoryStream pic = new MemoryStream();
            int born_year = DateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;
            //  sv tu 10-100,  co the thay doi
            if (((this_year - born_year) < 10) || ((this_year - born_year) > 100))
            {
                MessageBox.Show("The Student Age Must Be Between 10 and 100 year", "Invalid Birth Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);
                if (student.updateStudent(id, fname, lname, bdate, gender, phone, adrs, pic))
                {
                    MessageBox.Show("Student Edited", "Edited Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string total = student.TotalStudentCount().ToString();
                    LabelTotalStudent.Text = "Total Student: " + total;
                    LoadStudentDetails();
                }
                else
                {
                    MessageBox.Show("Error", "Edited Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Empty Fields", "Edited Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void ButtonDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog svf = new SaveFileDialog();
            svf.FileName = ("student_" + TextBoxID.Text);
            if((PictureBoxStudentImage.Image == null))
            {
                MessageBox.Show("No image in the picturebox");
            }else if(svf.ShowDialog() == DialogResult.OK)
            {
                //PictureBoxStudentImage.Image.Save((svf.FileName + ("." + ImageFormat.Jpeg.ToString())));
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            TextBoxID.Text = row.Cells["ID"].Value.ToString();
            TextBoxFname.Text = row.Cells["FName"].Value.ToString();
            TextBoxLname.Text = row.Cells["LName"].Value.ToString();
            DateTimePicker1.Value = Convert.ToDateTime(row.Cells["Birthday"].Value);
            string gender = row.Cells["Gender"].Value.ToString();
            if (gender == "Female")
            {
                RadioButtonFemale.Checked = true;
            }
            else
            {
                RadioButtonMale.Checked = true;
            }
            TextBoxPhone.Text = row.Cells["Phone"].Value.ToString();
            TextBoxAddress.Text = row.Cells["Address"].Value.ToString(); 
            byte[] image = (byte[])row.Cells["Picture"].Value;
            MemoryStream picture = new MemoryStream(image);
            PictureBoxStudentImage.Image = Image.FromStream(picture);

            
        }

        public void LoadStudentDetails()
        {
            MY_DB mydb = new MY_DB();
            DataTable studentData = GetStudentDataById(mydb);
            dataGridView1.DataSource = studentData;
            mydb.openConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM std");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            mydb.closeConnection();
            picol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            //dataGridView1.AllowUserToAddRows = false;
        }

        private DataTable GetStudentDataById(MY_DB mydb)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = mydb.getConnection())
            {
                connection.Open();

                string query = "SELECT ID, FName, LName, Birthday, Gender, Phone, Address, Picture FROM std";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        private void TextBoxID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                TextBoxFname.Focus();
                //e.SuppressKeyPress = true;
            }
        }

        private void TextBoxFname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                TextBoxLname.Focus();
                //e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                TextBoxID.Focus();
                //e.Handled = true;
            }
        }

        private void TextBoxPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                TextBoxAddress.Focus();
                //e.SuppressKeyPress = true;
            }
        }

        private void TextBoxLname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                TextBoxFname.Focus();
                //e.Handled = true;
            }
        }

        private void TextBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                TextBoxPhone.Focus();
                e.Handled = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow rowx = dataGridView1.Rows[e.RowIndex];
            if (e.ColumnIndex == dataGridView1.Columns["SelectedCourseColumn"].Index && e.RowIndex >= 0)
            {
                AddCourseFrm addCourseFrm = new AddCourseFrm();
                addCourseFrm.ShowDialog();
            }
        }
    }
}
