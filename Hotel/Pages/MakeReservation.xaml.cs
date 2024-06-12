using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Hotel.Classes;
using Hotel.ItemControl;

using MySql.Data.MySqlClient;

namespace Hotel.Pages
{
    public partial class MakeReservation : Window
    {
        private ObservableCollection<Rooms> _freeRooms;
        private ObservableCollection<Meals> _meals = new ObservableCollection<Meals>();
        private ObservableCollection<Clients> _clients = new ObservableCollection<Clients>();

        public MakeReservation()
        {
            InitializeComponent();
        }

        private void ReservationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //устанавливаем даты заезда и выезда по умолчанию
            SetDates();

            //получаем свободные номера для заселения/бронирования
            PopulateFreeRoomsComboBox();
        }

        //метод для заполнения ComboBox со свободными номерами
        private void PopulateFreeRoomsComboBox()
        {
            try
            {
                var connection = new ConnectionInfo(new ReadConfigFile().GetConnectionString());
                var roomData = connection.GetData("SELECT roomID, roomNumber, RoomTypes.roomType, roomCost FROM Rooms LEFT JOIN RoomTypes ON Rooms.roomType=roomTypeID WHERE isFree = 't'");

                var freeRooms = from DataRow row in roomData.Rows
                                select new Rooms
                                {
                                    ID = Convert.ToInt32(row["roomID"]),
                                    Number = row["roomNumber"].ToString(),
                                    Type = row["roomType"].ToString(),
                                    Cost = row["roomCost"].ToString()
                                };

                _freeRooms = new ObservableCollection<Rooms>(freeRooms);
                foreach (var room in freeRooms)
                {
                    SelectRoomBox.Items.Add($"{room.Number} | {room.Type} | {room.Cost} руб./ночь");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке свободных номеров: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //метод для установки дат по умолчанию
        private void SetDates()
        {
            ArrivalDatePicker.SelectedDate = DateTime.Now;
            DepartureDatePicker.SelectedDate = DateTime.Now.AddDays(1);
        }

        //метод для добавления нового клиента
        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно для добавления клиента
            AddClient addClientWindow = new AddClient();
            if (addClientWindow.ShowDialog() == true)
            {
                // Добавляем клиента в список клиентов
                var newClient = addClientWindow.NewClient;
                ClientsList.Items.Add(new ListBoxItem
                {
                    Content = $"{newClient.LastName} {newClient.FisrtName} {newClient.MiddleName}",
                    Tag = newClient
                });
                _clients.Add(newClient);
            }
        }

        //метод для добавления питания
        private void AddMealButton_Click(object sender, RoutedEventArgs e)
        {
            SelectMeal meal = new SelectMeal();
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
                    Content = $"{meal.Name} ({meal.Quantity}) | {meal.Cost} руб.",
                    Tag = newMeal
                });
            }
        }

        //метод добавления бронирования
        private void AddBookingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка выбора даты прибытия и отъезда
                if (ArrivalDatePicker.SelectedDate == null || DepartureDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Пожалуйста, выберите даты прибытия и отъезда.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка выбора номера
                if (SelectRoomBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Пожалуйста, выберите номер.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка наличия хотя бы одного клиента
                if (_clients.Count == 0)
                {
                    MessageBox.Show("Пожалуйста, добавьте хотя бы одного клиента.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка на правильность выбора даты (дата отъезда должна быть позже даты прибытия)
                if (DepartureDatePicker.SelectedDate <= ArrivalDatePicker.SelectedDate)
                {
                    MessageBox.Show("Дата отъезда должна быть позже даты прибытия.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string arrivalDate = ArrivalDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
                string departureDate = DepartureDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
                int days = (DepartureDatePicker.SelectedDate.Value - ArrivalDatePicker.SelectedDate.Value).Days;
                int roomID = _freeRooms[SelectRoomBox.SelectedIndex].ID;
                int[] clientsID = _clients.Select(c => c.ID).ToArray();
                double amount = CalculateFinalAmount(clientsID.Length, _meals.ToArray(), days);
                string bookingNumber = GenerateBookingNumber();

                string connectionString = new ReadConfigFile().GetConnectionString();
                ConnectionInfo db = new ConnectionInfo(connectionString);

                // Вставка данных бронирования
                string queryBooking = "INSERT INTO bookings (bookingNumber, arrivalDate, departureDate, room, amount) " +
                                      "VALUES (@BookingNumber, @ArrivalDate, @DepartureDate, @Room, @Amount); " +
                                      "SELECT LAST_INSERT_ID();";

                MySqlParameter[] bookingParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@BookingNumber", bookingNumber),
                    new MySqlParameter("@ArrivalDate", arrivalDate),
                    new MySqlParameter("@DepartureDate", departureDate),
                    new MySqlParameter("@Room", roomID),
                    new MySqlParameter("@Amount", amount)
                };

                int bookingID = db.ExecuteInsertAndGetId(queryBooking, bookingParameters);

                if (DateTime.Parse(arrivalDate) == DateTime.Now.Date)
                {
                    string queryRoom = "INSERT INTO rooms (isFree) VALUES (@isFree) WHERE roomID=@roomID; ";

                    MySqlParameter[] roomParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@isFree", 'f'),
                        new MySqlParameter("@roomID", roomID)
                    };

                    db.ExecuteInsertAndGetId(queryRoom, roomParameters);
                }

                // Вставка данных клиентов бронирования
                foreach (int clientID in clientsID)
                {
                    string queryBookingClients = "INSERT INTO bookingclients (booking, client) VALUES (@BookingID, @ClientID);";
                    MySqlParameter[] bookingClientsParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@BookingID", bookingID),
                        new MySqlParameter("@ClientID", clientID)
                    };
                    db.ExecuteCommand(queryBookingClients, bookingClientsParameters);
                }

                // Вставка данных блюд бронирования
                foreach (Meals meal in _meals)
                {
                    string queryBookingMeals = "INSERT INTO bookingmeals (booking, meal, quantity) VALUES (@BookingID, @MealID, @Quantity);";
                    MySqlParameter[] bookingMealsParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@BookingID", bookingID),
                        new MySqlParameter("@MealID", meal.ID),
                        new MySqlParameter("@Quantity", meal.Quantity)
                    };
                    db.ExecuteCommand(queryBookingMeals, bookingMealsParameters);
                }

                // Генерация документов
                GenerateDocuments(bookingNumber, arrivalDate, departureDate, days, amount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении бронирования: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //формирование документов
        private void GenerateDocuments(string bookingNumber, string arrivalDate, string departureDate, int days, double amount)
        {
            WordDocumentManager wordManager = new WordDocumentManager();

            // Если заезд не с текущего дня, то генерируем ваучер бронирования
            if (DateTime.Parse(arrivalDate) > DateTime.Now.Date)
            {
                // Подготовка данных для ваучера
                var voucherData = new Dictionary<string, string>
                {
                    { "BookingNumber", bookingNumber },
                    { "Weekday1", DateTime.Parse(arrivalDate).ToString("dddd") },
                    { "Day1", DateTime.Parse(arrivalDate).ToString("dd") },
                    { "Month1", DateTime.Parse(arrivalDate).ToString("MMMM") },
                    { "Year1", DateTime.Parse(arrivalDate).ToString("yyyy") },
                    { "Weekday2", DateTime.Parse(departureDate).ToString("dddd") },
                    { "Day2", DateTime.Parse(departureDate).ToString("dd") },
                    { "Month2", DateTime.Parse(departureDate).ToString("MMMM") },
                    { "Year2", DateTime.Parse(departureDate).ToString("yyyy") },
                    { "Info", SelectRoomBox.Text },
                    { "Days", days.ToString() },
                    { "Clients", _clients.Count.ToString() },
                    { "Amount", amount.ToString() }
                };
                string voucherTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\Resources"), "Voucher.docx");
                string voucherOutputPath = $"Voucher_{bookingNumber}.docx";
                wordManager.FillTemplate(voucherTemplatePath, voucherOutputPath, voucherData);
            }

            // Подготовка данных для чека об оплате
            var paymentReceiptData = new Dictionary<string, string>
            {
                { "RandNumber", new Random().Next(100000, 999999).ToString() },
                { "Day", DateTime.Now.ToString("dd") },
                { "Month", DateTime.Now.ToString("MMMM") },
                { "Year", DateTime.Now.ToString("yyyy") },
                { "Clients", string.Join(", ", _clients.Select(c => c.LastName + " " + c.FisrtName)) },
                { "RoomNumber", _freeRooms[SelectRoomBox.SelectedIndex].Number },
                { "ArrivalDate", DateTime.Parse(arrivalDate).ToString("dd.MM.yyyy") },
                { "DepartureDate", DateTime.Parse(departureDate).ToString("dd.MM.yyyy") },
                { "Days", days.ToString() },
                { "RoomCost", _freeRooms[SelectRoomBox.SelectedIndex].Cost },
                { "RoomAmount", (Convert.ToDouble(_freeRooms[SelectRoomBox.SelectedIndex].Cost) * days).ToString() },
                { "Meals", string.Join(", ", _meals.Select(m => m.Name)) },
                { "MealAmount", _meals.Sum(m => m.Cost * m.Quantity).ToString() },
                { "BookingAmount", amount.ToString() }
            };
            string paymentReceiptTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\Resources"), "PaymentReceipt.docx");
            string paymentReceiptOutputPath = $"PaymentReceipt_{bookingNumber}.docx";
            wordManager.FillTemplate(paymentReceiptTemplatePath, paymentReceiptOutputPath, paymentReceiptData);

            // Если заезд с текущего дня, то генерируем анкеты клиентов
            if (DateTime.Parse(arrivalDate) == DateTime.Now.Date)
            {
                foreach (var client in _clients)
                {
                    var clientProfileData = new Dictionary<string, string>
                    {
                        { "LastName", client.LastName },
                        { "FirstName", client.FisrtName },
                        { "MiddleName", client.MiddleName },
                        { "BirthDate", client.BirthDate },
                        { "PhoneNumber", client.PhoneNumber },
                        { "Email", client.Email },
                        { "RoomNumber", _freeRooms[SelectRoomBox.SelectedIndex].Number }
                    };
                    string clientProfileTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\Resources"), "ClientProfile.docx");
                    string clientProfileOutputPath = $"ClientProfile_{client.LastName}_{client.FisrtName}.docx";
                    wordManager.FillTemplate(clientProfileTemplatePath, clientProfileOutputPath, clientProfileData);
                }
            }
        }

        //расчет итоговой суммы
        private double CalculateFinalAmount(int clients, Meals[] meals, int days)
        {
            double amount = 0;
            double roomCost = Convert.ToDouble(_freeRooms[SelectRoomBox.SelectedIndex].Cost);
            double mealsCost = meals.Sum(meal => meal.Cost * meal.Quantity);

            // Стоимость проживания и питания на всех клиентов
            amount = (roomCost * days + mealsCost) * clients;

            return amount;
        }

        //генерация номера бронирования
        private string GenerateBookingNumber()
        {
            var random = new Random();
            char firstLetter = (char)('A' + random.Next(0, 26));
            char lastLetter = (char)('A' + random.Next(0, 26));
            string digits = random.Next(0, 1000).ToString("D3");

            return $"{firstLetter}{digits}{lastLetter}";
        }

        //удаления клиента из бронирования
        private void ClientsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ClientsList.SelectedItem != null)
            {
                int selectedIndex = ClientsList.SelectedIndex;
                ClientsList.Items.RemoveAt(selectedIndex);
                _clients.RemoveAt(selectedIndex);
            }
        }

        //удаление питания из бронирования
        private void MealsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MealsList.SelectedItem != null)
            {
                int selectedIndex = MealsList.SelectedIndex;
                var selectedMeal = (Meals)((ListBoxItem)MealsList.SelectedItem).Tag;
                _meals.Remove(selectedMeal);
                MealsList.Items.RemoveAt(selectedIndex);
            }
        }
    }
}
