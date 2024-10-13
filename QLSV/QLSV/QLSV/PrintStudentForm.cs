using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml.Drawing.Vml;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using OfficeOpenXml;
using System.ComponentModel;
using OfficeOpenXml.Drawing;

namespace QLSV
{
    public partial class PrintStudentForm : Form
    {
        STUDENT student = new STUDENT();

        public PrintStudentForm()
        {
            InitializeComponent();
        }

        private void PrintStudentForm_Load(object sender, EventArgs e)
        {
            RadioButtonNo.Checked = true;
            // dieu khien cac radiobutton chuyen trang thai
            if (RadioButtonNo.Checked)
            {
                DateTimePicker1.Enabled = false;
                DateTimePicker1.Enabled = false;
            }

            SqlCommand command = new SqlCommand("SELECT * FROM std");
            fillGrid(command);
        }

        private void RadioButtonYes_CheckedChanged_1(object sender, EventArgs e)
        {
            DateTimePicker1.Enabled = true;
            DateTimePicker1.Enabled = true;
        }

        private void RadioButtonNo_CheckedChanged(object sender, EventArgs e)
        {
            DateTimePicker1.Enabled = false;
            DateTimePicker1.Enabled = false;
        }

        private void ButtonCheck_Click(object sender, EventArgs e)
        {
            SqlCommand command;
            String query;
            if (RadioButtonYes.Checked)
            {
                string date1 = DateTimePicker1.Value.ToString("yyyy-MM-dd");
                string date2 = DateTimePicker2.Value.ToString("yyyy-MM-dd");

                if (RadioButtonMale.Checked)
                {
                    query = "SELECT * FROM std WHERE gender = 'Male' AND bdate BETWEEN '" + date1 + " 'AND'" + date2 + "'";
                }
                else if (RadioButtonFemale.Checked)
                {
                    query = "SELECT * FROM std WHERE gender = 'Female' AND bdate BETWEEN '" + date1 + " 'AND' " + date2 + "'";
                }
                else
                {
                    query = "SELECT * FROM std WHERE bdate BETWEEN '" + date1 + " 'AND' " + date2 + "'";
                }

                command = new SqlCommand(query);
                fillGrid(command);
            }
            else
            {
                if (RadioButtonMale.Checked)
                {
                    query = "SELECT * FROM std WHERE gender = 'Male'";
                }
                else if (RadioButtonFemale.Checked)
                {
                    query = "SELECT * FROM std WHERE gender = 'Female'";
                }
                else
                {
                    query = "SELECT * FROM std";
                }

                command = new SqlCommand(query);
                fillGrid(command);

            }
        }

        public void fillGrid(SqlCommand command)
        {
            DataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            DataGridView1.RowTemplate.Height = 80;
            DataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)DataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            DataGridView1.AllowUserToAddRows = false;
        }


        private void ButtonSaveToTextFile_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\DELL\Documents\studentlist.txt";

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
                        // Kiểm tra nếu là cột ngày sinh
                        if (cell.ColumnIndex == 3 && cell.Value != null)
                        {
                            DateTime bdate;
                            if (DateTime.TryParse(cell.Value.ToString(), out bdate))
                            {
                                writer.Write(bdate.ToString("yyyy-MM-dd"));
                            }
                        }
                        else
                        {
                            writer.Write(cell.Value?.ToString() ?? ""); // Ghi giá trị của ô vào file
                        }
                        writer.Write("\t|"); // Thêm dấu cách giữa các cột
                    }
                    writer.WriteLine(); // Xuống dòng sau mỗi hàng
                    writer.WriteLine("------------------------------------------------------------------------------------");
                }
            }

            MessageBox.Show("Dữ liệu đã được lưu vào file.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = DataGridView1.Rows[e.RowIndex];
            string studentId = row.Cells["ID"].Value.ToString();
            string firstName = row.Cells["FName"].Value.ToString();
            string lastName = row.Cells["LName"].Value.ToString();
            DateTime birthDate = Convert.ToDateTime(row.Cells["Birthday"].Value);
            string gender = row.Cells["Gender"].Value.ToString();
            string phone = row.Cells["Phone"].Value.ToString();
            string address = row.Cells["Address"].Value.ToString();
            byte[] image = (byte[])row.Cells["Picture"].Value;

            bool isMale = gender == "Male";

            UpdateDeleteStudentForm edit_delete_student = new UpdateDeleteStudentForm();
            edit_delete_student.SetInfo(studentId, firstName, lastName, birthDate, isMale, phone, address, image);
            edit_delete_student.ShowDialog();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            string searchText = TextBoxSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                DataTable searchData = SearchStudent(searchText);
                if (searchData.Rows.Count > 0)
                {
                    DataGridView1.DataSource = searchData;
                }
                else
                {
                    MessageBox.Show("No matching students found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Display all students again if no results found
                    LoadStudentDetails();
                }
            }
            else
            {
                LoadStudentDetails(); // Display all students again if TextBoxSearch is empty
            }
        }

        public void LoadStudentDetails()
        {
            MY_DB mydb = new MY_DB();
            DataTable studentData = GetStudentDataById(mydb);
            DataGridView1.DataSource = studentData;
            mydb.closeConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM std");
            DataGridView1.ReadOnly = true;
            DataGridViewImageColumn picol = new DataGridViewImageColumn();
            DataGridView1.RowTemplate.Height = 80;
            DataGridView1.DataSource = student.getStudents(command);
            picol = (DataGridViewImageColumn)DataGridView1.Columns[7];
            picol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            DataGridView1.AllowUserToAddRows = false;
        }

        private DataTable GetStudentDataById(MY_DB mydb)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = mydb.getConnection())
            {
                connection.Open();

                string query = "SELECT ID, FName, LName, Birthday, Gender, Phone, Address, Picture FROM std";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        private DataTable SearchStudent(string searchText)
        {
            DataTable dataTable = new DataTable();
            MY_DB mydb = new MY_DB();

            using (SqlConnection connection = mydb.getConnection())
            {
                connection.Open();

                string query = "SELECT ID, FName, LName, Birthday, Gender, Phone, Address, Picture FROM std WHERE FName LIKE @searchText OR LName LIKE @searchText OR Address LIKE @searchText";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchText", "%" + searchText);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        private void ButtonSaveToExcel_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel file (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Save Excel File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Students");

                    // Ghi tiêu đề cột
                    for (int i = 1; i <= DataGridView1.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i].Value = DataGridView1.Columns[i - 1].HeaderText;
                    }

                    // Ghi dữ liệu từ DataGridView vào Excel
                    for (int i = 0; i < DataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < DataGridView1.Columns.Count; j++)
                        {
                            if (DataGridView1.Columns[j].Name == "Birthday" && DataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                DateTime birthday = Convert.ToDateTime(DataGridView1.Rows[i].Cells[j].Value);
                                worksheet.Cells[i + 2, j + 1].Value = birthday.ToString("yyyy-MM-dd");
                            }
                            else if (DataGridView1.Columns[j].Name == "Picture" && DataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                byte[] imageData = (byte[])DataGridView1.Rows[i].Cells[j].Value;
                                ExcelPicture picture = worksheet.Drawings.AddPicture("Image" + i.ToString(), new MemoryStream(imageData));
                                picture.SetPosition(i + 1, 0, 1, 0);
                            }
                            else
                            {
                                worksheet.Cells[i + 2, j + 1].Value = DataGridView1.Rows[i].Cells[j].Value;
                            }
                        }
                    }

                    // Lưu tệp Excel
                    package.SaveAs(new FileInfo(filePath));
                }

                MessageBox.Show("Dữ liệu đã được lưu vào file Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            StudentReportForm studentReportForm = new StudentReportForm();
            studentReportForm.ShowDialog();
        }
    }
}
