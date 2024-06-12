using Hotel.Classes;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Hotel.ItemControl
{
    /// <summary>
    /// Логика взаимодействия для SelectMeal.xaml
    /// </summary>
    public partial class SelectMeal : Window
    {
        // Свойства для хранения выбранной информации о блюде
        public int ID { get; private set; }
        public int Quantity { get; private set; }
        public string Name { get; private set; }
        public double Cost { get; private set; }

        public SelectMeal()
        {
            InitializeComponent();
        }

        // Метод для обработки нажатия кнопки "Добавить"
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, выбраны ли блюдо и количество
            if (MealName.SelectedIndex == -1 || string.IsNullOrEmpty(MealQuantity.Text))
            {
                MessageBox.Show("Выберите необходимые данные.", "ВЫБОР", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Создание нового объекта Meals с выбранными данными
                Meals meal = new Meals
                {
                    ID = MealName.SelectedIndex + 1, // Предполагается, что ID основывается на выбранном индексе
                    Name = MealName.Text,
                    Quantity = Convert.ToInt32(MealQuantity.Text)
                };

                // Установка свойств выбранными данными
                ID = meal.ID;
                Name = meal.Name;
                Quantity = meal.Quantity;
                Cost = meal.Cost;

                // Закрытие окна и установка результата в true
                DialogResult = true;
            }
            catch (Exception ex)
            {
                // Обработка ошибок и отображение сообщения об ошибке
                MessageBox.Show($"Ошибка: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для проверки ввода количества блюда
        private void MealQuantity_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                // Проверка, что вводятся только цифры
                Regex regex = new Regex("^[0-9]+$");
                e.Handled = !regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                // Обработка ошибок и отображение сообщения об ошибке при вводе количества
                MessageBox.Show($"Ошибка при вводе количества: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
