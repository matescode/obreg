using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;

using AvalonDock;
using AvalonDock.Layout;

using ObReg.App.ViewModel;
using ObReg.Core;

namespace ObReg.App.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoadingWindow _loadingWindow;

        public MainWindow(LoadingWindow loadingWindow)
        {
            InitializeComponent();
            _loadingWindow = loadingWindow;
            DataContext = new WindowViewModel(this, dockingManager);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dockingManager_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void ShowWithCloseLoadingWindow()
        {
            Show(); 
            if (_loadingWindow != null)
            {
                _loadingWindow.ForceClose();
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
