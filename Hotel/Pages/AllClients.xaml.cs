using Hotel.Classes;
using System.Windows;

namespace Hotel.Pages
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class AllClients : Window
    {
        private readonly Navigation _navigation = new Navigation();

        private readonly string _name;

        public AllClients(string name)
        {
            InitializeComponent();

            _name = name;
        }

        private void ClientsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }
    }
}
