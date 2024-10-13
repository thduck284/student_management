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

namespace QLSV
{
    public partial class EditHumanResourceForm : Form
    {
        MY_DB mydb = new MY_DB();

        public EditHumanResourceForm(int ID, string fname, string lname, string us, string pass, Image image)
        {
            InitializeComponent();
            TextBoxID.Text = ID.ToString();
            TextBoxFName.Text = fname;
            TextBoxLName.Text = lname;
            TextBoxUserName.Text = us;
            TextBoxPassword.Text = pass;
            PictureBoxHRImage.Image = image;
            TextBoxID.Enabled = false;
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (!verif())
            {
                MessageBox.Show("Empty Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!checkInforContact())
            {
                return;
            }

            using (SqlCommand command = new SqlCommand("UPDATE hr SET FName = @fn, LName = @ln, UserName = @us, Password = @pas, Picture = @pic WHERE ID = @id", mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@id", TextBoxID.Text);
                command.Parameters.AddWithValue("@fn", TextBoxFName.Text);
                command.Parameters.AddWithValue("@ln", TextBoxLName.Text);
                command.Parameters.AddWithValue("@us", TextBoxUserName.Text);
                command.Parameters.AddWithValue("@pas", TextBoxPassword.Text);
                Image pic = PictureBoxHRImage.Image;
                byte[] imageBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    pic.Save(ms, pic.RawFormat);
                    imageBytes = ms.ToArray();
                }
                command.Parameters.AddWithValue("@pic", imageBytes);

                mydb.openConnection();

                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("My Infor was Edited", "Edit Infor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Edit Infor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                mydb.closeConnection();
            }
        }

        private bool checkInforContact()
        {
            if (InvalidCharacters(TextBoxFName.Text) && InvalidCharacters(TextBoxLName.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME and LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (InvalidCharacters(TextBoxFName.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (InvalidCharacters(TextBoxLName.Text))
            {
                MessageBox.Show("Invalid characters in LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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

        private bool ContainsSpecialCharacters(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        bool verif()
        {
            if ((TextBoxFName.Text.Trim() == "")
                        || (TextBoxLName.Text.Trim() == "")
                        || (PictureBoxHRImage.Image == null)
                        || (TextBoxID.Text == null)
                        || (TextBoxUserName.Text.Trim() == "")
                        || (TextBoxPassword.Text == ""))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxHRImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
