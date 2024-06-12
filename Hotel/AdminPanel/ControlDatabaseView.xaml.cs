using Hotel.Classes;
using Hotel.ItemControl;
using Hotel.Pages;
using System;
using System.Windows;

namespace Hotel.AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для ControlDatabaseView.xaml
    /// </summary>
    public partial class ControlDatabaseView : Window
    {
        // Создаем экземпляры необходимых классов
        private Navigation NAVIGATION = new Navigation();
        private BackupDatabase BACKUP = new BackupDatabase();
        private HashPassword Security = new HashPassword();

        // Поля для хранения имени пользователя и пароля администратора
        private string PASSWORD = String.Empty;
        private string NAME = String.Empty;

        // Флаг для перезапуска приложения
        private bool RESTART = false;

        public ControlDatabaseView(string name, string adminPassword)
        {
            InitializeComponent();
            NAME = name;
            PASSWORD = adminPassword;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку резервного копирования базы данных
        /// </summary>
        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        MessageBox.Show("Ошибка при создании резервной копии.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при резервном копировании базы данных: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку восстановления базы данных
        /// </summary>
        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        MessageBox.Show("Ошибка при восстановлении базы данных.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при восстановлении базы данных: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку экспорта таблицы
        /// </summary>
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                                MessageBox.Show("Экспорт таблицы выполнен успешно.", "ЭКСПОРТ", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                            MessageBox.Show("Ошибка при экспорте таблицы.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    if (password != PASSWORD) MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте таблицы: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку импорта таблицы
        /// </summary>
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordWindow passwordWindow = new PasswordWindow();
                if (passwordWindow.ShowDialog() == true)
                {
                    string password = passwordWindow.Password;
                    if (password == PASSWORD)
                    {
                        if (BACKUP.ImportTable())
                        {
                            MessageBox.Show("Импорт таблицы выполнен успешно.", "ИМПОРТ", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        MessageBox.Show("Ошибка при импорте таблицы.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    MessageBox.Show("Неверный пароль!", "ВВОД ПАРОЛЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте таблицы: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку настройки подключения
        /// </summary>
        private void ConfigConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите изменить настройки подключения?\nПриложение нужно будет перезапустить.", "НАСТРОЙКА ПОДКЛЮЧЕНИЯ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    RESTART = true;
                    NAVIGATION.OpenAsNewPage(new DatabaseConnectionSetup(), this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при настройке подключения: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик события закрытия окна
        /// </summary>
        private void DatabaseControlWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!RESTART)
                {
                    AdminPanelView adminPanelView = new AdminPanelView(NAME, PASSWORD);
                    adminPanelView.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при закрытии окна: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
