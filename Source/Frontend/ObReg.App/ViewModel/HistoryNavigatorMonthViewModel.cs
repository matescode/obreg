using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;

using ObReg.Core;
using ObReg.App.Windows;

namespace ObReg.App.ViewModel
{
	public class HistoryNavigatorMonthViewModel : BaseViewModel
	{
		private int _year;
		private int _month;

		public HistoryNavigatorMonthViewModel(int year, int month)
		{
			_year = year;
			_month = month;
			DateTimeFormatInfo info = DateTimeFormatInfo.CurrentInfo;
			Month = info.GetMonthName(_month);
		}

		public string Month
		{
			get;
			private set;
		}

		public ICommand SelectMonthCommand
		{
			get
			{
				return new RelayCommand(param => ExecuteSelectMonthCommand());
			}
		}

		#region Internals

		private void ExecuteSelectMonthCommand()
		{
			SearchParameter searchParam = new SearchParameter(_year, _month);
			ResultWindow resultWindow = new ResultWindow();
			resultWindow.resultList.DataContext = new OrderItemDataProvider(OrderItemType.Results, searchParam);
			resultWindow.ShowDialog();
		}

		#endregion
	}
}
