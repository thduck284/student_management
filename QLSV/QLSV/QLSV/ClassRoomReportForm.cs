using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class ClassRoomReportForm : Form
    {
        MY_DB mydb = new MY_DB();
        int id;
        string tengv;

        public ClassRoomReportForm(int id, string fullname)
        {
            InitializeComponent();
            this.id = id;
            tengv = fullname;
        }

        private void ClassRoomReportForm_Load(object sender, EventArgs e)
        {
            labelTenGV.Text = tengv;
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            headerStyle.Font = new Font("Arial", 14);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Blue;
            dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;
            string courseName = GetCourseName(id);
            getData(courseName);
        }

        private string GetCourseName(int id)
        {
            string courseName = "";

            try
            {
                mydb.openConnection();

                string query = "SELECT DISTINCT Name FROM course WHERE ID = @id";

                SqlCommand command = new SqlCommand(query, mydb.getConnection());
                command.Parameters.AddWithValue("@id", id);

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    courseName = result.ToString();
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

            return courseName;
        }

        private void getData(string courseName)
        {
            DataTable scoresTable = new DataTable();

            try
            {
                mydb.openConnection();

                string scoreQuery = "SELECT std.ID as [ID Student], std.FName as [First Name], " +
                            "std.LName as [Last Name], score.Name as [Name Subject], AVG(score.Score) as Score " +
                            "FROM score " +
                            "JOIN std ON score.ID = std.ID " +
                            "WHERE score.Name = @name " +
                            "GROUP BY std.ID, std.FName, std.LName, score.Name ";

                SqlDataAdapter adapter = new SqlDataAdapter(scoreQuery, mydb.getConnection());
                adapter.SelectCommand.Parameters.AddWithValue("@name", courseName);

                adapter.Fill(scoresTable);
                dataGridView1.DataSource = scoresTable;

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
    }
}
