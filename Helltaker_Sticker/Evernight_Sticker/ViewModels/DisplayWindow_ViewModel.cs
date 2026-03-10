using Evernight_Sticker.Components;
using Evernight_Sticker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Evernight_Sticker.ViewModels
{
    public class DisplayWindow_ViewModel : ViewModelBase
    {
        private DisplayWindow _displayWindow;
        private string _title;
        private DisplaySettings _displaySettings;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string FilePath
        {
            get => _displaySettings.FilePath;
            set
            {
                _displaySettings.FilePath = value;
                OnPropertyChanged();
            }
        }

        public double Left
        {
            get => _displaySettings.Left;
            set
            {
                _displaySettings.Left = value;
                OnPropertyChanged();
            }
        }

        public double Top
        {
            get => _displaySettings.Top;
            set
            {
                _displaySettings.Top = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get => _displaySettings.Width;
            set
            {
                _displaySettings.Width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get => _displaySettings.Height;
            set
            {
                _displaySettings.Height = value;
                OnPropertyChanged();
            }
        }

        public DisplayWindow_ViewModel(DisplayWindow displayWindow, DisplaySettings displaySettings)
        {
            _displayWindow = displayWindow;
            _displaySettings = displaySettings;
        }

        internal void Window_Closing()
        {
            
        }
    }
}
