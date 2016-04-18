using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using AvalonDock;
using AvalonDock.Layout;
using ObReg.App.Controls;
using ObReg.App.Windows;
using ObReg.Core;

namespace ObReg.App.ViewModel
{
	public class WindowViewModel : BaseViewModel
	{
		private DockingManager _dockingManager;
		private OrderItemDataProvider _resolvedListDataProvider;
		private OrderItemDataProvider _mainListDataProvider;
		private Window _mainWindow;

		public WindowViewModel(Window mainWindow, DockingManager dockingManager)
		{
			_mainWindow = mainWindow;
			_dockingManager = dockingManager;
			LoadWindowSettings();
		}

		public OrderItemDataProvider ResolvedListDataProvider
		{
			get
			{
				return _resolvedListDataProvider ?? (_resolvedListDataProvider = new OrderItemDataProvider(OrderItemType.Resolved));
			}
		}

		public OrderItemDataProvider MainListDataProvider
		{
			get
			{
				return _mainListDataProvider ?? (
					_mainListDataProvider = new OrderItemDataProvider(
						OrderItemType.Unresolved,
						null, 
						() => ResolvedListDataProvider.NotifyPropertyChanged("Data")
					)
				);
			}
		}

		public ICommand OpenHistoryWindow
		{
			get
			{
				return new RelayCommand(param => ExecuteOpenHistoryWindow());
			}
		}

		public ICommand ExitApplication
		{
			get
			{
				return new RelayCommand(param => ExecuteApplicationExit());
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

		#region Internals and Helpers

		private void ExecuteOpenHistoryWindow()
		{
			HistoryWindow window = new HistoryWindow();
			window.DataContext = new HistoryWindowViewModel();
			window.Width = _mainWindow.Width + 55;
			window.ShowDialog();
		}

		private void ExecuteApplicationExit()
		{
			IOrderItemDataAccessLayer dataAccessLayer = ModelFactory.CreateOrderItemAccessLayer();
			dataAccessLayer.SaveChanges();
			dataAccessLayer.Destroy();

			IConfig config = ModelFactory.Configuration;
			config.Set("CurrentWidth", _mainWindow.Width);
			config.Set("CurrentHeight", _mainWindow.Height);
			config.Set("Left", _mainWindow.Left);
			config.Set("Top", _mainWindow.Top);
			config.Save();

			Application.Current.Shutdown();
		}

		private void ExecuteFindItemsCommand(object param)
		{
			OrderItemType searchType = ((_dockingManager.ActiveContent as OrderItemList).DataContext as OrderItemDataProvider).ItemType;
			SearchParameter searchParam = new SearchParameter(searchType, param.ToString());
			Action postAction = null;
			if (searchParam.ItemType == OrderItemType.Unresolved)
			{
				postAction = () => ResolvedListDataProvider.NotifyPropertyChanged("Data");
			}
			ResultWindow resultWindow = new ResultWindow();
			resultWindow.Title = string.Format("Výsledek hledání: {0}", param.ToString());
			resultWindow.resultList.DataContext = new OrderItemDataProvider(OrderItemType.Results, searchParam, postAction);
			resultWindow.Width = _mainWindow.Width;
			resultWindow.ShowDialog();
		}

		private bool CanExecuteFindItemsCommand(object param)
		{
			return !string.IsNullOrWhiteSpace(param.ToString());
		}

		private void LoadWindowSettings()
		{
			IConfig config = ModelFactory.Configuration;
			_mainWindow.Width = config.GetInt("CurrentWidth");
			_mainWindow.Height = config.GetInt("CurrentHeight");
			_mainWindow.Left = config.GetInt("Left");
			_mainWindow.Top = config.GetInt("Top");
		}

		#endregion
	}
}
