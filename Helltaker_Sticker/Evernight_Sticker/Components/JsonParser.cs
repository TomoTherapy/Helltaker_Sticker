using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evernight_Sticker.Components
{
    public class JsonParser
    {
        private readonly string path = @"EvernightSticker_Settings.json";
        private Settings _settings;

        public JsonParser()
        {
            LoadSettings();
        }

        public Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    LoadSettings();
                }
                return _settings;
            }
        }

        public void SaveSettings()
        {
            string json = Serialize(_settings);
            File.WriteAllText(path, json);
        }

        public void LoadSettings()
        {
            try
            {
                string text = File.ReadAllText(path);
                _settings = Deserialize<Settings>(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
                _settings = new Settings(); // Load default settings if file is missing or corrupted
            }
        }

        public static T Deserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    
        public static string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

    }
}
