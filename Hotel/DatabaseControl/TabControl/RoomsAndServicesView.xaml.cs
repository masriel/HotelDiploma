using Hotel.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Hotel.DatabaseControl.TabControl
{
    /// <summary>
    /// Логика взаимодействия для RoomsAndServicesView.xaml
    /// </summary>
    public partial class RoomsAndServicesView : UserControl
    {
        private readonly string _connectionString;
        private readonly ConnectionInfo _db;
        private readonly Navigation _navigation = new Navigation();

        private DataTable _rooms;
        private DataTable _roomsOriginal;

        public RoomsAndServicesView()
        {
            InitializeComponent();
            var config = new ReadConfigFile();
            _connectionString = config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        // Метод, вызываемый при загрузке окна
        private void RoomsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Метод для загрузки данных из базы данных
        private void LoadData()
        {
            const string query = "SELECT roomID, roomNumber, Rooms.roomType as typeID, RoomTypes.roomType as type, " +
                "maxOccupancy, roomPhoto, roomDescription, roomCost, isFree " +
                "FROM Rooms " +
                "LEFT JOIN RoomTypes ON Rooms.roomType = roomTypeID;";
            _rooms = _db.GetData(query);

            if (_rooms != null)
            {
                _roomsOriginal = _rooms.Copy();
                Rooms.ItemsSource = _rooms.DefaultView;
                ConfigureDataGrid();
            }
        }

        // Метод для конфигурации DataGrid
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

        // Метод для поиска в таблице комнат
        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                Rooms.ItemsSource = _roomsOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            var dv = new DataView(_roomsOriginal)
            {
                RowFilter = $"type LIKE '%{searchText}%'"
            };

            var newTable = _roomsOriginal.Clone();
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            foreach (DataRow row in _roomsOriginal.Rows)
            {
                var type = row["type"].ToString().ToLower();

                if (!type.Contains(searchText))
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

        // Метод для удаления комнаты
        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (Rooms.SelectedItem is DataRowView selectedRow)
            {
                var roomId = Convert.ToInt32(selectedRow["roomID"]);
                if (roomId > 0)
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить комнату {selectedRow["roomNumber"]}?", "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            const string query = "DELETE FROM Rooms WHERE roomID = @RoomID";
                            using (var connection = new MySqlConnection(_connectionString))
                            {
                                connection.Open();
                                using (var command = new MySqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@RoomID", roomId);
                                    int result = command.ExecuteNonQuery();

                                    if (result <= 0)
                                    {
                                        MessageBox.Show("Комната не удалена.", "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
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

        // Метод для изменения статуса комнаты
        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (Rooms.SelectedItem is DataRowView selectedRow)
            {
                var roomId = Convert.ToInt32(selectedRow["roomID"]);
                if (roomId > 0)
                {
                    var isFree = Convert.ToString(selectedRow["isFree"]);
                    var newStatus = isFree == "t" ? "f" : "t"; // Изменение статуса на противоположный
                    try
                    {
                        const string query = "UPDATE Rooms SET isFree = @IsFree WHERE roomID = @RoomID";
                        using (var connection = new MySqlConnection(_connectionString))
                        {
                            connection.Open();
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@IsFree", newStatus);
                                command.Parameters.AddWithValue("@RoomID", roomId);
                                int result = command.ExecuteNonQuery();

                                if (result <= 0)
                                {
                                    MessageBox.Show("Статус не изменен.", "СТАТУС", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
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

        // Метод для редактирования комнаты
        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (Rooms.SelectedItem is DataRowView selectedRow)
            {
                var typeId = Convert.ToInt32(selectedRow["typeID"]);
                if (typeId > 0)
                {
                    var type = Convert.ToString(selectedRow["type"]);
                    var occupancy = Convert.ToString(selectedRow["maxOccupancy"]);
                    var description = Convert.ToString(selectedRow["roomDescription"]);
                    var cost = Convert.ToString(selectedRow["roomCost"]);
                    var photo = Convert.ToString(selectedRow["roomPhoto"]);

                    if (MessageBox.Show("Вы обновите информацию всех номеров данного вида!", "ОБРАТИТЕ ВНИМАНИЕ", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        if (new RoomEdit(typeId, type, occupancy, description, cost, photo).ShowDialog() == true)
                        {
                            LoadData();
                        }
                    }
                }
            }
        }
    }
}
