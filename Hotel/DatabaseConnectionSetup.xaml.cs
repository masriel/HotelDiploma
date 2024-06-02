using System;
using System.Windows;

using Hotel.Classes;
using Hotel.Pages;

namespace Hotel
{
    public partial class DatabaseConnectionSetup : Window
    {
        public event EventHandler ConfigSaved;

        public DatabaseConnectionSetup()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigData config = new ConfigData
            {
                Server = HostnameText.Text,
                Port = PortText.Text,
                Database = SchemaText.Text,
                Username = UsernameText.Text,
                Password = PasswordText.Password
            };

            ReadConfigFile configManager = new ReadConfigFile();
            configManager.SaveConfig(config);

            ConfigSaved?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}