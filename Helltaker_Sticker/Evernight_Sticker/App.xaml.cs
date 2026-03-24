using Evernight_Sticker.Components;
using Evernight_Sticker.ViewModels;
using Evernight_Sticker.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Evernight_Sticker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool IsOnClose { get; set; }

        public List<DisplayWindow> DisplayWindows { get; set; }
        public JsonParser JsonParser { get; set; }

        public App()
        {
            IsOnClose = false;
            JsonParser = new JsonParser();

            DisplayWindows = new List<DisplayWindow>();
        }

        public void SaveCurrentState()
        {
            JsonParser.Settings.DisplaySettings.Clear();

            foreach (DisplayWindow displayWindow in DisplayWindows)
            {
                DisplaySettings settings = (displayWindow.DataContext as DisplayWindow_ViewModel).DisplaySettings.Copy();
                JsonParser.Settings.DisplaySettings.Add(settings);
            }
            JsonParser.SaveSettings();
        }
    }
}
