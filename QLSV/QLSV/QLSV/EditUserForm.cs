using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLSV
{
    public partial class EditUserForm : Form
    {
        MY_DB db = new MY_DB();

        public EditUserForm()
        {
            InitializeComponent();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            string username = TextBoxUserName.Text;
            string password = TextBoxPassword.Text;
            string email = TextBoxEmail.Text + "@email.com";

            if (!checkRegister(username, password, email))
            {
                return;
            }

            // Thực hiện câu lệnh SQL để thêm vào cơ sở dữ liệu
            string query = "INSERT INTO Login (username, password, email) VALUES (@username, @password, @email)";

            SqlCommand command = new SqlCommand(query, db.getConnection());

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@email", email);

            // Mở kết nối
            db.openConnection();

            // Thử thực hiện truy vấn
            try
            {
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Registration successful", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error while adding registration information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đóng kết nối sau khi thực hiện
                db.closeConnection();
            }
        }

        public static bool IsValidEmail(string email)
        {
            // Kiểm tra xem chuỗi có null hoặc trống không
            if (string.IsNullOrEmpty(email))
                return false;

            // Sử dụng biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

        bool checkRegister(string username, string password, string email)
        {
            // Check username
            if (ContainsSpecialCharacters(username))
            {
                MessageBox.Show("Username must not contain special characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check password
            if (ContainsSpecialCharacters(password))
            {
                MessageBox.Show("Password must not contain special characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if username already exists
            if (UsernameExists(username))
            {
                MessageBox.Show("Username already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        bool ContainsSpecialCharacters(string input)
        {
            // Check if the input contains special characters
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        bool UsernameExists(string username)
        {
            // Thực hiện zz vấn để kiểm tra xem username đã tồn tại trong cơ sở dữ liệu hay chưa
            string query = "SELECT COUNT(*) FROM Login WHERE username = @username";
            SqlCommand command = new SqlCommand(query, db.getConnection());
            command.Parameters.AddWithValue("@username", username);

            db.openConnection();
            int count = (int)command.ExecuteScalar();
            db.closeConnection();

            // Nếu count lớn hơn 0, tức là username đã tồn tại
            return count > 0;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            string username = TextBoxUserName.Text;
            string password = TextBoxPassword.Text;
            string email = TextBoxEmail.Text + "@email.com";

            if (!checkEdit(username, password, email))
            {
                return;
            }

            using (SqlCommand command = new SqlCommand("UPDATE Login SET Username = @us, Password = @password, Email = @email", db.getConnection()))
            {
                if (!checkRegister(username, password, email))
                {
                    return;
                }

                command.Parameters.Add("@us", SqlDbType.VarChar).Value = username;
                command.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
                command.Parameters.Add("@password", SqlDbType.VarChar).Value = email;

                db.openConnection();
                int rowsAffected = command.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show("Edited user successfully");
            }
        }

        bool checkEdit(string username, string password, string email)
        {
            // Check username
            if (ContainsSpecialCharacters(username))
            {
                MessageBox.Show("Username must not contain special characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check password
            if (ContainsSpecialCharacters(password))
            {
                MessageBox.Show("Password must not contain special characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if(UsernameExists(TextBoxUserName.Text))
            {
                MessageBox.Show("User is not exist.");
                return;
            }

            SqlCommand command = new SqlCommand("DELETE FROM Login WHERE Username = @us", db.getConnection());
            command.Parameters.Add("@us", SqlDbType.VarChar).Value = TextBoxUserName.Text;
            db.openConnection();
            command.ExecuteNonQuery();
            MessageBox.Show("Remove user succefully");
            db.closeConnection();
        }
    }
}
