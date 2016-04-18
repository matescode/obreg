using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;

namespace ObReg.App.ViewModel
{
	public class RelayCommand : ICommand
	{
		#region Members

		private Action<object> _execute;
		private Predicate<object> _canExecutePredicate;

		#endregion

		public RelayCommand(Action<object> executeAction, Predicate<object> canExecutePredicate = null)
		{
			_execute = executeAction;
			_canExecutePredicate = canExecutePredicate;
		}

		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			return (_canExecutePredicate == null) ? true : _canExecutePredicate(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		#endregion
	}
}
