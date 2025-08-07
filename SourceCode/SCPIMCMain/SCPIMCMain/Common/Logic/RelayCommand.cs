using System.Windows.Input;

namespace SCPIMCMain.Common.Logic
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object?> _execute;
        private Func<object?, bool> _can_execute;

        public RelayCommand(Action<object?> __execute, Func<object?, bool> __canExecute)
        {
            _execute = __execute;
            _can_execute = __canExecute;
        }

        public bool CanExecute(object? __parameter)
        {
            if (_can_execute == null)
            {
                return true;
            }
            return _can_execute.Invoke(__parameter);
        }

        public void Execute(object? __parameter)
        {
            if (CanExecute(__parameter))
            {
                _execute.Invoke(__parameter);
            }
        }
    }
}