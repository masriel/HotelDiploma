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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        Navigation NAVIGATION = new Navigation();
        public MainView()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) this.Close();
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            FullScreenButton.Visibility = Visibility.Collapsed;
            SmallScreenButton.Visibility = Visibility.Visible;

            this.WindowState = WindowState.Maximized;
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SmallScreenButton_Click(object sender, RoutedEventArgs e)
        {
            FullScreenButton.Visibility = Visibility.Visible;
            SmallScreenButton.Visibility = Visibility.Collapsed;

            this.WindowState = WindowState.Normal;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Rooms> rooms = new List<Rooms>();
            rooms.Add(new Rooms { Type = "Одноместный", Cost = "8000", Occupancy = "4", Description = "Description description description description description description description" });
            rooms.Add(new Rooms { Type = "Одноместный", Cost = "8000", Occupancy = "4", Description = "Description description description description description description description" });
            rooms.Add(new Rooms { Type = "Одноместный", Cost = "8000", Occupancy = "4", Description = "Description description description description description description description" });
            Rooms.ItemsSource = rooms;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) NAVIGATION.OpenAsNewPage(new Login(), this);
        }
    }
}
