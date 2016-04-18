using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ObReg.Core;
using ObReg.App.Windows;
using ObReg.App.Controls;
using System.Windows.Input;

namespace ObReg.App.ViewModel
{
	public class HistoryWindowViewModel
	{
		#region Properties

		public OrderItemDataProvider HistoryDataProvider
		{
			get
			{
				return new OrderItemDataProvider(OrderItemType.History);
			}
		}

		public HistoryNavigatorViewModel HistoryNavigator
		{
			get
			{
				return new HistoryNavigatorViewModel();
			}
		}

		public ICommand FindItemsCommand
		{
			get
			{
				return new RelayCommand(
					param => ExecuteFindItemsCommand(param),
					param => CanExecuteFindItemsCommand(param)
				);
			}
		}

		#endregion

		#region Internals and Helpers

		private void ExecuteFindItemsCommand(object param)
		{
			SearchParameter searchParam = new SearchParameter(OrderItemType.Resolved, param.ToString());
			ResultWindow resultWindow = new ResultWindow();
			resultWindow.Title = string.Format("Výsledek hledání: {0}", param.ToString());
			resultWindow.resultList.DataContext = new OrderItemDataProvider(OrderItemType.Results, searchParam);
			resultWindow.Width = ModelFactory.Configuration.GetInt("CurrentWidth");
			resultWindow.ShowDialog();
		}

		private bool CanExecuteFindItemsCommand(object param)
		{
			return !string.IsNullOrWhiteSpace(param.ToString());
		}

		#endregion
	}
}
