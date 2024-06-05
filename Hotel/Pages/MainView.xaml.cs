using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Hotel.Classes;

namespace Hotel.Pages
{
    public partial class MainView : Window
    {
        private readonly Navigation _navigation = new Navigation();
        private readonly string _name;
        private readonly ConnectionInfo _db;
        private readonly ReadConfigFile _config = new ReadConfigFile();
        private ObservableCollection<Rooms> _allRooms;
        private ObservableCollection<Rooms> _displayedRooms;
        private int currentPage = 1;
        private int itemsPerPage = 1;
        private int totalItems;
        private int totalPages;

        public MainView(string name)
        {
            InitializeComponent();
            _name = name;
            _db = new ConnectionInfo(_config.GetConnectionString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkerName.Text = _name;
            LoadRooms();
            SearchText.TextChanged += SearchText_TextChanged;
            FilterBox.SelectionChanged += FilterBox_SelectionChanged;
        }

        private void LoadRooms()
        {
            _allRooms = new ObservableCollection<Rooms>();
            _displayedRooms = new ObservableCollection<Rooms>();

            DataTable roomData = _db.GetData("SELECT roomID, roomNumber, RoomTypes.roomType as typeID, RoomTypes.roomType, maxOccupancy, roomDescription, roomPhoto, roomCost, isFree " +
                                             "FROM Rooms LEFT JOIN RoomTypes ON Rooms.roomType = RoomTypes.roomTypeID");

            foreach (DataRow row in roomData.Rows)
            {
                _allRooms.Add(new Rooms
                {
                    ID = Convert.ToInt32(row["roomID"]),
                    Type = row["roomType"].ToString(),
                    Number = Convert.ToString(row["roomNumber"]),
                    Occupancy = row["maxOccupancy"].ToString(),
                    Description = row["roomDescription"].ToString(),
                    Photo = Path.Combine("pack://application:,,,/Resources", row["roomPhoto"].ToString()),
                    Cost = row["roomCost"].ToString(),
                    IsFree = row["isFree"].ToString() == "t" ? "Свободен" : "Занят"
                });
            }

            totalItems = _allRooms.Count;
            ApplyPagination();
        }

        private void ApplyPagination()
        {
            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            _displayedRooms.Clear();
            var paginatedRooms = _allRooms.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            foreach (var room in paginatedRooms)
            {
                _displayedRooms.Add(room);
            }

            Rooms.ItemsSource = _displayedRooms;

            CurrentPage.Text = currentPage.ToString();
            TotalPages.Text = totalPages.ToString();
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortRooms();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortRooms();
        }

        private void DescButton_Click(object sender, RoutedEventArgs e)
        {
            SortRooms(false);
            ApplyPagination();
        }

        private void AscButton_Click(object sender, RoutedEventArgs e)
        {
            SortRooms(true);
            ApplyPagination();
        }

        private void FilterAndSortRooms()
        {
            string searchText = SearchText.Text.ToLower();
            string filterType = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            IEnumerable<Rooms> filteredRooms = _allRooms;

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredRooms = filteredRooms.Where(r => r.Type.ToLower().Contains(searchText));
            }

            switch (filterType)
            {
                case "Статус":
                    filteredRooms = filteredRooms.OrderBy(r => r.IsFree);
                    break;
                case "Кол-во жильцов":
                    filteredRooms = filteredRooms.OrderBy(r => Convert.ToInt32(r.Occupancy));
                    break;
                case "Цена":
                    filteredRooms = filteredRooms.OrderBy(r => Convert.ToDouble(r.Cost));
                    break;
            }

            _allRooms = new ObservableCollection<Rooms>(filteredRooms);
            totalItems = _allRooms.Count;
            currentPage = 1;
            ApplyPagination();
        }

        private void SortRooms(bool ascending)
        {
            string filterType = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            IEnumerable<Rooms> sortedRooms = _allRooms;

            switch (filterType)
            {
                case "Статус":
                    sortedRooms = ascending ? sortedRooms.OrderBy(r => r.IsFree) : sortedRooms.OrderByDescending(r => r.IsFree);
                    break;
                case "Кол-во жильцов":
                    sortedRooms = ascending ? sortedRooms.OrderBy(r => Convert.ToInt32(r.Occupancy)) : sortedRooms.OrderByDescending(r => Convert.ToInt32(r.Occupancy));
                    break;
                case "Цена":
                    sortedRooms = ascending ? sortedRooms.OrderBy(r => Convert.ToDouble(r.Cost)) : sortedRooms.OrderByDescending(r => Convert.ToDouble(r.Cost));
                    break;
            }

            _allRooms = new ObservableCollection<Rooms>(sortedRooms);
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
            itemsPerPage = 2;
            ApplyPagination();
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
            itemsPerPage = 1;
            ApplyPagination();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _navigation.OpenAsNewPage(new Login(), this);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ApplyPagination();
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                ApplyPagination();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchText.Clear();
            FilterBox.SelectedIndex = -1;
            LoadRooms();
        }
    }
}
