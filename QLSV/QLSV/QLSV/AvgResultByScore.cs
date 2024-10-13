using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using OfficeOpenXml;

namespace QLSV
{
    public partial class AvgResultByScore : Form
    {
        MY_DB mydb = new MY_DB();

        public AvgResultByScore()
        {
            InitializeComponent();
        }

        private void AvgResultByScore_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = avgScore();         
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

            foreach(DataRow item in tablecourse.Rows)
            {
                table.Columns.Add(new DataColumn(item["Name"].ToString(), typeof(float)));
            }

            table.Columns.Add(new DataColumn("Average Score", typeof(float)));
            table.Columns.Add(new DataColumn("Result", typeof(string)));

            return table;
        }

        private DataTable avgScore()
        {
            DataTable dt = new DataTable ();
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

                dt.Rows[i][dt.Columns.Count - 2] = sum /count;
                if (sum / count > 5)
                {
                    dt.Rows[i][dt.Columns.Count - 1] = "PASS";
                }
                else
                {
                    dt.Rows[i][dt.Columns.Count - 1] = "FAIL";
                }
            }

            return dt;
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            string input = TextBoxSearch.Text;
            DataTable dt = new DataTable();
            dt = avgScore();

            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Student ID", typeof(int)));
            table.Columns.Add(new DataColumn("First Name", typeof(string)));
            table.Columns.Add(new DataColumn("Last Name", typeof(string)));
            DataTable tablecourse = new DataTable();
            tablecourse = getCourse();

            foreach (DataRow item in tablecourse.Rows)
            {
                table.Columns.Add(new DataColumn(item["Name"].ToString(), typeof(float)));
            }
            table.Columns.Add(new DataColumn("Average Score", typeof(float)));
            table.Columns.Add(new DataColumn("Result", typeof(string)));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == input || sameFName(dt.Rows[i][1].ToString(), input))
                {
                    table.ImportRow(dt.Rows[i]);
                }
            }

            if(table.Rows.Count == 0)
            {
                MessageBox.Show("Can not Find out");
            }
            else
            {
                dataGridView1.DataSource = table;
            }
            
        }

        private bool sameFName(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            if (m > n) return false;
            int count = 0;

            for(int i = 0; i < m; i++)
            {
                if (s[i] == t[i]) count++;
            }

            return (count == m) ? true : false;
        }

        private void ButtonPrint_Click(object sender, EventArgs e)
        {
            ResultReportForm resultReportForm = new ResultReportForm();
            resultReportForm.ShowDialog();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public float staticByResult()
        {
            DataTable dt = new DataTable();
            dt = avgScore();
            float pass = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pass += (dt.Rows[i][8].ToString() == "PASS") ? 1 : 0;
            }

            return pass * 100 / dt.Rows.Count;
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

        private void buttonToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "AverageResults.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                FileInfo file = new FileInfo(saveFileDialog.FileName);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Average Results");

                    DataTable dt = (DataTable)dataGridView1.DataSource;

                    // Copy DataTable to Excel
                    for (int i = 1; i <= dt.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i].Value = dt.Columns[i - 1].ColumnName;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j];
                        }
                    }

                    package.Save();
                }

                MessageBox.Show("Data exported to Excel successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
