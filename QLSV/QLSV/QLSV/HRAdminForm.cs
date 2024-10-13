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

namespace QLSV
{
    public partial class HRAdminForm : Form
    {
        MY_DB mydb = new MY_DB();

        public HRAdminForm()
        {
            InitializeComponent();
            timer1.Interval = 5000;
            timer1.Tick += timer1_Tick;
        }

        private void HRAdminForm_Load(object sender, EventArgs e)
        {
            LoadData();
            timer1.Start();
        }

        private void LoadData()
        {
            try
            {
                mydb.openConnection();

                string query = "SELECT * FROM hr";
                SqlCommand command = new SqlCommand(query, mydb.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                mydb.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dataGridView1.RowTemplate.Height = 80;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
