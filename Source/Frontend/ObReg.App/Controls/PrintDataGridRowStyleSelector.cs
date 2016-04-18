using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ObReg.App.Controls
{
	public class PrintDataGridRowStyleSelector : StyleSelector
	{
		public override Style SelectStyle(object item, DependencyObject container)
		{
			DataGrid grid = ItemsControl.ItemsControlFromItemContainer(container) as DataGrid;
			int index = grid.Items.IndexOf(item);			

			Style style = new Style(typeof(DataGridRow));
			Setter backgroundSetter = new Setter();
			backgroundSetter.Property = DataGridRow.BackgroundProperty;
			if (index % 2 == 0)
			{
				backgroundSetter.Value = Brushes.White;
			}
			else
			{
				backgroundSetter.Value = Brushes.LightGray;
			}
			style.Setters.Add(backgroundSetter);
			return style;
		}
	}
}
