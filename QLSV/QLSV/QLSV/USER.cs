using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    internal class USER
    {
        MY_DB db = new MY_DB();

        public DataTable getUserById(int userid)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT * FROM user WHERE ID = @id", db.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = userid;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return table;
        }

        public bool insertHumanResource(int id, string fname, string lname, string username, string password, MemoryStream picture)
        {
            if (userExist(username, "", id))
            {
                MessageBox.Show("This Username or ID already exists in the database. Please choose a different one.", "Duplicate Username or ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("INSERT INTO hr (ID, FName, LName, UserName, Password, Picture) VALUES (@id, @fn, @ln, @un, @pass, @pic)", db.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@fn", SqlDbType.NVarChar).Value = fname;
            command.Parameters.Add("@ln", SqlDbType.NVarChar).Value = lname;
            command.Parameters.Add("@un", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@pass", SqlDbType.VarChar).Value = password;
            command.Parameters.Add("@pic", SqlDbType.Image).Value = picture.ToArray();
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }
        }

        public bool userExist(string username, string operation, int userid)
        {
            string query = "";
            if(operation == "Student User")
            {
                query = "SELECT COUNT(*) FROM Login WHERE username = @us";
            }
            else
            {
                query = "SELECT COUNT(*) FROM hr WHERE ID = @id OR UserName = @us";
            }
            db.openConnection();
            
            SqlCommand cmd = new SqlCommand(query, db.getConnection());
            cmd.Parameters.AddWithValue("@us", username);
            if(operation != "Student User")
            {
                cmd.Parameters.AddWithValue("@id", userid);
            }

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            db.closeConnection();

            return count > 0;
        }

        // Cập nhật, giống như trong Student
        public bool updateHumanResource(int userid, string fname, string lname, string username, string password, MemoryStream picture)
        {
            SqlCommand command = new SqlCommand("UPDATE hr SET FName = @fn, LName = @ln, UserName = @un, Password = @pass, Picture = @pic WHERE ID = @uid", db.getConnection());
            command.Parameters.Add("@fn", SqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", SqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@un", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@pass", SqlDbType.VarChar).Value = password;
            command.Parameters.Add("@pic", SqlDbType.Image).Value = picture.ToArray();
            command.Parameters.Add("@uid", SqlDbType.Int).Value = userid;
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }
        }

    }
}
