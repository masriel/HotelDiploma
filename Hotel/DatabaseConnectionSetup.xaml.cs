using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

using Hotel.Classes;
using Hotel.Pages;

namespace Hotel
{
    public partial class DatabaseConnectionSetup : Window
    {
        private string FILE_PATH = Directory.GetCurrentDirectory().Replace("Hotel\\bin\\Debug", "db_config.json");
        private ConfigData _databaseConfig;

        private Navigation WINDOW_CHANGES;

        public DatabaseConnectionSetup()
        {
            InitializeComponent();
        }

        private void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(_databaseConfig);
            File.WriteAllText(FILE_PATH, json);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _databaseConfig.Server = HostnameText.Text;
            _databaseConfig.Port = PortText.Text;
            _databaseConfig.Database = SchemaText.Text;
            _databaseConfig.Username = UsernameText.Text;
            _databaseConfig.Password = PasswordText.Password;

            try
            {
                SaveConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"При сохранении данных возникла ошибка\n{ex.Message}\nПопробуйте записать данные вручную", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Настройки сохранены", "УСПЕХ", MessageBoxButton.OK, MessageBoxImage.Information);

            string conn_string = $"Server={_databaseConfig.Server};Port={_databaseConfig.Port};Database={_databaseConfig.Database};Uid={_databaseConfig.Username};Pwd={_databaseConfig.Password};";
            ConnectionInfo info = new ConnectionInfo(conn_string);
            if(!info.CheckConnection(conn_string)) return;

            WINDOW_CHANGES = new Navigation();
            WINDOW_CHANGES.OpenAsNewPage(new Login(), this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FILE_PATH))
            {
                string json = File.ReadAllText(FILE_PATH);
                _databaseConfig = JsonConvert.DeserializeObject<ConfigData>(json);
            }
            else
            {
                _databaseConfig = new ConfigData();
            }

            HostnameText.Text = _databaseConfig.Server;
            PortText.Text = _databaseConfig.Port;
            SchemaText.Text = _databaseConfig.Database;
            UsernameText.Text = _databaseConfig.Username;
            PasswordText.Password = _databaseConfig.Password;
        }
    }
}