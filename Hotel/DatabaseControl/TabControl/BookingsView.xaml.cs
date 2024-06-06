using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Hotel.Classes;
using System.Security.Cryptography;

namespace Hotel.DatabaseControl.TabControl
{
    /// <summary>
    /// Interaction logic for BookingsView.xaml
    /// </summary>
    public partial class BookingsView : UserControl
    {
        private readonly string _connectionString;
        private readonly ConnectionInfo _db;
        private readonly Navigation _navigation = new Navigation();

        private DataTable _bookings;
        private DataTable _bookingsOriginal;

        public BookingsView()
        {
            InitializeComponent();
            var config = new ReadConfigFile();
            _connectionString = config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        private void BookingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

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

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // Restore original table if search string is empty
                Bookings.ItemsSource = _bookingsOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            var dv = new DataView(_bookingsOriginal)
            {
                RowFilter = $"bookingNumber LIKE '%{searchText}%'"
            };

            var newTable = _bookingsOriginal.Clone(); // Clone original table structure

            // Add filtered rows to the new table
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            // Add remaining rows
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

        private void DeleteBooking(int bookingId)
        {
            try
            {
                // Retrieve related booking clients and meals IDs before deleting the booking
                var bookingData = _db.GetData($"SELECT bookingClientsID FROM BookingClients WHERE booking = {bookingId}");
                var bookingMealsData = _db.GetData($"SELECT bookingMealID FROM BookingMeals WHERE booking = {bookingId}");

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    try
                    {
                        // Delete booking clients
                        foreach (DataRow row in bookingData.Rows)
                        {
                            var bookingClientsId = Convert.ToInt32(row["bookingClientsID"]);
                            _db.ExecuteCommand("DELETE FROM BookingClients WHERE bookingClientsID = @BookingClientsID", new MySqlParameter("@BookingClientsID", bookingClientsId));
                        }

                        // Delete booking meals
                        foreach (DataRow row in bookingMealsData.Rows)
                        {
                            var bookingMealID = Convert.ToInt32(row["bookingMealID"]);
                            _db.ExecuteCommand("DELETE FROM BookingMeals WHERE bookingMealID = @bookingMealID", new MySqlParameter("@bookingMealID", bookingMealID));
                        }

                        // Delete booking
                        _db.ExecuteCommand("DELETE FROM Bookings WHERE bookingID = @BookingID", new MySqlParameter("@BookingID", bookingId));

                        // Commit transaction
                        transaction.Commit();

                        MessageBox.Show("Запись удалена!", "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData(); // Reload data after deletion
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction in case of an error
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
