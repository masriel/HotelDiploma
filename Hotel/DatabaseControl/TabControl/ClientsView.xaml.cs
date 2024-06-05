using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Hotel.Classes;

namespace Hotel.DatabaseControl.TabControl
{
    /// <summary>
    /// Interaction logic for ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        private readonly string _connectionString;
        private readonly ConnectionInfo _db;
        private readonly Navigation _navigation = new Navigation();

        private DataTable _clients;
        private DataTable _clientsOriginal;

        public ClientsView()
        {
            InitializeComponent();
            var config = new ReadConfigFile();
            _connectionString = config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        private void ClientsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            const string query = "SELECT clientID, lastName, firstName, middleName, " +
                "DATE_FORMAT(birthDate, '%d.%m.%Y') AS birthDate, phoneNumber, email, passport, birthCertificate " +
                "FROM Clients " +
                "LEFT JOIN ClientPassports ON passport = passportID " +
                "LEFT JOIN BirthCertificate ON birthCertificate = birthCertificateID;";
            _clients = _db.GetData(query);

            if (_clients != null)
            {
                _clientsOriginal = _clients.Copy();
                Clients.ItemsSource = _clients.DefaultView;
                ConfigureDataGrid();
            }
        }

        private void ConfigureDataGrid()
        {
            foreach (DataGridColumn column in Clients.Columns)
            {
                switch (column.Header.ToString())
                {
                    case "lastName":
                        column.Header = "Фамилия";
                        break;
                    case "firstName":
                        column.Header = "Имя";
                        break;
                    case "middleName":
                        column.Header = "Отчество";
                        break;
                    case "birthDate":
                        column.Header = "Дата рождения";
                        break;
                    case "phoneNumber":
                        column.Header = "Телефон";
                        break;
                    case "email":
                        column.Header = "Эл. почта";
                        break;
                    default:
                        column.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // Restore original table if search string is empty
                Clients.ItemsSource = _clientsOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            var dv = new DataView(_clientsOriginal)
            {
                RowFilter = $"lastName LIKE '%{searchText}%' OR firstName LIKE '%{searchText}%'"
            };

            var newTable = _clientsOriginal.Clone(); // Clone original table structure

            // Add filtered rows to the new table
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            // Add remaining rows
            foreach (DataRow row in _clientsOriginal.Rows)
            {
                var name = row["lastName"].ToString().ToLower();
                var login = row["firstName"].ToString().ToLower();

                if (!name.Contains(searchText) && !login.Contains(searchText))
                {
                    newTable.ImportRow(row);
                }
            }

            Clients.ItemsSource = newTable.DefaultView;
            ConfigureDataGrid();

            if (newTable.Rows.Count > 0)
            {
                Clients.SelectedIndex = 0;
                Clients.ScrollIntoView(Clients.SelectedItem);
            }
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (Clients.SelectedItem is DataRowView selectedRow)
            {
                var clientId = Convert.ToInt32(selectedRow["clientID"]);
                var last = Convert.ToString(selectedRow["lastName"]);
                var first = Convert.ToString(selectedRow["firstName"]);
                var middle = Convert.ToString(selectedRow["middleName"]);
                var phone = Convert.ToString(selectedRow["phoneNumber"]);
                var email = selectedRow["email"] != DBNull.Value ? Convert.ToString(selectedRow["email"]) : string.Empty;
                var birth = Convert.ToString(selectedRow["birthDate"]);
                var passport = selectedRow["passport"] != DBNull.Value ? Convert.ToInt32(selectedRow["passport"]) : 0;
                var bc = selectedRow["birthCertificate"] != DBNull.Value ? Convert.ToInt32(selectedRow["birthCertificate"]) : 0;

                if (clientId > 0)
                {
                    var clientEdit = new ClientEdit(clientId, first, last, middle, birth, phone, email, passport, bc);
                    if (clientEdit.ShowDialog() == true)
                    {
                        LoadData();
                    }
                }
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (Clients.SelectedItem is DataRowView selectedRow)
            {
                var clientId = Convert.ToInt32(selectedRow["clientID"]);

                if (MessageBox.Show("Вы уверены, что хотите удалить клиента?", "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DeleteClient(clientId);
                    LoadData();
                }
            }
        }

        private void DeleteClient(int clientId)
        {
            try
            {
                var clientData = _db.GetData($"SELECT passport, birthCertificate FROM Clients WHERE clientID = {clientId}");
                if (clientData.Rows.Count > 0)
                {
                    var clientRow = clientData.Rows[0];
                    var passportId = clientRow["passport"] != DBNull.Value ? (int?)Convert.ToInt32(clientRow["passport"]) : null;
                    var birthCertificateId = clientRow["birthCertificate"] != DBNull.Value ? (int?)Convert.ToInt32(clientRow["birthCertificate"]) : null;

                    using (var connection = new MySqlConnection(_connectionString))
                    {
                        connection.Open();
                        var transaction = connection.BeginTransaction();

                        try
                        {
                            _db.ExecuteCommand("DELETE FROM Clients WHERE clientID = @ClientID", new MySqlParameter("@ClientID", clientId));

                            if (passportId.HasValue)
                            {
                                _db.ExecuteCommand("DELETE FROM ClientPassports WHERE passportID = @PassportID", new MySqlParameter("@PassportID", passportId.Value));
                            }

                            if (birthCertificateId.HasValue)
                            {
                                _db.ExecuteCommand("DELETE FROM BirthCertificate WHERE birthCertificateID = @BirthCertificateID", new MySqlParameter("@BirthCertificateID", birthCertificateId.Value));
                            }

                            transaction.Commit();
                            MessageBox.Show("Клиент успешно удален!", "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData(); // Reload data after deletion
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show(ex.Message, "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
