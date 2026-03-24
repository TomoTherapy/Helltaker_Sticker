using Evernight_Sticker.Components;
using Evernight_Sticker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Evernight_Sticker.Views
{
    /// <summary>
    /// Interaction logic for DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        private DisplayWindow_ViewModel _viewModel;
        private App _app;

        

        public DisplayWindow(DisplaySettings settings)
        {
            InitializeComponent();

            var viewModel = new DisplayWindow_ViewModel(this, settings);

            DataContext = viewModel;
            _viewModel = viewModel;

            _app = Application.Current as App;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _app.SaveCurrentState();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }
            this.DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Close_button_Click();
            _app.SaveCurrentState();
        }

        private void FrameIntervaleDefault_button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FrameIntervaleDefault_button_Click();
            _app.SaveCurrentState();
        }
    }
}
