using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

using MySql.Data.MySqlClient;

using Hotel.Classes;

namespace Hotel.DatabaseControl.TabControl
{
    /// <summary>
    /// Логика взаимодействия для BookingsView.xaml
    /// </summary>
    public partial class BookingsView : UserControl
    {
        private readonly string _connectionString; // Строка подключения к базе данных
        private readonly ConnectionInfo _db; // Объект для работы с базой данных
        private readonly Navigation _navigation = new Navigation(); // Объект для навигации (не используется)

        private DataTable _bookings; // Таблица с бронированиями
        private DataTable _bookingsOriginal; // Исходная таблица бронирований

        // Конструктор класса
        public BookingsView()
        {
            InitializeComponent();
            var config = new ReadConfigFile();
            _connectionString = config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        // Метод, вызываемый при загрузке окна
        private void BookingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Метод для загрузки данных из базы данных
        private void LoadData()
        {
            const string query = "SELECT bookingID, bookingClientsID, bookingNumber, " +
                "client, lastName, firstName, phoneNumber, passport, birthCertificate, " +
                "DATE_FORMAT(arrivalDate, '%d.%m.%Y') as arrivalDate, DATE_FORMAT(departureDate, '%d.%m.%Y') as departureDate, " +
                "room, roomNumber, " +
                "meal, mealName, quantity, mealCost, " +
                "amount FROM BookingClients " +
                "LEFT JOIN Bookings ON BookingClients.booking = bookingID " +
                "LEFT JOIN BookingMeals ON BookingClients.booking = BookingMeals.booking " +
                "LEFT JOIN Meals ON meal = mealID LEFT JOIN Rooms ON room = roomID " +
                "LEFT JOIN Clients ON client = clientID " +
                "ORDER BY bookingNumber;";

            _bookings = _db.GetData(query);

            if (_bookings != null)
            {
                _bookingsOriginal = _bookings.Copy();
                Bookings.ItemsSource = _bookings.DefaultView;
                ConfigureDataGrid();
            }
        }

        // Метод для конфигурации DataGrid
        private void ConfigureDataGrid()
        {
            foreach (DataGridColumn column in Bookings.Columns)
            {
                switch (column.Header.ToString())
                {
                    case "bookingNumber":
                        column.Header = "Рег. номер";
                        break;
                    case "arrivalDate":
                        column.Header = "Заселение";
                        break;
                    case "departureDate":
                        column.Header = "Выселение";
                        break;
                    case "lastName":
                        column.Header = "Фамилия";
                        break;
                    case "firstName":
                        column.Header = "Имя";
                        break;
                    case "phoneNumber":
                        column.Header = "Телефон";
                        break;
                    case "roomNumber":
                        column.Header = "Комната";
                        break;
                    default:
                        column.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        // Метод для поиска в таблице бронирований
        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // Восстановление исходной таблицы, если строка поиска пуста
                Bookings.ItemsSource = _bookingsOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            var dv = new DataView(_bookingsOriginal)
            {
                RowFilter = $"bookingNumber LIKE '%{searchText}%'"
            };

            var newTable = _bookingsOriginal.Clone(); // Клонирование структуры исходной таблицы

            // Добавление отфильтрованных строк в новую таблицу
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            // Добавление оставшихся строк
            foreach (DataRow row in _bookingsOriginal.Rows)
            {
                var name = row["bookingNumber"].ToString().ToLower();

                if (!name.Contains(searchText))
                {
                    newTable.ImportRow(row);
                }
            }

            Bookings.ItemsSource = newTable.DefaultView;
            ConfigureDataGrid();

            if (newTable.Rows.Count > 0)
            {
                Bookings.SelectedIndex = 0;
                Bookings.ScrollIntoView(Bookings.SelectedItem);
            }
        }

        // Метод для удаления бронирования
        private void DeleteBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (Bookings.SelectedItem is DataRowView selectedRow)
            {
                var bookingId = Convert.ToInt32(selectedRow["bookingID"]);
                var bookingNumber = Convert.ToString(selectedRow["bookingNumber"]);

                if (MessageBox.Show($"Вы уверены, что хотите удалить запись {bookingNumber}?", "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DeleteBooking(bookingId);
                    LoadData();
                }
            }
        }

        // Метод для удаления бронирования из базы данных
        private void DeleteBooking(int bookingId)
        {
            try
            {
                // Получение связанных клиентов и блюд перед удалением бронирования
                var bookingData = _db.GetData($"SELECT bookingClientsID FROM BookingClients WHERE booking = {bookingId}");
                var bookingMealsData = _db.GetData($"SELECT bookingMealID FROM BookingMeals WHERE booking = {bookingId}");

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    try
                    {
                        // Удаление связанных клиентов бронирования
                        foreach (DataRow row in bookingData.Rows)
                        {
                            var bookingClientsId = Convert.ToInt32(row["bookingClientsID"]);
                            _db.ExecuteCommand("DELETE FROM BookingClients WHERE bookingClientsID = @BookingClientsID", new MySqlParameter("@BookingClientsID", bookingClientsId));
                        }

                        // Удаление связанных блюд бронирования
                        foreach (DataRow row in bookingMealsData.Rows)
                        {
                            var bookingMealID = Convert.ToInt32(row["bookingMealID"]);
                            _db.ExecuteCommand("DELETE FROM BookingMeals WHERE bookingMealID = @bookingMealID", new MySqlParameter("@bookingMealID", bookingMealID));
                        }

                        // Удаление самого бронирования
                        _db.ExecuteCommand("DELETE FROM Bookings WHERE bookingID = @BookingID", new MySqlParameter("@BookingID", bookingId));

                        // Подтверждение транзакции
                        transaction.Commit();

                        MessageBox.Show("Запись удалена!", "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData(); // Перезагрузка данных после удаления
                    }
                    catch (Exception ex)
                    {
                        // Откат транзакции в случае ошибки
                        transaction.Rollback();
                        MessageBox.Show(ex.Message, "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для редактирования бронирования
        private void EditBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (Bookings.SelectedItem is DataRowView selectedRow)
            {
                var bookingId = Convert.ToInt32(selectedRow["bookingID"]);

                if (new BookingEdit(bookingId).ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }
    }
}
