using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObReg.Core
{
	public class HistoryDateHierarchy
	{
		public HistoryDateHierarchy(int year, IEnumerable<int> months)
		{
			Year = year;
			Months = months;
		}

		public int Year
		{
			get;
			private set;
		}

		public IEnumerable<int> Months
		{
			get;
			private set;
		}
	}
}
