using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.Cmp;

namespace QLSV
{
    public partial class HumanResourceForm : Form
    {
        MY_DB mydb = new MY_DB();
        int ID;

        public HumanResourceForm(Image image, int ID)
        {
            InitializeComponent();
            PictureBoxHumanResource.Image = image;
            PictureBoxHumanResource.SizeMode = PictureBoxSizeMode.StretchImage;
            this.ID = ID;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddContactForm addContactForm = new AddContactForm();
            addContactForm.ShowDialog();
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            EditContactForm editContactForm = new EditContactForm();
            editContactForm.ShowDialog();
        }

        private void ButtonSelectContact_Click(object sender, EventArgs e)
        {
            SelectContactForm selectContactForm = new SelectContactForm();
            selectContactForm.ShowDialog();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (!IDisExist())
            {
                MessageBox.Show("This ID Contact does not exist. Please select another", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlCommand command = new SqlCommand("DELETE FROM [contact] WHERE ID = @id", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.VarChar).Value = TextBoxID.Text;

            mydb.openConnection();
            int rowsAffected = command.ExecuteNonQuery();
            mydb.closeConnection();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Contact Deleted", "Deleted Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Deleted Contact", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonShowList_Click(object sender, EventArgs e)
        {
            ContactShowFullListForm contactShowFullListForm = new ContactShowFullListForm();
            contactShowFullListForm.ShowDialog();
        }

        private void ButtonAddGroup_Click(object sender, EventArgs e)
        {
            if(TextBoxGroupName.Text == "")
            {
                MessageBox.Show("Please enter Group Name which need to add");
                return;
            }

            if (GroupNameIsExist())
            {
                MessageBox.Show("This Group Name already exists in the database. Please choose a different one.", "Duplicate Group Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(!checkInput(TextBoxGroupName.Text))
            {
                MessageBox.Show("Invalid characters in Group Name. Please enter only letters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = getIDGroup();
            SqlCommand command = new SqlCommand("INSERT INTO [group] (IDGroup, Name) VALUES (@id, @name)", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = TextBoxGroupName.Text;

            mydb.openConnection();

            if (command.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("New Group Added", "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TextBoxGroupName.Clear();
            }
            else
            {
                MessageBox.Show("Added Group", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            mydb.closeConnection();
        }

        private void ButtonEditGroup_Click(object sender, EventArgs e)
        {
            if (GroupNameIsExist())
            {
                MessageBox.Show("This Group Name already exists in the database. Please choose a different one.", "Duplicate Group Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlCommand command = new SqlCommand("UPDATE [group] SET Name = @new WHERE Name = @old", mydb.getConnection()))
            {
                command.Parameters.Add("@old", SqlDbType.NVarChar).Value = ComboBoxEditGroup.SelectedItem;
                command.Parameters.Add("@new", SqlDbType.NVarChar).Value = TextBoxNewName.Text;

                mydb.openConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if(rowsAffected == 1)
                {
                    MessageBox.Show("Group Name Edited", "Edited Group Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TextBoxNewName.Text = ""; 
                }
                else
                {
                    MessageBox.Show("Error", "Edited Group Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonRemoveGroup_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [group] WHERE Name = @name", mydb.getConnection());
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = ComboBoxRemoveGroup.SelectedItem;

            mydb.openConnection();
            int rowsAffected = command.ExecuteNonQuery();
            mydb.closeConnection();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Group Name Deleted", "Deleted Group Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Deleted Group Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int getIDGroup()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM [group]";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count + 1;
        }

        private bool GroupNameIsExist()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM [group] WHERE Name = @name";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@name", TextBoxNewName.Text);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        private bool checkInput(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void ButtonEditInfo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = getAllInfor();
            string fn = dt.Rows[0]["FName"].ToString();
            string ln = dt.Rows[0]["LName"].ToString();
            string us = dt.Rows[0]["UserName"].ToString();
            string pas = dt.Rows[0]["Password"].ToString();
            Image pic;
            byte[] imageData = (byte[])dt.Rows[0]["Picture"];
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                pic = Image.FromStream(ms);
            }
            EditHumanResourceForm editHumanResourceForm = new EditHumanResourceForm(ID, fn, ln, us, pas, pic);
            editHumanResourceForm.ShowDialog();

        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            TextBoxID.Text = "";
            TextBoxGroupName.Text = "";
            TextBoxNewName.Text = "";
            ComboBoxEditGroup.Items.Clear();
            ComboBoxRemoveGroup.Items.Clear();
        }

        private DataTable getAllInfor()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT * FROM hr WHERE ID = @id", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = ID;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            
            return table;
        }

        private void ComboBoxEditGroup_Click(object sender, EventArgs e)
        {
            ComboBoxEditGroup.Items.Clear();
            SqlCommand command = new SqlCommand("SELECT Name FROM [group]", mydb.getConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                ComboBoxEditGroup.Items.Add(row["Name"].ToString());
            }
        }

        private void ComboBoxRemoveGroup_Click(object sender, EventArgs e)
        {
            ComboBoxRemoveGroup.Items.Clear();
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT Name FROM [group]";

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                ComboBoxRemoveGroup.Items.Add(row["Name"].ToString());
            }
        }

        private bool IDisExist()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM contact WHERE idstatic = @ID";
            using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@ID", TextBoxID.Text);
                int count = (int)command.ExecuteScalar();
                mydb.closeConnection();
                return count > 0;
            }
        }
    }
}
