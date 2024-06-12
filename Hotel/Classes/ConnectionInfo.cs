using System;
using System.Data;
using System.Windows;

using MySql.Data.MySqlClient;

namespace Hotel.Classes
{
    public class ConnectionInfo
    {
        // Строка подключения к базе данных
        public string ConnectionString { get; private set; }

        // Конструктор, инициализирующий строку подключения
        public ConnectionInfo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        // Метод для проверки подключения к базе данных
        public bool CheckConnection()
        {
            try
            {
                // Создание и открытие подключения
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                }
                // Сообщение об успешном подключении
                MessageBox.Show("Вы подключились к базе.", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception e)
            {
                // Сообщение об ошибке подключения
                MessageBox.Show($"ОШИБКА:\n{e.Message}", "ПОДКЛЮЧЕНИЕ К БАЗЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Метод для получения данных из базы данных
        public DataTable GetData(string query)
        {
            var dataTable = new DataTable();

            try
            {
                // Создание подключения и команды для выполнения запроса
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        // Использование адаптера для заполнения DataTable
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Сообщение об ошибке выполнения запроса
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
                // Создание подключения и команды для выполнения запроса
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Добавление параметров к команде, если они есть
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        connection.Open();
                        // Выполнение команды
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Сообщение об ошибке выполнения команды
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для выполнения команды и получения ID вставленной записи
        public int ExecuteInsertAndGetId(string query, params MySqlParameter[] parameters)
        {
            int id = 0;
            try
            {
                // Создание подключения и команды для выполнения запроса
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Добавление параметров к команде, если они есть
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        connection.Open();
                        // Выполнение команды и получение вставленного ID
                        id = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                // Сообщение об ошибке выполнения команды
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return id;
        }
    }
}
