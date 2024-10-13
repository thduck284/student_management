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
using OfficeOpenXml;

namespace QLSV
{
    public partial class ClassRoomForm : Form
    {
        int id;
        string fullname;
        MY_DB mydb = new MY_DB();

        public ClassRoomForm(int id, string fullname)
        {
            InitializeComponent();
            this.id = id;
            this.fullname = fullname;
        }

        private void ClassRoomForm_Load(object sender, EventArgs e)
        {
            labelTitle.Text += fullname;
            string courseName = GetCourseName(id);
            labelMonHoc.Text += " " + courseName;
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

        private void buttonToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Scores");

                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;
                    }

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                        }
                    }

                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Excel Files|*.xlsx;*.xls";
                        sfd.FileName = "ScoresByTeacher.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            FileInfo fi = new FileInfo(sfd.FileName);
                            excelPackage.SaveAs(fi);
                            MessageBox.Show("Exported successfully!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            ClassRoomReportForm classRoomReportForm = new ClassRoomReportForm(id, fullname);
            classRoomReportForm.ShowDialog();
        }
    }
}
