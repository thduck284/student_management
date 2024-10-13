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

namespace QLSV
{
    public partial class StudentReportForm : Form
    {
        STUDENT student = new STUDENT();

        public StudentReportForm()
        {
            InitializeComponent();
        }

        private void StudentReportForm_Load(object sender, EventArgs e)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            headerStyle.Font = new Font("Arial", 14);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Blue;
            dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;
            SqlCommand command = new SqlCommand("SELECT * FROM std");
            this.reportViewer1.RefreshReport();
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns[7].Width = 100;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                byte[] imageData = (byte[])row.Cells[7].Value;
                if (imageData != null && imageData.Length > 0)
                {
                    Image image;
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        image = Image.FromStream(ms);
                    }

                    Image resizedImage = ResizeImage(image, new Size(60, 60));

                    row.Cells[7].Value = resizedImage;
                }
            }
        }

        private Image ResizeImage(Image image, Size size)
        {
            return new Bitmap(image, size);
        }
    }
}
