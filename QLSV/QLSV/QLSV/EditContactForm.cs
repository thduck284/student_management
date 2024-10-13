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

namespace QLSV
{
    public partial class EditContactForm : Form
    {
        MY_DB mydb = new MY_DB();
        private bool isTeacher;
        private Label lblDescription;
        private RadioButton rbTeacher;
        private RadioButton rbOther;
        private Button btnConfirm;

        public EditContactForm()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void ButtonSelectContact_Click(object sender, EventArgs e)
        {
            SelectContactForm selectContactForm = new SelectContactForm();
            selectContactForm.ShowDialog();
        }

        private void ButtonEditContact_Click(object sender, EventArgs e)
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

            string type;
            if(isTeacher)
            {
                type = "teacher"; 
            }
            else
            {
                type = "other";
            }

            string query;

            if(isTeacher)
            {
                query = "UPDATE contact SET ID = @id, FName = @fn, LName = @ln, IDGroup = @idgr, GroupName = @group, Phone = @phone, Email = @email, Address = @adrs, Picture = @pic , Type = @type WHERE IDStatic = @ids";
            }
            else
            {
                query = "UPDATE contact SET FName = @fn, LName = @ln, IDGroup = @idgr, GroupName = @group, Phone = @phone, Email = @email, Address = @adrs, Picture = @pic, Type = @type WHERE IDStatic = @ids";
            }

            using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
            {
                if(isTeacher)
                {
                    command.Parameters.AddWithValue("@id", TextBoxIDS.Text);
                }
                command.Parameters.AddWithValue("@ids", TextBoxID.Text);
                command.Parameters.AddWithValue("@fn", TextBoxFname.Text);
                command.Parameters.AddWithValue("@ln", TextBoxLname.Text);
                command.Parameters.AddWithValue("@idgr", getIDGroup());
                command.Parameters.AddWithValue("@group", ComboBoxGroup.SelectedItem);
                command.Parameters.AddWithValue("@phone", TextBoxPhone.Text);
                command.Parameters.AddWithValue("@email", TextBoxEmail.Text + "@email.com");
                command.Parameters.AddWithValue("@adrs", TextBoxAddress.Text);
                command.Parameters.AddWithValue("@type", type);
                Image pic = PictureBoxContactImage.Image;
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
                    MessageBox.Show("New Contact Edited", "Edit Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Error", "Edit Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                mydb.closeConnection();
            }
        }

        private bool checkInforContact()
        {
            if (!int.TryParse(TextBoxID.Text, out _))
            {
                MessageBox.Show("Invalid ID. Please enter a valid numeric ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!IDContactIsExist(Convert.ToInt32(TextBoxID.Text)))
            {
                MessageBox.Show("This ID does not exist in the database. Please choose a different one.", "Exist ID Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(isTeacher && !int.TryParse(TextBoxIDS.Text, out _))
            {
                MessageBox.Show("Invalid ID Subject. Please enter a valid numeric ID Subject.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(isTeacher && !IDSubjectExist(Convert.ToInt32(TextBoxIDS.Text)))
            {
                MessageBox.Show("This ID Subject does not exist in the database. Please choose a different one.", "Exist ID Subject", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (InvalidCharacters(TextBoxFname.Text) && InvalidCharacters(TextBoxLname.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME and LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (InvalidCharacters(TextBoxFname.Text))
            {
                MessageBox.Show("Invalid characters in FIRSTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (InvalidCharacters(TextBoxLname.Text))
            {
                MessageBox.Show("Invalid characters in LASTNAME. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ContainsSpecialCharacters(TextBoxEmail.Text))
            {
                MessageBox.Show("Invalid characters in Email. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!long.TryParse(TextBoxPhone.Text, out _))
            {
                MessageBox.Show("Invalid phone number. Please enter a valid numeric phone number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool IDContactIsExist(int id)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM contact WHERE IDStatic = @id";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", TextBoxID.Text);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        private bool IDSubjectExist(int id)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM course WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", TextBoxIDS.Text);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
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

        private void ClearFields()
        {
            TextBoxID.Clear();
            TextBoxFname.Clear();
            TextBoxLname.Clear();
            TextBoxPhone.Clear();
            TextBoxAddress.Clear();
            PictureBoxContactImage.Image = null;
            TextBoxEmail.Clear();
            ComboBoxGroup.Items.Remove(ComboBoxGroup.SelectedItem);
            ComboBoxGroup.Items.Clear();
        }

        bool verif()
        {
            if ((TextBoxFname.Text.Trim() == "")
                        || (TextBoxLname.Text.Trim() == "")
                        || (TextBoxAddress.Text.Trim() == "")
                        || (TextBoxPhone.Text.Trim() == "")
                        || (PictureBoxContactImage.Image == null)
                        || (TextBoxID.Text == null)
                        || (ComboBoxGroup.SelectedIndex == -1)
                        || (TextBoxEmail.Text == ""))
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
                PictureBoxContactImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private int getIDGroup()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT IDGroup FROM [group] WHERE Name = @name", mydb.getConnection());
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = ComboBoxGroup.SelectedItem;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            int id = Convert.ToInt32(table.Rows[0]["IDGroup"]);
            return id;
        }

        private void ComboBoxGroup_Click(object sender, EventArgs e)
        {
            ComboBoxGroup.Items.Clear();
            SqlCommand command = new SqlCommand("SELECT Name FROM [group]", mydb.getConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                ComboBoxGroup.Items.Add(row["Name"].ToString());
            }
        }

        private void EditContactForm_Load(object sender, EventArgs e)
        {
            createControl();
            this.Size = new Size(845, 350);
        }

        private void createControl()
        {
            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }

            lblDescription = new Label();
            lblDescription.Text = "Select Contact Type";
            lblDescription.Font = new Font("Monotype Corsiva", 24, FontStyle.Bold | FontStyle.Italic);
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = Color.Purple;
            lblDescription.Location = new Point(270, 30);
            this.Controls.Add(lblDescription);

            rbTeacher = new RadioButton();
            rbTeacher.Text = "Teacher";
            rbTeacher.Font = new Font("Microsoft Sans Serif", 13.8f, FontStyle.Bold);
            rbTeacher.AutoSize = true;


            rbOther = new RadioButton();
            rbOther.Text = "Other";
            rbOther.Font = new Font("Microsoft Sans Serif", 13.8f, FontStyle.Bold);
            rbOther.AutoSize = true;

            rbTeacher.Location = new Point(300, 120);
            this.Controls.Add(rbTeacher);
            rbOther.Location = new Point(480, 120);
            this.Controls.Add(rbOther);

            btnConfirm = new Button();
            btnConfirm.Text = "Confirm";
            btnConfirm.Font = new Font("Microsoft Sans Serif", 16.2f, FontStyle.Bold);
            btnConfirm.ForeColor = Color.Black; 
            btnConfirm.BackColor = Color.Silver; 
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.AutoSize = true;
            btnConfirm.Location = new Point(370, 200);
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);
        }

        private void HideControls()
        {
            lblDescription.Visible = false;
            rbTeacher.Visible = false;
            rbOther.Visible = false;
            btnConfirm.Visible = false;
        }

        private void UnhideControls()
        {
            lblDescription.Visible = true;
            rbTeacher.Visible = true;
            rbOther.Visible = true;
            btnConfirm.Visible = true;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (rbTeacher.Checked)
            {
                isTeacher = true;
            }
            else if (rbOther.Checked)
            {
                isTeacher = false;
            }
            else
            {
                MessageBox.Show("Please select a contact type.");
                return;
            }

            this.Size = new Size(818, 840);

            foreach (Control control in this.Controls)
            {
                control.Visible = true;
            }

            if(!isTeacher)
            {
                labelIDS.Visible = false;
                TextBoxIDS.Visible = false;
            }

            HideControls();
        }

        private void buttonSelectType_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }

            this.Size = new Size(845, 350);
            UnhideControls();
        }
    }
}
