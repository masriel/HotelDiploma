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
                MessageBox.Show("Резервное копирование базы данных выполнено успешно.", "РЕЗЕРВНОЕ КОПИРОВАНИЕ БАЗЫ ДАННЫХ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string backupFilePath = openFileDialog.FileName;

                // Запрашиваем пароль перед восстановлением базы данных
                PasswordWindow passwordWindow = new PasswordWindow();
                if (passwordWindow.ShowDialog() == true)
                {
                    string password = passwordWindow.Password;
                    if (password == PASSWORD)
                    { 
                        BACKUP.RestoreBackup(backupFilePath);
                        MessageBox.Show("Восстановление базы данных выполнено успешно.", "ВОССТАНОВЛЕНИЕ БАЗЫ ДАННЫХ", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            if (passwordWindow.ShowDialog() == true)
            {
                string password = passwordWindow.Password;
                if (password == PASSWORD)
                {
                    BACKUP.ExportTable(new List<string> { "Users", "UserTypes" });
                    MessageBox.Show("Экспорт таблицы выполнен успешно.", "ЭКПОРТ", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string backupFilePath = openFileDialog.FileName;

                // Запрашиваем пароль перед импортом базы данных
                PasswordWindow passwordWindow = new PasswordWindow();
                if (passwordWindow.ShowDialog() == true)
                {
                    string password = passwordWindow.Password;
                    if (password == PASSWORD)
                    {
                        BACKUP.ImportTable(backupFilePath);
                        MessageBox.Show("Импорт таблицы выполнен успешно.", "ИМПОРТ", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
