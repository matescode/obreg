using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ObReg.App.Controls
{
	/// <summary>
	/// Interaction logic for CurrentTimeControl.xaml
	/// </summary>
	public partial class CurrentTimeControl : UserControl
	{
		#region Members

		private DispatcherTimer mainTimer; 

		#endregion

		public CurrentTimeControl()
		{
			InitializeComponent();
			SetTime();
			mainTimer = new DispatcherTimer();
			mainTimer.Tick += new EventHandler(mainTimer_Tick);
			mainTimer.Interval = new TimeSpan(0, 0, 1);
			mainTimer.Start();
		}

		private void mainTimer_Tick(object sender, EventArgs e)
		{
			SetTime();
		}

		private void SetTime()
		{
			DateTime date = DateTime.Now;
			currentDateValue.Text = string.Format("Dnes je: {0}.{1}.{2}", date.Day, date.Month, date.Year);
		}
	}
}
