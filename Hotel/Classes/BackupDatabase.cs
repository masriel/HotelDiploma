using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

using MySqlConnector;

namespace Hotel.Classes
{
    public class BackupDatabase
    {
        private ReadConfigFile _config = new ReadConfigFile();
        private string CONNECTION_STRING = String.Empty;

        private Dictionary<string, string> CONFIG_INFO;

        public BackupDatabase() 
        {
            CONFIG_INFO = _config.ReadFile();
            CONNECTION_STRING = _config.GetConnectionString();
        }

        public void CreateBackup()
        {
            CONFIG_INFO["path"] = Path.Combine(CONFIG_INFO["path"], $"{CONFIG_INFO["database"]}_{DateTime.Now:yyyy_MM_dd_HH_mm}_BACKUP.sql");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(CONFIG_INFO["path"]);
                            conn.Close();
                        }
                    }
                }
                //MessageBox.Show("Резервное копирование выполнено успешно.", "РЕЗЕРВНОЕ КОПИРОВАНИЕ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RestoreBackup(string backupPath)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ImportFromFile(backupPath);
                            conn.Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ImportTable(string backupPath)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ImportFromFile(backupPath);

                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportTable(List<string> tableNames)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string backupDirectory = currentDirectory.Replace("Hotel\\bin\\Debug", "Backups");
            CONFIG_INFO["path"] = Path.Combine(backupDirectory, $"{tableNames[0]}_BACKUPS");


            if (!Directory.Exists(CONFIG_INFO["path"]))
            {
                Directory.CreateDirectory(CONFIG_INFO["path"]);
            }

            CONFIG_INFO["path"] = Path.Combine(CONFIG_INFO["path"], $"{tableNames[0]}_{DateTime.Now:yyyy_MM_dd_HH_mm}_BACKUP.sql");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            // Устанавливаем параметры для экспорта только одной таблицы
                            mb.ExportInfo.TablesToBeExportedList = tableNames;
                            mb.ExportToFile(CONFIG_INFO["path"]);

                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
