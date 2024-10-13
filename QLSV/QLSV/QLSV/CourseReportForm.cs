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
    public partial class CourseReportForm : Form
    {
        MY_DB myDB = new MY_DB();

        public CourseReportForm()
        {
            InitializeComponent();
        }

        private void CourseReportForm_Load(object sender, EventArgs e)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            headerStyle.Font = new Font("Arial", 14);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Blue;
            dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;

            try
            {
                myDB.openConnection();

                SqlCommand command = new SqlCommand("SELECT DISTINCT * FROM course", myDB.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi Cơ sở dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myDB.closeConnection();
            }

            dataGridView1.AllowUserToAddRows = false;
            
        }
    }
}
