using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class ManageGroupForm : Form
    {
        MY_DB mydb = new MY_DB();

        public ManageGroupForm()
        {
            InitializeComponent();
        }

        private void ButtonAddGroup_Click(object sender, EventArgs e)
        {
            if (TextBoxGroupName.Text == "")
            {
                MessageBox.Show("Please enter Group Name which need to add");
                return;
            }

            if (GroupNameIsExist())
            {
                MessageBox.Show("This Group Name already exists in the database. Please choose a different one.", "Duplicate Group Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!checkInput(TextBoxGroupName.Text))
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
                loadGroupName();
            }
            else
            {
                MessageBox.Show("Added Group", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            mydb.closeConnection();
        }

        private int getIDGroup()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM [group]";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count + 1;
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

        private bool GroupNameIsExist()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM [group] WHERE Name = @name";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@name", TextBoxNewName.Text);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
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

                if (rowsAffected == 1)
                {
                    MessageBox.Show("Group Name Edited", "Edited Group Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TextBoxNewName.Text = "";
                    loadGroupName();
                }
                else
                {
                    MessageBox.Show("Error", "Edited Group Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ComboBoxEditGroup_SelectedIndexChanged(object sender, EventArgs e)
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

        private void ComboBoxRemoveGroup_SelectedIndexChanged(object sender, EventArgs e)
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
                loadGroupName();
            }
            else
            {
                MessageBox.Show("Deleted Group Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadGroupName()
        {
            listBoxGroup.Items.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT name FROM group", mydb.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach(DataRow row in table.Rows)
            {
                listBoxGroup.Items.Add(row["name"].ToString());
            }
        }
    }
}
