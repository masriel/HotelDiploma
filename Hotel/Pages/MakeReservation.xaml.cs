using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hotel.Classes;
using Hotel.ItemControl;
using Xceed.Wpf.AvalonDock.Themes;

namespace Hotel.Pages
{
    /// <summary>
    /// Interaction logic for MakeReservation.xaml
    /// </summary>
    public partial class MakeReservation : Window
    {
        private ObservableCollection<Rooms> _freeRooms;
        private ObservableCollection<Meals> _meal;

        private Dictionary<int, string> _clients = new Dictionary<int, string>();
        private Dictionary<int, int> _meals = new Dictionary<int, int>();

        public MakeReservation()
        {
            InitializeComponent();
        }

        private void ReservationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetDates();
            PopulateFreeRoomsComboBox();
        }

        private void PopulateFreeRoomsComboBox()
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
            foreach (var room in freeRooms) { 
                SelectRoomBox.Items.Add($"{room.Number} | {room.Type} | {room.Cost} руб./ночь");
            }
        }

        private void SetDates()
        {
            ArrivalDatePicker.SelectedDate = DateTime.Now;
            DepartureDatePicker.SelectedDate = DateTime.Now.AddDays(1);
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            //AddClient addClient = new AddClient();
            //if (addClient.ShowDialog() == true) { ClientsList.Items.Add(addClient.Name); _clients.Add(addClient.ID, addClient.Name); }
            ClientsList.Items.Add("Иванов Иван Иванович");
            _clients.Add(1, "Иванов Иван Иванович");
        }

        private void AddMealButton_Click(object sender, RoutedEventArgs e)
        {
            SelectMeal meal = new SelectMeal();
            if(meal.ShowDialog() == true) { MealsList.Items.Add( $"{meal.Name} ({meal.Quantity})" ); _meals.Add(meal.ID, meal.Quantity);  }
        }

        private void AddBookingButton_Click(object sender, RoutedEventArgs e)
        {
            string _arrival = ArrivalDatePicker.SelectedDate != null ? DateTime.Parse(Convert.ToString(ArrivalDatePicker.SelectedDate)).ToString("yyyy-MM-dd") : "";
            string _departure = DepartureDatePicker.SelectedDate != null ? DateTime.Parse(Convert.ToString(DepartureDatePicker.SelectedDate)).ToString("yyyy-MM-dd") : "";
            int days = (DepartureDatePicker.SelectedDate.Value - ArrivalDatePicker.SelectedDate.Value).Days;
            int _room = _freeRooms[SelectRoomBox.SelectedIndex].ID;
            int[] _mealsID = _meals.Keys.ToArray();
            int[] _mealsQuantity = _meals.Values.ToArray();
            int[] _clientsID = _clients.Keys.ToArray();
            double amount = CalculateFinalAmount(_clientsID.Length, _mealsID, _mealsQuantity, days);
        }

        private double CalculateFinalAmount(int clients, int[] mealsId, int[] mealsQuantity, int days)
        {
            double amount = 0;
            double _roomCost = Convert.ToDouble(_freeRooms[SelectRoomBox.SelectedIndex].Cost);
            double _mealsCost = 0;
            for (int i = 0; i < mealsId.Length; i++)
            {
                _mealsCost = mealsId[i] == 1 ? 700 * mealsQuantity[i] : mealsId[i] == 2 ? 950 * mealsQuantity[i] : 800 * mealsQuantity[i];
            }

            amount = (_roomCost * days + _mealsCost) * clients;

            return amount;
        }

        private void ClientsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ClientsList.SelectedItem != null)
            {
                ClientsList.Items.Remove(ClientsList.SelectedItem);
                _clients.Remove(ClientsList.SelectedIndex);
            }
        }

        private void MealsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MealsList.SelectedItem != null)
            {
                MealsList.Items.Remove(MealsList.SelectedItem);
                _meals.Remove(MealsList.SelectedIndex);
            }
        }
    }
}
