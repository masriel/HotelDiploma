using System;
using System.Resources;
using System.Collections;
using System.IO;
using System.Windows;
using MySql.Data.MySqlClient;
using Resx.Resources;

namespace Hotel.Classes
{
    public class ConnectionInfo
    {
        private ReadConfigFile CONFIG_FILE;
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
    }
}
