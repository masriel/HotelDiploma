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
        private string CONNECTION_STRING = String.Empty;

        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private ConnectionInfo db;
        private ReadConfigFile _config = new ReadConfigFile();
        Navigation NAVIGATION = new Navigation();

        private DataTable clients = new DataTable();
        private DataTable clientsOriginal;

        public ClientsView()
        {
            InitializeComponent();

            CONNECTION_STRING = _config.GetConnectionString();
            db = new ConnectionInfo(CONNECTION_STRING);
        }

        private void ClientsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            clients = db.GetData("select clientID, lastName, firstName, middleName, DATE_FORMAT(birthDate, '%d.%m.%Y') as birthDate, phoneNumber, email, passport, birthCertificate, passportSeries, passportNumber, ClientPassports.issueDate, ClientPassports.issuingAuthority, registrationNumber, BirthCertificate.issueDate, BirthCertificate.issuingAuthority " +
                "from Clients " +
                "left join ClientPassports on passport=passportID " +
                "left join BirthCertificate on birthCertificate=birthCertificateID;");

            if (clients != null)
            {
                clientsOriginal = clients.Copy();
                Clients.ItemsSource = clients.DefaultView;
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
            string searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // Восстанавливаем исходную таблицу, если строка поиска пустая
                Clients.ItemsSource = clientsOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            DataView dv = new DataView(clientsOriginal);
            dv.RowFilter = $"lastName LIKE '%{searchText}%' OR firstName LIKE '%{searchText}%'";

            DataTable newTable = clientsOriginal.Clone(); // Копируем структуру исходной таблицы

            // Добавляем отфильтрованные строки в начало новой таблицы
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            // Добавляем оставшиеся строки
            foreach (DataRow row in clientsOriginal.Rows)
            {
                string name = row["lastName"].ToString().ToLower();
                string login = row["firstName"].ToString().ToLower();

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
    }
}
