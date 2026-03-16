using Evernight_Sticker.Components;
using Evernight_Sticker.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Evernight_Sticker.ViewModels
{
    public class DisplayWindow_ViewModel : ViewModelBase
    {
        private DisplayWindow _displayWindow;
        
        private string _title;
        private DisplaySettings _displaySettings;
        private App _app;

        private const int FRAME_COUNT = 10;

        private Bitmap _originalBitmap;
        private Bitmap[] _bitmapCollection = new Bitmap[FRAME_COUNT];
        private BitmapSource[] _bitmapSourceCollection = new BitmapSource[FRAME_COUNT];


        private ImageSource _currentFrame;

        public ImageSource CurrentFrame
        {
            get => _currentFrame;
            set => SetProperty(ref _currentFrame, value);
        }

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

        public MainWindow_ViewModel MainWindow_ViewModel { get => Application.Current.MainWindow.DataContext as MainWindow_ViewModel; }

        public int FrameInterval
        {
            get => MainWindow_ViewModel.FrameInterval;
            set
            {
                MainWindow_ViewModel.FrameInterval = value;
                OnPropertyChanged();
            }
        }

        // To delete object to save memory
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);


        /// <summary>
        /// Constructor for DisplayWindow_ViewModel. Initializes the bitmap collection for the display window based on the provided display settings.
        /// </summary>
        /// <param name="displayWindow"></param>
        /// <param name="displaySettings"></param>
        public DisplayWindow_ViewModel(DisplayWindow displayWindow, DisplaySettings displaySettings)
        {
            _displayWindow = displayWindow;
            _displaySettings = displaySettings;

            _app = Application.Current as App;

            GenerateSpriteCollection(displaySettings);
        }

        internal void Window_Closing()
        {
            DiposeSpriteCollection();
        }

        internal void Close_button_Click()
        {
            _displayWindow.Close();

            _app.DisplayWindows.Remove(_displayWindow);
            _app.SaveCurrentState();
        }

        internal void NextFrame(int frame)
        {
            CurrentFrame = _bitmapSourceCollection[frame];
        }

        private void GenerateSpriteCollection(DisplaySettings settings)
        {
            DiposeSpriteCollection();

            Title = settings.Name;

            string filePath = settings.FilePath;

            if (!File.Exists(filePath))
            {
                return;
            }

            _originalBitmap = new Bitmap(filePath);

            int frameWidth = _originalBitmap.Width / FRAME_COUNT;

            for (int i = 0; i < FRAME_COUNT; i++)
            {
                _bitmapCollection[i] = _originalBitmap.Clone(new Rectangle(i * frameWidth, 0, frameWidth, _originalBitmap.Height), _originalBitmap.PixelFormat);
                IntPtr hBitmap = _bitmapCollection[i].GetHbitmap();
                try
                {
                    _bitmapSourceCollection[i] = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(hBitmap);
                }
            }
        }

        private void DiposeSpriteCollection()
        {
            if (_originalBitmap != null)
            {
                _originalBitmap.Dispose();
                _originalBitmap = null;
            }

            for (int i = 0; i < FRAME_COUNT; i++)
            {
                if (_bitmapCollection[i] != null)
                {
                    _bitmapCollection[i].Dispose();
                    _bitmapCollection[i] = null;
                }

                _bitmapSourceCollection[i] = null;
            }
        }

        internal void FrameIntervaleDefault_button_Click()
        {
            FrameInterval = 80;
        }
    }
}
