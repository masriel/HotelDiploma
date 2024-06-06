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
            _db = new ConnectionInfo(_config.GetConnectionString());
        }

        public void UpdateRoomStatus()
        {
            try
            {
                // Get current date
                DateTime currentDate = DateTime.Today;
                string currentDateString = currentDate.ToString("yyyy-MM-dd");

                // Get all bookings with departure date less than or equal to the current date
                string query1 = $"SELECT room FROM Bookings WHERE departureDate <= '{currentDateString}'";
                DataTable bookingsData = _db.GetData(query1);

                // Create a list of room IDs that should be free
                var roomsToBeFree = new List<int>();

                if (bookingsData.Rows.Count > 0)
                {
                    foreach (DataRow row in bookingsData.Rows)
                    {
                        int roomId = Convert.ToInt32(row["room"]);
                        roomsToBeFree.Add(roomId);
                    }

                    // Update the status of rooms to free
                    foreach (int roomId in roomsToBeFree)
                    {
                        _db.ExecuteCommand($"UPDATE Rooms SET isFree = 't' WHERE roomID = {roomId}");
                    }
                }

                // Update the status of rooms that are currently booked
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

                    foreach (int roomId in roomsOccupied)
                    {
                        _db.ExecuteCommand($"UPDATE Rooms SET isFree = 'f' WHERE roomID = {roomId}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
