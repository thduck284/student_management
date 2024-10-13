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
using System.Windows.Media.TextFormatting;

namespace QLSV
{
    public partial class ContactShowFullListForm : Form
    {
        MY_DB mydb = new MY_DB();

        public ContactShowFullListForm()
        {
            InitializeComponent();
        }

        private void ContactShowFullListForm_Load(object sender, EventArgs e)
        {
            DataTable groupName = new DataTable();
            groupName = getGroupName();

            foreach(DataRow row in groupName.Rows)
            {
                ListBoxGroup.Items.Add(row["Name"].ToString());
            }

            dataGridView1.RowTemplate.Height = 80;

            dataGridView1.DataSource = getAllContact();
        }

        private DataTable getGroupName()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT Name FROM [group]", mydb.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return table;
        }

        private DataTable getAllContact()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT IDStatic as ID, FName as [First Name], LName as [Last Name], IDGroup as [ID Group], GroupName as [Group Name], Type, Phone, Email, Address, Picture FROM contact", mydb.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return table;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count - 1)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["ID"].Value);
                if(isTeacher(id))
                {
                    string fname = dataGridView1.Rows[rowIndex].Cells["First Name"].Value.ToString();
                    string lname = dataGridView1.Rows[rowIndex].Cells["Last Name"].Value.ToString();
                    string fullname = fname + " " + lname;
                    ClassRoomForm classRoomForm = new ClassRoomForm(id, fullname);
                    classRoomForm.ShowDialog();
                }
            }
        }

        private bool isTeacher(int id)
        {
            mydb.openConnection();
            string query = "SELECT COUNT(*) FROM contact WHERE IDStatic = @id AND Type = @type";
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection());
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@type", "teacher");

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }
    }
}
