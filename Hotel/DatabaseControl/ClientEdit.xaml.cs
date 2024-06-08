using System;
using System.Windows;

using MySql.Data.MySqlClient;

using Hotel.Classes;

namespace Hotel.DatabaseControl
{
    public partial class ClientEdit : Window
    {
        private readonly string _connectionString;
        private readonly ConnectionInfo _db;
        private readonly ReadConfigFile _config = new ReadConfigFile();

        private string _firstName, _lastName, _middleName, _birthDate, _phone, _email;
        private int _clientId, _passportExist, _bcExist;

        public ClientEdit(int id, string firstName, string lastName, string middleName, string birthDate, string phone, string email, int passport, int bc)
        {
            InitializeComponent();

            _connectionString = _config.GetConnectionString();
            _db = new ConnectionInfo(_connectionString);

            _clientId = id;
            _firstName = firstName;
            _lastName = lastName;
            _middleName = middleName;
            _birthDate = birthDate;
            _phone = phone;
            _email = email;
            _passportExist = passport;
            _bcExist = bc;
        }

        private void ClientEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClientData();
        }

        private void LoadClientData()
        {
            if (_passportExist != 0)
            {
                GetPassport(_passportExist);
            }

            if (_bcExist != 0)
            {
                GetBirthCertificate(_bcExist);
            }

            FirstName.Text = _firstName;
            LastName.Text = _lastName;
            MiddleName.Text = _middleName;
            BirthDate.Text = _birthDate;
            PhoneNumber.Text = _phone;
            Email.Text = _email;
        }

        private void ClientEditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Сохранить данную информацию?", "РЕДАКТИРОВАНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                EditClient();
            }
            DialogResult = true;
        }

        private void GetPassport(int id)
        {
            var passport = _db.GetData($"SELECT passportSeries, passportNumber, DATE_FORMAT(issueDate, '%Y-%m-%d') as issueDate, issuingAuthority FROM ClientPassports WHERE passportID = {id};").Rows[0];
            SeriesNumber.Text = $"{passport["passportSeries"]} {passport["passportNumber"]}";
            IssueDate.Text = DateTime.Parse(Convert.ToString(passport["issueDate"])).ToString("dd.MM.yyyy");
            IssuingAuthority.Text = passport["issuingAuthority"].ToString();
        }

        private void GetBirthCertificate(int id)
        {
            var bc = _db.GetData($"SELECT registrationNumber, DATE_FORMAT(issueDate, '%Y-%m-%d') as issueDate, issuingAuthority FROM BirthCertificate WHERE birthCertificateID = {id};").Rows[0];
            RegNumber.Text = bc["registrationNumber"].ToString();
            BirthCertificateIssueDate.Text = DateTime.Parse(Convert.ToString(bc["issueDate"])).ToString("dd.MM.yyyy");
            BirthCertificateIssuingAuthority.Text = bc["issuingAuthority"].ToString();
        }

        private void EditClient()
        {
            _firstName = FirstName.Text;
            _lastName = LastName.Text;
            _middleName = MiddleName.Text;
            _phone = PhoneNumber.Text;
            _email = Email.Text;
            _birthDate = BirthDate.Text;

            if (!CheckFields(_firstName, _lastName, _birthDate, _phone))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_passportExist != 0)
            {
                UpdateClientAndPassport();
            }
            else if (_bcExist != 0)
            {
                UpdateClientAndBirthCertificate();
            }
        }

        private bool CheckFields(string firstName, string lastName, string birthDate, string phone)
        {
            return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(birthDate) && !string.IsNullOrEmpty(phone);
        }

        private void UpdateClientAndPassport()
        {
            var passportSeriesNumber = SeriesNumber.Text.Split(' ');
            var passportSeries = passportSeriesNumber[0];
            var passportNumber = passportSeriesNumber.Length > 1 ? passportSeriesNumber[1] : string.Empty;
            var passportIssueDate = IssueDate.Text;
            var passportIssuingAuthority = IssuingAuthority.Text;

            ExecuteClientUpdateQuery();
            ExecutePassportUpdateQuery(passportSeries, passportNumber, passportIssueDate, passportIssuingAuthority);

            MessageBox.Show("Информация обновлена успешно!", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateClientAndBirthCertificate()
        {
            var birthCertRegNumber = RegNumber.Text;
            var birthCertIssueDate = BirthCertificateIssueDate.Text;
            var birthCertIssuingAuthority = BirthCertificateIssuingAuthority.Text;

            ExecuteClientUpdateQuery();
            ExecuteBirthCertificateUpdateQuery(birthCertRegNumber, birthCertIssueDate, birthCertIssuingAuthority);

            MessageBox.Show("Информация обновлена успешно!", "РЕДАКТИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteClientUpdateQuery()
        {
            string query = "UPDATE Clients SET firstName = @FirstName, lastName = @LastName, middleName = @MiddleName, birthDate = @BirthDate, phoneNumber = @PhoneNumber, email = @Email WHERE clientID = @ClientID";
            var parameters = new[]
            {
                new MySqlParameter("@FirstName", _firstName),
                new MySqlParameter("@LastName", _lastName),
                new MySqlParameter("@MiddleName", _middleName),
                new MySqlParameter("@BirthDate", DateTime.Parse(_birthDate).ToString("yyyy-MM-dd")),
                new MySqlParameter("@PhoneNumber", _phone),
                new MySqlParameter("@Email", _email),
                new MySqlParameter("@ClientID", _clientId)
            };
            _db.ExecuteCommand(query, parameters);
        }

        private void ExecutePassportUpdateQuery(string passportSeries, string passportNumber, string issueDate, string issuingAuthority)
        {
            string query = "UPDATE ClientPassports SET passportSeries = @PassportSeries, passportNumber = @PassportNumber, issueDate = @IssueDate, issuingAuthority = @IssuingAuthority WHERE passportID = @PassportID";
            var parameters = new[]
            {
                new MySqlParameter("@PassportSeries", passportSeries),
                new MySqlParameter("@PassportNumber", passportNumber),
                new MySqlParameter("@IssueDate", DateTime.Parse(issueDate).ToString("yyyy-MM-dd")),
                new MySqlParameter("@IssuingAuthority", issuingAuthority),
                new MySqlParameter("@PassportID", _passportExist)
            };
            _db.ExecuteCommand(query, parameters);
        }

        private void ExecuteBirthCertificateUpdateQuery(string registrationNumber, string issueDate, string issuingAuthority)
        {
            string query = "UPDATE BirthCertificates SET registrationNumber = @RegistrationNumber, issueDate = @IssueDate, issuingAuthority = @IssuingAuthority WHERE birthCertificateID = @BirthCertificateID";
            var parameters = new[]
            {
                new MySqlParameter("@RegistrationNumber", registrationNumber),
                new MySqlParameter("@IssueDate", DateTime.Parse(issueDate).ToString("yyyy-MM-dd")),
                new MySqlParameter("@IssuingAuthority", issuingAuthority),
                new MySqlParameter("@BirthCertificateID", _bcExist)
            };
            _db.ExecuteCommand(query, parameters);
        }
    }
}
