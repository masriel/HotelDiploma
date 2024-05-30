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

        private string PASSWORD = String.Empty;
        public AdminPanelView()
        {
            InitializeComponent();
            PASSWORD = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Выйти из системы?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) NAVIGATION.OpenAsNewPage(new Login(), this);
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
            NAVIGATION.OpenAsDialog(new AdminPanel.ControlDatabaseView(PASSWORD));
        }
    }
}
