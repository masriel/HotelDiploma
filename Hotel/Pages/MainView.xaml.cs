using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using MySql.Data.MySqlClient;
using Hotel.Classes;

namespace Hotel.Pages
{
    public partial class MainView : Window
    {
        Navigation NAVIGATION = new Navigation();
        private string NAME;
        private ConnectionInfo db;
        private ReadConfigFile _config = new ReadConfigFile();
        private List<Rooms> allRooms;

        public MainView(string name)
        {
            InitializeComponent();
            NAME = name;
            db = new ConnectionInfo(_config.GetConnectionString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkerName.Text = NAME;
            LoadRooms();
            SearchText.TextChanged += SearchText_TextChanged;
            FilterBox.SelectionChanged += FilterBox_SelectionChanged;
        }

        private void LoadRooms()
        {
            allRooms = new List<Rooms>();

            DataTable roomData = db.GetData("SELECT roomID, roomNumber, RoomTypes.roomType as typeID, RoomTypes.roomType, maxOccupancy, roomDescription, roomPhoto, roomCost, isFree " +
                                             "FROM Rooms LEFT JOIN RoomTypes ON Rooms.roomType = RoomTypes.roomTypeID;");

            foreach (DataRow row in roomData.Rows)
            {
                allRooms.Add(new Rooms
                {
                    ID = Convert.ToInt32(row["roomID"]),
                    Type = row["roomType"].ToString(),
                    Occupancy = row["maxOccupancy"].ToString(),
                    Description = row["roomDescription"].ToString(),
                    Photo = row["roomPhoto"].ToString(),
                    Cost = row["roomCost"].ToString()
                });
            }

            Rooms.ItemsSource = allRooms;
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterRooms();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterRooms();
        }

        private void DescButton_Click(object sender, RoutedEventArgs e)
        {
            SortRooms(false);
        }

        private void AscButton_Click(object sender, RoutedEventArgs e)
        {
            SortRooms(true);
        }

        private void FilterRooms()
        {
            string searchText = SearchText.Text.ToLower();
            string filterType = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            IEnumerable<Rooms> filteredRooms = allRooms;

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredRooms = filteredRooms.Where(r => r.Type.ToLower().Contains(searchText));
            }

            switch (filterType)
            {
                case "Площадь":
                    // Assuming you have a property for room area
                    filteredRooms = filteredRooms.OrderBy(r => r.Description);
                    break;
                case "Кол-во жильцов":
                    filteredRooms = filteredRooms.OrderBy(r => r.Occupancy);
                    break;
                case "Цена":
                    filteredRooms = filteredRooms.OrderBy(r => r.Cost);
                    break;
            }

            Rooms.ItemsSource = filteredRooms;
        }

        private void SortRooms(bool ascending)
        {
            string filterType = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            IEnumerable<Rooms> sortedRooms = allRooms;

            switch (filterType)
            {
                case "Площадь":
                    // Assuming you have a property for room area
                    sortedRooms = ascending ? sortedRooms.OrderBy(r => r.Description) : sortedRooms.OrderByDescending(r => r.Description);
                    break;
                case "Кол-во жильцов":
                    sortedRooms = ascending ? sortedRooms.OrderBy(r => r.Occupancy) : sortedRooms.OrderByDescending(r => r.Occupancy);
                    break;
                case "Цена":
                    sortedRooms = ascending ? sortedRooms.OrderBy(r => r.Cost) : sortedRooms.OrderByDescending(r => r.Cost);
                    break;
            }

            Rooms.ItemsSource = sortedRooms;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
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

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NAVIGATION.OpenAsNewPage(new Login(), this);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
