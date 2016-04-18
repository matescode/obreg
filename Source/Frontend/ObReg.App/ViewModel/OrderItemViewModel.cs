using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using ObReg.Core;
using System.Windows;

namespace ObReg.App.ViewModel
{
	public delegate void ItemEndEditEventHandler(IEditableObject sender);

	public class OrderItemViewModel : BaseViewModel, IEditableObject
	{
		private OrderItem _orderItem;
		private Visibility _resolveButtonVisibility;

		public event ItemEndEditEventHandler ItemEndEdit;

		public OrderItemViewModel()
		{
			_orderItem = new OrderItem();
			ReceiveDate = DateTime.Now;
		}

		public OrderItemViewModel(OrderItem orderItem)
		{
			_orderItem = orderItem;
		}

		public OrderItem Source
		{
			get
			{
				return _orderItem;
			}
		}

		public long Id
		{
			get
			{
				return _orderItem.Id;
			}
			set
			{
				_orderItem.Id = value;
				NotifyPropertyChanged("Id");
			}
		}

		public string Code
		{
			get
			{
				return _orderItem.Code;
			}
			set
			{
				_orderItem.Code = value;
				NotifyPropertyChanged("Code");
			}
		}

		public string Text
		{
			get
			{
				return _orderItem.Text;
			}
			set
			{
				_orderItem.Text = value;
				NotifyPropertyChanged("Text");
			}
		}

		public long Count
		{
			get
			{
				return _orderItem.Count;
			}
			set
			{
				_orderItem.Count = value;
				NotifyPropertyChanged("Count");
			}
		}

		public long FinalCount
		{
			get
			{
				return _orderItem.FinalCount;
			}
			set
			{
				_orderItem.FinalCount = value;
				NotifyPropertyChanged("FinalCount");
			}
		}

		public DateTime ReceiveDate
		{
			get
			{
				return _orderItem.ReceiveDate;
			}
			set
			{
				_orderItem.ReceiveDate = value;
				NotifyPropertyChanged("ReceiveDate");
			}
		}

		public string ReadonlyReceiveDate
		{
			get
			{
				return GetReadonlyDate(ReceiveDate);
			}
		}

		public DateTime? EstimatedDate
		{
			get
			{
				return _orderItem.EstimatedDate;
			}
			set
			{
				_orderItem.EstimatedDate = value;
				NotifyPropertyChanged("EstimatedDate");
			}
		}

		public string ReadonlyEstimatedDate
		{
			get
			{
				return GetReadonlyDate(EstimatedDate);
			}
		}

		public DateTime? TerminationDate
		{
			get
			{
				return _orderItem.TerminationDate;
			}
			set
			{
				_orderItem.TerminationDate = value;
				NotifyPropertyChanged("TerminationDate");
			}
		}

		public string ReadonlyTerminationDate
		{
			get
			{
				return GetReadonlyDate(TerminationDate);
			}
		}

		public long Status
		{
			get
			{
				return _orderItem.Status;
			}
			set
			{
				_orderItem.Status = value;
				NotifyPropertyChanged("Status");
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

		#region Internals and Helpers

		private string GetReadonlyDate(DateTime? date)
		{
			return (date.HasValue) ? string.Format("{0}.{1}.{2}", date.Value.Day, date.Value.Month, date.Value.Year) : "-";
		}

		#endregion

		#region IEditableObject Members

		public void BeginEdit()
		{
		}

		public void CancelEdit()
		{
		}

		public void EndEdit()
		{
			if (ItemEndEdit != null)
			{
				ItemEndEdit(this);
			}
		}

		#endregion
	}
}
