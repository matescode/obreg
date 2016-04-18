using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

using ObReg.Core;
using ObReg.App.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Threading;
using System.IO.Packaging;
using System.Windows.Media;
using System.Printing;

namespace ObReg.App.ViewModel
{
	public class OrderItemDataProvider : BaseViewModel
	{
		private IOrderItemDataAccessLayer _dataAccessLayer;
		private OrderItemType _itemType;
		private Action _postExecuteResolveItem;
		private Visibility _resolveButtonVisibility;

		public OrderItemDataProvider(OrderItemType itemType, SearchParameter searchParam = null, Action postExecuteResolveItem = null)
		{
			_dataAccessLayer = ModelFactory.CreateOrderItemAccessLayer();
			_itemType = itemType;
			_postExecuteResolveItem = postExecuteResolveItem;
			SearchParameter = searchParam;
			Init();
		}

		public OrderItemType ItemType
		{
			get
			{
				return _itemType;
			}
		}

		public SearchParameter SearchParameter
		{
			get;
			private set;
		}

		public OrderItemListViewModel Data
		{
			get
			{
				OrderItemListViewModel viewModel = new OrderItemListViewModel(_itemType);

				foreach (OrderItem orderItem in _dataAccessLayer.GetData(_itemType, SearchParameter))
				{
					viewModel.Add(new OrderItemViewModel(orderItem));
				}

				viewModel.CollectionChanged += new NotifyCollectionChangedEventHandler(ViewModelCollectionChanged);
				viewModel.ItemEndEdit += new ItemEndEditEventHandler(OrderItemEndEdit);
				return viewModel;
			}
		}

		public Visibility ResolveButtonVisibility
		{
			get
			{
				return _resolveButtonVisibility;
			}
			set
			{
				_resolveButtonVisibility = value;
				NotifyPropertyChanged("ResolveButtonVisibility");
			}
		}

		public Visibility DatePickerVisibility
		{
			get;
			private set;
		}

		public Visibility ReadonlyVisibility
		{
			get;
			private set;
		}

		public bool CanUserAddRows
		{
			get;
			private set;
		}

		public bool IsReadonly
		{
			get;
			private set;
		}

		public ICommand ResolveItemCommand
		{
			get
			{
				return new RelayCommand(
					param => ExecuteResolveItem(param), 
					param => CanExecuteResolveItem(param)
				);
			}
		}

		public ICommand PrintCommand
		{
			get
			{
				return new RelayCommand(param => ExecutePrintCommand(param));
			}
		}

		#region Internals and Helpers

		private void Init()
		{
			switch (_itemType)
			{
				case OrderItemType.Unresolved:
					IsReadonly = false;
					CanUserAddRows = true;
					ResolveButtonVisibility = Visibility.Visible;
					DatePickerVisibility = Visibility.Visible;
					ReadonlyVisibility = Visibility.Hidden;
					break;

				case OrderItemType.History:
				case OrderItemType.Resolved:
					IsReadonly = true;
					CanUserAddRows = false;
					ResolveButtonVisibility = Visibility.Hidden;
					DatePickerVisibility = Visibility.Hidden;
					ReadonlyVisibility = Visibility.Visible;
					break;

				case OrderItemType.Results:
					CanUserAddRows = false;
					if (SearchParameter != null && SearchParameter.ItemType == OrderItemType.Unresolved)
					{
						IsReadonly = false;
						ResolveButtonVisibility = Visibility.Visible;
						DatePickerVisibility = Visibility.Visible;
						ReadonlyVisibility = Visibility.Hidden;
					}
					else
					{
						IsReadonly = true;
						ResolveButtonVisibility = Visibility.Hidden;
						DatePickerVisibility = Visibility.Hidden;
						ReadonlyVisibility = Visibility.Visible;
					}
					break;
			}
		}

		private void ViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (object item in e.OldItems)
				{
					OrderItemViewModel orderItemViewModel = (item as OrderItemViewModel);
					_dataAccessLayer.DeleteOrderItem(orderItemViewModel.Source);
				}
			}
		}

		private void OrderItemEndEdit(IEditableObject sender)
		{
			_dataAccessLayer.UpdateOrderItem((sender as OrderItemViewModel).Source);
		}

		private void ExecuteResolveItem(object param)
		{
			_dataAccessLayer.ResolveItem((param as OrderItemViewModel).Source);
			NotifyPropertyChanged("Data");
			if (_postExecuteResolveItem != null)
			{
				_postExecuteResolveItem();
			}
		}

		private bool CanExecuteResolveItem(object param)
		{
			return ((param is OrderItemViewModel) && (param as OrderItemViewModel).TerminationDate.HasValue);
		}

		private void ExecutePrintCommand(object parameter)
		{
			PrintDialog printDialog = new PrintDialog();
			bool? dlgResult = printDialog.ShowDialog();
			if (dlgResult == true)
			{
				DataGrid printGrid = Application.Current.Resources["PrintDataGrid"] as DataGrid;

				if (printGrid.Parent != null)
				{
					FixedPage parentPage = printGrid.Parent as FixedPage;
					parentPage.Children.Remove(printGrid);
				}

				printGrid.DataContext = Data;
				
				ICollectionView dataView = CollectionViewSource.GetDefaultView(printGrid.Items);
				ICollectionView sourceDataView = CollectionViewSource.GetDefaultView((parameter as DataGrid).ItemsSource);
				
				dataView.SortDescriptions.Clear();
				foreach (SortDescription sortDescription in sourceDataView.SortDescriptions)
				{
					dataView.SortDescriptions.Add(sortDescription);
				}
				dataView.Refresh();

				FixedDocument fixedDocument = new FixedDocument();
				PageContent pageContent = new PageContent();
				FixedPage fixedPage = new FixedPage();

				fixedPage.Children.Add(printGrid);
				(pageContent as System.Windows.Markup.IAddChild).AddChild(fixedPage);
				fixedDocument.Pages.Add(pageContent);

				XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printDialog.PrintQueue);

				writer.Write(fixedDocument, printDialog.PrintTicket);
			}
		}

		#endregion
	}
}
