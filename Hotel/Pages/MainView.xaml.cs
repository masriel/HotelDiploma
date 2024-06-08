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
        private readonly RoomStatusUpdater _roomStatusUpdater = new RoomStatusUpdater();
        private readonly ReadConfigFile _config = new ReadConfigFile();
        private ObservableCollection<Rooms> _allRooms;
        private ObservableCollection<Rooms> _displayedRooms;
        private int _currentPage = 1;
        private int _itemsPerPage = 2;
        private int _totalItems;
        private int _totalPages;

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
            SearchText.TextChanged += (s, args) => FilterAndSortRooms();
            FilterBox.SelectionChanged += (s, args) => FilterAndSortRooms();
        }

        private void LoadRooms()
        {
            _allRooms = new ObservableCollection<Rooms>(GetRoomsFromDatabase());
            _displayedRooms = new ObservableCollection<Rooms>(_allRooms);
            _totalItems = _allRooms.Count;
            ApplyPagination();
        }

        private IEnumerable<Rooms> GetRoomsFromDatabase()
        {
            var roomData = _db.GetData("SELECT roomID, roomNumber, RoomTypes.roomType as typeID, RoomTypes.roomType, maxOccupancy, roomDescription, roomPhoto, roomCost, isFree " +
                                       "FROM Rooms LEFT JOIN RoomTypes ON Rooms.roomType = RoomTypes.roomTypeID");

            return from DataRow row in roomData.Rows
                   select new Rooms
                   {
                       ID = Convert.ToInt32(row["roomID"]),
                       Type = row["roomType"].ToString(),
                       Number = row["roomNumber"].ToString(),
                       Occupancy = row["maxOccupancy"].ToString(),
                       Description = row["roomDescription"].ToString(),
                       Photo = Path.Combine("pack://application:,,,/Resources", row["roomPhoto"].ToString()),
                       Cost = row["roomCost"].ToString(),
                       IsFree = row["isFree"].ToString() == "t" ? "Свободен" : "Занят"
                   };
        }

        private void ApplyPagination()
        {
            _totalPages = (int)Math.Ceiling((double)_totalItems / _itemsPerPage);
            var paginatedRooms = _displayedRooms.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
            Rooms.ItemsSource = new ObservableCollection<Rooms>(paginatedRooms);
            CurrentPage.Text = _currentPage.ToString();
            TotalPages.Text = _totalPages.ToString();
        }

        private void FilterAndSortRooms()
        {
            string searchText = SearchText.Text.ToLower();
            string filterType = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var filteredRooms = _allRooms.AsEnumerable();

            // Применить фильтрацию по типу номера только если searchText не пустой
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredRooms = filteredRooms.Where(r => r.Type.ToLower().Contains(searchText));
            }

            // Применить сортировку в зависимости от выбранного типа фильтрации
            if (filterType != null)
            {
                filteredRooms = filterType switch
                {
                    "Статус" => filteredRooms.OrderBy(r => r.IsFree),
                    "Кол-во жильцов" => filteredRooms.OrderBy(r => Convert.ToInt32(r.Occupancy)),
                    "Цена" => filteredRooms.OrderBy(r => Convert.ToDouble(r.Cost)),
                    _ => filteredRooms
                };
            }

            _displayedRooms = new ObservableCollection<Rooms>(filteredRooms);
            _totalItems = _displayedRooms.Count;
            _currentPage = 1;
            ApplyPagination();
        }

        private void SortRooms(bool ascending)
        {
            string filterType = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var sortedRooms = _displayedRooms.AsEnumerable();

            if (filterType != null)
            {
                sortedRooms = filterType switch
                {
                    "Статус" => ascending ? sortedRooms.OrderBy(r => r.IsFree) : sortedRooms.OrderByDescending(r => r.IsFree),
                    "Кол-во жильцов" => ascending ? sortedRooms.OrderBy(r => Convert.ToInt32(r.Occupancy)) : sortedRooms.OrderByDescending(r => Convert.ToInt32(r.Occupancy)),
                    "Цена" => ascending ? sortedRooms.OrderBy(r => Convert.ToDouble(r.Cost)) : sortedRooms.OrderByDescending(r => Convert.ToDouble(r.Cost)),
                    _ => sortedRooms
                };
            }

            _displayedRooms = new ObservableCollection<Rooms>(sortedRooms);
            ApplyPagination();
        }

        private void DescButton_Click(object sender, RoutedEventArgs e)
        {
            SortRooms(false);
        }

        private void AscButton_Click(object sender, RoutedEventArgs e)
        {
            SortRooms(true);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Maximized, 2, FullScreenButton, SmallScreenButton);
        }

        private void SmallScreenButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Normal, 1, SmallScreenButton, FullScreenButton);
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void SetWindowState(WindowState state, int itemsPerPage, Button hideButton, Button showButton)
        {
            WindowState = state;
            _itemsPerPage = itemsPerPage;
            hideButton.Visibility = Visibility.Collapsed;
            showButton.Visibility = Visibility.Visible;
            ApplyPagination();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _navigation.OpenAsNewPage(new Login(), this);
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                ApplyPagination();
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                ApplyPagination();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchText.Clear();
            FilterBox.SelectedIndex = -1;
            _displayedRooms = new ObservableCollection<Rooms>(_allRooms);
            _totalItems = _displayedRooms.Count;
            _currentPage = 1;
            ApplyPagination();
        }

        private void UpdateStatusButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _roomStatusUpdater.UpdateRoomStatus();
                MessageBox.Show("Информация о доступности номеров актуальная!", "АКТУАЛИЗАЦИЯ", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clients_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var clients = new Clients(_name);
            clients.Owner = this;
            this.Hide();
            _navigation.OpenAsDialog(clients);
        }

        private void Bookings_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
