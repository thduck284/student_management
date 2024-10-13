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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
    public partial class RemoveScoreForm : Form
    {
        MY_DB mydb = new MY_DB();
        int id = -1;
        int semester = -1;
        string name = "";

        public RemoveScoreForm()
        {
            InitializeComponent();
        }

        private void RemoveScoreForm_Load(object sender, EventArgs e)
        {
            loadDataScore();
        }

        private void ButtonRemoveScore_Click(object sender, EventArgs e)
        {
            if(name == "")
            {
                MessageBox.Show("Please Click Content which you need to Remove", "Remove Score", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand command = new SqlCommand("DELETE FROM score WHERE ID = @id AND Semester = @semester AND Name = @name", mydb.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@semester", SqlDbType.Int).Value = semester;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;

            mydb.openConnection();
            int rowsAffected = command.ExecuteNonQuery();
            mydb.closeConnection();
            loadDataScore();
        }

        void loadDataScore()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM score");
            dataGridView1.ReadOnly = true;
            command.Connection = mydb.getConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            dataGridView1.Columns["ID"].Width = 150; 
            dataGridView1.Columns["Name"].Width = 150; // Đặt độ rộng của cột Name là 150px
            dataGridView1.Columns["Semester"].Width = 80; // Đặt độ rộng của cột Semester là 80px
            dataGridView1.Columns["Score"].Width = 60; // Đặt độ rộng của cột Score là 60px
            dataGridView1.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            id = Convert.ToInt32(row.Cells["ID"].Value);
            name = row.Cells["Name"].Value.ToString();
            semester = Convert.ToInt32(row.Cells["Semester"].Value);
        }
    }
}
