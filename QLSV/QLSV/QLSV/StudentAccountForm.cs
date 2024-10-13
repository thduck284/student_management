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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLSV
{
    public partial class StudentAccountForm : Form
    {
        MY_DB mydb = new MY_DB();

        public StudentAccountForm()
        {
            InitializeComponent();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (!checkInput(TextBoxEmail.Text))
            {
                MessageBox.Show("Email is not valid. Please enter only letters and digits.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                mydb.openConnection();

                string query = "INSERT INTO login (username, password, email) VALUES (@username, @psw, @email)";
                SqlCommand command = new SqlCommand(query, mydb.getConnection());
                command.Parameters.AddWithValue("@username", TextBoxUserName.Text);
                command.Parameters.AddWithValue("@psw", TextBoxPassword.Text);
                command.Parameters.AddWithValue("@email", TextBoxEmail.Text);

                command.ExecuteNonQuery();

                MessageBox.Show("User account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mydb.closeConnection();
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            TypeAccountForm typeAccountForm = new TypeAccountForm();
            typeAccountForm.ShowDialog();
            this.Close();
        }

        private void ButtonComeBack_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Visible = false;
            Login_Form login_Form = new Login_Form();
            login_Form.ShowDialog();
        }

        private bool checkInput(string input)
        {
            foreach(char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false; 
                }
            }

            return true;
        }

        private bool isUserExist()
        {
            try
            {
                mydb.openConnection();

                string query = "SELECT COUNT(*) FROM login WHERE username = @username AND email = @email";
                SqlCommand command = new SqlCommand(query, mydb.getConnection());
                command.Parameters.AddWithValue("@username", TextBoxUserName.Text);
                command.Parameters.AddWithValue("@username", TextBoxEmail.Text + "@email.com");

                int count = (int)command.ExecuteScalar();

                if(count > 0)
                {
                    return true;
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

            return false;
        }        
    }
}
