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
using System.Windows.Media.TextFormatting;
using static System.Windows.Forms.AxHost;

namespace QLSV
{
    public partial class AddContactForm : Form
    {
        MY_DB mydb = new MY_DB();
        bool isTeacher = false;

        public AddContactForm()
        {
            InitializeComponent();
        }

        private void ButtonAddContact_Click(object sender, EventArgs e)
        {
            if(!verif())
            {
                MessageBox.Show("Empty Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!checkInforContact())
            {
                return;
            }

            int staticid = getIDStatic();
            string type = (isTeacher == true) ? "teacher" : "other";

            using (SqlCommand command = new SqlCommand("insert into contact (ID, FName, LName, IDGroup, GroupName, Phone, Email, Address, Picture, Type, IDStatic) VALUES (@id, @fn, @ln, @idgr, @group, @phone, @email, @adrs, @pic, @type, @ids)", mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@id", Convert.ToInt32(TextBoxID.Text));
                command.Parameters.AddWithValue("@fn", TextBoxFname.Text);
                command.Parameters.AddWithValue("@ln", TextBoxLname.Text);
                command.Parameters.AddWithValue("@idgr", getIDGroup());
                command.Parameters.AddWithValue("@group", ComboBoxGroup.SelectedItem);
                command.Parameters.AddWithValue("@phone", TextBoxPhone.Text);
                command.Parameters.AddWithValue("@email", TextBoxEmail.Text + "@email.com");
                command.Parameters.AddWithValue("@adrs", TextBoxAddress.Text);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@ids", staticid);
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
                    MessageBox.Show("New Contact Added", "Add Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Error", "Add Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            else if (IDContactIsExist(Convert.ToInt32(TextBoxID.Text)) && !isTeacher)
            {
                MessageBox.Show("This ID already exists in the database. Please choose a different one.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(isTeacher && IDSubjectExist())
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

            if(ContainsSpecialCharacters(TextBoxEmail.Text))
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
            string query = "SELECT COUNT(*) FROM contact WHERE ID = @id AND Type = @type";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", TextBoxID.Text);
            cmd.Parameters.AddWithValue("@type", TextBoxID.Text);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        private bool IDSubjectExist()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM course WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", TextBoxID.Text);

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

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int getIDStatic()
        {
            int idStatic = 0;
            try
            {
                mydb.openConnection();
                string query = "SELECT COUNT(*) AS TotalContacts FROM contact";
                SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    idStatic = Convert.ToInt32(reader["TotalContacts"]) + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mydb.closeConnection();
            }
            return idStatic;
        }

        private void ComboBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
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

        private void radioButtonTeacher_CheckedChanged(object sender, EventArgs e)
        {
            labelID.Visible = false;

            labelIDSubject.Visible = true;
            TextBoxID.Visible = true;
            buttonSelect.Visible = true;

            groupBoxSelect.Visible = false;
            radioButtonOther.Visible = false;
            radioButtonTeacher.Visible = false;
            labelSelect.Visible = false;
            isTeacher = true;
        }

        private void radioButtonOther_CheckedChanged(object sender, EventArgs e)
        {
            labelIDSubject.Visible = false;

            labelSelect.Visible = false;
            labelID.Visible = true;
            TextBoxID.Visible = true;
            buttonSelect.Visible = true;

            groupBoxSelect.Visible = false;
            radioButtonOther.Visible = false;
            radioButtonTeacher.Visible = false;
            isTeacher = false;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            labelID.Visible = false;
            TextBoxID.Visible = false;
            buttonSelect.Visible = false;

            labelIDSubject.Visible = false;
            TextBoxID.Visible = false;
            buttonSelect.Visible = false;

            groupBoxSelect.Visible = true;
            radioButtonOther.Visible = true;
            radioButtonTeacher.Visible = true;
            labelSelect.Visible = true;
        }
    }
}
