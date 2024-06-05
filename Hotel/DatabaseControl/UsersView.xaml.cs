using Hotel.Classes;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using Hotel.Pages;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : Window
    {
        private string CONNECTION_STRING = String.Empty;
        private MySqlConnection CONNECTION;
        private MySqlCommand COMMAND;

        private ConnectionInfo db;
        private ReadConfigFile _config = new ReadConfigFile();

        private Navigation NAVIGATION = new Navigation();

        private DataTable users = new DataTable();
        private DataTable usersOriginal;

        private string PASSWORD, NAME;

        public UsersView(string name, string pwd)
        {
            InitializeComponent();

            CONNECTION_STRING = _config.GetConnectionString();
            db = new ConnectionInfo(CONNECTION_STRING);

            NAME = name; PASSWORD = pwd;
        }

        private void UsersWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            users = db.GetData("select userID, userName, userEmail, typeName, typeID from Users left join UserTypes on userType=typeID;");
            usersOriginal = users.Copy();

            if (users != null)
            {
                Users.ItemsSource = users.DefaultView;
                ConfigureDataGrid();
            }
        }

        private void ConfigureDataGrid()
        {
            // Скрываем столбцы
            Users.Columns[0].Visibility = Visibility.Collapsed;
            Users.Columns[4].Visibility = Visibility.Collapsed;

            // Устанавливаем заголовки
            Users.Columns[1].Header = "Имя пользователя";
            Users.Columns[2].Header = "Логин";
            Users.Columns[3].Header = "Тип пользователя";
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchText.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // Восстанавливаем исходную таблицу, если строка поиска пустая
                Users.ItemsSource = usersOriginal.DefaultView;
                ConfigureDataGrid();
                return;
            }

            DataView dv = new DataView(usersOriginal);
            dv.RowFilter = $"userName LIKE '%{searchText}%' OR userEmail LIKE '%{searchText}%'";

            DataTable newTable = usersOriginal.Clone(); // Копируем структуру исходной таблицы

            // Добавляем отфильтрованные строки в начало новой таблицы
            foreach (DataRowView row in dv)
            {
                newTable.ImportRow(row.Row);
            }

            // Добавляем оставшиеся строки
            foreach (DataRow row in usersOriginal.Rows)
            {
                string name = row["userName"].ToString().ToLower();
                string login = row["userEmail"].ToString().ToLower();

                if (!name.Contains(searchText) && !login.Contains(searchText))
                {
                    newTable.ImportRow(row);
                }
            }

            Users.ItemsSource = newTable.DefaultView;
            ConfigureDataGrid();

            if (newTable.Rows.Count > 0)
            {
                Users.SelectedIndex = 0;
                Users.ScrollIntoView(Users.SelectedItem);
            }
        }

        private void Users_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var hitTestResult = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));

            if (hitTestResult != null && hitTestResult.VisualHit is DataGridRow)
            {
                DataGridRow selectedRow = (DataGridRow)hitTestResult.VisualHit;
                selectedRow.IsSelected = true;
            }
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            NAVIGATION.OpenAsDialog(new UserRegistration(0, "", "", 0, false));
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (Users.SelectedItem != null)
            {
                DataRowView selectedRow = Users.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int userId = Convert.ToInt32(selectedRow[0]);
                    if (userId != 0)
                    {
                        if (MessageBox.Show($"Вы действительно хотите удалить пользователя {selectedRow[2]}/{selectedRow[1]}?", "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            try
                            {
                                string query = $"delete from Users where userID={userId}";
                                using (CONNECTION = new MySqlConnection(CONNECTION_STRING))
                                {
                                    CONNECTION.Open();

                                    COMMAND = new MySqlCommand(query, CONNECTION);
                                    COMMAND.ExecuteNonQuery();
                                }

                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void Users_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                ContextMenu contextMenu = dataGrid.ContextMenu;
                if (contextMenu != null)
                {
                    foreach (MenuItem item in contextMenu.Items)
                    {
                        item.IsEnabled = true;
                    }
                }
            }
        }

        private void UsersWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new AdminPanelView(NAME, PASSWORD).Show();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (Users.SelectedItem != null)
            {
                DataRowView selectedRow = Users.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int userId = Convert.ToInt32(selectedRow[0]), type = Convert.ToInt32(selectedRow[4]);
                    string name = Convert.ToString(selectedRow[1]), login = Convert.ToString(selectedRow[2]);

                    if (userId != 0)
                    {
                        NAVIGATION.OpenAsDialog(new UserRegistration(userId, name, login, type, true));
                    }
                }
            }
        }
    }
}
