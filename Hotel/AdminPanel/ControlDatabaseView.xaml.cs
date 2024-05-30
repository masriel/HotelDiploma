using Hotel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            if (passwordWindow.ShowDialog() == true)
            {
                string password = passwordWindow.Password;
                if (password == PASSWORD)
                {
                    if (BACKUP.CreateBackup())
                    {
                        MessageBox.Show("Восстановление базы данных выполнено успешно.", "ВОССТАНОВЛЕНИЕ БАЗЫ ДАННЫХ", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    return;
                }
                MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            if (passwordWindow.ShowDialog() == true)
            {
                string password = passwordWindow.Password;
                if (password == PASSWORD)
                {
                    if (BACKUP.RestoreBackup())
                    {
                        MessageBox.Show("Восстановление базы данных выполнено успешно.", "ВОССТАНОВЛЕНИЕ БАЗЫ ДАННЫХ", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    return;
                }
                MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    SelectTableWindow selectTable = new SelectTableWindow();
                    if (selectTable.ShowDialog() == true)
                    {
                        if (BACKUP.ExportTable(selectTable.Tables))
                        {
                            MessageBox.Show("Экспорт таблицы выполнен успешно.", "ЭКПОРТ", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        return;
                    }
                }
                MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            if (passwordWindow.ShowDialog() == true)
            {
                string password = passwordWindow.Password;
                if (password == PASSWORD)
                {
                    if(BACKUP.ImportTable())
                    {
                        MessageBox.Show("Импорт таблицы выполнен успешно.", "ИМПОРТ", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    return;
                }
                MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
