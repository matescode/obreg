using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ObReg.Core;
using System.Windows.Input;
using ObReg.App.Windows;

namespace ObReg.App.ViewModel
{
	public class HistoryNavigatorYearViewModel : BaseViewModel
	{
		public HistoryNavigatorYearViewModel(HistoryDateHierarchy hierarchy)
		{
			Months = hierarchy.Months.Select(item => new HistoryNavigatorMonthViewModel(hierarchy.Year, item));
			Year = hierarchy.Year;
		}

		public int Year
		{
			get;
			private set;
		}

		public IEnumerable<HistoryNavigatorMonthViewModel> Months
		{
			get;
			private set;
		}

		public ICommand SelectYearCommand
		{
			get
			{
				return new RelayCommand(param => ExecuteSelectYearCommand());
			}
		}

		#region Internals

		private void ExecuteSelectYearCommand()
		{
			SearchParameter searchParam = new SearchParameter(Year, -1);
			ResultWindow resultWindow = new ResultWindow();
			resultWindow.resultList.DataContext = new OrderItemDataProvider(OrderItemType.Results, searchParam);
			resultWindow.ShowDialog();
		}

		#endregion
	}
}
