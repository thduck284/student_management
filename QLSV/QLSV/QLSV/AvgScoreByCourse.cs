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
using System.Xml.Linq;

namespace QLSV
{
    public partial class AvgScoreByCourse : Form
    {
        MY_DB mydb = new MY_DB();

        public AvgScoreByCourse()
        {
            InitializeComponent();
        }

        private void AvgScoreByCourse_Load(object sender, EventArgs e)
        {
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.DataSource = avgScore();
            dataGridView1.Columns["Name"].Width = 150;
            dataGridView1.Columns["AvgScore"].Width = 250;
        }

        private DataTable avgScore()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT Name, AVG(score.Score) As AvgScore FROM score GROUP BY Name";

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}
