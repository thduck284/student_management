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
    public partial class ScoreReportForm : Form
    {
        MY_DB mydb = new MY_DB();

        public ScoreReportForm()
        {
            InitializeComponent();
        }

        private void ScoreReportForm_Load(object sender, EventArgs e)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            headerStyle.Font = new Font("Arial", 14);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Blue;
            dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;
            dataGridView1.ReadOnly = true;
            SqlCommand command = new SqlCommand("SELECT std.ID as [ID Student], std.FName as [First Name], std.LName as [Last Name], score.Score, score.Semester, score.Description FROM score LEFT JOIN std ON score.ID = std.ID");
            command.Connection = mydb.getConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            dataGridView1.DataSource = table;
        }
    }
}
