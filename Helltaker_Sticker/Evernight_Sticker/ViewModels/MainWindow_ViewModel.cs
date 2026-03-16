using Evernight_Sticker.Components;
using Evernight_Sticker.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = System.Windows.Application;
using Timer = System.Windows.Forms.Timer;

namespace Evernight_Sticker.ViewModels
{
    public class MainWindow_ViewModel : ViewModelBase
    {
        private MainWindow _mainWindow;

        private NotifyIcon _noti;
        private List<DisplayWindow> _displayWindows;
        private JsonParser _jsonParser;
        private App _app;

        private Timer _timer;

        private int _frame;
        private int _frameInterval;

        public int Frame
        {
            get => _frame;
            set => SetProperty(ref _frame, value);
        }

        public int FrameInterval
        {
            get => _frameInterval;
            set
            {
                SetProperty(ref _frameInterval, value);
                _timer.Interval = value;
            }
        }

        public bool Topmost
        {
            get => _jsonParser.Settings.Topmost;
            set
            {
                _jsonParser.Settings.Topmost = value;
                foreach (DisplayWindow displayWindow in _displayWindows)
                {
                    displayWindow.Topmost = value;
                }
            }
        }

        /// <summary>
        /// Constructor for MainWindow_ViewModel. Initializes the NotifyIcon and sets up the context menu for the system tray icon.
        /// Initiate Timer for frame animation
        /// </summary>
        /// <param name="mainWindow"></param>
        public MainWindow_ViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            _app = Application.Current as App;
            _displayWindows = _app.DisplayWindows;
            _jsonParser = _app.JsonParser;

            // Generate NotifyIcon and context menu
            GenerateNotifyIcon();

            // Initiate Timer for frame animation
            _frame = -1;
            _frameInterval = 80;

            _timer = new Timer();
            _timer.Interval = _frameInterval;
            _timer.Tick += NextFrame;
            _timer.Start();
        }

        private void NextFrame(object sender, EventArgs e)
        {
            _frame++;
            // the evernight png has only 10 frames, so reset to 0 when it reaches 10
            if (_frame == 10)
            {
                _frame = 0;
            }

            foreach (DisplayWindow displayWindow in _displayWindows)
            {
                (displayWindow.DataContext as DisplayWindow_ViewModel).NextFrame(_frame);
            }
        }

        internal void Window_Closing()
        {
            _noti.Visible = false;
            _noti.Icon = null;
            _noti.Dispose();
        }

        private void GenerateNotifyIcon()
        {
            _noti?.Dispose();

            // tree level 0
            ContextMenu menu = new ContextMenu();
            _noti = new NotifyIcon
            {
                Icon = new Icon(@"Resources\icon.ico", new Size(10, 10)),
                Visible = true,
                Text = "Evernight",
                ContextMenu = menu
            };

            Topmost = _jsonParser.Settings.Topmost;

            #region Level 1
            // topmost
            MenuItem topmostItem = new MenuItem
            {
                Text = "Always on Top",
                Checked = Topmost
            };
            topmostItem.Click += (s, e) =>
            {
                Topmost = !Topmost;
                topmostItem.Checked = Topmost;
            };

            // reset display windows
            MenuItem resetItem = new MenuItem
            {
                Text = "Reset Display Windows"
            };
            resetItem.Click += (s, e) =>
            {
                foreach (DisplayWindow displayWindow in _displayWindows)
                {
                    displayWindow.Close();
                }

                _displayWindows.Clear();

                DisplaySettings settings = new DisplaySettings
                {
                    FilePath = @"Resources\Evernight.png",
                    Name = "Evernight",
                    Top = 100,
                    Left = 100,
                    Width = 300,
                    Height = 300,
                };
                _displayWindows.Add(new DisplayWindow(settings));
                _displayWindows.First().Show();
            };

            // exit
            MenuItem exitItem = new MenuItem
            {
                Text = "Exit"
            };
            exitItem.Click += (s, e) =>
            {
                _app.SaveCurrentState();
                _app.IsOnClose = true;

                foreach (var displayWindow in _displayWindows)
                {
                    displayWindow.Close();
                }
                _displayWindows.Clear();

                _mainWindow.Close();
                _app.Shutdown();
            };
            #endregion

            #region Level 2
            MenuItem displayItem = new MenuItem
            {
                Text = "Summon"
            };

            // dynamically add display windows to menu
            Directory.GetFiles(@"Resources\", "*.png").ToList().ForEach(file =>
            {
                MenuItem item = new MenuItem
                {
                    Text = Path.GetFileNameWithoutExtension(file)
                };
                item.Click += (s, e) =>
                {
                    DisplaySettings settings = new DisplaySettings
                    {
                        FilePath = file,
                        Name = Path.GetFileNameWithoutExtension(file),
                        Top = 100,
                        Left = 100,
                        Width = 300,
                        Height = 300,
                    };
                    DisplayWindow displayWindow = new DisplayWindow(settings);
                    _displayWindows.Add(displayWindow);
                    displayWindow.Show();
                };
                displayItem.MenuItems.Add(item);
            });

            #endregion

            // add to menu
            menu.MenuItems.Add(displayItem);
            menu.MenuItems.Add(topmostItem);
            menu.MenuItems.Add(resetItem);
            menu.MenuItems.Add(exitItem);
        }
    }
}
