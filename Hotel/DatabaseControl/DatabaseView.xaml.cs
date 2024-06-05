using Hotel.Classes;
using Hotel.Pages;
using System;
using System.Collections.Generic;
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
