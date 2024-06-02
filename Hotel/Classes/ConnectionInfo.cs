using System;
using System.Data;
using System.Windows;

using MySql.Data.MySqlClient;

namespace Hotel.Classes
{
    public class ConnectionInfo
    {
        public string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;

        public ConnectionInfo(string info)
        {
            CONNECTION_STRING = info;
        }

        public bool CheckConnection(string connectionString)
        {
            try
            {
                using (CONNECTION = new MySqlConnection(connectionString))
                {
                    CONNECTION.Open();
                }
                MessageBox.Show("Вы подключились к базе.", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"ОШИБКА:\n{e.Message}", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public DataTable GetData(string query)
        {
            DataTable dataTable = new DataTable();

            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
    }
}
