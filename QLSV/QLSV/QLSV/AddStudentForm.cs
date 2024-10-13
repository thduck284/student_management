using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QLSV
{
    public partial class AddStudentForm : Form
    {
        STUDENT student = new STUDENT();

        public AddStudentForm()
        {
            InitializeComponent();
        }

        // button cancel ( close the form )
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // button insert a new student
        private void ButtonAddStudent_Click(object sender, EventArgs e)
        {
            if(!int.TryParse(TextBoxID.Text, out _) || student.isStudentIdExists(Convert.ToInt32(TextBoxID.Text)) || InvalidCharacters(TextBoxFname.Text) || InvalidCharacters(TextBoxLname.Text) || !RadioButtonMale.Checked && !RadioButtonFemale.Checked || !long.TryParse(TextBoxPhone.Text, out _))
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
            if ( ((this_year - born_year) < 10) || ((this_year - born_year) > 100) )
            {
                MessageBox.Show("The Student Age Must Be Between 10 and 100 year", "Invalid Birth Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);
                if (student.insertStudent(id, fname, lname, bdate, gender, phone, adrs, pic))
                {
                    MessageBox.Show("New Student Added", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Add Student",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ClearFields();
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
            else if(InvalidCharacters(TextBoxFname.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(InvalidCharacters(TextBoxLname.Text))
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

        private void TextBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                TextBoxPhone.Focus();
                e.Handled = true;
            }
        }

        private void TextBoxLname_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                TextBoxFname.Focus();
                //e.Handled = true;
            }
        }
    }
}
