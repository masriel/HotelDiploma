using Hotel.AdminPanel;
using Hotel.Classes;
using Hotel.DatabaseControl;
using System.Windows;
using System.Windows.Input;

namespace Hotel.Pages
{
    /// <summary>
    /// Interaction logic for AdminPanelView.xaml
    /// </summary>
    public partial class AdminPanelView : Window
    {
        private Navigation NAVIGATION = new Navigation();

        private string PASSWORD, NAME;

        public AdminPanelView(string name, string pwd)
        {
            InitializeComponent();
            PASSWORD = pwd;
            NAME = name;

            App.Current.Exit += Current_Exit; // Подписываемся на событие закрытия приложения
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            BackupDatabase exit = new BackupDatabase();
            // Создаем резервную копию базы данных при закрытии приложения
            exit.CreateExitBackup();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) NAVIGATION.OpenAsNewPage(new Login(), this);
        }

        private void Users_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NAVIGATION.OpenAsNewPage(new UsersView(NAME, PASSWORD), this);
        }

        private void Database_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NAVIGATION.OpenAsNewPage(new DatabaseView(NAME, PASSWORD), this);
        }

        private void DatabaseControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NAVIGATION.OpenAsNewPage(new ControlDatabaseView(NAME, PASSWORD), this);
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AdminPanelWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdminNameText.Text = NAME;
        }
    }
}
