using Hotel.Classes;
using Hotel.ItemControl;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hotel.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private ReadConfigFile _config = new ReadConfigFile();
        private string CONNECTION_STRING = string.Empty;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;
        private Navigation NAVIGATION = new Navigation();
        private HashPassword Security = new HashPassword();
        private int TRY_COUNT;

        public Login()
        {
            InitializeComponent();
            CONNECTION_STRING = _config.GetConnectionString();  // Получение строки подключения
        }

        private void ShowPwdButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangeVisibility(PasswordText, HelperBoxForPassword, false);
        }

        private void ShowPwdButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HelperBoxForPassword.Text = PasswordText.Password;
            ChangeVisibility(PasswordText, HelperBoxForPassword, true);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Close();
        }

        // Показать/скрыть пароль
        private void ChangeVisibility(PasswordBox pwd, TextBox text, bool isShow)
        {
            if (isShow)
            {
                pwd.Visibility = Visibility.Collapsed;
                text.Visibility = Visibility.Visible;
            }
            else
            {
                text.Visibility = Visibility.Collapsed;
                pwd.Visibility = Visibility.Visible;
            }
        }

        // Авторизация
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string USERNAME = LoginText.Text, PASSWORD = Security.HashPasswd(PasswordText.Password);

            if (!CheckFields(USERNAME, PASSWORD))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TRY_COUNT <= 0)
                NAVIGATION.OpenAsDialog(new Captcha());

            try
            {
                using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
                {
                    CONNECTION.Open();

                    // Проверка существует ли пользователь
                    string checkUserQuery = $"SELECT COUNT(*) FROM users WHERE userEmail='{USERNAME}';";
                    COMMAND = new MySqlCommand(checkUserQuery, CONNECTION);
                    int userCount = Convert.ToInt32(COMMAND.ExecuteScalar());
                    if (userCount == 0)
                    {
                        MessageBox.Show("Пользователь не существует.", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Проверка на правильность ввода
                    string checkLoginQuery = $"SELECT COUNT(*) FROM users WHERE userEmail='{USERNAME}' AND userPassword='{PASSWORD}';";
                    COMMAND = new MySqlCommand(checkLoginQuery, CONNECTION);
                    int loginCount = Convert.ToInt32(COMMAND.ExecuteScalar());
                    if (loginCount == 0)
                    {
                        MessageBox.Show("Неправильный логин или пароль.", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        TRY_COUNT--;

                        if (TRY_COUNT == 0)
                        {
                            _ = BlockInputFieldsForDurationAsync(10000);
                        }

                        return;
                    }

                    // Получение имени пользователя
                    string getUserName = $"SELECT userName FROM users WHERE userEmail='{USERNAME}' AND userPassword='{PASSWORD}';";
                    COMMAND = new MySqlCommand(getUserName, CONNECTION);
                    object resultBack = COMMAND.ExecuteScalar();
                    string name = resultBack?.ToString() ?? string.Empty;

                    // Определение типа пользователя
                    string getUserType = $"SELECT userType FROM users WHERE userEmail='{USERNAME}' AND userPassword='{PASSWORD}';";
                    COMMAND = new MySqlCommand(getUserType, CONNECTION);
                    resultBack = COMMAND.ExecuteScalar();
                    int userType = resultBack != null ? Convert.ToInt32(resultBack) : 0;

                    // Авторизация
                    if (userType == 1)
                        NAVIGATION.OpenAsNewPage(new AdminPanelView(name, PASSWORD), this);
                    else
                        NAVIGATION.OpenAsNewPage(new MainView(name), this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Проверка на пустые поля
        private bool CheckFields(string username, string password)
        {
            return !(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password));
        }

        private void LoginWin_Loaded(object sender, RoutedEventArgs e)
        {
            TRY_COUNT = 3;  // Установка начального значения попыток входа
        }

        // Блокировка ввода на определенное время
        private async Task BlockInputFieldsForDurationAsync(int milliseconds)
        {
            PasswordText.Password = string.Empty;
            InputGrid.IsEnabled = false;
            TimerText.Visibility = Visibility.Visible;

            int seconds = milliseconds / 1000;

            for (int i = 0; i < seconds; i++)
            {
                SecondsText.Text = $"{seconds - i}";
                await Task.Delay(1000);
            }

            InputGrid.IsEnabled = true;
            TimerText.Visibility = Visibility.Collapsed;
        }
    }
}