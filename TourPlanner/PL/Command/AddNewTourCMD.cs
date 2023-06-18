using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TourPlanner.PL.Command
{
    internal class AddNewTourCMD : ICommand
    {
        private MainVM _viewModel;

        public AddNewTourCMD(MainVM viewModel)
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
            return _viewModel.CanAdd;
        }

        public void Execute(object parameter)
        {
            _viewModel.CreateNewTour();
        }
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainVM.CanAdd))
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
