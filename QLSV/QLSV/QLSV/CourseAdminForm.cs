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
using Microsoft.ReportingServices.Diagnostics.Internal;

namespace QLSV
{
    public partial class CourseAdminForm : Form
    {
        MY_DB mydb = new MY_DB();

        public CourseAdminForm()
        {
            InitializeComponent();
            timer1.Interval = 5000;
            timer1.Tick += timer1_Tick;
        }

        private void CourseAdminForm_Load(object sender, EventArgs e)
        {
            LoadData();
            timer1.Start();
        }

        private void LoadData()
        {
            mydb.openConnection();

            string query = "SELECT * FROM course";
            SqlCommand command = new SqlCommand(query, mydb.getConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            mydb.closeConnection();
            dataGridView1.DataSource = dataTable;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ManageCouresesForm manageCouresesForm = new ManageCouresesForm();
            manageCouresesForm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
