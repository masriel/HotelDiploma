using Hotel.Classes;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using MySql.Data.MySqlClient;

namespace Hotel.DatabaseControl.TabControl
{
    /// <summary>
    /// Interaction logic for BookingsView.xaml
    /// </summary>
    public partial class BookingsView : UserControl
    {
        private string CONNECTION_STRING = String.Empty;

        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private ConnectionInfo db;
        private ReadConfigFile _config = new ReadConfigFile();
        Navigation NAVIGATION = new Navigation();

        private DataTable bookings = new DataTable();
        private DataTable bookingsOriginal;

        public BookingsView()
        {
            InitializeComponent();

            CONNECTION_STRING = _config.GetConnectionString();
            db = new ConnectionInfo(CONNECTION_STRING);
        }

        private void LoadData()
        {
            bookings = db.GetData("select bookingClientsID, client, bookingID, arrivalDate, departureDate, room, bookingMealID, meal, quantity " +
                "from BookingClients " +
                "left join Bookings on BookingClients.booking=bookingID " +
                "left join BookingMeals on BookingClients.booking=bookingID;");
            bookingsOriginal = bookings.Copy();

            if (bookings != null)
            {
                Bookings.ItemsSource = bookings.DefaultView;
                ConfigureDataGrid();
            }
        }

        private void ConfigureDataGrid()
        {
            foreach (DataGridColumn column in Bookings.Columns)
            {
                
            }
        }

        private void BookingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
