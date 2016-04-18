using System;
using System.Threading;
using System.Threading.Tasks;
using ObReg.App.Windows;
using ObReg.Core;

namespace ObReg.App
{
    public class Bootstrap
    {
        [STAThread]
        public static void Main(string[] args)
        {
            ObRegApplication applicaton = new ObRegApplication();
            //applicaton.Run(new MainWindow(loadingWindow));
            applicaton.Run();
        }
    }
}