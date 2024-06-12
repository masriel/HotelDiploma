using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Hotel.Classes
{
    public class RoomStatusUpdater
    {
        private readonly ConnectionInfo _db;
        private readonly ReadConfigFile _config = new ReadConfigFile();

        public RoomStatusUpdater()
        {
            // Инициализация строки подключения
            _db = new ConnectionInfo(_config.GetConnectionString());
        }

        public void UpdateRoomStatus()
        {
            try
            {
                // Текущая дата
                DateTime currentDate = DateTime.Today;
                string currentDateString = currentDate.ToString("yyyy-MM-dd");

                // Получение всех бронирований с датой выезда меньше или равной текущей дате
                string query1 = $"SELECT room FROM Bookings WHERE departureDate <= '{currentDateString}'";
                DataTable bookingsData = _db.GetData(query1);

                // Создание списка номеров, которые должны быть свободны
                var roomsToBeFree = new List<int>();

                if (bookingsData.Rows.Count > 0)
                {
                    foreach (DataRow row in bookingsData.Rows)
                    {
                        int roomId = Convert.ToInt32(row["room"]);
                        roomsToBeFree.Add(roomId);
                    }

                    // Обновление статуса номеров на свободные
                    UpdateRoomStatus(roomsToBeFree, true);
                }

                // Получение всех текущих бронирований
                string query2 = $"SELECT room FROM Bookings WHERE arrivalDate <= '{currentDateString}' AND departureDate > '{currentDateString}'";
                DataTable currentBookings = _db.GetData(query2);

                var roomsOccupied = new List<int>();

                if (currentBookings.Rows.Count > 0)
                {
                    foreach (DataRow row in currentBookings.Rows)
                    {
                        int roomId = Convert.ToInt32(row["room"]);
                        roomsOccupied.Add(roomId);
                    }

                    // Обновление статуса номеров на занятые
                    UpdateRoomStatus(roomsOccupied, false);
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateRoomStatus(List<int> roomIds, bool isFree)
        {
            // Обновление статуса каждого номера
            foreach (int roomId in roomIds)
            {
                string status = isFree ? "t" : "f";
                _db.ExecuteCommand($"UPDATE Rooms SET isFree = '{status}' WHERE roomID = {roomId}");
            }
        }
    }
}