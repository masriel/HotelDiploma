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
