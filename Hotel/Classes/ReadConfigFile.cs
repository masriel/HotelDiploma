using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hotel.Classes
{
    public class ReadConfigFile
    {
        // Путь к файлу конфигурации
        private readonly string FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_config.json");
        private ConfigData CONFIG;

        // Словарь для хранения информации о конфигурации
        private readonly Dictionary<string, string> CONFIG_INFO = new Dictionary<string, string>
        {
            { "server", "" },
            { "port", "" },
            { "database", "" },
            { "uid", "" },
            { "pwd", "" }
        };

        // Метод для чтения файла конфигурации и возвращения словаря с данными конфигурации
        public Dictionary<string, string> ReadFile()
        {
            // Проверка наличия файла и чтение данных
            if (File.Exists(FILE_PATH))
            {
                string json = File.ReadAllText(FILE_PATH);
                CONFIG = JsonConvert.DeserializeObject<ConfigData>(json);
            }

            // Заполнение словаря данными из файла конфигурации
            CONFIG_INFO["server"] = CONFIG.Server;
            CONFIG_INFO["port"] = CONFIG.Port;
            CONFIG_INFO["database"] = CONFIG.Database;
            CONFIG_INFO["uid"] = CONFIG.Username;
            CONFIG_INFO["pwd"] = CONFIG.Password;

            return CONFIG_INFO;
        }

        // Метод для получения строки подключения к базе данных
        public string GetConnectionString()
        {
            // Проверка наличия файла и чтение данных
            if (File.Exists(FILE_PATH))
            {
                string json = File.ReadAllText(FILE_PATH);
                CONFIG = JsonConvert.DeserializeObject<ConfigData>(json);
            }

            // Формирование строки подключения
            string connString = $"Server={CONFIG.Server};Port={CONFIG.Port};Database={CONFIG.Database};Uid={CONFIG.Username};Pwd={CONFIG.Password};";
            return connString;
        }

        // Метод для загрузки конфигурации из файла
        public ConfigData LoadConfig()
        {
            // Проверка наличия файла и его пустоты
            if (!File.Exists(FILE_PATH) || new FileInfo(FILE_PATH).Length == 0)
            {
                return null;
            }

            // Чтение данных конфигурации из файла
            string json = File.ReadAllText(FILE_PATH);
            return JsonConvert.DeserializeObject<ConfigData>(json);
        }

        // Метод для сохранения конфигурации в файл
        public void SaveConfig(ConfigData config)
        {
            // Сериализация данных конфигурации в JSON
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            // Запись данных в файл
            File.WriteAllText(FILE_PATH, json);
        }
    }
}
