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
    public partial class RemoveContactForm : Form
    {
        MY_DB mydb = new MY_DB();

        public RemoveContactForm()
        {
            InitializeComponent();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if(!IDisExist())
            {
                MessageBox.Show("This ID Contact does not exist. Please select another", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlCommand command = new SqlCommand("DELETE FROM [contact] WHERE IDStatic = @id", mydb.getConnection());
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

        private void ButtonSelectContact_Click(object sender, EventArgs e)
        {
            SelectContactForm selectContactForm = new SelectContactForm();
            selectContactForm.ShowDialog();
        }
    }
}
