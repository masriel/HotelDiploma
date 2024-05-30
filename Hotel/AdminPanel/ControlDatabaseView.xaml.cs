using System;
using System.Windows;

using Hotel.ItemControl;
using Hotel.Classes;
using Hotel.Pages;

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
                        MessageBox.Show("Резервное копирование базы данных выполнено успешно.", "РЕЗЕРВНОЕ КОПИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void ConfigConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите изменить настройки подключения?\nПриложение перезапустится.", "НАСТРОЙКА ПОДКЛЮЧЕНИЯ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) 
            {
                DatabaseConnectionSetup databaseConnectionSetup = new DatabaseConnectionSetup();
                NAVIGATION.OpenAsNewPage(databaseConnectionSetup, this);
            }
        }
    }
}
