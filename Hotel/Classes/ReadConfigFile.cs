using Newtonsoft.Json;
using Hotel.Classes;
using System.IO;

namespace Hotel.Classes
{


    public class ReadConfigFile
    {
        private string FILE_PATH = Directory.GetCurrentDirectory().Replace("Hotel\\bin\\Debug", "db_config.json");
        private ConfigData CONFIG;

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
