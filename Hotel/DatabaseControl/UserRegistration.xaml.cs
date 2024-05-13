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

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Interaction logic for UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Window
    {
        public UserRegistration()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

            if (!(char.IsLetter(inputChar) && (inputChar >= 'а' && inputChar <= 'я' || inputChar >= 'А' && inputChar <= 'Я') || inputChar == '.' || inputChar == ' '))
            {
                e.Handled = true;
            }

        }

        private void UsernameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;

            if (string.IsNullOrWhiteSpace(text))
                return;

            StringBuilder result = new StringBuilder(text.Length);

            bool capitalizeNext = true;

            foreach (char c in text)
            {
                if (char.IsWhiteSpace(c) || c == '.')
                {
                    capitalizeNext = true;
                }
                else
                {
                    result.Append(capitalizeNext ? char.ToUpper(c) : c);
                    capitalizeNext = false;
                }
            }

            textBox.Text = result.ToString();
            textBox.CaretIndex = textBox.Text.Length;
        }
    }
}
