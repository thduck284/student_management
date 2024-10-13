using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace QLSV
{
    class COURSE
    {
        MY_DB mydb = new MY_DB();

        public bool checkCourseName(string coursename, int semester)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM course WHERE Name = @name and Semester = @semester";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@name", coursename);
            cmd.Parameters.AddWithValue("@semester", semester);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count > 0) return false;
            return true;
        }

        public bool insertCourse(int courseID, string courseName, int period, int semester, string description)
        {
            using (SqlCommand command = new SqlCommand("insert into course (ID, Name, Period, Semester, Description) VALUES (@id, @name, @period, @semester, @description)", mydb.getConnection()))
            {
                // Thêm tham số để tránh SQL injection
                command.Parameters.AddWithValue("@id", courseID);
                command.Parameters.AddWithValue("@name", courseName);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@semester", semester);
                command.Parameters.AddWithValue("@description", description);

                mydb.openConnection();

                if (command.ExecuteNonQuery() > 0)
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
        }

        public bool removeCourse(int courseID)
        {
            SqlCommand command = new SqlCommand("DELETE FROM course WHERE ID = @id", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = courseID;
            mydb.openConnection();
            if ((command.ExecuteNonQuery()) == 1)
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

        public bool editCourse(int courseID, string courseName, int period, int semester, string description)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE course SET Name = @name, Period = @period, Semester = @semester, Description = @description  WHERE ID = @id and Semester = @semester", mydb.getConnection()))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = courseID;
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = courseName;
                    command.Parameters.Add("@period", SqlDbType.Int).Value = period;
                    command.Parameters.Add("@semester", SqlDbType.Int).Value = semester;
                    command.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                    
                    mydb.openConnection();

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected == 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating course: {ex.Message}");
                return false;
            }
            finally
            {
                mydb.closeConnection();
            }
        }

        public bool courseIDIsExist(int courseID, int semester)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM course WHERE ID = @id and Semester = @semester";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", courseID);
            cmd.Parameters.AddWithValue("@semester", semester);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public bool updateCourse(int courseID, string courseName, int period, int semester,string description)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE course SET Name = @name, Period = @period, Semester = @semester, Description = @description WHERE ID = @id", mydb.getConnection()))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = courseID;
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = courseName;
                    command.Parameters.Add("@period", SqlDbType.Int).Value = period;
                    command.Parameters.Add("@semester", SqlDbType.Int).Value = semester;
                    command.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
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

        public double totalCourses()
        {
            mydb.openConnection();
            string query = "SELECT COUNT(id) FROM course";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            double count = Convert.ToDouble(cmd.ExecuteScalar());
            return count;
        }

        public DataTable getDataCourses()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM course");
            command.Connection = mydb.getConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public DataTable getAllCourses()
        {
            SqlCommand command = new SqlCommand("SELECT name, semester FROM course");
            command.Connection = mydb.getConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool deleteCourse(int id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM course WHERE ID = @id", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            mydb.openConnection();
            if ((command.ExecuteNonQuery()) == 1)
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



    }
}
