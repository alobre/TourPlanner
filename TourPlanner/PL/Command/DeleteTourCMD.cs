using System;
using System.Windows.Input;

namespace TourPlanner.PL.Command
{
    public class DeleteTourCMD : ICommand
    {
        private readonly Action<Tour> _execute;

        public event EventHandler CanExecuteChanged;

        public DeleteTourCMD(Action<Tour> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            // You can implement your own logic here to determine if the command can be executed
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is Tour tour)
            {
                _execute?.Invoke(tour);
            }
        }
    }
}
