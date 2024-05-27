using Newtonsoft.Json;
using Hotel.Classes;
using System.IO;
using System.Collections.Generic;
using System;

namespace Hotel.Classes
{


    public class ReadConfigFile
    {
        private string FILE_PATH = Directory.GetCurrentDirectory().Replace("Hotel\\bin\\Debug", "db_config.json");
        private ConfigData CONFIG;
        private Dictionary<string, string> CONFIG_INFO = new Dictionary<string, string>
        {
            { "server", "" },
            { "port", "" },
            { "database", "" },
            { "uid", "" },
            { "pwd", "" },
            { "path", "" },
        };

        public Dictionary<string, string> ReadFile()
        {
            if (File.Exists(FILE_PATH))
            {
                string json = File.ReadAllText(FILE_PATH);
                CONFIG = JsonConvert.DeserializeObject<ConfigData>(json);
            }

            CONFIG_INFO["server"] = CONFIG.Server;
            CONFIG_INFO["port"] = CONFIG.Port;
            CONFIG_INFO["database"] = CONFIG.Database;
            CONFIG_INFO["uid"] = CONFIG.Username;
            CONFIG_INFO["pwd"] = CONFIG.Password;

            string currentDirectory = Directory.GetCurrentDirectory();
            string backupDirectory = currentDirectory.Replace("Hotel\\bin\\Debug", "Backups");
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            CONFIG_INFO["path"] = backupDirectory;

            return CONFIG_INFO;
        }

        public string GetConnectionString()
        {
            if (File.Exists(FILE_PATH))
            {
                string json = File.ReadAllText(FILE_PATH);
                CONFIG = JsonConvert.DeserializeObject<ConfigData>(json);
            }

            string conn_string = $"Server={CONFIG.Server};Port={CONFIG.Port};Database={CONFIG.Database};Uid={CONFIG.Username};Pwd={CONFIG.Password};";
            return conn_string;
        }
    }
}
