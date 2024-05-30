using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
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
using Brushes = System.Windows.Media.Brushes;

namespace Hotel.ItemControl
{
    /// <summary>
    /// Interaction logic for Captcha.xaml
    /// </summary>
    public partial class Captcha : Window
    {
        private Random RANDOM = new Random();

        private string CAPTCHA_TEXT = String.Empty;
        public Captcha()
        {
            InitializeComponent();
        }

        private void CaptchaWin_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            CAPTCHA_TEXT = GenerateRandomText(5);
            DrawingVisual drawingVisual = new DrawingVisual();
            int width = 165;
            int height = 100;

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // Background
                drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                // Create text
                FormattedText formattedText = new FormattedText(
                    CAPTCHA_TEXT,
                    System.Globalization.CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Montserrat"),
                    24,
                    Brushes.Black,
                    new NumberSubstitution(),
                    1);

                // Randomize text position
                double maxX = width - formattedText.Width;
                double maxY = height - formattedText.Height;
                double posX = RANDOM.NextDouble() * maxX;
                double posY = RANDOM.NextDouble() * maxY;

                drawingContext.DrawText(formattedText, new System.Windows.Point(posX, posY));

                // Add random lines
                for (int i = 0; i < 10; i++)
                {
                    drawingContext.DrawLine(
                        new System.Windows.Media.Pen(Brushes.Black, 1),
                        new System.Windows.Point(RANDOM.Next(width), RANDOM.Next(height)),
                        new System.Windows.Point(RANDOM.Next(width), RANDOM.Next(height)));
                }

                // Add noise
                for (int i = 0; i < 100; i++)
                {
                    drawingContext.DrawEllipse(
                        Brushes.Black,
                        null,
                        new System.Windows.Point(RANDOM.Next(width), RANDOM.Next(height)),
                        1,
                        1);
                }
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            CaptchaPic.Source = bmp;
        }

        private string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[RANDOM.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        private void RefreshCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }

        private void CheckCapthaButton_Click(object sender, RoutedEventArgs e)
        {
            string InputText = CaptchaText.Text;
            if (InputText.Trim() == "")
            {
                MessageBox.Show("Введите текст с картинки!", "ПРОВЕРКА", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(InputText.Trim() == CAPTCHA_TEXT.Trim())
            {
                this.Close();
                return;
            }

            MessageBox.Show("Ошибка ввода капчи!", "ПРОВЕРКА", MessageBoxButton.OK, MessageBoxImage.Error);
            GenerateCaptcha();
            CaptchaText.Text = string.Empty;
        }
    }
}
