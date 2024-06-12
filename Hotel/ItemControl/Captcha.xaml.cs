using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hotel.ItemControl
{
    /// <summary>
    /// Логика взаимодействия для Captcha.xaml
    /// </summary>
    public partial class Captcha : Window
    {
        private Random RANDOM = new Random();
        private string CAPTCHA_TEXT = string.Empty;

        public Captcha()
        {
            InitializeComponent();
        }

        private void CaptchaWin_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }

        // Метод для генерации капчи
        private void GenerateCaptcha()
        {
            CAPTCHA_TEXT = GenerateRandomText(5); // Генерация случайного текста для капчи
            DrawingVisual drawingVisual = new DrawingVisual();
            int width = 165;
            int height = 100;

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // Фон капчи
                drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                // Создание текста капчи
                FormattedText formattedText = new FormattedText(
                    CAPTCHA_TEXT,
                    System.Globalization.CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Montserrat"),
                    24,
                    Brushes.Black,
                    new NumberSubstitution(),
                    1);

                // Случайное расположение текста на картинке
                double maxX = width - formattedText.Width;
                double maxY = height - formattedText.Height;
                double posX = RANDOM.NextDouble() * maxX;
                double posY = RANDOM.NextDouble() * maxY;

                drawingContext.DrawText(formattedText, new System.Windows.Point(posX, posY));

                // Добавление случайных линий для сложности капчи
                for (int i = 0; i < 10; i++)
                {
                    drawingContext.DrawLine(
                        new System.Windows.Media.Pen(Brushes.Black, 1),
                        new System.Windows.Point(RANDOM.Next(width), RANDOM.Next(height)),
                        new System.Windows.Point(RANDOM.Next(width), RANDOM.Next(height)));
                }

                // Добавление случайных точек для шума
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

            // Создание изображения капчи
            RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            CaptchaPic.Source = bmp;
        }

        // Метод для генерации случайного текста
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

        // Обработчик кнопки обновления капчи
        private void RefreshCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }

        // Обработчик кнопки проверки капчи
        private void CheckCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            string inputText = CaptchaText.Text;
            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Введите текст с картинки!", "ПРОВЕРКА", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (inputText.Trim() == CAPTCHA_TEXT.Trim())
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка ввода капчи!", "ПРОВЕРКА", MessageBoxButton.OK, MessageBoxImage.Error);
                CaptchaText.Text = string.Empty;
                GenerateCaptcha();
            }
        }
    }
}