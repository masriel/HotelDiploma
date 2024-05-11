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

using MySql.Data.MySqlClient;
using Hotel.Classes;
using Hotel.Pages;
using System.Configuration;

namespace Hotel.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string CONNECTION_STRING =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private Navigation WINDOW_CHANGES = new Navigation();
        private HashPassword Security = new HashPassword();

        public Login()
        {
            InitializeComponent();
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
            this.Close();
        }

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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string USERNAME = LoginText.Text, PASSWORD = Security.HashPasswd(PasswordText.Password);
            if(!CheckFields(USERNAME, PASSWORD)) 
            {
                MessageBox.Show("Все поля должны быть заполнены!", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            try
            {
                using(CONNECTION = new MySqlConnection(CONNECTION_STRING))
                {
                    CONNECTION.Open();

                    //проверка существует ли пользователь
                    string checkUserQuery = $"select count(*) from users where userEmail='{USERNAME}';";
                    COMMAND = new MySqlCommand(checkUserQuery, CONNECTION);
                    int userCount = Convert.ToInt32(COMMAND.ExecuteScalar());
                    if(userCount == 0)
                    {
                        MessageBox.Show("Пользователь не существует.", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string checkLoginQuery = $"select count(*) from users where userEmail='{USERNAME}' and userPassword='{PASSWORD}';";
                    COMMAND = new MySqlCommand(checkLoginQuery, CONNECTION);
                    int loginCount = Convert.ToInt32(COMMAND.ExecuteScalar());
                    if (loginCount == 0)
                    {
                        MessageBox.Show("Неправильный логин или пароль.", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    MessageBox.Show("Вы успешно вошли в систему!", "АВТОРИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Information);
                    WINDOW_CHANGES.OpenAsNewPage(new Main(), this);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckFields(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            return true;
        }
    }
}
