using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using ObReg.Core;

namespace ObReg.App.Controls
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class AutoCompleteControl : UserControl, INotifyPropertyChanged
	{
		private class AutoCompleteListBoxItem
		{
			public AutoCompleteListBoxItem(string text, string tooltip)
			{
				Text = text;
				ToolTip = tooltip;
			}

			public string Text { get; private set; }

			public string ToolTip { get; private set; }

			public override string ToString()
			{
				return Text;
			}
		}

		private IOrderItemDataAccessLayer _dataAccessLayer;

		public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(AutoCompleteControl), new PropertyMetadata(string.Empty, OnCodePropertyChanged));

		public AutoCompleteControl()
		{
			InitializeComponent();
			_dataAccessLayer = ModelFactory.CreateOrderItemAccessLayer();
		}

		public string Code
		{
			get
			{
				return (string)GetValue(CodeProperty);
			}
			set
			{
				SetValue(CodeProperty, value);
			}
		}

		private void OnCodePropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				editBox.Text = Code;
			}
		}

		private static void OnCodePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteControl autoCompleteControl = dependencyObject as AutoCompleteControl;
			autoCompleteControl.OnPropertyChanged("Code");
			autoCompleteControl.OnCodePropertyChanged(e);
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			OkButtonAction();
			if (popup.IsOpen)
			{
				editBox.Focus();
				possibleValuesListBox.Items.Filter = null;
			}
		}

		private void OkButtonAction()
		{
			popup.IsOpen = !popup.IsOpen;
			okButton.Content = popup.IsOpen ? "<<" : ">>";
			if (popup.IsOpen)
			{
				InitValues();
			}
		}

		private void editBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SetValue(CodeProperty, editBox.Text);

			if (popup.IsOpen)
			{
				possibleValuesListBox.Items.Filter = (item => (item as AutoCompleteListBoxItem).Text.ToString().ToUpper().StartsWith(editBox.Text.ToUpper()));

				if (possibleValuesListBox.Items.Count == 0)
				{
					OkButtonAction();
				}
			}
		}

		private void InitValues()
		{
			List<AutoCompleteListBoxItem> items = new List<AutoCompleteListBoxItem>();

			IEnumerable<KeyValuePair<string, string>> values = _dataAccessLayer.GetAutoCompleteValues();

			foreach (KeyValuePair<string, string> value in values)
			{
				items.Add(new AutoCompleteListBoxItem(value.Key, value.Value));
			}
			possibleValuesListBox.ItemsSource = items;
		}

		private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			AutoCompleteListBoxItem item = (sender as TextBlock).DataContext as AutoCompleteListBoxItem;
			SetValue(CodeProperty, item.Text);
			OkButtonAction();
		}

		private void popup_Closed(object sender, EventArgs e)
		{
			okButton.Content = popup.IsOpen ? "<<" : ">>";
		}

		#region INotifyPropertyChanged Implementation

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

		#endregion
	}
}
