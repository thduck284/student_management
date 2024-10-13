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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class Login_Form : Form
    {
        MY_DB mydb = new MY_DB();

        public Login_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"C:\Users\DELL\Pictures\Screenshots\Screenshot 2024-03-06 204129.png");
        }

        private void btt_Login_Click(object sender, EventArgs e)
        {
            if(TextBoxUsername.Text == "" || TextBoxPassword.Text == "")
            {
                MessageBox.Show("Empty Field", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!RadioButtonHumanResource.Checked && !RadioButtonStudent.Checked && !RadioButtonAdmin.Checked)
            {
                MessageBox.Show("Please select user type (Admin/Human Resource/Student)", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query;

            if (RadioButtonHumanResource.Checked)
            {
                query = "SELECT * FROM hr WHERE UserName = @User AND Password = @Password";
            }
            else if (RadioButtonStudent.Checked)
            {
                query = "SELECT * FROM Login WHERE username = @User AND password = @Password";
                
            }
            else
            {
                query = "SELECT * FROM admin WHERE username = @User AND password = @Password";
            }

            try
            {
                MY_DB db = new MY_DB();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                SqlCommand command = new SqlCommand(query, db.getConnection());
                command.Parameters.Add("@User", SqlDbType.VarChar).Value = TextBoxUsername.Text;
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = TextBoxPassword.Text;
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    this.Visible = false;
                    if(RadioButtonStudent.Checked)
                    {
                        MainForm01 mainForm01 = new MainForm01();
                        mainForm01.ShowDialog();
                    }
                    else if(RadioButtonHumanResource.Checked)
                    {
                        DataTable dt = new DataTable();
                        dt = getImage_ID();
                        int id = Convert.ToInt32(dt.Rows[0]["ID"]);
                        byte[] imageData = (byte[])dt.Rows[0]["Picture"];
                        Image image;
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            image = Image.FromStream(ms);
                        }
                        HumanResourceForm humanResourceForm =  new HumanResourceForm(image, id);
                        humanResourceForm.ShowDialog();
                    }
                    else
                    {
                        AdminForm adminForm = new AdminForm();
                        adminForm.ShowDialog();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btt_Register_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Close();
            TypeAccountForm addRegisterForm = new TypeAccountForm();
            addRegisterForm.ShowDialog();
        }

        private void btt_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btt_Login_Click(sender, e);
            }
            else if(e.KeyCode == Keys.Up)
            {               
                TextBoxUsername.Focus();
                e.Handled = true;
            }

        }

        private void TextBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                TextBoxPassword.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void ButtonForgetPassword_Click(object sender, EventArgs e)
        {
            SendCodeForm forgetPasswordForm = new SendCodeForm();
            forgetPasswordForm.ShowDialog();
        }

        private DataTable getImage_ID()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = mydb.getConnection())
            {
                connection.Open();

                string query = "SELECT ID, Picture FROM hr WHERE username = @us";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@us", TextBoxUsername.Text);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

    }
}
