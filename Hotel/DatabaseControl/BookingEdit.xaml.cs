using Hotel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Interaction logic for BookingEdit.xaml
    /// </summary>
    public partial class BookingEdit : Window
    {
        private string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private ConnectionInfo db;
        private ReadConfigFile _config = new ReadConfigFile();
        //bookingID, bookingNumber, arrivalDate, departureDate, room, amount
        //bookingMealID, booking, meal, quantity
        private int ID, item = 0;
        List<Rooms> _rooms;

        public BookingEdit(int id)
        {
            InitializeComponent();

            ID = id;

            CONNECTION_STRING = _config.GetConnectionString();
            db = new ConnectionInfo(CONNECTION_STRING);
        }

        private void BookingEditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Сохранить данную информацию?", "РЕДАКТИРОВАНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                
            }
            e.Cancel = false;
        }

        private void AddMeal_Click(object sender, RoutedEventArgs e)
        {
            if (MealsList.Items.Count < 3)
            {
                MealsList.Items.Add(new ListViewItem().Content = "Ужин (2) | 950 руб.");
            }
        }

        private void CalculateAmountButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadBookingDetails()
        {
            try
            {
                string query = $"SELECT * FROM Bookings " +
                    $"LEFT JOIN Rooms ON room=roomID " +
                    $"LEFT JOIN BookingClients ON bookingID=BookingClients.booking " +
                    $"LEFT JOIN BookingMeals ON bookingID=BookingMeals.booking " +
                    $"WHERE bookingID={ID};";

                DataTable bookingDetails = db.GetData(query);

                if (bookingDetails.Rows.Count > 0)
                {
                    DataRow row = bookingDetails.Rows[0];

                    BookNumber.Text = row["bookingNumber"].ToString();
                    ArrivalDatePicker.SelectedDate = Convert.ToDateTime(row["arrivalDate"]);
                    DepartureDatePicker.SelectedDate = Convert.ToDateTime(row["departureDate"]);
                    _rooms = GetAvailableRooms(Convert.ToInt32(row["room"]));
                    RoomsFree.SelectedIndex = item;
                    Amount.Text = row["amount"].ToString();

                    LoadMeals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading booking details: {ex.Message}");
            }
        }

        private List<Rooms> GetAvailableRooms(int currentRoomId)
        {
            List<Rooms> rooms = new List<Rooms>();

            try
            {
                string query = $"SELECT roomID, roomNumber, Rooms.roomType AS type, isFree, roomTypeID, RoomTypes.roomType, maxOccupancy, roomCost, roomPhoto, roomDescription FROM Rooms LEFT JOIN RoomTypes ON Rooms.roomType=roomTypeID WHERE isFree = 't' OR roomID = {currentRoomId}";

                DataTable roomData = db.GetData(query);

                foreach (DataRow row in roomData.Rows)
                {
                    rooms.Add(new Rooms
                    {
                        ID = Convert.ToInt32(row["roomID"]),
                        Description = $"{row["roomNumber"]} | {row["roomType"]} | {row["roomCost"]} руб./ночь"
                    });

                    RoomsFree.Items.Add($"{row["roomNumber"]} | {row["roomType"]} | {row["roomCost"]} руб./ночь");
                    if (Convert.ToInt32(row["roomID"]) == currentRoomId) { item = RoomsFree.Items.Count - 1; }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rooms: {ex.Message}");
            }

            return rooms;
        }

        private void LoadMeals()
        {
            try
            {
                string query = $"select * from Meals left join BookingMeals on mealID=BookingMeals.meal WHERE booking = {ID}";
                DataTable mealsData = db.GetData(query);

                MealsList.Items.Clear();

                foreach (DataRow row in mealsData.Rows)
                {
                    MealsList.Items.Add(new ListBoxItem
                    {
                        Content = $"{row["mealName"]} ({row["quantity"]}) | {row["mealCost"]} руб."
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading meals: {ex.Message}");
            }
        }

        private void BookingEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookingDetails();
        }
    }
}
