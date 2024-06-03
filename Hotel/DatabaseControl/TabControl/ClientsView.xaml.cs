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
            clientsOriginal = clients.Copy();

            if (clients != null)
            {
                Clients.ItemsSource = clients.DefaultView;
                ConfigureDataGrid();
            }
        }

        private void ConfigureDataGrid()
        {
            foreach (DataGridColumn column in Clients.Columns)
            {
                if (column.Header.ToString() == "lastName")
                {
                    column.Header = "Фамилия";
                }
                else if (column.Header.ToString() == "firstName")
                {
                    column.Header = "Имя";
                }
                else if (column.Header.ToString() == "middleName")
                {
                    column.Header = "Отчество";
                }
                else if (column.Header.ToString() == "birthDate")
                {
                    column.Header = "Дата рождения";
                }
                else if (column.Header.ToString() == "phoneNumber")
                {
                    column.Header = "Телефон";
                }
                else if (column.Header.ToString() == "email")
                {
                    column.Header = "Эл. почта";
                }
                else
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
