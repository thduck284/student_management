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
    public partial class SelectContactForm : Form
    {
        MY_DB mydb = new MY_DB();

        public SelectContactForm()
        {
            InitializeComponent();
        }

        private void SelectContactForm_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT IDStatic as [ID Contact], FName as [First Name], LName as [Last Name], IDGroup as [ID Group] FROM contact", mydb.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }
    }
}
