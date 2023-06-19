using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TourPlanner.PL.Command
{
    internal class DeleteTourCMD : ICommand
    {
        private MainVM _viewModel;

        public DeleteTourCMD(MainVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.TargetTour != null;
        }

        public void Execute(object parameter)
        {
            if (parameter is Tour tour)
            {
                _viewModel.DeleteTourAsync(tour);
            }
        }
    }
}
