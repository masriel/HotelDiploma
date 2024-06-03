using Hotel.Classes;
using MySql.Data.MySqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Interaction logic for UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Window
    {
        private ReadConfigFile _config = new ReadConfigFile();
        private string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;
        Navigation NAVIGATION = new Navigation();

        private HashPassword Security = new HashPassword();

        private int ID, TYPE;
        private string NAME, LOGIN;
        private bool IsEdit = false;

        public UserRegistration(int id, string name, string login, int type, bool isEdit)
        {
            InitializeComponent();

            ID = id;
            NAME = name;
            LOGIN = login;
            TYPE = type;
            IsEdit = isEdit;
        }

        private void ShowPwdButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HelperBoxForPassword.Text = PasswordText.Password;
            ChangeVisibility(PasswordText, HelperBoxForPassword, true);
        }

        private void ShowPwdButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangeVisibility(PasswordText, HelperBoxForPassword, false);
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

        private void EmailText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char inputChar = e.Text[0];

            if (!((inputChar >= 'a' && inputChar <= 'z') || (inputChar >= 'A' && inputChar <= 'Z') || char.IsDigit(inputChar) || inputChar == '@' || inputChar == '-' || inputChar == '.'))
            {
                e.Handled = true;
            }

        }

        private void UsernameText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char inputChar = e.Text[0];

            if (!((inputChar >= 'а' && inputChar <= 'я') || (inputChar >= 'А' && inputChar <= 'Я') || inputChar == ' '))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            string currentText = textBox.Text;
            int selectionStart = textBox.SelectionStart;

            string newText;
            if (string.IsNullOrEmpty(currentText))
            {
                newText = e.Text.ToUpper();
            }
            else
            {
                if (selectionStart > 0 && currentText[selectionStart - 1] == ' ')
                {
                    newText = e.Text.ToUpper();
                }
                else
                {
                    newText = e.Text.ToLower();
                }
            }

            textBox.Text = currentText.Insert(selectionStart, newText);
            textBox.SelectionStart = selectionStart + newText.Length;

            e.Handled = true;
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordText.Password = Security.GeneratePassword(8);
        }

        private bool CheckFields(string username, string login, string password, int role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || role == -1) return false;
            return true;
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            string USERNAME = UsernameText.Text, LOGIN = EmailText.Text, PASSWORD = Security.HashPasswd(PasswordText.Password);
            int ROLE = UserRole.SelectedIndex == 0 ? 1 : 2;

            if (!IsEdit)
            {
                if (!CheckFields(USERNAME, LOGIN, PASSWORD, ROLE))
                {
                    MessageBox.Show("Все поля должны быть заполнены!", "ДОБАВЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
                    {
                        CONNECTION.Open();

                        string checkNewUserQuery = $"select count(*) from users where userEmail='{LOGIN}';";
                        COMMAND = new MySqlCommand(checkNewUserQuery, CONNECTION);
                        int userCount = Convert.ToInt32(COMMAND.ExecuteScalar());
                        if (userCount != 0)
                        {
                            MessageBox.Show("Пользователь уже существует.", "ДОБАВЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        string addNewUser = $"insert into users (userName, userEmail, userPassword, userType) values ('{USERNAME}','{LOGIN}','{PASSWORD}',{ROLE}); select LAST_INSERT_ID();";
                        COMMAND = new MySqlCommand(addNewUser, CONNECTION);
                        object newUserID = Convert.ToInt32(COMMAND.ExecuteScalar());
                        if (newUserID == null || newUserID == DBNull.Value)
                        {
                            MessageBox.Show("Произошла ошибка при добавлении пользователя.", "ДОБАВЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        MessageBox.Show("Пользователь успешно добавлен.", "ДОБАВЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);

                        UsernameText.Text = String.Empty;
                        EmailText.Text = String.Empty;
                        PasswordText.Password = String.Empty;
                        UserRole.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }
            EditUser(ID, USERNAME, LOGIN, PASSWORD, ROLE);
        }

        private void AddUserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }

        private void AddUserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CONNECTION_STRING = _config.GetConnectionString();

            UsernameText.Text = NAME;
            EmailText.Text = LOGIN;
            UserRole.SelectedIndex = TYPE != 0 ? TYPE - 1 : -1;
            AddUserButton.Content = IsEdit ? "РЕДАКТИРОВАТЬ" : "ДОБАВИТЬ";
        }

        private void EditUser(int id, string name, string login, string password, int role)
        {
            if (!CheckFields(name, login, password, role))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
                {
                    CONNECTION.Open();

                    string editUser = $"update Users set userName='{name}', userEmail='{login}', userPassword='{password}', userType={role} where userID={id}";
                    COMMAND = new MySqlCommand(editUser, CONNECTION);
                    int rowsAffected = COMMAND.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Данные пользователя успешно обновлены.", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                        NAVIGATION.OpenAsNewPage(new UsersView(), this);
                        return;
                    }
                    MessageBox.Show("Произошла ошибка при редактировании пользователя.", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
