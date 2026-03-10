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
    }
}
