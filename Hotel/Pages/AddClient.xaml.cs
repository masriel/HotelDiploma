using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Hotel.Classes;

using MySql.Data.MySqlClient;

namespace Hotel.Pages
{
    public partial class AddClient : Window
    {
        public int ID;
        public string Name;

        public AddClient()
        {
            InitializeComponent();
        }

        // Обработка изменения даты рождения
        private void BirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime selectDate = DateTime.Parse(Convert.ToString(BirthDate.SelectedDate));
            int age = today.Year - selectDate.Year;

            if (today < selectDate.AddYears(age))
            {
                age--;
            }

            // Показать соответствующие поля в зависимости от возраста
            if (age < 14)
            {
                PassportView.Visibility = Visibility.Collapsed;
                BirthCertificateView.Visibility = Visibility.Visible;
            }
            else
            {
                PassportView.Visibility = Visibility.Visible;
                BirthCertificateView.Visibility = Visibility.Collapsed;
            }
        }

        // Валидация ввода для поля Email
        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Z0-9@\\-_.]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        // Валидация ввода для текстовых полей
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[а-яА-ЯёЁ]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        // Автоматическое преобразование первой буквы в верхний регистр
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            int caretIndex = textBox.CaretIndex;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string text = textBox.Text;
                textBox.Text = char.ToUpper(text[0]) + text.Substring(1);

                textBox.CaretIndex = caretIndex;
            }
        }

        // Обработка нажатия на кнопку добавления клиента
        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            string _lastName = LastName.Text;
            string _firstName = FirstName.Text;
            string _middleName = MiddleName.Text;
            string _email = Email.Text;
            string _phone = PhoneNumber.Text;
            string _birthDate = BirthDate.Text != "" ? DateTime.Parse(BirthDate.Text).ToString("yyyy-MM-dd") : "";

            // Проверка на заполненность всех полей
            if (string.IsNullOrWhiteSpace(_lastName) || string.IsNullOrWhiteSpace(_firstName) ||
                string.IsNullOrWhiteSpace(_middleName) || string.IsNullOrWhiteSpace(_email) ||
                string.IsNullOrWhiteSpace(_phone) || BirthDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string connectionString = new ReadConfigFile().GetConnectionString();
                ConnectionInfo db = new ConnectionInfo(connectionString);

                DateTime birthDate = DateTime.Parse(_birthDate);
                int age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now < birthDate.AddYears(age))
                {
                    age--;
                }

                int passportId = 0;
                int birthCertificateId = 0;

                // Вставка данных паспорта или свидетельства о рождении в зависимости от возраста
                if (age >= 14)
                {
                    var passportSeriesNumber = SeriesNumber.Text.Split(' ');
                    var passportSeries = passportSeriesNumber[0];
                    var passportNumber = passportSeriesNumber.Length > 1 ? passportSeriesNumber[1] : string.Empty;
                    string passportIssueDate = DateTime.Parse(IssueDate.Text).ToString("yyyy-MM-dd");
                    string passportIssuingAuthority = IssuingAuthority.Text;

                    string queryPassport = "INSERT INTO ClientPassports (passportSeries, passportNumber, issueDate, issuingAuthority) " +
                                           "VALUES (@PassportSeries, @PassportNumber, @IssueDate, @IssuingAuthority);" +
                                           "SELECT LAST_INSERT_ID();";

                    MySqlParameter[] passportParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@PassportSeries", passportSeries),
                        new MySqlParameter("@PassportNumber", passportNumber),
                        new MySqlParameter("@IssueDate", passportIssueDate),
                        new MySqlParameter("@IssuingAuthority", passportIssuingAuthority)
                    };

                    passportId = db.ExecuteInsertAndGetId(queryPassport, passportParameters);
                }
                else
                {
                    string registrationNumber = RegNumber.Text;
                    string birthCertificateIssueDate = DateTime.Parse(BirthCertificateIssueDate.Text).ToString("yyyy-MM-dd");
                    string birthCertificateIssuingAuthority = BirthCertificateIssuingAuthority.Text;

                    string queryBirthCertificate = "INSERT INTO BirthCertificate (registrationNumber, issueDate, issuingAuthority) " +
                                                   "VALUES (@RegistrationNumber, @IssueDate, @IssuingAuthority);" +
                                                   "SELECT LAST_INSERT_ID();";

                    MySqlParameter[] birthCertificateParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@RegistrationNumber", registrationNumber),
                        new MySqlParameter("@IssueDate", birthCertificateIssueDate),
                        new MySqlParameter("@IssuingAuthority", birthCertificateIssuingAuthority)
                    };

                    birthCertificateId = db.ExecuteInsertAndGetId(queryBirthCertificate, birthCertificateParameters);
                }

                // Вставка данных клиента
                string queryClient = "INSERT INTO Clients (lastName, firstName, middleName, birthDate, phoneNumber, email, passport, birthCertificate) " +
                                     "VALUES (@LastName, @FirstName, @MiddleName, @BirthDate, @PhoneNumber, @Email, @PassportID, @BirthCertificateID);";

                MySqlParameter[] clientParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@LastName", _lastName),
                    new MySqlParameter("@FirstName", _firstName),
                    new MySqlParameter("@MiddleName", _middleName),
                    new MySqlParameter("@Email", _email),
                    new MySqlParameter("@PhoneNumber", _phone),
                    new MySqlParameter("@BirthDate", _birthDate),
                    new MySqlParameter("@PassportID", passportId != 0 ? (object)passportId : DBNull.Value),
                    new MySqlParameter("@BirthCertificateID", birthCertificateId != 0 ? (object)birthCertificateId : DBNull.Value)
                };

                ID = db.ExecuteInsertAndGetId(queryClient, clientParameters);
                Name = $"{LastName.Text} {FirstName.Text} {MiddleName.Text}";

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddClientWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = false;
        }
    }
}
