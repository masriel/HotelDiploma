using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MySql.Data.MySqlClient;
using Hotel.Classes;
using Hotel.ItemControl;

namespace Hotel.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private ReadConfigFile _config = new ReadConfigFile();

        private string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private Navigation NAVIGATION = new Navigation();
        private HashPassword Security = new HashPassword();

        private int TRY_COUNT;

        public Login()
        {
            InitializeComponent();
            CONNECTION_STRING = _config.GetConnectionString();  //получение строки подключения
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
            if (MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) this.Close();
        }

        //показать/скрыть пароль
        private void ChangeVisibility(PasswordBox pwd, TextBox text, bool isShow)
        {
            if (isShow)
            {
                pwd.Visibility = Visibility.Collapsed;
                text.Visibility = Visibility.Visible;
                return;
            }
            text.Visibility = Visibility.Collapsed;
            pwd.Visibility = Visibility.Visible;
        }

        //авторизация
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //string USERNAME = LoginText.Text, PASSWORD = Security.HashPasswd(PasswordText.Password);

            //if (!CheckFields(USERNAME, PASSWORD))
            //{
            //    MessageBox.Show("Все поля должны быть заполнены!", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //if (TRY_COUNT <= 0) NAVIGATION.OpenAsDialog(new Captcha());

            //try
            //{
            //    using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
            //    {
            //        CONNECTION.Open();

            //        //проверка существует ли пользователь
            //        string checkUserQuery = $"select count(*) from users where userEmail='{USERNAME}';";
            //        COMMAND = new MySqlCommand(checkUserQuery, CONNECTION);
            //        int userCount = Convert.ToInt32(COMMAND.ExecuteScalar());
            //        if (userCount == 0)
            //        {
            //            MessageBox.Show("Пользователь не существует.", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }

            //        //проверка на правильность ввода
            //        string checkLoginQuery = $"select count(*) from users where userEmail='{USERNAME}' and userPassword='{PASSWORD}';";
            //        COMMAND = new MySqlCommand(checkLoginQuery, CONNECTION);
            //        int loginCount = Convert.ToInt32(COMMAND.ExecuteScalar());
            //        if (loginCount == 0)
            //        {
            //            MessageBox.Show("Неправильный логин или пароль.", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            //            TRY_COUNT--;

            //            if (TRY_COUNT == 0)
            //            {
            //                _ = BlockInputFieldsForDurationAsync(10000);
            //            }

            //            return;
            //        }

            //        //получение имени пользователя
            //        string getUserName = $"select userName from users  where userEmail='{USERNAME}' and userPassword='{PASSWORD}';";
            //        COMMAND = new MySqlCommand(getUserName, CONNECTION);
            //        object resultBack = COMMAND.ExecuteScalar();
            //        string name = String.Empty;
            //        if (resultBack != null)
            //        {
            //            name = Convert.ToString(resultBack);
            //        }

            //        //определение типа пользователя
            //        string getUserType = $"select userType from users  where userEmail='{USERNAME}' and userPassword='{PASSWORD}';";
            //        int userType = 0;
            //        COMMAND = new MySqlCommand(getUserType, CONNECTION);
            //        resultBack = COMMAND.ExecuteScalar();
            //        if (resultBack != null)
            //        {
            //            userType = Convert.ToInt32(resultBack);
            //        }

            //        //авторизация
            //        if (userType == 1) NAVIGATION.OpenAsNewPage(new AdminPanelView(name, PASSWORD), this);
            //        else NAVIGATION.OpenAsNewPage(new MainView(), this);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            //NAVIGATION.OpenAsNewPage(new AdminPanelView("Екатерина Мухина", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"), this);
            NAVIGATION.OpenAsNewPage(new MainView("Екатерина Мухина"), this);
        }

        //проверка на пустые поля
        private bool CheckFields(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            return true;
        }

        private void LoginWin_Loaded(object sender, RoutedEventArgs e)
        {
            TRY_COUNT = 3;
        }

        private async Task BlockInputFieldsForDurationAsync(int milliseconds)
        {
            PasswordText.Password = String.Empty;
            InputGrid.IsEnabled = false;
            TimerText.Visibility = Visibility.Visible;

            int seconds = milliseconds / 1000;

            for (int i = 0; i < seconds; i++)
            {
                if (i == 0)
                {
                    SecondsText.Text = $"{seconds - i}";
                    await Task.Delay(1000);
                    continue;
                }
                SecondsText.Text = $"0{seconds - i}";
                await Task.Delay(1000);
            }

            InputGrid.IsEnabled = true;
            TimerText.Visibility = Visibility.Collapsed;
        }
    }
}
