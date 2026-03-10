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
        DisplayWindow_ViewModel _viewModel;

        public DisplayWindow(DisplaySettings settings)
        {
            InitializeComponent();

            var viewModel = new DisplayWindow_ViewModel(this, settings);

            DataContext = viewModel;
            _viewModel = viewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {

        }
    }
}
