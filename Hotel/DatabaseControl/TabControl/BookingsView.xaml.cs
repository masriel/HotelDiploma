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
            bookings = db.GetData("select bookingClientsID, BookingClients.booking, client, bookingNumber, DATE_FORMAT(arrivalDate, '%d.%m.%Y') as arrivalDate, DATE_FORMAT(departureDate, '%d.%m.%Y') as departureDate, room, meal, quantity, mealName, mealCost, roomNumber, roomType, firstName, lastName, middleName, birthDate, phoneNumber, email, passport, birthCertificate " +
                "from BookingClients " +
                "left join Bookings on BookingClients.booking = bookingID " +
                "left join BookingMeals on BookingClients.booking = bookingID " +
                "left join Meals on meal = mealID " +
                "left join Rooms on room = roomID " +
                "left join Clients on client = clientID " +
                " order by bookingNumber;");

            if (bookings != null)
            {
                bookingsOriginal = bookings.Copy();
                Bookings.ItemsSource = bookings.DefaultView;
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
                    case "mealName":
                        column.Header = "Питание";
                        break;
                    default:
                        column.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void BookingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // Восстанавливаем исходную таблицу, если строка поиска пустая
                Bookings.ItemsSource = bookingsOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            DataView dv = new DataView(bookingsOriginal);
            dv.RowFilter = $"bookingNumber LIKE '%{searchText}%'";

            DataTable newTable = bookingsOriginal.Clone(); // Копируем структуру исходной таблицы

            // Добавляем отфильтрованные строки в начало новой таблицы
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            // Добавляем оставшиеся строки
            foreach (DataRow row in bookingsOriginal.Rows)
            {
                string name = row["bookingNumber"].ToString().ToLower();

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
    }
}
