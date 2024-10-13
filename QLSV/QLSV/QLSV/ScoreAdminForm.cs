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
    public partial class ScoreAdminForm : Form
    {
        MY_DB mydb = new MY_DB();

        public ScoreAdminForm()
        {
            InitializeComponent();
        }

        private void ScoreAdminForm_Load(object sender, EventArgs e)
        {
            mydb.openConnection();

            string query = "SELECT * FROM score";
            SqlCommand command = new SqlCommand(query, mydb.getConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            mydb.closeConnection();
            dataGridView1.DataSource = dataTable;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ManageScoreForm manageScoreForm = new ManageScoreForm();
            manageScoreForm.ShowDialog();
        }        
    }
}
