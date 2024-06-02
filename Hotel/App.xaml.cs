using System;
using System.Linq;
using System.Windows;

using Hotel.Classes;
using Hotel.Pages;

namespace Hotel
{
    public partial class App : Application
    {
        private ReadConfigFile readConfigFile = new ReadConfigFile();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigData config = readConfigFile.LoadConfig();
            if (config == null)
            {
                OpenDatabaseConnectionSetup();
            }
            else
            {
                OpenLoginWindow();
            }
        }

        public void OpenDatabaseConnectionSetup()
        {
            DatabaseConnectionSetup setupForm = new DatabaseConnectionSetup();
            setupForm.ConfigSaved += OnConfigSaved;
            setupForm.Show();
        }

        public void OpenLoginWindow()
        {
            Login login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
        }

        private void OnConfigSaved(object sender, EventArgs e)
        {
            (sender as Window)?.Close();
            OpenLoginWindow();
        }
    }
}
