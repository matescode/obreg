using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ObReg.Core;

namespace ObReg.App.ViewModel
{
	public class HistoryNavigatorViewModel : BaseViewModel
	{
		public HistoryNavigatorViewModel()
		{
			IOrderItemDataAccessLayer dataAccessLayer = ModelFactory.CreateOrderItemAccessLayer();
			HistoryData = dataAccessLayer.GetHistorySignatures().Select(item => new HistoryNavigatorYearViewModel(item));
		}

		public IEnumerable<HistoryNavigatorYearViewModel> HistoryData
		{
			get;
			private set;
		}
	}
}
