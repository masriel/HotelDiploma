using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using Hotel.Classes; // Предполагается, что класс Meals находится в этом пространстве имен

namespace Hotel.ItemControl
{
    /// <summary>
    /// Interaction logic for SelectMeal.xaml
    /// </summary>
    public partial class SelectMeal : Window
    {
        public int ID { get; private set; }
        public int Quantity { get; private set; }
        public string Name { get; private set; }
        public double Cost { get; private set; }

        public SelectMeal()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (MealName.SelectedIndex == -1 || string.IsNullOrEmpty(MealQuantity.Text))
            {
                MessageBox.Show("Выберите необходимые данные.", "ВЫБОР", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Meals meal = new Meals
                {
                    ID = MealName.SelectedIndex + 1,
                    Name = MealName.Text,
                    Quantity = Convert.ToInt32(MealQuantity.Text)
                };

                ID = meal.ID;
                Name = meal.Name;
                Quantity = meal.Quantity;
                Cost = meal.Cost * Quantity;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MealQuantity_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
