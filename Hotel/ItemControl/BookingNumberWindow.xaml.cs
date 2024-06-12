using System;
using System.Windows;

namespace Hotel.ItemControl
{
    /// <summary>
    /// Логика взаимодействия для BookingNumberWindow.xaml
    /// </summary>
    public partial class BookingNumberWindow : Window
    {
        // Свойство для хранения номера бронирования
        public string BookingNumber { get; private set; }

        public BookingNumberWindow()
        {
            InitializeComponent();
        }

        private void SearchBookingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем введенный номер бронирования
                BookingNumber = BookingNumberText.Text;

                // Проверяем, что поле не пустое
                if (string.IsNullOrEmpty(BookingNumber))
                {
                    // Если пустое, выводим предупреждение
                    MessageBox.Show("Пожалуйста, введите номер бронирования.", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Если все в порядке, закрываем окно и устанавливаем результат в true
                DialogResult = true;
            }
            catch (Exception ex)
            {
                // Обработка ошибок и отображение сообщения об ошибке
                MessageBox.Show($"Ошибка: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
