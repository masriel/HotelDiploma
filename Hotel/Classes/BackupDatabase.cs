using MySqlConnector;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;

namespace Hotel.Classes
{
    public class BackupDatabase
    {
        // Приватное поле для чтения конфигурационного файла
        private ReadConfigFile _config = new ReadConfigFile();
        // Строка подключения к базе данных
        private string CONNECTION_STRING = String.Empty;
        // Словарь для хранения конфигурационной информации
        private Dictionary<string, string> CONFIG_INFO;

        // Конструктор класса
        public BackupDatabase()
        {
            // Чтение конфигурационной информации из файла
            CONFIG_INFO = _config.ReadFile();
            // Получение строки подключения из конфигурации
            CONNECTION_STRING = _config.GetConnectionString();
        }

        public void CreateExitBackup()
        {
            try
            {
                // Создаем имя файла для резервной копии базы данных, например, "Backup_yyyyMMddHHmmss.bak"
                string backupFileName = $"Backup_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.bak";

                // Полный путь к файлу резервной копии
                string backupDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\Hotel\bin\Debug\", @"\Backups"));
                string backupFilePath = Path.Combine(backupDirectory, backupFileName);

                // Создаем папку Backups, если она не существует
                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                // Создаем соединение с базой данных
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            // Экспорт базы данных в файл
                            mb.ExportToFile(backupFilePath);

                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании резервной копии: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для создания резервной копии базы данных
        public bool CreateBackup()
        {
            // Диалог сохранения файла
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
                FileName = $"{CONFIG_INFO["database"]}_backup_{DateTime.Now:yyyy-MM-dd_HH-mm}.sql"
            };

            // Если пользователь выбрал место для сохранения
            if (saveFileDialog.ShowDialog() == true)
            {
                string backupPath = saveFileDialog.FileName;
                try
                {
                    // Подключение к базе данных
                    using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();

                                // Экспорт базы данных в файл
                                mb.ExportToFile(backupPath);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        // Метод для восстановления базы данных из резервной копии
        public bool RestoreBackup()
        {
            // Диалог открытия файла
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*"
            };

            // Если пользователь выбрал файл для восстановления
            if (openFileDialog.ShowDialog() == true)
            {
                string backupFilePath = openFileDialog.FileName;
                try
                {
                    // Подключение к базе данных
                    using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();

                                // Импорт базы данных из файла
                                mb.ImportFromFile(backupFilePath);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        // Метод для импорта таблицы из SQL файла
        public bool ImportTable()
        {
            // Диалог открытия файла
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*"
            };

            // Если пользователь выбрал файл для импорта
            if (openFileDialog.ShowDialog() == true)
            {
                string backupFilePath = openFileDialog.FileName;
                try
                {
                    // Подключение к базе данных
                    using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();

                                // Импорт таблицы из файла
                                mb.ImportFromFile(backupFilePath);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        // Метод для экспорта таблиц в SQL файл
        public bool ExportTable(List<string> tableNames)
        {
            // Диалог сохранения файла
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
                FileName = $"{tableNames[0]}_backup_{DateTime.Now:yyyy-MM-dd_HH-mm}.sql"
            };

            // Если пользователь выбрал место для сохранения
            if (saveFileDialog.ShowDialog() == true)
            {
                string exportPath = saveFileDialog.FileName;
                try
                {
                    // Подключение к базе данных
                    using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();

                                // Экспорт указанных таблиц в файл
                                mb.ExportInfo.TablesToBeExportedList = tableNames;
                                mb.ExportToFile(exportPath);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }
    }
}