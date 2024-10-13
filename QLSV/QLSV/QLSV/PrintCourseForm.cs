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
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;

namespace QLSV
{
    public partial class PrintCourseForm : Form
    {
        COURSE course = new COURSE();

        public PrintCourseForm()
        {
            InitializeComponent();
        }

        private void PrintCourseForm_Load(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM course");
            fillGrid(command);
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

        public void fillGrid(SqlCommand command)
        {
            DataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            DataGridView1.RowTemplate.Height = 80;
            DataGridView1.DataSource = course.getDataCourses();
            foreach (DataGridViewColumn column in DataGridView1.Columns)
            {
                column.Width = 200; 
            }

            DataGridView1.AllowUserToAddRows = false;
        }

        private void ButtonPrint_Click(object sender, EventArgs e)
        {
            CourseReportForm courseReportForm = new CourseReportForm();
            courseReportForm.ShowDialog();
        }

        private void buttonToExcel_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "courselist.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(saveFileDialog.FileName);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("CourseList");
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
                            worksheet.Cells[i + 2, j + 1].Value = DataGridView1.Rows[i].Cells[j].Value.ToString();
                        }
                    }

                    package.Save();
                }

                MessageBox.Show("Dữ liệu đã được lưu vào file Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
