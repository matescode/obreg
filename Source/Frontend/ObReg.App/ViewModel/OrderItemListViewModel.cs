using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using AvalonDock.Layout;
using ObReg.App.Controls;
using ObReg.Core;
using System.Windows;
using System.ComponentModel;

namespace ObReg.App.ViewModel
{
	public class OrderItemListViewModel : ObservableCollection<OrderItemViewModel>
	{
		public event ItemEndEditEventHandler ItemEndEdit;

		public OrderItemListViewModel(OrderItemType type)
		{
			Type = type;
		}

		public OrderItemType Type
		{
			get; private set;
		}

		public string Title
		{
			get
			{
				switch (Type)
				{
					case OrderItemType.Unresolved:
						return "Hlavní";
					
					case OrderItemType.Resolved:
						return "Vyřízené";
					
					case OrderItemType.History:
						return "Historie";

					case OrderItemType.Results:
						return "Výsledky hledání";

					default:
						return String.Empty;
				}
			}
		}

		protected override void InsertItem(int index, OrderItemViewModel item)
		{
			base.InsertItem(index, item);
			
			item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
		}

		private void ItemEndEditHandler(IEditableObject sender)
		{
			if (ItemEndEdit != null)
			{
				ItemEndEdit(sender);
			}
		}
	}
}
