using System;
using System.Windows;

using MySql.Data.MySqlClient;

using Hotel.Classes;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Логика взаимодействия для RoomEdit.xaml
    /// </summary>
    public partial class RoomEdit : Window
    {
        private string _type, _occupancy, _desc, _cost;
        private int _id;

        private readonly string _connectionString;
        private readonly ConnectionInfo _db;
        private readonly ReadConfigFile _config = new ReadConfigFile();

        public RoomEdit(int id, string type, string occupancy, string desc, string cost)
        {
            InitializeComponent();

            _id = id;
            _type = type;
            _occupancy = occupancy;
            _desc = desc;
            _cost = cost;

            _connectionString = _config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        private void RoomEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomData();
        }

        private void LoadRoomData()
        {
            TypeRoom.Text = _type;
            DescriptionRoom.Text = _desc;
            OccupancyRoom.Text = _occupancy;
            CostRoom.Text = _cost;
        }

        private void RoomEditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Сохранить данную информацию?", "РЕДАКТИРОВАНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                EditRoom();
            }
        }

        private void EditRoom()
        {
            _type = TypeRoom.Text;
            _desc = DescriptionRoom.Text;
            _occupancy = OccupancyRoom.Text;
            _cost = CostRoom.Text;

            if (!CheckFields(_type, _occupancy, _desc, _cost))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                UpdateRoom();
                MessageBox.Show("Информация обновлена успешно!", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void UpdateRoom()
        {
            string query = "UPDATE RoomTypes SET roomType=@Type, maxOccupancy=@Occupancy, roomCost=@Cost, roomDescription=@Desc WHERE roomTypeID=@ID";
            var parameters = new[]
            {
                new MySqlParameter("@Type", _type),
                new MySqlParameter("@Occupancy", _occupancy),
                new MySqlParameter("@Cost", _cost),
                new MySqlParameter("@Desc", _desc),
                new MySqlParameter("@ID", _id)
            };

            _db.ExecuteCommand(query, parameters);
        }

        private bool CheckFields(string type, string occupancy, string desc, string cost)
        {
            return !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(occupancy) && !string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(cost);
        }
    }
}
