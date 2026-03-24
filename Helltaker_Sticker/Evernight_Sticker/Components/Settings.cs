using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evernight_Sticker.Components
{
    [Serializable]
    public class Settings
    {
        public List<DisplaySettings> DisplaySettings { get; set; }
        public bool Topmost { get; set; }

        public Settings()
        {
            DisplaySettings = new List<DisplaySettings>();
            Topmost = true;
        }
    }

    [Serializable]
    public class DisplaySettings
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public DisplaySettings()
        {
            Name = string.Empty;
            FilePath = string.Empty;
            Left = 0;
            Top = 0;
            Width = 200;
            Height = 200;
        }

        public DisplaySettings(string name, string filePath, double left, double top, double width, double height)
        {
            Name = name;
            FilePath = filePath;
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public DisplaySettings(DisplaySettings displaySettings)
        { 
            Name = displaySettings.Name;
            FilePath = displaySettings.FilePath;
            Left = displaySettings.Left;
            Top = displaySettings.Top;
            Width = displaySettings.Width;
            Height = displaySettings.Height;
        }

        public DisplaySettings Copy()
        {
            return new DisplaySettings(this);
        }
    }
}
