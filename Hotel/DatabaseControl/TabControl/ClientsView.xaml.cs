using Hotel.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Hotel.DatabaseControl.TabControl
{
    /// <summary>
    /// Логика взаимодействия для ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        private readonly string _connectionString; // Строка подключения к базе данных
        private readonly ConnectionInfo _db; // Объект для работы с базой данных
        private readonly Navigation _navigation = new Navigation(); // Объект для навигации (не используется)

        private DataTable _clients; // Таблица с клиентами
        private DataTable _clientsOriginal; // Исходная таблица клиентов

        // Конструктор класса
        public ClientsView()
        {
            InitializeComponent();
            var config = new ReadConfigFile();
            _connectionString = config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);
        }

        // Метод, вызываемый при загрузке окна
        private void ClientsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Метод для загрузки данных из базы данных
        private void LoadData()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для конфигурации DataGrid
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

        // Метод для поиска в таблице клиентов
        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchText = SearchText.Text.ToLower();

                if (string.IsNullOrEmpty(searchText))
                {
                    // Восстановление исходной таблицы, если строка поиска пуста
                    Clients.ItemsSource = _clientsOriginal.DefaultView;
                    ConfigureDataGrid();
                    return;
                }

                var dv = new DataView(_clientsOriginal)
                {
                    RowFilter = $"lastName LIKE '%{searchText}%' OR firstName LIKE '%{searchText}%'"
                };

                var newTable = _clientsOriginal.Clone(); // Клонирование структуры исходной таблицы

                // Добавление отфильтрованных строк в новую таблицу
                foreach (DataRowView row in dv)
                {
                    newTable.ImportRow(row.Row);
                }

                // Добавление оставшихся строк
                foreach (DataRow row in _clientsOriginal.Rows)
                {
                    var name = row["lastName"].ToString().ToLower();
                    var firstName = row["firstName"].ToString().ToLower();

                    if (!name.Contains(searchText) && !firstName.Contains(searchText))
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для редактирования клиента
        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Clients.SelectedItem is DataRowView selectedRow)
                {
                    var clientId = Convert.ToInt32(selectedRow["clientID"]);
                    var lastName = Convert.ToString(selectedRow["lastName"]);
                    var firstName = Convert.ToString(selectedRow["firstName"]);
                    var middleName = Convert.ToString(selectedRow["middleName"]);
                    var phoneNumber = Convert.ToString(selectedRow["phoneNumber"]);
                    var email = selectedRow["email"] != DBNull.Value ? Convert.ToString(selectedRow["email"]) : string.Empty;
                    var birthDate = Convert.ToString(selectedRow["birthDate"]);
                    var passport = selectedRow["passport"] != DBNull.Value ? Convert.ToInt32(selectedRow["passport"]) : 0;
                    var birthCertificate = selectedRow["birthCertificate"] != DBNull.Value ? Convert.ToInt32(selectedRow["birthCertificate"]) : 0;

                    if (clientId > 0)
                    {
                        var clientEdit = new ClientEdit(clientId, firstName, lastName, middleName, birthDate, phoneNumber, email, passport, birthCertificate);
                        if (clientEdit.ShowDialog() == true)
                        {
                            LoadData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для удаления клиента
        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Clients.SelectedItem is DataRowView selectedRow)
                {
                    var clientId = Convert.ToInt32(selectedRow["clientID"]);

                    if (MessageBox.Show("Вы уверены, что хотите удалить клиента?", "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        DeleteClient(clientId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для удаления клиента из базы данных
        private void DeleteClient(int clientId)
        {
            try
            {
                // Получение данных клиента перед удалением
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
                            // Удаление клиента
                            _db.ExecuteCommand("DELETE FROM Clients WHERE clientID = @ClientID", new MySqlParameter("@ClientID", clientId));

                            // Удаление паспорта клиента
                            if (passportId.HasValue)
                            {
                                _db.ExecuteCommand("DELETE FROM ClientPassports WHERE passportID = @PassportID", new MySqlParameter("@PassportID", passportId.Value));
                            }

                            // Удаление свидетельства о рождении клиента
                            if (birthCertificateId.HasValue)
                            {
                                _db.ExecuteCommand("DELETE FROM BirthCertificate WHERE birthCertificateID = @BirthCertificateID", new MySqlParameter("@BirthCertificateID", birthCertificateId.Value));
                            }

                            // Подтверждение транзакции
                            transaction.Commit();
                            MessageBox.Show("Клиент успешно удален!", "УДАЛЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData(); // Перезагрузка данных после удаления
                        }
                        catch (Exception ex)
                        {
                            // Откат транзакции в случае ошибки
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
