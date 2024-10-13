using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QLSV
{
    class STUDENT
    {

        MY_DB mydb = new MY_DB();

        public bool isStudentIdExists(int id)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM std WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", id);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        //  function to insert a new student
        public bool insertStudent(int Id, string fname, string lname, DateTime bdate, string gender, string phone, string address, MemoryStream picture)
        { 
            using (SqlCommand command = new SqlCommand("insert into std (ID, FName, LName, Birthday, Gender, Phone, Address, Picture) VALUES (@id, @fn, @ln, @bdt, @gdr, @phn, @adrs, @pic)", mydb.getConnection()))
            {
                // Thêm tham số để tránh SQL injection
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@fn", fname);
                command.Parameters.AddWithValue("@ln", lname);
                command.Parameters.AddWithValue("@bdt", bdate);
                command.Parameters.AddWithValue("@gdr", gender);
                command.Parameters.AddWithValue("@phn", phone);
                command.Parameters.AddWithValue("@adrs", address);
                command.Parameters.AddWithValue("@pic", picture.ToArray()); 

                mydb.openConnection();

                if (command.ExecuteNonQuery() > 0)
                {
                    //MessageBox.Show("Dữ liệu đã được thêm thành công");
                    return true;
                }
                else
                {
                    MessageBox.Show("Thêm dữ liệu thất bại");
                    return false;
                }

                mydb.closeConnection();
            }
        }

        public DataTable getStudents(SqlCommand command)
        {
            command.Connection = mydb.getConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool deleteStudent(int id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM std WHERE ID = @id", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            mydb.openConnection();
            if((command.ExecuteNonQuery()) == 1)
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }
        }

        public bool updateStudent(int id, string fname, string lname, DateTime bdate, string gender, string phone, string address, MemoryStream picture)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE std SET FName = @fn, LName = @ln, Birthday = @bdt, Gender = @gdr, Phone = @phn, Address = @adrs, Picture = @pic  WHERE ID = @ID", mydb.getConnection()))
                {
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@fn", SqlDbType.NVarChar).Value = fname;
                    command.Parameters.Add("@ln", SqlDbType.NVarChar).Value = lname;
                    command.Parameters.Add("@bdt", SqlDbType.DateTime).Value = bdate;
                    command.Parameters.Add("@gdr", SqlDbType.NVarChar).Value = gender;
                    command.Parameters.Add("@phn", SqlDbType.VarChar).Value = phone;
                    command.Parameters.Add("@adrs", SqlDbType.NVarChar).Value = address;
                    command.Parameters.Add("@pic", SqlDbType.Image).Value = picture.ToArray();

                    mydb.openConnection();

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected == 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student: {ex.Message}");
                return false;
            }
            finally
            {
                mydb.closeConnection();
            }
        }

        public double TotalStudentCount()
        {
            try
            {
                mydb.openConnection();
                string query = "SELECT COUNT(*) FROM std";
                SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
                double count = Convert.ToDouble(cmd.ExecuteScalar());
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting total students: {ex.Message}");
                return -1; // Trả về -1 khi có lỗi xảy ra
            }
            finally
            {
                mydb.closeConnection();
            }
        }

        public double TotalMaleStudentCount()
        {
            try
            {
                mydb.openConnection();
                string query = "SELECT COUNT(*) FROM std WHERE gender = 'Male'";
                SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
                double count = Convert.ToDouble(cmd.ExecuteScalar());
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting total male students: {ex.Message}");
                return -1; // Trả về -1 khi có lỗi xảy ra
            }
            finally
            {
                mydb.closeConnection();
            }
        }

        public double TotalFemaleStudentCount()
        {
            try
            {
                mydb.openConnection();
                string query = "SELECT COUNT(*) FROM std WHERE Gender = 'Female'";
                SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
                double count = Convert.ToDouble(cmd.ExecuteScalar());
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting total female students: {ex.Message}");
                return -1; // Trả về -1 khi có lỗi xảy ra
            }
            finally
            {
                mydb.closeConnection();
            }
        }

    }
}
