using Hotel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Hotel.Classes;
using Hotel.ItemControl;

namespace Hotel.AdminPanel
{
    /// <summary>
    /// Interaction logic for ControlDatabaseView.xaml
    /// </summary>
    public partial class ControlDatabaseView : Window
    {
        private Navigation NAVIGATION = new Navigation();

        private BackupDatabase BACKUP = new BackupDatabase();
        private HashPassword Security = new HashPassword();

        private string PASSWORD = String.Empty;

        public ControlDatabaseView(string adminPassword)
        {
            InitializeComponent();
            PASSWORD = adminPassword;
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            if(passwordWindow.ShowDialog() == true)
            {
                string password = passwordWindow.Password;
                if(password != PASSWORD)
                {
                    MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                BACKUP.CreateBackup();
            }
        }
    }
}
