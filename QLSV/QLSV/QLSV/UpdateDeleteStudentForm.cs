using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class UpdateDeleteStudentForm : Form
    {
        STUDENT student = new STUDENT();

        public UpdateDeleteStudentForm()
        {
            InitializeComponent();
            TextBoxID.Focus();
        }

        string curID; 

        public void ButtonFind_Click(object sender, EventArgs e)
        {
            if(TextBoxID.Text == " ")
            {
                MessageBox.Show("Please enter id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }else if(!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid ID. Please enter a valid numeric ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = int.Parse(TextBoxID.Text);
            SqlCommand command = new SqlCommand("SELECT * FROM std WHERE ID = " + id);

            DataTable table = new DataTable();
            table = student.getStudents(command);

            if (table.Rows.Count > 0)
            {
                TextBoxFname.Text = table.Rows[0]["FName"].ToString();
                TextBoxLname.Text = table.Rows[0]["LName"].ToString();
                DateTimePicker1.Value = (DateTime)table.Rows[0]["Birthday"];

                if (table.Rows[0]["Gender"].ToString() == "Female")
                {
                    RadioButtonFemale.Checked = true;
                }
                else 
                {
                    RadioButtonMale.Checked = true;
                }

                TextBoxPhone.Text = table.Rows[0]["Phone"].ToString();
                TextBoxAddress.Text = table.Rows[0]["Address"].ToString();

                byte[] pic = (byte[])table.Rows[0]["Picture"];
                MemoryStream picture = new MemoryStream(pic);
                PictureBoxStudentImage.Image = Image.FromStream(picture);
                curID = TextBoxID.Text;
            }
            else
            {
                MessageBox.Show("not found", "Find student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }           
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
                    ClearFields();
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

        // button browse image
        private void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TextBoxID.Text);
            if(student.isStudentIdExists(id))
            {
                STUDENT student = new STUDENT();
                student.deleteStudent(Convert.ToInt32(TextBoxID.Text));
                MessageBox.Show("Success, deleted student", "Deleted Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            else
            {
                MessageBox.Show("Error! Student with ID " + id.ToString() + " does not exist in the database.", "Student Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonUploadImage_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }

        public void SetInfo(string id, string fname, string lname, DateTime bdate, bool gender, string phone, string address, byte[] picture)
        {
            TextBoxID.Text = id;
            TextBoxFname.Text = fname;
            TextBoxLname.Text = lname;
            DateTimePicker1.Value = bdate;
            curID = id;

            if (gender)
            {
                RadioButtonMale.Checked = true;
            }
            else
            {
                RadioButtonFemale.Checked = true;
            }

            TextBoxPhone.Text = phone;
            TextBoxAddress.Text = address;

            if (picture != null)
            {
                MemoryStream ms = new MemoryStream(picture);
                PictureBoxStudentImage.Image = Image.FromStream(ms);
            }         
        }

        private bool validateInput()
        {
            if (!int.TryParse(TextBoxID.Text, out _) || InvalidCharacters(TextBoxFname.Text) || InvalidCharacters(TextBoxLname.Text) || !RadioButtonMale.Checked && !RadioButtonFemale.Checked || !long.TryParse(TextBoxPhone.Text, out _) || curID != TextBoxID.Text)
            {
                return true;
            }
            return false;
        }

        private void ValidateInput()
        {
            if(curID != TextBoxID.Text)
            {
                MessageBox.Show("Modification of the ID is not allowed, only the information is allowed to be edited.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid ID. Please enter a valid numeric ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ClearFields()
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

        private void TextBoxLname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                TextBoxFname.Focus();
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

        private void TextBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                TextBoxPhone.Focus();
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddCourseFrm addCourseFrm = new AddCourseFrm();
            addCourseFrm.ShowDialog();
        }
    }
}
