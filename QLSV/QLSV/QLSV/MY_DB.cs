using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QLSV
{
    class MY_DB
    {
        private string connectionString = $"Data Source=DESKTOP-0Q6FFSV;Initial Catalog=myDB;Integrated Security=True;";
        private SqlConnection connection;

        public MY_DB()
        {
            connection = new SqlConnection(connectionString);
        }

        public SqlConnection getConnection()
        {
            return connection;
        }

        public void openConnection()
        {
            try
            {              
                connection.Open();

                Console.WriteLine("To connect to SQL Server successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred when opening the connection.: " + ex.Message);
            }
        }

        public void closeConnection()
        {
            try
            {
                // Đóng kết nối
                connection.Close();

                Console.WriteLine("The SQL Server connection has been closed..");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred when closing the connection.: " + ex.Message);
            }
        }
    }
}
