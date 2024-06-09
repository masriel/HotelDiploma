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

using Microsoft.Win32;

using Hotel.Classes;
using MySqlConnector;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();
        }

        private void test_Loaded(object sender, RoutedEventArgs e)
        {
            //WordDocumentManager word = new WordDocumentManager();

            //string templatePath = "\\\\EKATERINA-PC\\Diploma\\project\\Hotel\\Hotel\\Resources\\Voucher.docx";

            //var saveFileDialog = new SaveFileDialog
            //{
            //    Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*",
            //    FileName = $"Voucher_A123B.docx"
            //};

            //if (saveFileDialog.ShowDialog() == true)
            //{
            //    string outputPath = saveFileDialog.FileName;
            //    Dictionary<string, string> data = new Dictionary<string, string>
            //    {
            //        { "BookingNumber", "A123B" },
            //        { "Weekday1", "Понедельник" },
            //        { "Day1", "17" },
            //        { "Month1", "июнь" },
            //        { "Year1", "2024" },
            //        { "Weekday2", "Среда" },
            //        { "Day2", "19" },
            //        { "Month2", "июнь" },
            //        { "Year2", "2024" },
            //        { "Info", "блаблаблаблаблаблаблабла" },
            //        { "Days", "3" },
            //        { "Clients", "2" },
            //        { "Amount", "6800" }
            //    };
            //    word.FillTemplate(templatePath, outputPath, data);

            //    MessageBox.Show("Ваучер успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            //}

            NumberToWords s = new NumberToWords();
            string ef = s.NumberToWordsRussian(5960);
        }
    }
}
