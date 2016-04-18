using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObReg.Core
{
	public class SearchParameter
	{
		private SearchParameter(OrderItemType itemType, string code, int year, int month)
		{
			Code = code;
			ItemType = itemType;
			Year = year;
			Month = month;
		}

		public SearchParameter(OrderItemType itemType, string code)
			: this(itemType, code, -1, -1)
		{
		}

		public SearchParameter(int year, int month)
			: this(OrderItemType.Resolved, string.Empty, year, month)
		{
		}

		public OrderItemType ItemType
		{
			get;
			private set;
		}

		public string Code
		{
			get;
			private set;
		}

		public int Year
		{
			get;
			private set;
		}

		public int Month
		{
			get;
			private set;
		}
	}
}
