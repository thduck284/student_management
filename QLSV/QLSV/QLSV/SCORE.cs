using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV
{
    internal class SCORE
    {
        MY_DB mydb = new MY_DB();

        public bool insertScore(int studentID, int semester, string name, float score, string description)
        {
            SqlCommand command = new SqlCommand("INSERT INTO score (ID, Name, Semester, Score, Description) VALUES (@id, @name, @semester, @score, @descr)", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = studentID;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            command.Parameters.Add("@score", SqlDbType.Float).Value = score;
            command.Parameters.Add("@semester", SqlDbType.Int).Value = semester;
            command.Parameters.Add("@descr", SqlDbType.NVarChar).Value = description;

            mydb.openConnection();

            if (command.ExecuteNonQuery() == 1)
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

        // Kiểm tra xem điểm của sinh viên trong một khóa học đã tồn tại chưa
        public bool studentScoreExist(int studentId, int courseID)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM score WHERE ID = @sid AND Course_ID = @cid", mydb.getConnection());
            command.Parameters.Add("@sid", SqlDbType.Int).Value = studentId;
            command.Parameters.Add("@cid", SqlDbType.Int).Value = courseID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return (table.Rows.Count > 0);
        }

        // Lấy điểm trung bình theo khóa học
        public DataTable getAvgScoreByCourse(int id, int semester, string name)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT ID, Semester, Name, AVG(score.Score) As AvgScore FROM score WHERE ID = @id and Semester =  @semester and Name = @name GROUP BY ID, Semester, Name";
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@semester", SqlDbType.Int).Value = semester;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        // Xóa điểm của sinh viên trong một khóa học
        public bool deleteScore(int studentID, int courseID)
        {
            SqlCommand command = new SqlCommand("DELETE FROM score WHERE id = @sid AND Course_ID = @cid", mydb.getConnection());
            command.Parameters.Add("@sid", SqlDbType.Int).Value = studentID;
            command.Parameters.Add("@cid", SqlDbType.Int).Value = courseID;

            mydb.openConnection();
            int rowsAffected = command.ExecuteNonQuery();
            mydb.closeConnection();

            return (rowsAffected > 0);
        }

    }

}
