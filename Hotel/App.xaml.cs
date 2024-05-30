using Hotel.Classes;
using Hotel.Pages;
using System;
using System.IO;
using System.Windows;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        ReadConfigFile readConfigFile = new ReadConfigFile();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigData config = readConfigFile.LoadConfig();
            if (config == null)
            {
                DatabaseConnectionSetup setupForm = new DatabaseConnectionSetup();
                setupForm.ConfigSaved += OnConfigSaved;
                setupForm.Show();
                ShutdownMode = ShutdownMode.OnExplicitShutdown;
            }
            else
            {
                Login login = new Login();
                Application.Current.MainWindow = login;
                login.Show();
            }
        }

        private void OnConfigSaved(object sender, EventArgs e)
        {
            (sender as Window)?.Close();

            Login login = new Login();
            Application.Current.MainWindow = login;
            login.Show();
            //ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }
}