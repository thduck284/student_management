using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class StaticResultForm : Form
    {
        MY_DB mydb = new MY_DB();
        int passCount;
        int failCount;

        public StaticResultForm()
        {
            InitializeComponent();
        }

        private void StaticResultForm_Load(object sender, EventArgs e)
        {
            getPassFail();
            int total = passCount + failCount;
            label3.Text = "C#: " + avgScore("C#").ToString();
            label4.Text = "Java: " + avgScore("Java").ToString();
            label5.Text = "Cloud Computing: " + avgScore("Cloud Computing").ToString();
            label6.Text = "Machine Learning: " + avgScore("Machine Learning").ToString();
            label7.Text = "PASS: " + (passCount * 100/total).ToString();
            label8.Text = "FAIL: " + (failCount * 100/total).ToString();           
        } 

        private float avgScore(string subject)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT AVG(Score) As AvgScore FROM score WHERE Name = @name";
            command.Parameters.AddWithValue("name", subject);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return Convert.ToSingle(table.Rows[0]["AvgScore"]);
        }

        private void buttonChart_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ChartForm charForm = new ChartForm();
            charForm.ShowDialog();
            this.Close();
        }


        private void getPassFail()
        {
            DataTable dt = new DataTable();
            dt = InforStudent();
            passCount = 0;
            failCount = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                float sum = 0;
                int count = 0;
                for (int j = 3; j < dt.Columns.Count - 2; j++)
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = mydb.getConnection();
                    command.CommandText = "SELECT AVG(Score) as avg FROM score WHERE ID = @id AND Name = @name";
                    command.Parameters.AddWithValue("@name", dt.Columns[j].ColumnName);
                    command.Parameters.AddWithValue("@id", dt.Rows[i][0]);

                    mydb.openConnection();

                    object result = command.ExecuteScalar();

                    mydb.closeConnection();

                    if (result != null && result != DBNull.Value)
                    {
                        dt.Rows[i][j] = result;
                        sum += Convert.ToSingle(result);
                        count++;
                    }
                    else
                    {
                        sum += 0;
                    }
                }

                dt.Rows[i][dt.Columns.Count - 2] = sum / count;
                if (sum / count > 5)
                {
                    passCount++;
                }
                else
                {
                    failCount++;
                }
            }
        }

        private DataTable InforStudent()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT ID as [Student ID], FName as [First Name], LName as [Last Name] FROM std";

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            DataTable tablecourse = new DataTable();
            tablecourse = getCourse();

            foreach (DataRow item in tablecourse.Rows)
            {
                table.Columns.Add(new DataColumn(item["Name"].ToString(), typeof(float)));
            }

            table.Columns.Add(new DataColumn("Average Score", typeof(float)));
            table.Columns.Add(new DataColumn("Result", typeof(string)));

            return table;
        }

        private DataTable getCourse()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = mydb.getConnection();
            command.CommandText = "SELECT DISTINCT Name FROM score";

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}
