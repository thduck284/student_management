using System;
using System.Collections;
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
    public partial class EditHRForm : Form
    {
        MY_DB mydb = new MY_DB();
        int select;

        public EditHRForm()
        {
            InitializeComponent();
        }

        private void EditHRForm_Load(object sender, EventArgs e)
        {
            labelUS.Visible = false;
            TextBoxUserName.Visible = false;
            buttonSelect.Visible = false;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if(!verif())
            {
                MessageBox.Show("Empty Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!checkInput())
            {
                return;
            }

            try
            {
                MemoryStream pic = new MemoryStream();
                PictureBoxUserImage.Image.Save(pic, PictureBoxUserImage.Image.RawFormat);
                using (SqlCommand command = new SqlCommand("UPDATE hr SET fname = @fname, lname = @lname, username = @username, password = @password, picture = @picture WHERE id = @id", mydb.getConnection()))
                {
                    command.Parameters.AddWithValue("@id", TextBoxID.Text);
                    command.Parameters.AddWithValue("@fname", TextBoxFName.Text);
                    command.Parameters.AddWithValue("@lname", TextBoxLName.Text);
                    command.Parameters.AddWithValue("@username", TextBoxUserName.Text);
                    command.Parameters.AddWithValue("@password", TextBoxPassword.Text);
                    command.Parameters.AddWithValue("@picture", pic.ToArray());

                    mydb.openConnection();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("The HR information has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to update HR information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    mydb.closeConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxUserImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private bool verif()
        {
            if(TextBoxID.Text == ""||
                TextBoxFName.Text == "" ||
                TextBoxLName.Text == "" || 
                TextBoxUserName.Text == "" ||
                TextBoxPassword.Text == "" ||
                PictureBoxUserImage.Image == null)
            {
                return false;
            }

            return true;
        }

        private bool checkInput()
        {
            if(!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid ID Human Resources. Please enter a valid numeric ID Human Resources.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(!IDisExist())
            {
                MessageBox.Show("This ID Human Resources does not exist. Please select another", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(containSpecialChar(TextBoxFName.Text))
            {
                MessageBox.Show("Invalid First Name Human Resources.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (containSpecialChar(TextBoxLName.Text))
            {
                MessageBox.Show("Invalid Last Name Human Resources.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(select == 1 && usernameIsExist())
            {
                MessageBox.Show("Duplicate account Human Resources.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(select == 0 && !usernameIsExist())
            {
                MessageBox.Show("Account does not exist. Can not edit your account", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private bool containSpecialChar(string input)
        {
            foreach(char c in input)
            {
                if(!char.IsLetterOrDigit(c) && c != ' ')
                {
                    return true;
                }
            }

            return false;
        }

        private bool IDisExist()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM hr WHERE id = @ID";
            using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@ID", TextBoxID.Text);
                int count = (int)command.ExecuteScalar();
                mydb.closeConnection();
                return count > 0;
            }
        }

        private bool usernameIsExist()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM hr WHERE username = @username";
            using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@username", TextBoxUserName.Text);
                int count = (int)command.ExecuteScalar();
                mydb.closeConnection();
                return count > 0;
            }
        }

        private void radioButtonYes_CheckedChanged(object sender, EventArgs e)
        {
            labelUS.Visible = true;
            TextBoxUserName.Visible = true;
            buttonSelect.Visible = true;
            label.Visible = false;
            radioButtonNo.Visible = false;
            radioButtonYes.Visible = false;
            groupBox.Visible = false;
            select = 1; 
        }

        private void radioButtonNo_CheckedChanged(object sender, EventArgs e)
        {
            labelUS.Visible = true;
            TextBoxUserName.Visible = true;
            buttonSelect.Visible = true;
            label.Visible = false;
            radioButtonNo.Visible = false;
            radioButtonYes.Visible = false;
            groupBox.Visible = false;
            select = 0;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            labelUS.Visible = false;
            TextBoxUserName.Visible = false;
            buttonSelect.Visible = false;
            label.Visible = true;
            radioButtonNo.Visible = true;
            radioButtonYes.Visible = true;
            groupBox.Visible = true;
        }
    }
}
