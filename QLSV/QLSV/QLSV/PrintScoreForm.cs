using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;

namespace QLSV
{
    public partial class PrintScoreForm : Form
    {
        MY_DB mydb = new MY_DB();

        public PrintScoreForm()
        {
            InitializeComponent();
        }

        private void PrintScoreForm_Load(object sender, EventArgs e)
        {
            DataGridView1.DataSource = getDataScore();
        }

        private void ButtonToFile_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\DELL\Documents\courselist.txt";

            // Kiểm tra và tạo file nếu không tồn tại
            if (!File.Exists(path))
            {
                File.Create(path).Close(); // Đóng luồng sau khi tạo file
            }

            // Mở file để ghi
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Lặp qua từng dòng của DataGridView và ghi vào file
                foreach (DataGridViewRow row in DataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {

                        writer.Write(cell.Value?.ToString() ?? ""); // Ghi giá trị của ô vào file
                        writer.Write("\t|"); // Thêm dấu cách giữa các cột
                    }
                    writer.WriteLine(); // Xuống dòng sau mỗi hàng
                    writer.WriteLine("------------------------------------------------------------------------------------");
                }
            }

            MessageBox.Show("Dữ liệu đã được lưu vào file.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonPrint_Click(object sender, EventArgs e)
        {
            ScoreReportForm scoreReportForm = new ScoreReportForm();
            scoreReportForm.ShowDialog();
        }

        public DataTable getDataScore()
        {
            SqlCommand command = new SqlCommand("SELECT std.ID as [ID Student], std.FName as [First Name], std.LName as [Last Name], score.Score, score.Semester, score.Description FROM score LEFT JOIN std ON score.ID = std.ID");
            command.Connection = mydb.getConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        private void buttonToExcel_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "scorelist.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(saveFileDialog.FileName);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ScoreList");
                    int totalCols = DataGridView1.Columns.Count;
                    int totalRows = DataGridView1.Rows.Count;

                    for (int i = 1; i <= totalCols; i++)
                    {
                        worksheet.Cells[1, i].Value = DataGridView1.Columns[i - 1].HeaderText;
                    }

                    for (int i = 0; i < totalRows; i++)
                    {
                        for (int j = 0; j < totalCols; j++)
                        {
                            object cellValue = DataGridView1.Rows[i].Cells[j].Value;
                            worksheet.Cells[i + 2, j + 1].Value = cellValue != null ? cellValue.ToString() : "";
                        }
                    }

                    package.Save();
                }

                MessageBox.Show("Dữ liệu đã được lưu vào file Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
