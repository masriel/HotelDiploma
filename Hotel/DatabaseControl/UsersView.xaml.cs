using Hotel.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : Window
    {
        private string CONNECTION_STRING = String.Empty;

        private ConnectionInfo db; 
        private ReadConfigFile _config = new ReadConfigFile();
        Navigation NAVIGATION = new Navigation();

        private DataTable users = new DataTable();

        public UsersView()
        {
            InitializeComponent();
            CONNECTION_STRING = _config.GetConnectionString();
            db = new ConnectionInfo(CONNECTION_STRING);
        }

        private void UsersWindow_Loaded(object sender, RoutedEventArgs e)
        {
            users = db.GetData("select userID, userName, userEmail, typeName from Users left join UserTypes on userType=typeID;");
            Users.ItemsSource = users.DefaultView;

            Users.Columns[0].Visibility = Visibility.Collapsed;

            Users.Columns[1].Header = "Имя пользователя";
            Users.Columns[2].Header = "Эл. почта";
            Users.Columns[3].Header = "Тип пользователя";
        }

        private void SearchText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string searchText = SearchText.Text.ToLower();

            foreach (var row in Users.Items)
            {
                DataGridRow dataGridRow = (DataGridRow)Users.ItemContainerGenerator.ContainerFromItem(row);
                if (dataGridRow != null)
                {
                    DataRowView dataRowView = (DataRowView)dataGridRow.Item;
                    string name = dataRowView[1].ToString().ToLower();
                    string login = dataRowView[2].ToString().ToLower();

                    if (name.Contains(searchText) || login.Contains(searchText))
                    {
                        dataGridRow.IsSelected = true;
                    }
                    else
                    {
                        dataGridRow.IsSelected = false;
                    }
                }
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
            NAVIGATION.OpenAsNewPage(new UserRegistration("", "", "", ""), this);
        }
    }
}
