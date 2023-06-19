using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BL.Models;

namespace TourPlanner.PL.Command
{
    internal class ChangeSelectedTourCMD : ICommand
    {
        private MainVM _viewModel;

        public ChangeSelectedTourCMD(MainVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            // Update the selected tour
            Tour selectedTour = parameter as Tour;

            /*_viewModel.SelectedTour = selectedTour;*/
            await _viewModel.SetSelectedTour(selectedTour);
        }
    }
}
