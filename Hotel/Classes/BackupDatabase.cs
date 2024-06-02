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

        public bool CreateBackup()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
                FileName = $"{CONFIG_INFO["database"]}_backup_{DateTime.Now:yyyy-MM-dd_HH-mm}.sql"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string backupPath = saveFileDialog.FileName;
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

                                mb.ExportToFile(backupPath);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        public bool RestoreBackup()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string backupFilePath = openFileDialog.FileName;
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

                                mb.ImportFromFile(backupFilePath);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        public bool ImportTable()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string backupFilePath = openFileDialog.FileName;
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

                                mb.ImportFromFile(backupFilePath);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        public bool ExportTable(List<string> tableNames)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
                FileName = $"{tableNames[0]}_backup_{DateTime.Now:yyyy-MM-dd_HH-mm}.sql"
            };
            if (saveFileDialog.ShowDialog() == true)
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

                                mb.ExportInfo.TablesToBeExportedList = tableNames;
                                mb.ExportToFile(CONFIG_INFO["path"]);

                                conn.Close();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }
    }
}
