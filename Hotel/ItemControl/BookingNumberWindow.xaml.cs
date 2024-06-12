using System;
using System.Windows;

namespace Hotel.ItemControl
{
    /// <summary>
    /// Interaction logic for BookingNumberWindow.xaml
    /// </summary>
    public partial class BookingNumberWindow : Window
    {
        public string BookingNumber { get; private set; }
        public BookingNumberWindow()
        {
            InitializeComponent();
        }

        private void SearchBookingButton_Click(object sender, RoutedEventArgs e)
        {
            BookingNumber = BookingNumberText.Text;
            DialogResult = true;
        }
    }
}
