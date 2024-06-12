using Hotel.Classes;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Логика взаимодействия для RoomEdit.xaml
    /// </summary>
    public partial class RoomEdit : Window
    {
        private string _type, _occupancy, _desc, _cost, _photo;
        private int _id;

        private readonly string _connectionString;
        private readonly ConnectionInfo _db;
        private readonly ReadConfigFile _config = new ReadConfigFile();

        public RoomEdit(int id, string type, string occupancy, string desc, string cost, string photo)
        {
            InitializeComponent();

            _id = id;
            _type = type;
            _occupancy = occupancy;
            _desc = desc;
            _cost = cost;
            _photo = photo;

            _connectionString = _config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        private void RoomEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomData();
        }

        private void LoadRoomData()
        {
            string photo = Path.Combine("pack://application:,,,/Resources", _photo);
            PhotoRoom.Source = new BitmapImage(new Uri(photo));
            TypeRoom.Text = _type;
            DescriptionRoom.Text = _desc;
            OccupancyRoom.Text = _occupancy;
            CostRoom.Text = _cost;
        }

        private void PhotoRoom_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                // Открытие диалогового окна для выбора файла изображения
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    // Получение пути к выбранному файлу
                    string selectedImagePath = openFileDialog.FileName;

                    // Создание нового имени файла для сохранения в ресурсах проекта
                    string newFileName = $"RoomPhoto_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(selectedImagePath)}";

                    // Путь для сохранения файла в ресурсах проекта
                    //string destinationPath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @$"\Resources\{newFileName}");
                    string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @$"Resources\{newFileName}");

                    // Копирование выбранного файла в папку ресурсов проекта
                    File.Copy(selectedImagePath, destinationPath, true);

                    // Обновление источника изображения для элемента Image
                    PhotoRoom.Source = new BitmapImage(new Uri(destinationPath));

                    // Обновление имени фотографии
                    _photo = newFileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора фотографии: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void UpdateRoom()
        {
            string query = "UPDATE RoomTypes SET roomType=@Type, maxOccupancy=@Occupancy, roomCost=@Cost, roomDescription=@Desc, roomPhoto=@roomPhoto WHERE roomTypeID=@ID";
            var parameters = new[]
            {
                new MySqlParameter("@Type", _type),
                new MySqlParameter("@Occupancy", _occupancy),
                new MySqlParameter("@Cost", _cost),
                new MySqlParameter("@Desc", _desc),
                new MySqlParameter("@ID", _id),
                new MySqlParameter("@roomPhoto", _photo)
            };

            _db.ExecuteCommand(query, parameters);
        }

        private bool CheckFields(string type, string occupancy, string desc, string cost)
        {
            return !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(occupancy) && !string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(cost);
        }
    }
}