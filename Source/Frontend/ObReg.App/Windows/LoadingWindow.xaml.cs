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
using System.Windows.Shapes;
using ObReg.Core;

namespace ObReg.App.Windows
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private bool _ignoreClosing = true;
        private TaskScheduler _uiScheduler;

        public LoadingWindow()
        {
            InitializeComponent();
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            StartLoading();
        }

        private void LoadingWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_ignoreClosing)
            {
                e.Cancel = true;
            }
        }

        public void ForceClose()
        {
            _ignoreClosing = false;
            Close();
        }

        public void StartLoading()
        {
            Task.Factory.StartNew(() =>
            {
                IOrderItemDataAccessLayer accessLayer = ModelFactory.CreateOrderItemAccessLayer();
                accessLayer.Preload();
            }).ContinueWith(task =>
                {
                    MainWindow mainWindow = new MainWindow(this);
                    mainWindow.ShowWithCloseLoadingWindow();
                }, new CancellationToken(), TaskContinuationOptions.None, _uiScheduler);
        }
    }
}
