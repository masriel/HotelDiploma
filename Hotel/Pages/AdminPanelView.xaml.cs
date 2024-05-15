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

namespace Hotel.Pages
{
    /// <summary>
    /// Interaction logic for AdminPanelView.xaml
    /// </summary>
    public partial class AdminPanelView : Window
    {
        private Navigation NAVIGATION = new Navigation();
        public AdminPanelView()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            NAVIGATION.OpenAsNewPage(new Login(), this);
        }

        private void Users_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("нажатие на пользователей");
        }

        private void Database_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("нажатие на бд");
        }

        private void DatabaseControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("нажатие на управление бд");
        }
    }
}
