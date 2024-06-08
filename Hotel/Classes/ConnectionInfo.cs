using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Hotel.Classes
{
    public class ConnectionInfo
    {
        // Строка подключения к базе данных
        public string CONNECTION_STRING { get; private set; }

        // Конструктор, инициализирующий строку подключения
        public ConnectionInfo(string info)
        {
            CONNECTION_STRING = info;
        }

        // Метод для проверки подключения к базе данных
        public bool CheckConnection(string connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                }
                MessageBox.Show("Вы подключились к базе.", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"ОШИБКА:\n{e.Message}", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Метод для получения данных из базы данных
        public DataTable GetData(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var connection = new MySqlConnection(CONNECTION_STRING))
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            return dataTable;
        }

        // Метод для выполнения команд (вставка, обновление, удаление) в базе данных
        public void ExecuteCommand(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = new MySqlConnection(CONNECTION_STRING))
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для выполнения команды и получения ID вставленной записи
        public int ExecuteInsertAndGetId(string query, params MySqlParameter[] parameters)
        {
            int id = 0;
            try
            {
                using (var connection = new MySqlConnection(CONNECTION_STRING))
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return id;
        }
    }
}
