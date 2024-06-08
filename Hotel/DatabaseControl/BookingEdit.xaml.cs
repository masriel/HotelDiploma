using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Hotel.Classes;
using Hotel.ItemControl;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Логика взаимодействия для BookingEdit.xaml
    /// </summary>
    public partial class BookingEdit : Window
    {
        private readonly string _connectionString;
        private readonly ConnectionInfo _db;
        private readonly ReadConfigFile _config = new ReadConfigFile();
        private int _id, _item = 0, _countClients = 0, _countDay = 1;
        private double _finalAmount;
        private List<Rooms> _rooms = new List<Rooms>();
        private List<Meals> _meals = new List<Meals>();

        public BookingEdit(int id)
        {
            InitializeComponent();
            _id = id;
            _connectionString = _config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        private void BookingEditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Сохранить данную информацию?", "РЕДАКТИРОВАНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                EditBooking();
            }
            e.Cancel = false;
        }

        private void EditBooking()
        {
            Amount.Text = CalculateAmount().ToString();

            int roomId = Convert.ToInt32(_rooms[RoomsFree.SelectedIndex].ID);
            var mealIDs = new Dictionary<int, int>();
            foreach (var meal in _meals)
            {
                mealIDs.Add(meal.ID, meal.Quantity);
            }

            if (!ValidateDates(out string arrivalDate, out string departureDate))
            {
                MessageBox.Show("Дата отъезда не может быть раньше даты прибытия.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UpdateBookingInDatabase(roomId, mealIDs, arrivalDate, departureDate, _finalAmount);
        }

        private void UpdateBookingInDatabase(int roomId, Dictionary<int, int> mealIDs, string arrivalDate, string departureDate, double amount)
        {
            try
            {
                string query = $"UPDATE Bookings SET arrivalDate='{arrivalDate}', departureDate='{departureDate}', room={roomId}, amount={amount} WHERE bookingID={_id};";
                query += $"DELETE FROM BookingMeals WHERE booking={_id};";
                foreach (var meal in mealIDs)
                {
                    query += $"INSERT INTO BookingMeals (booking, meal, quantity) VALUES ({_id}, {meal.Key}, {meal.Value});";
                }

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand(query, connection);
                    int result = command.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        MessageBox.Show("Информация не обновлена.", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                        DialogResult = false;
                    }
                    else
                    {
                        DialogResult = true;
                        UpdateRoomStatus(roomId, "f");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateDates(out string arrivalDate, out string departureDate)
        {
            DateTime arr = ArrivalDatePicker.SelectedDate.Value;
            DateTime dep = DepartureDatePicker.SelectedDate.Value;

            if (dep < arr)
            {
                arrivalDate = null;
                departureDate = null;
                return false;
            }

            arrivalDate = arr.ToString("yyyy-MM-dd");
            departureDate = dep.ToString("yyyy-MM-dd");
            return true;
        }

        private void UpdateRoomStatus(int roomId, string status)
        {
            try
            {
                string query = $"UPDATE Rooms SET isFree='{status}' WHERE roomID={roomId}";
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса комнаты: {ex.Message}");
            }
        }

        private void AddMeal_Click(object sender, RoutedEventArgs e)
        {
            if (MealsList.Items.Count < 3)
            {
                var meal = new SelectMeal();
                if (meal.ShowDialog() == true)
                {
                    var newMeal = new Meals
                    {
                        ID = meal.ID,
                        Name = meal.Name,
                        Quantity = meal.Quantity
                    };

                    _meals.Add(newMeal);
                    MealsList.Items.Add(new ListBoxItem
                    {
                        Content = $"{meal.Name} ({meal.Quantity}) | {meal.Cost * meal.Quantity} руб.",
                        Tag = newMeal
                    });
                }
            }
        }

        private void MealsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MealsList.SelectedItem is ListBoxItem selectedItem)
            {
                Meals selectedMeal = (Meals)selectedItem.Tag;
                _meals.Remove(selectedMeal);
                MealsList.Items.Remove(selectedItem);
            }
        }

        private void CalculateAmountButton_Click(object sender, RoutedEventArgs e)
        {
            Amount.Text = CalculateAmount().ToString();
        }

        private double CalculateAmount()
        {
            int i = RoomsFree.SelectedIndex;
            double roomCost = Convert.ToDouble(_rooms[i].Cost);

            double mealCost = 0;
            foreach (var item in _meals)
            {
                mealCost += item.Cost * item.Quantity;
            }

            _finalAmount = (roomCost * _countDay + mealCost) * _countClients;
            return _finalAmount;
        }

        private void LoadBookingDetails()
        {
            try
            {
                string query = $"SELECT * FROM Bookings " +
                               $"LEFT JOIN Rooms ON room=roomID " +
                               $"LEFT JOIN BookingClients ON bookingID=BookingClients.booking " +
                               $"LEFT JOIN BookingMeals ON bookingID=BookingMeals.booking " +
                               $"WHERE bookingID={_id};";

                var bookingDetails = _db.GetData(query);
                if (bookingDetails.Rows.Count > 0)
                {
                    DataRow row = bookingDetails.Rows[0];
                    PopulateBookingDetails(row);
                    _rooms = GetAvailableRooms(Convert.ToInt32(row["room"]));
                    RoomsFree.SelectedIndex = _item;
                    _meals = LoadMeals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных бронирования: {ex.Message}");
            }
        }

        private void PopulateBookingDetails(DataRow row)
        {
            _countClients++;
            BookNumber.Text = row["bookingNumber"].ToString();
            ArrivalDatePicker.SelectedDate = Convert.ToDateTime(row["arrivalDate"]);
            DepartureDatePicker.SelectedDate = Convert.ToDateTime(row["departureDate"]);
            Amount.Text = row["amount"].ToString();
        }

        private List<Rooms> GetAvailableRooms(int currentRoomId)
        {
            var rooms = new List<Rooms>();
            try
            {
                string query = $"SELECT roomID, roomNumber, Rooms.roomType AS type, isFree, roomTypeID, RoomTypes.roomType, maxOccupancy, roomCost, roomPhoto, roomDescription FROM Rooms LEFT JOIN RoomTypes ON Rooms.roomType=roomTypeID WHERE isFree = 't' OR roomID = {currentRoomId}";

                var roomData = _db.GetData(query);
                foreach (DataRow row in roomData.Rows)
                {
                    rooms.Add(new Rooms
                    {
                        ID = Convert.ToInt32(row["roomID"]),
                        Cost = Convert.ToString(row["roomCost"]),
                        Description = $"{row["roomNumber"]} | {row["roomType"]} | {row["roomCost"]} руб./ночь"
                    });

                    RoomsFree.Items.Add($"{row["roomNumber"]} | {row["roomType"]} | {row["roomCost"]} руб./ночь");
                    if (Convert.ToInt32(row["roomID"]) == currentRoomId)
                    {
                        _item = RoomsFree.Items.Count - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о комнатах: {ex.Message}");
            }

            return rooms;
        }

        private List<Meals> LoadMeals()
        {
            var meals = new List<Meals>();
            try
            {
                string query = $"SELECT * FROM Meals LEFT JOIN BookingMeals ON mealID=BookingMeals.meal WHERE booking={_id}";
                var mealsData = _db.GetData(query);

                foreach (DataRow row in mealsData.Rows)
                {
                    var meal = new Meals
                    {
                        ID = Convert.ToInt32(row["mealID"]),
                        Name = Convert.ToString(row["mealName"]),
                        Quantity = Convert.ToInt32(row["quantity"])
                    };

                    meals.Add(meal);
                    MealsList.Items.Add(new ListBoxItem
                    {
                        Content = $"{row["mealName"]} ({row["quantity"]}) | {meal.Cost * meal.Quantity} руб.",
                        Tag = meal
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о питании: {ex.Message}");
            }

            return meals;
        }

        private void BookingEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookingDetails();
            if (!ValidateDates(out _, out _))
            {
                MessageBox.Show("Дата отъезда не может быть раньше даты прибытия.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _countDay = (DepartureDatePicker.SelectedDate.Value - ArrivalDatePicker.SelectedDate.Value).Days;
            }
        }
    }
}
