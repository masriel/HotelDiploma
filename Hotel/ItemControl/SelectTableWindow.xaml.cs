using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Hotel.ItemControl
{
    /// <summary>
    /// Interaction logic for SelectTableWindow.xaml
    /// </summary>
    public partial class SelectTableWindow : Window
    {
        public List<string> Tables = new List<string>();
        public SelectTableWindow()
        {
            InitializeComponent();
        }

        private void Users_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Tables.Clear();
            Tables.Add("Users");
            Tables.Add("UserTypes");
            DialogResult = true;
        }

        private void Rooms_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Tables.Clear();
            Tables.Add("Rooms");
            Tables.Add("RoomTypes");
            DialogResult = true;
        }

        private void Meals_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Tables.Clear();
            Tables.Add("Meals");
            DialogResult = true;
        }

        private void Clients_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Tables.Clear();
            Tables.Add("Clients");
            Tables.Add("ClientPassports");
            Tables.Add("BirthCertificate");
            DialogResult = true;
        }

        private void Bookings_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Tables.Clear();
            Tables.Add("Bookings");
            Tables.Add("BookingClients");
            Tables.Add("BookingMeals");
            DialogResult = true;
        }
    }
}
