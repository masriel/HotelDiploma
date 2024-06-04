using Hotel.Classes;
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
    /// Interaction logic for RoomsAndServicesView.xaml
    /// </summary>
    public partial class RoomsAndServicesView : UserControl
    {
        private string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private ConnectionInfo db;
        private ReadConfigFile _config = new ReadConfigFile();

        Navigation NAVIGATION = new Navigation();

        private DataTable rooms = new DataTable();
        private DataTable roomsOriginal;

        public RoomsAndServicesView()
        {
            InitializeComponent();
            CONNECTION_STRING = _config.GetConnectionString();
            db = new ConnectionInfo(CONNECTION_STRING);
        }

        private void RoomsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            rooms = db.GetData("SELECT roomID, roomNumber, Rooms.roomType as typeID, RoomTypes.roomType as type, maxOccupancy, roomPhoto, roomDescription, roomCost, isFree " +
                               "FROM Rooms " +
                               "LEFT JOIN RoomTypes ON Rooms.roomType = roomTypeID;");

            if (rooms != null)
            {
                roomsOriginal = rooms.Copy();
                Rooms.ItemsSource = rooms.DefaultView;
                ConfigureDataGrid();
            }
        }

        private void ConfigureDataGrid()
        {
            foreach (DataGridColumn column in Rooms.Columns)
            {
                switch (column.Header.ToString())
                {
                    case "roomNumber":
                        column.Header = "Номер";
                        break;
                    case "type":
                        column.Header = "Вид";
                        break;
                    case "maxOccupancy":
                        column.Header = "Кол-во мест";
                        break;
                    case "roomCost":
                        column.Header = "Стоимость";
                        break;
                    default:
                        column.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                Rooms.ItemsSource = roomsOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            DataView dv = new DataView(roomsOriginal);
            dv.RowFilter = $"type LIKE '%{searchText}%'";

            DataTable newTable = roomsOriginal.Clone();
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            foreach (DataRow row in roomsOriginal.Rows)
            {
                string name = row["type"].ToString().ToLower();

                if (!name.Contains(searchText))
                {
                    newTable.ImportRow(row);
                }
            }

            Rooms.ItemsSource = newTable.DefaultView;
            ConfigureDataGrid();

            if (newTable.Rows.Count > 0)
            {
                Rooms.SelectedIndex = 0;
                Rooms.ScrollIntoView(Rooms.SelectedItem);
            }
        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (Rooms.SelectedItem != null)
            {
                DataRowView selectedRow = Rooms.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int roomId = Convert.ToInt32(selectedRow["roomID"]);
                    if (roomId > 0)
                    {
                        if (MessageBox.Show($"Вы действительно хотите удалить комнату {selectedRow["roomNumber"]}?", "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            try
                            {
                                string query = $"delete from Rooms where roomID={roomId}";
                                using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
                                {
                                    CONNECTION.Open();

                                    COMMAND = new MySqlCommand(query, CONNECTION);
                                    //int result = COMMAND.ExecuteNonQuery();
                                    //if (result <= 0)
                                    //{
                                    //    MessageBox.Show("Комната не удалена.", "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                                    //    return;
                                    //}
                                }
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (Rooms.SelectedItem != null)
            {
                DataRowView selectedRow = Rooms.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int roomId = Convert.ToInt32(selectedRow["roomID"]);
                    if (roomId > 0)
                    {
                        string status = Convert.ToString(selectedRow["isFree"]);
                        if (status != null)
                        {
                            status = status == "f" ? "t" : "f";
                            try
                            {
                                string query = $"update Rooms set isFree='{status}' where roomID={roomId}";
                                using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
                                {
                                    CONNECTION.Open();

                                    COMMAND = new MySqlCommand(query, CONNECTION);
                                    int result = COMMAND.ExecuteNonQuery();
                                    if (result <= 0)
                                    {
                                        MessageBox.Show("Статус не изменен.", "СТАТУС", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (Rooms.SelectedItem != null)
            {
                DataRowView selectedRow = Rooms.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int typeId = Convert.ToInt32(selectedRow["typeID"]);
                    if (typeId > 0)
                    {
                        string type = Convert.ToString(selectedRow["type"]), occupancy = Convert.ToString(selectedRow["maxOccupancy"]), desc = Convert.ToString(selectedRow["roomDescription"]), cost = Convert.ToString(selectedRow["roomCost"]);
                        if (MessageBox.Show("Вы обновите информацию всех номеров данного вида!", "ОБРАТИТЕ ВНИМАНИЕ", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                        {
                            if (new RoomEdit(typeId, type, occupancy, desc, cost).ShowDialog() == true)
                            {
                                LoadData();
                            }
                        }
                    }
                }
            }
        }
    }
}
