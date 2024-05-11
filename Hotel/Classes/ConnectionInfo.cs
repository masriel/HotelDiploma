using System;
using System.Configuration;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Hotel.Classes
{
    public class ConnectionInfo
    {
        public string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;

        public ConnectionInfo(string info) { 
            CONNECTION_STRING = info;
        }

        public void CheckConnection(string connectionString)
        {
            try
            {
                using (CONNECTION = new MySqlConnection(connectionString))
                {
                    CONNECTION.Open();
                }
                MessageBox.Show("Вы подключились к базе.", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Information);

                ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString = connectionString;
            }
            catch (Exception e)
            {
                MessageBox.Show($"ОШИБКА:\n{e.Message}", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
