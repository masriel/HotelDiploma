using Hotel.Classes;
using Hotel.Pages;
using System.Windows;

namespace Hotel.DatabaseControl
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : Window
    {
        private Navigation NAVIGATION = new Navigation();

        private string NAME, PASSWORD;

        public DatabaseView(string name, string pwd)
        {
            InitializeComponent();

            NAME = name; PASSWORD = pwd;
        }

        private void DatabaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new AdminPanelView(NAME, PASSWORD).Show();
        }
    }
}
