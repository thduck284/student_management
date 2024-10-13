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
using System.Windows.Input;
using Microsoft.ReportingServices.Diagnostics.Internal;

namespace QLSV
{
    public partial class RemoveHRForm : Form
    {
        MY_DB mydb = new MY_DB();

        public RemoveHRForm()
        {
            InitializeComponent();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if(!IDHRExist())
            {
                MessageBox.Show("This Human Resources ID does not exist in the database. Can not Remove and please choose a different one.", "Invalid ID Human Resources", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                mydb.openConnection();
                int id = Convert.ToInt32(TextBoxIDHRRemove.Text);
                string query = "DELETE FROM hr WHERE id = @ID";
                using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    mydb.closeConnection();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Delete Human Resources successly", "Delete HR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error: Unable to delete Human Resources.", "Delete HR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IDHRExist()
        {
            mydb.openConnection();
            int id = Convert.ToInt32(TextBoxIDHRRemove.Text);
            string query = "SELECT COUNT(*) FROM hr WHERE id = @ID";
            using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@ID", id);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
