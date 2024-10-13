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
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Font = System.Drawing.Font;
using OfficeOpenXml;

namespace QLSV
{
    public partial class CourseStudentListForm : Form
    {
        MY_DB mydb = new MY_DB();

        public CourseStudentListForm()
        {
            InitializeComponent();
        }

        private void CourseStudentListForm_Load(object sender, EventArgs e)
        {
            loadSemester();
            ComboBoxCourse.Enabled = false;
        }

        private void ComboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<object> itemsToRemove = new List<object>();

            foreach (var item in ComboBoxCourse.Items)
            {
                itemsToRemove.Add(item);
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                ComboBoxCourse.Items.Remove(itemToRemove);
            }

            ComboBoxCourse.Enabled = true;
            DataTable dt = new DataTable();
            dt = getAllCourses();
            foreach(DataRow item in dt.Rows)
            {
                ComboBoxCourse.Items.Add(item["Name"].ToString());
            }
        }

        private DataTable getInforStudent()
        {
            SqlCommand command = new SqlCommand("SELECT std.ID, std.FName, std.LName, std.Birthday FROM std, coursestd WHERE std.ID = coursestd.ID and coursestd.Semester = @semester and coursestd.Name = @name", mydb.getConnection());
            command.Parameters.AddWithValue("@semester", Convert.ToInt32(ComboBoxSemester.SelectedItem));
            command.Parameters.AddWithValue("@name", ComboBoxCourse.SelectedItem.ToString());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        void loadSemester()
        {
            ComboBoxSemester.Items.Add("1");
            ComboBoxSemester.Items.Add("2");
            ComboBoxSemester.Items.Add("3");
        }

        private DataTable getAllCourses()
        {
            if (ComboBoxSemester.SelectedItem == null || !int.TryParse(ComboBoxSemester.SelectedItem.ToString(), out int semester))
            {
                // Nếu không, trả về null hoặc thực hiện hành động phù hợp với trường hợp này
                MessageBox.Show("Please select a valid semester.", "Invalid Semester", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT Name FROM course WHERE Semester = @semester";
            command.Parameters.AddWithValue("@semester", semester); // Sử dụng AddWithValue thay vì Add để thêm tham số

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        private void ComboBoxCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getInforStudent();
            dataGridView1.Columns["ID"].Width = 100;
            dataGridView1.Columns["FName"].Width = 200;
            dataGridView1.Columns["LName"].Width = 125;
            dataGridView1.Columns["Birthday"].Width = 200;
        }

        private void ButtonPrint_Click(object sender, EventArgs e)
        {
            /*SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12);

                // Mở tài liệu
                doc.Open();

                // Tạo bảng
                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.GetType() == typeof(byte[]))
                        {
                            // Nếu cell chứa ảnh, thêm ảnh vào bảng
                            byte[] imageData = (byte[])cell.Value;
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageData);
                            PdfPCell pdfCell = new PdfPCell(img, true);
                            table.AddCell(pdfCell);
                        }
                        else
                        {
                            cell.Style.Font = new Font("Arial", 10);
                            // Nếu cell chứa văn bản, thêm văn bản vào bảng
                            string cellText = cell.Value?.ToString() ?? "";
                            PdfPCell pdfCell = new PdfPCell(new Phrase(cellText, font)); // Sử dụng font cho văn bản
                            table.AddCell(pdfCell);
                        }
                    }
                }
                // Thêm bảng vào tài liệu
                doc.Add(table);

                // Đóng tài liệu
                doc.Close();

                MessageBox.Show("Dữ liệu đã được lưu vào file PDF.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel File|*.xlsx";
            saveFileDialog1.Title = "Save Course Data";
            saveFileDialog1.FileName = "dshocsinh.xlsx";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Set LicenseContext
                    OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Ds hoc sinh dang ki mon");

                        // Set column headers directly
                        worksheet.Cells[1, 1].Value = "MSSV";
                        worksheet.Cells[1, 2].Value = "Họ và Tên";
                        worksheet.Cells[1, 3].Value = " ";
                        worksheet.Cells[1, 4].Value = "Ngày Sinh";

                        // Bôi đậm và căn giữa các cột
                        using (ExcelRange range = worksheet.Cells[1, 1, 1, 4])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Căn giữa
                        }

                        // Đổ dữ liệu từ DataGridView vào Excel
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                if (j == 3)
                                {
                                    DateTime birthdate = (DateTime)dataGridView1.Rows[i].Cells[j].Value;
                                    worksheet.Cells[i + 2, j + 1].Value = birthdate.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    if (j < 3)
                                    {
                                        worksheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                                    }
                                    else
                                    {
                                        worksheet.Cells[i + 2, j + 2].Value = dataGridView1.Rows[i].Cells[j].Value;
                                    }
                                }
                            }
                        }

                        // điều chỉnh độ rộng của các cột 
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //thêm viền
                        using (ExcelRange range = worksheet.Cells[1, 1, dataGridView1.Rows.Count, dataGridView1.Columns.Count])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        FileInfo excelFile = new FileInfo(saveFileDialog1.FileName);
                        excelPackage.SaveAs(excelFile);
                    }

                    MessageBox.Show("Đã lưu file thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving course data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }
    }
}
