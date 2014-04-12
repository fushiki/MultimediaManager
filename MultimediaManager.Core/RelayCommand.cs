using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultimediaManager.Core
{
    public class RelayCommand:ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    public class RelayCommand<T>:ICommand
    {
        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is T)
                return _canExecute == null ? true : _canExecute((T)parameter);
            else
                throw new InvalidCastException("Parameter type is=" + parameter.GetType().ToString() + ". Expected=" + typeof(T).ToString() + ".");
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if(parameter is T)
                _execute((T)parameter);
            else
                throw new InvalidCastException("Parameter type is=" + parameter.GetType().ToString() + ". Expected=" + typeof(T).ToString() + ".");
        }
    }
}
