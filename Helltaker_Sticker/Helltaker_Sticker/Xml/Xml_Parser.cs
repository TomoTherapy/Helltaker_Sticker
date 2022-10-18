using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Helltaker_Sticker.Xml
{
    public class Xml_Parser
    {
        private string path = @"HelltakerSticker_Settings.xml";
        private XmlSerializer SettingsSerializer;
        public Settings settings { get; set; }

        public Xml_Parser()
        {
            SettingsSerializer = new XmlSerializer(typeof(Settings));
        }

        public void SaveSettings()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    SettingsSerializer.Serialize(writer, settings);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadSettings()
        {
            try
            {
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        settings = SettingsSerializer.Deserialize(reader) as Settings;
                    }
                }
                else
                {
                    settings = new Settings();
                }
            }
            catch
            {
                settings = new Settings();
            }
        }
    }

    public class Settings
    {
        public List<GirlSetting> GirlSettings { get; set; }
        public string Language { get; set; }
        public bool Topmost { get; set; }

        public Settings()
        {
            GirlSettings = new List<GirlSetting>();

            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("KR"))
            {
                Language = "Korean";
            }
            else
            {
                Language = "English";
            }
        }
    }

    public class GirlSetting
    {
        public string Name { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
    }
}
