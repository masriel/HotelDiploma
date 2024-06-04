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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Interaction logic for RoomEdit.xaml
    /// </summary>
    public partial class RoomEdit : Window
    {
        private string TYPE, OCCUPANCY, DESC, NUMBER, COST;
        private int ID;

        private string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private ConnectionInfo db;
        private ReadConfigFile _config = new ReadConfigFile();

        public RoomEdit(int id, string type, string occupancy, string desc, string cost)
        {
            InitializeComponent();

            ID = id;
            TYPE = type; OCCUPANCY = occupancy; DESC = desc; COST = cost;

            CONNECTION_STRING = _config.GetConnectionString();
            db = new ConnectionInfo(CONNECTION_STRING);
        }

        private void RoomEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TypeRoom.Text = TYPE;
            DescriptionRoom.Text = DESC;
            OccupancyRoom.Text = OCCUPANCY;
            CostRoom.Text = COST;
        }

        private void RoomEditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Сохранить данную информацию?", "РЕДАКТИРОВАНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                EditRoom();
            }
            e.Cancel = false;
        }

        private void EditRoom()
        {
            TYPE = TypeRoom.Text;
            DESC = DescriptionRoom.Text;
            OCCUPANCY = OccupancyRoom.Text;
            COST = CostRoom.Text;

            if (!CheckFields(TYPE, OCCUPANCY, DESC, COST))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string query = $"update RoomTypes set roomType='{TYPE}', maxOccupancy={OCCUPANCY}, roomCost={COST}, roomDescription='{DESC}' where roomTypeID={ID};";
                using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
                {
                    CONNECTION.Open();

                    COMMAND = new MySqlCommand(query, CONNECTION);
                    int result = COMMAND.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        MessageBox.Show("Информация не обновлена.", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                        DialogResult = false;
                    }
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckFields(string type, string occupancy, string desc, string cost)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(occupancy) || string.IsNullOrEmpty(desc) || string.IsNullOrEmpty(cost)) return false;
            return true;
        }
    }
}
