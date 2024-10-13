using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;

namespace QLSV
{
    public partial class StudentListForm : Form
    {
        STUDENT student = new STUDENT();
        bool x;

        public StudentListForm()
        {
            InitializeComponent();

        }

        public void LoadStudentDetails()
        {
            MY_DB mydb = new MY_DB();
            DataTable studentData = GetStudentDataById(mydb);
            dataGridView1.DataSource = studentData;
            mydb.closeConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM std");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            picol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
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

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            x = true;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Đảm bảo có ít nhất 7 cột trong DataGridView
            if (dataGridView1.Columns.Count >= 7)
            {
                // Kiểm tra nếu cột thứ 7 là một cột hình ảnh
                if (dataGridView1.Columns[6] is DataGridViewImageColumn)
                {
                    DataGridViewImageColumn picol = (DataGridViewImageColumn)dataGridView1.Columns[6];
                    picol.ImageLayout = DataGridViewImageCellLayout.Stretch;
                }
            }

            SqlCommand command = new SqlCommand("SELECT * FROM std");
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(x)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
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
        }

        private void ButtonRefresh_KeyDown(object sender, KeyEventArgs e)
        {           
            if (e.Control && e.KeyCode == Keys.F)
            {
                // Xử lý khi nhấn Ctrl + F
                // ...
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ReadExcel(string filePath)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    // Đọc tiêu đề từ dòng đầu tiên
                    for (int col = 1; col <= colCount; col++)
                    {
                        // Kiểm tra xem ô đầu tiên trong cột có giá trị không
                        if (worksheet.Cells[1, col].Value != null)
                        {
                            // Nếu có giá trị, thêm cột vào DataGridView
                            dataGridView1.Columns.Add("Column" + col, worksheet.Cells[1, col].Value?.ToString());
                        }
                    }

                    // Bắt đầu đọc từ dòng thứ hai
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataGridViewRow dgvRow = new DataGridViewRow();
                        bool hasData = false;
                        for (int col = 1; col <= colCount; col++)
                        {
                            if (worksheet.Cells[row, col].Value != null)
                            {
                                hasData = true;
                                if ((col == 4 || col == 5) && InvalidCharacters(worksheet.Cells[row, col].Value.ToString()))
                                {
                                    MessageBox.Show("Error Excel, in row " + (row - 1).ToString() + ", col " + (col - 1).ToString());
                                    return;
                                }
                                else if (!int.TryParse(worksheet.Cells[row, col].Value.ToString(), out _) && col == 1)
                                {
                                    MessageBox.Show("Error Excel, in row " + (row - 1).ToString() + ", col " + (col).ToString());
                                    return;
                                }
                                else if (col == 3 && !IsNumeric(worksheet.Cells[row, col].Value.ToString()))
                                {
                                    MessageBox.Show("Error Excel, in row " + (row - 1).ToString() + ", col " + (col - 1).ToString());
                                    return;
                                }
                                else
                                {
                                    dgvRow.Cells.Add(new DataGridViewTextBoxCell { Value = worksheet.Cells[row, col].Value });
                                }

                                
                            }
                            if(row > 1 && col == 6)
                            {
                                string email = worksheet.Cells[row, 3].Value + "@student.hcmute.edu.vn";
                                dgvRow.Cells.Add(new DataGridViewTextBoxCell { Value = email});
                            }
                        }

                        if (hasData)
                        {
                            dataGridView1.Rows.Add(dgvRow);
                        }
                    }
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading Excel file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool InvalidCharacters(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    // Nếu ký tự không phải là một số, trả về false
                    return false;
                }
            }
            // Nếu tất cả các ký tự là số, trả về true
            return true;
        }

        private void ButtonImport_Click_1(object sender, EventArgs e)
        {
            x = false;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();            

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                // Thiết lập LicenseContext trước khi sử dụng EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // hoặc LicenseContext.Commercial, tùy thuộc vào giấy phép bạn sử dụng

                ReadExcel(filePath);
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void readExcel(string filePath)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    // Đọc tiêu đề từ dòng đầu tiên
                    for (int col = 1; col <= colCount; col++)
                    {
                        dataGridView1.Columns.Add("Column" + col, worksheet.Cells[1, col].Value?.ToString());
                    }

                    // Bắt đầu đọc từ dòng thứ hai
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataGridViewRow dgvRow = new DataGridViewRow();
                        bool hasData = false;
                        for (int col = 1; col <= colCount; col++)
                        {
                            if (worksheet.Cells[row, col].Value != null)
                            {
                                hasData = true;
                                dgvRow.Cells.Add(new DataGridViewTextBoxCell { Value = worksheet.Cells[row, col].Value });
                            }
                        }
                        if (hasData)
                        {
                            dataGridView1.Rows.Add(dgvRow);
                        }
                    }
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading Excel file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
