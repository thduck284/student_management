using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLSV
{
    public partial class HRAccountForm : Form
    {
        MY_DB db = new MY_DB();
        USER user = new USER();

        public HRAccountForm()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (!verif())
            {
                MessageBox.Show("Empty Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = TextBoxUserName.Text;
            string password = TextBoxPassword.Text;
            if (!checkRegister(username, password))
            {
                return;
            }
            
            int id = -1;
            string fname = "";
            string lname = "";
            fname = TextBoxFName.Text;
            lname = TextBoxLName.Text;
            MemoryStream pic = new MemoryStream();

            if (int.TryParse(TextBoxID.Text, out _))
            {
                id = Convert.ToInt32(TextBoxID.Text);
            }
            
            if (PictureBoxUserImage.Image != null)
            {
                PictureBoxUserImage.Image.Save(pic, PictureBoxUserImage.Image.RawFormat);
            }

            if (user.insertHumanResource(id, fname, lname, username, password, pic))
            {
                MessageBox.Show("New User Added", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
        }

        bool checkRegister(string username, string password)
        {
            if (ContainsSpecialCharacters(username))
            {
                MessageBox.Show("Username must not contain special characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ContainsSpecialCharacters(password))
            {
                MessageBox.Show("Password must not contain special characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid ID. Please enter a valid numeric ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (InvalidNameUser(TextBoxFName.Text) && InvalidNameUser(TextBoxLName.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME and LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (InvalidNameUser(TextBoxFName.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (InvalidNameUser(TextBoxLName.Text))
            {
                MessageBox.Show("Invalid characters in LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        bool ContainsSpecialCharacters(string input)
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

        bool InvalidNameUser(string input)
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
            if (TextBoxID.Text == "" || TextBoxFName.Text == "" || TextBoxLName.Text == "" ||
                   TextBoxUserName.Text == "" || TextBoxPassword.Text == "" || PictureBoxUserImage.Image == null)
            {
                return false;
            }

            return true;
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

        private void ButtonComeBack_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Visible = false;
            Login_Form login_Form = new Login_Form();
            login_Form.ShowDialog();
        }

        private void ClearFields()
        {
            TextBoxID.Clear();
            TextBoxFName.Clear();
            TextBoxLName.Clear();
            TextBoxUserName.Clear();
            TextBoxPassword.Clear();
            PictureBoxUserImage.Image = null;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            TypeAccountForm typeAccountForm = new TypeAccountForm();
            typeAccountForm.ShowDialog();
            this.Close();
        }
    }
}
