using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TourPlanner.PL.Command
{
    internal class RelayCommandAsync<T> : ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommandAsync(Func<T, Task> execute)
            : this(execute, null)
        {
        }

        public RelayCommandAsync(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }

        private async Task ExecuteAsync(T parameter)
        {
            await _execute?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
