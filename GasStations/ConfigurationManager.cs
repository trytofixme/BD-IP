using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GasStations
{
    public class ConfigurationManager
    {
        private Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();

        public ConfigurationManager(string filePath)
        {
            ConnectionStrings["default"] =
                "Host=localhost; Database=gas_stations_dev; User ID=postgres; Password=postgres;";
            // if (filePath == null)
            // {
            //     return;
            // }
            // try
            // {
            //     var jsonString = File.ReadAllText(filePath);
            //
            //     var configData = JsonConvert.DeserializeObject<ConfigurationManager>(jsonString);
            //     if (configData != null)
            //     {
            //         //ConfigurationData = (Dictionary<string, JObject>)configData;
            //     }
            // }
            // catch (Exception ex)
            // {
            //     MessageBox.Show(ex.Message, "Некорректные данные соединения", MessageBoxButton.OK, MessageBoxImage.Error);
            // }
        }
        //
        public string GetConnectionsString()
        {
            if (!ConnectionStrings.TryGetValue("default", out var defaultConnection))
            {
                return string.Empty;
            }
        
            return defaultConnection;
        
        }
    }
}