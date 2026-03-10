using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Helltaker_Sticker.Xml;

namespace Helltaker_Sticker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool Language { get; set; }
        public bool IsOnClose { get; set; }
        public List<HellGirl> Girls { get; set; }
        public Xml_Parser xml_Parser;
		private Mutex mutex;

        public App()
        {
			string mutexId = "HelltakerSticker";

			// Try to open the mutex
			bool createdNew;
			mutex = new Mutex(false, mutexId, out createdNew);

			if (!createdNew)
			{
				// Mutex already exists, which means another instance of the application is running
				Console.WriteLine("Another instance of the application is already running.");
				Application.Current.Shutdown();
				return;
			}

			IsOnClose = false;
            xml_Parser = new Xml_Parser();
            xml_Parser.LoadSettings();

            if (xml_Parser.settings.Language == "Korean") Language = true;
            else Language = false;

            Girls = new List<HellGirl>();
        }

        public void SaveCurrentState()
        {
            xml_Parser.settings.GirlSettings.Clear();

            foreach (HellGirl girl in Girls)
            {
                xml_Parser.settings.GirlSettings.Add(new GirlSetting() { Name = girl.Title, Top = girl.Top, Left = girl.Left });
            }

            xml_Parser.SaveSettings();
        }

    }
}
