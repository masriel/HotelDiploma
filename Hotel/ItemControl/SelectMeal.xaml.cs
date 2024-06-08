using System;
using System.Collections.Generic;
using System.Windows;

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

        private readonly Dictionary<int, double> mealCosts = new Dictionary<int, double>
        {
            { 1, 700 },
            { 2, 950 },
            { 3, 800 }
        };

        public SelectMeal(int days)
        {
            InitializeComponent();

            for (int i = 1; i <= days; i++)
            {
                MealQuantity.Items.Add(i);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (MealName.SelectedIndex == -1 || MealQuantity.SelectedItem == null)
            {
                MessageBox.Show("Выберите необходимые данные.", "ВЫБОР", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ID = MealName.SelectedIndex + 1;
            Name = MealName.Text;
            Quantity = Convert.ToInt32(MealQuantity.SelectedItem);
            Cost = mealCosts.ContainsKey(ID) ? mealCosts[ID] : 0;

            DialogResult = true;
        }
    }
}
