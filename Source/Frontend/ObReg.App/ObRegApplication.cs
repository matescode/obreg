using System;
using System.Windows;

namespace ObReg.App
{
    public class ObRegApplication : Application
    {
        public ObRegApplication()
        {
            StartupUri = new Uri(@"Windows\LoadingWindow.xaml", UriKind.Relative);
            LoadResources();
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        private void LoadResources()
        {
            Resources = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/ObReg.App;Component/ApplicationResources.xaml")
                };
        }
    }
}