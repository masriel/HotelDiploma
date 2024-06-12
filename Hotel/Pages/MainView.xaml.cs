using Hotel.Classes;
using Hotel.ItemControl;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            App.Current.Exit += Current_Exit; // Подписываемся на событие закрытия приложения
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            BackupDatabase exit = new BackupDatabase();
            // Создаем резервную копию базы данных при закрытии приложения
            exit.CreateExitBackup();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WorkerName.Text = _name;
            LoadRooms();
            SearchText.TextChanged += (s, args) => FilterAndSortRooms();
            FilterBox.SelectionChanged += (s, args) => FilterAndSortRooms();
        }


        // Загрузка всех номеров из базы данных
        private void LoadRooms()
        {
            _allRooms = new ObservableCollection<Rooms>(GetRoomsFromDatabase());
            _displayedRooms = new ObservableCollection<Rooms>(_allRooms);
            _totalItems = _allRooms.Count;
            ApplyPagination();
        }

        // Получение номеров из базы данных
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

        // Применение пагинации к отображаемым номерам
        private void ApplyPagination()
        {
            _totalPages = (int)Math.Ceiling((double)_totalItems / _itemsPerPage);
            var paginatedRooms = _displayedRooms.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
            Rooms.ItemsSource = new ObservableCollection<Rooms>(paginatedRooms);
            CurrentPage.Text = _currentPage.ToString();
            TotalPages.Text = _totalPages.ToString();
        }

        // Фильтрация и сортировка номеров по введенному тексту и выбранному типу
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

        // Сортировка номеров по возрастанию или убыванию
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

        // Обработчик кнопки закрытия приложения
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        // Обработчик кнопки перехода в полноэкранный режим
        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Maximized, 2, FullScreenButton, SmallScreenButton);
        }

        // Обработчик кнопки перехода в нормальный режим окна
        private void SmallScreenButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Normal, 1, SmallScreenButton, FullScreenButton);
        }

        // Обработчик кнопки сворачивания окна
        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Метод для установки состояния окна и настройки пагинации
        private void SetWindowState(WindowState state, int itemsPerPage, Button hideButton, Button showButton)
        {
            WindowState = state;
            _itemsPerPage = itemsPerPage;
            hideButton.Visibility = Visibility.Collapsed;
            showButton.Visibility = Visibility.Visible;
            ApplyPagination();
        }

        // Обработчик кнопки выхода из системы
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _navigation.OpenAsNewPage(new Login(), this);
            }
        }

        // Обработчик кнопки перехода на предыдущую страницу
        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                ApplyPagination();
            }
        }

        // Обработчик кнопки перехода на следующую страницу
        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                ApplyPagination();
            }
        }

        // Обработчик кнопки очистки фильтров и сортировки
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchText.Clear();
            FilterBox.SelectedIndex = -1;
            _displayedRooms = new ObservableCollection<Rooms>(_allRooms);
            _totalItems = _displayedRooms.Count;
            _currentPage = 1;
            ApplyPagination();
        }

        // Обработчик кнопки обновления статуса номеров
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

        // Обработчик клика на раздел "Клиенты"
        private void Clients_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var clients = new AllClients(_name);
            clients.Owner = this;
            this.Hide();
            _navigation.OpenAsDialog(clients);
        }

        // Обработчик клика на раздел "Бронирования"
        private void Bookings_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var reservation = new MakeReservation();
            reservation.Owner = this;
            this.Hide();
            reservation.ShowDialog();
        }

        // Обработчик клика на раздел "Регистрация бронирования"
        private void RegistrateBooking_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var bookingNumberWindow = new BookingNumberWindow();
            if (bookingNumberWindow.ShowDialog() == true)
            {
                var number = bookingNumberWindow.BookingNumber;
                if (!string.IsNullOrEmpty(number))
                {
                    var bookingData = _db.GetData("SELECT bookingID, room, bookingNumber, roomNumber, firstName, lastName, middleName, birthDate, phoneNumber, email " +
                                                  "FROM Bookings " +
                                                  "LEFT JOIN BookingClients ON BookingClients.booking = bookingID " +
                                                  "LEFT JOIN Rooms ON room = roomID " +
                                                  "LEFT JOIN Clients ON client=clientID " +
                                                  $"WHERE bookingNumber='{number}';");

                    var bookingInformationList = new ObservableCollection<BookingInfomation>(
                        from DataRow row in bookingData.Rows
                        select new BookingInfomation
                        {
                            ID = Convert.ToInt32(row["bookingID"]),
                            Number = row["bookingNumber"].ToString(),
                            ClientFirstName = row["firstName"].ToString(),
                            ClientLastName = row["lastName"].ToString(),
                            ClientMiddleName = row["middleName"].ToString(),
                            ClientPhoneNumber = row["phoneNumber"].ToString(),
                            ClientEmail = row["email"].ToString(),
                            ClientBirthDate = DateTime.Parse(Convert.ToString(row["birthDate"])).ToString("dd.MM.yyyy"),
                            RoomNumber = row["roomNumber"].ToString(),
                            RoomID = Convert.ToInt32(row["room"])
                        });

                    var wordManager = new WordDocumentManager();

                    foreach (var booking in bookingInformationList)
                    {
                        var clientProfileData = new Dictionary<string, string>
                        {
                            { "LastName", booking.ClientLastName },
                            { "FirstName", booking.ClientFirstName },
                            { "MiddleName", booking.ClientMiddleName },
                            { "BirthDate", booking.ClientBirthDate },
                            { "PhoneNumber", booking.ClientPhoneNumber },
                            { "Email", booking.ClientEmail },
                            { "RoomNumber", booking.RoomNumber }
                        };

                        string clientProfileTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\Resources"), "ClientProfile.docx");
                        string clientProfileOutputPath = $"ClientProfile_{booking.ClientLastName}_{booking.ClientFirstName}.docx";
                        wordManager.FillTemplate(clientProfileTemplatePath, clientProfileOutputPath, clientProfileData);

                        string queryRoom = "UPDATE rooms SET isFree=@isFree WHERE roomID=@roomID;";
                        string connectionString = new ReadConfigFile().GetConnectionString();
                        var db = new ConnectionInfo(connectionString);

                        var roomParameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@isFree", 'f'),
                            new MySqlParameter("@roomID", booking.RoomID)
                        };

                        db.ExecuteCommand(queryRoom, roomParameters);
                    }
                }
            }
        }
    }
}
