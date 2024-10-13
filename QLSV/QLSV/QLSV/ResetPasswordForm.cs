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
    public partial class ResetPasswordForm : Form
    {
        string email;
        MY_DB mydb = new MY_DB();

        public ResetPasswordForm(string email)
        {
            InitializeComponent();
            this.email = email; 
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if(TextBoxPassword.Text == "" || TextBoxRePassword.Text == "")
                {
                    MessageBox.Show("Empty Fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(TextBoxPassword.Text != TextBoxRePassword.Text)
                {
                    MessageBox.Show("Passwords do not match. Please enter matching passwords.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlCommand command = new SqlCommand("UPDATE login SET password = @password WHERE email = @email", mydb.getConnection()))
                {
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                    command.Parameters.Add("@password", SqlDbType.NVarChar).Value = TextBoxPassword.Text;
                    
                    mydb.openConnection();

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update password. User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mydb.closeConnection();
            }
        }
    }
}
