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
    public partial class ResultReportForm : Form
    {
        MY_DB mydb = new MY_DB();

        public ResultReportForm()
        {
            InitializeComponent();
        }

        private void ResultReportForm_Load(object sender, EventArgs e)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            headerStyle.Font = new Font("Arial", 10);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Blue;
            dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;
            dataGridView1.ReadOnly = true;
            DataTable dt = new DataTable();
            dt = inforStudent();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                float sum = 0;
                int count = 0;
                for (int j = 3; j < dt.Columns.Count - 2; j++)
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = mydb.getConnection();
                    command.CommandText = "SELECT AVG(Score) as avg FROM score WHERE ID = @id AND Name = @name";
                    command.Parameters.AddWithValue("@name", dt.Columns[j].ColumnName);
                    command.Parameters.AddWithValue("@id", dt.Rows[i][0]);

                    // Mở kết nối database
                    mydb.openConnection();

                    // Thực thi truy vấn và lấy kết quả
                    object result = command.ExecuteScalar();

                    // Đóng kết nối database
                    mydb.closeConnection();

                    // Kiểm tra nếu kết quả không rỗng
                    if (result != null && result != DBNull.Value)
                    {
                        // Gán giá trị trung bình vào DataTable
                        dt.Rows[i][j] = result;
                        sum += Convert.ToSingle(result);
                        count++;
                    }
                    else
                    {
                        sum += 0;
                    }
                }
                dt.Rows[i][dt.Columns.Count - 2] = sum / count;
                if (sum/count < 5)
                {
                    dt.Rows[i][dt.Columns.Count - 1] = "FAIL";
                }
                else
                {
                    dt.Rows[i][dt.Columns.Count - 1] = "PASS";
                }
            }

            dataGridView1.DataSource = dt;
        }

        private DataTable inforStudent()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT ID as [Student ID], FName as [First Name], LName as [Last Name] FROM std";

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            DataTable tablecourse = new DataTable();
            tablecourse = getCourse();

            foreach (DataRow item in tablecourse.Rows)
            {
                table.Columns.Add(new DataColumn(item["Name"].ToString(), typeof(float)));
            }

            table.Columns.Add(new DataColumn("Average Score", typeof(float)));
            table.Columns.Add(new DataColumn("Result", typeof(string)));

            return table;
        }

        private DataTable getCourse()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT DISTINCT Name FROM score";

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}
