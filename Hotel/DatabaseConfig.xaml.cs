using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace Hotel
{
    public partial class DatabaseConfig : Window
    {
        public DatabaseConfig()
        {
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var connectionData = new ConnectionData
            {
                Server = HostnameText.Text,
                Port = PortText.Text,
                Database = SchemaText.Text,
                Username = UsernameText.Text,
                Password = PasswordText.Password
            };
            
            string jsonData = JsonConvert.SerializeObject(connectionData, Formatting.Indented);

            try
            {
                File.WriteAllText("db_config.json", jsonData);
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public class ConnectionData
        {
            public string Server { get; set; }
            public string Port { get; set; }
            public string Database { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}