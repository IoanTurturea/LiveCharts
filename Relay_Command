using System;
using System.Windows.Input;

namespace Microsenzori
{
    public class Class_RelayCommand : ICommand
    {
        private Action _action;

        public Class_RelayCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
