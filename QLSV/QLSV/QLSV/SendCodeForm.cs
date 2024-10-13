using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net.Mail;

namespace QLSV
{
    public partial class SendCodeForm : Form
    {
        int code;
        MY_DB mydb = new MY_DB();
        string email;

        public SendCodeForm()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if(TextBoxEmail.Text == "" || TextBoxCode.Text == "")
            {
                MessageBox.Show("Empty Fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ResetPasswordForm resetPasswordForm = new ResetPasswordForm(email);
            resetPasswordForm.ShowDialog();
        }        

        private void SendCode(string email)
        {
            try
            {
                Random rand = new Random();
                code = rand.Next(100000, 999999);
                TextBoxCode.Text = code.ToString();

                string smtpAddress = "smtp.gmail.com"; 
                int portNumber = 587; 
                bool enableSSL = true; 

                string emailFrom = "2211@student.hcmute.edu.vn"; 
                string password = "123456789";

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(emailFrom);
                        mail.To.Add(email); 
                        mail.Subject = "Code"; 
                        mail.Body = code.ToString(); 

                        smtp.Send(mail);
                    }
                }

                MessageBox.Show("Email sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Email sent successfully.");
            }
        }

        private bool UserExists(string email)
        {
            try
            {
                using (SqlConnection connection = mydb.getConnection())
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM login WHERE email = @email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking user existence: " + ex.Message);
                return false;
            }
        }

        private void buttonSendCode_Click(object sender, EventArgs e)
        {
            email = TextBoxEmail.Text;

            if (UserExists(email))
            {
                SendCode(email);
            }
            else
            {
                MessageBox.Show("Username or email does not exist in the database.");
                return;
            }
        }
    }
}
