using Hotel.Classes;
using System.Windows;

namespace Hotel.ItemControl
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private HashPassword Security = new HashPassword();

        public string Password { get; private set; }

        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void CheckPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            Password = Security.HashPasswd(PasswordText.Password);
            DialogResult = true;
        }
    }
}
