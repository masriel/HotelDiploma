using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hotel.Classes
{
    public class HashPassword
    {
        // Метод для хеширования пароля с использованием SHA-256
        public string HashPasswd(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Преобразование строки пароля в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Вычисление хеша пароля
                byte[] hash = sha256.ComputeHash(bytes);

                // Преобразование байтового массива в шестнадцатеричную строку
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hash)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        // Метод для проверки пароля: сравнение хеша введенного пароля с хешем сохраненного пароля
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Хеширование введенного пароля
            string hashedInput = HashPasswd(password);
            // Сравнение хешей
            return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        // Метод для генерации случайного пароля заданной длины
        public string GeneratePassword(int length)
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_-+=<>?";
            const string allChars = upperCase + lowerCase + digits + specialChars;

            Random random = new Random();

            // Строка для хранения сгенерированного пароля
            StringBuilder password = new StringBuilder();

            // Обязательные символы: один из каждого типа
            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Добавление случайных символов до достижения необходимой длины
            while (password.Length < length)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Перемешивание символов в пароле для увеличения энтропии
            return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
        }
    }
}
