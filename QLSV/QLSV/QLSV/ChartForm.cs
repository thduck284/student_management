using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLSV
{
    public partial class ChartForm : Form
    {
        MY_DB mydb = new MY_DB();
        private float passCount = 0;
        private float failCount = 0;
        string type = "barchar";

        public ChartForm()
        {
            InitializeComponent();
        }

        private void buttonSwapChart_Click(object sender, EventArgs e)
        {
            PanelChart.Controls.Clear();

            if (type == "barchar")
            {
                type = "piechar";
                chart();
            }
            else
            {
                type = "barchar";
                chart();
            }
        }

        private void buttonStaticResult_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            StaticResultForm staticResultForm = new StaticResultForm();
            staticResultForm.ShowDialog();
            this.Close();
        }

        private void comboBoxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            PanelChart.Controls.Clear();
            chart();
        }

        private void chart()
        {
            int semester = Convert.ToInt32(comboBoxSemester.SelectedItem);
            DataTable dt = new DataTable();
            string query = "SELECT name, AVG(score) AS AVGScore " +
                           "FROM score " +
                           "WHERE semester = @semester " +
                           "GROUP BY name";
            using (SqlCommand command = new SqlCommand(query, mydb.getConnection()))
            {
                command.Parameters.AddWithValue("@semester", semester);

                mydb.openConnection();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }

                mydb.closeConnection();
            }

            if (type == "barchar")
            {
                comboBoxSemester.Enabled = true;
                Chart chart = new Chart();
                chart.Dock = DockStyle.Fill;
                PanelChart.Controls.Add(chart);
                ChartArea chartArea = new ChartArea();
                chart.ChartAreas.Add(chartArea);
                chart.Titles.Add("Average Score By Course");
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisY.Maximum = 10;

                var series = chart.Series.Add("Average Score By Course");
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                chart.Titles[0].Font = new Font("Arial", 16, FontStyle.Bold);
                chartArea.AxisX.Title = "Course";
                chartArea.AxisX.TitleFont = new Font("Arial", 14, FontStyle.Bold); 
                chartArea.AxisY.Title = "Score";
                chartArea.AxisY.TitleFont = new Font("Arial", 14, FontStyle.Bold);
                chartArea.AxisX.LabelStyle.Font = new Font("Arial", 12);
                chartArea.AxisY.LabelStyle.Font = new Font("Arial", 12);

                foreach (DataRow row in dt.Rows)
                {
                    string subjectName = row["name"].ToString();
                    double averageScore = Convert.ToDouble(row["AVGScore"]);

                    series.Points.AddXY($"{subjectName}", averageScore);
                }

                chart.ChartAreas[0].AxisX.Title = "Môn";
                chart.ChartAreas[0].AxisY.Title = "Điểm";
            }
            else
            {
                getPassFail();
                comboBoxSemester.Enabled = false;

                float totalCount = passCount + failCount;

                Chart pieChart = new Chart();
                pieChart.BackColor = Color.Transparent;
                pieChart.Size = new Size(700, 700);

                ChartArea chartArea = new ChartArea();
                pieChart.ChartAreas.Add(chartArea);

                Series series = new Series("Pie");
                series.ChartType = SeriesChartType.Pie;
                pieChart.Series.Add(series);

                series.Points.AddXY("PASS", passCount / totalCount * 100);
                series.Points.AddXY("FAIL", failCount / totalCount * 100);

                series.Points[0].Color = Color.Blue;
                series.Points[1].Color = Color.Red;

                int x = (PanelChart.Width - pieChart.Width) / 2;
                int y = (PanelChart.Height - pieChart.Height) / 2;
                pieChart.Location = new Point(x, y);

                series.Font = new Font("Arial", 12f, FontStyle.Bold);

                Title title = new Title("Pie Chart Result By Score", Docking.Top, new Font("Arial", 16, FontStyle.Bold), Color.Black);
                pieChart.Titles.Add(title);

                PanelChart.Controls.Add(pieChart);
            }
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

        private void ChartForm_Load(object sender, EventArgs e)
        {
            comboBoxSemester.Enabled = true;
        }
    }
}
